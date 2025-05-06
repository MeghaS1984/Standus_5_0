using Microsoft.AspNetCore.Mvc;
using Standus_5_0.Areas.Identity.Models;
using Standus_5_0.Areas.Report.Models;
using Standus_5_0.Data;

namespace Standus_5_0.Areas.Report.Controllers
{
    public class UserRights111Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        //private UserRightsContext rights = new UserRightsContext("ERP");
        //private ReportContext Reports = new ReportContext("ERP");
        // GET: UserRights

        public UserRights111Controller(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index(string? UserID)
        {

            var roles = (from rpt in _context.Reports != null ? _context.Reports.ToList() : Enumerable.Empty<Reports>()
                         join rgt in _context.ApplicationUser != null ? _context.ApplicationUser.ToList() : Enumerable.Empty<ApplicationUser>() on rpt.ReportID.ToString().Trim() equals rgt.Report.Trim()
                         where rgt != null && rgt.Id != UserID 
                         select new
                         {
                             reportid = rpt.ReportID,
                             reportName = rpt.ReportName,
                             userid = rgt.Id,
                             status = "Deny",
                             groupid = rpt.GroupID
                         }
                ).ToList();
            var roles1 = (from rpt in _context.Reports != null ? _context.Reports.ToList() : Enumerable.Empty<Reports>()
                          where !roles.Any(p => p.reportid == rpt.ReportID)
                          select new
                          {
                              reportid = rpt.ReportID,
                              reportName = rpt.ReportName,
                              userid = UserID,
                              status = "Grant",
                              groupid = rpt.GroupID
                          }
                ).ToList();

            //var allroles = roles.Concat(roles1).ToList();

            var allroles = roles.Concat(roles1).ToList(); 

            ViewBag.Roles = allroles.OrderBy(i => i.groupid);
            return PartialView();
        }

        public JsonResult grant(int reportid, string? userid)
        {
            ApplicationUser  ruser = new ApplicationUser ();
            ruser.Report = reportid.ToString();
            ruser.Id = userid;
            _context.ApplicationUser.Add(ruser);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public JsonResult deny(int reportid, string userid)
        {
            // Dim ruser As New UserRights
            var ruser = from usr in _context.ApplicationUser 
                        where usr.Report == reportid.ToString().Trim() & usr.Id == userid
                        select usr;

            if (ruser.Any())
            {
                _context.ApplicationUser.Remove(ruser.First());
                _context.SaveChanges();
                TempData["IsInUpdateMode"] = true;
            }

            // Dim val As New RouteValueDictionary
            // Val.Add("UserID", userid)
            return Json(new { success = true });
        }
    }
}

public class UserRoles
{
}
