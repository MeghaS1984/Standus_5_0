using Microsoft.VisualBasic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.Report.Models;
using Standus_5_0.Data;

namespace Standus_5_0.Areas.Report.Controllers
{
    [Area("Report")]
    public class ReportsSubqueriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsSubqueriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReportsSubqueries
        public ActionResult Index(int Reportid)
        {
            ViewData["ReportID"] = Reportid;
            return View(_context.ReportsSubquery.Where(f => f.ReportID == Reportid).OrderBy(f => f.ID));
        }

        // GET: ReportsSubqueries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            ReportsSubquery reportsSubquery =   _context.ReportsSubquery.Find(id);

            if (reportsSubquery == null)
                return NotFound();

            return View(reportsSubquery);
        }


        // GET: ReportsSubqueries/Create
        public ActionResult Create(int Reportid)
        {
            ViewData["ReportID"] = Reportid;
            return View();
        }

        // POST: ReportsSubqueries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(ReportsSubquery reportsSubquery)
        {
            RouteValueDictionary val = new RouteValueDictionary();
            val.Add("Reportid", reportsSubquery.ReportID);
            if (ModelState.IsValid)
            {
                _context.ReportsSubquery.Add(reportsSubquery);
                _context.SaveChanges();
                return RedirectToAction("Index", val);
            }
            return View(reportsSubquery);
        }

        // GET: ReportsSubqueries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Information.IsNothing(id))
                return BadRequest();
            ReportsSubquery reportsSubquery = _context.ReportsSubquery.Find(id);
            if (reportsSubquery == null)
                return NotFound();
            return View(reportsSubquery);
        }
        // POST: ReportsSubqueries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(ReportsSubquery reportsSubquery)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(reportsSubquery).State = EntityState.Modified;
                _context.SaveChanges();

                RouteValueDictionary val = new RouteValueDictionary();
                val.Add("Reportid", reportsSubquery.ReportID);

                return RedirectToAction("Index", val);
            }
            return View(reportsSubquery);
        }

        // GET: ReportsSubqueries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Information.IsNothing(id))
                return BadRequest();
            ReportsSubquery reportsSubquery = _context.ReportsSubquery.Find(id);
            if (reportsSubquery != null)
                return NotFound();
            return View(reportsSubquery);
        }
        // POST: ReportsSubqueries/Delete/5
        [HttpPost()]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportsSubquery reportsSubquery = _context.ReportsSubquery.Find(id);

            RouteValueDictionary val = new RouteValueDictionary();
            val.Add("Reportid", reportsSubquery.ReportID);

            _context.ReportsSubquery.Remove(reportsSubquery);
            _context.SaveChanges();
            return RedirectToAction("Index", val);
        }
    }
}
