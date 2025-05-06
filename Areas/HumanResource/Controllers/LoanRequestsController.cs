using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class LoanRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private List<SelectListItem>  loanType;
        private List<SelectListItem> status;
        public LoanRequestsController(ApplicationDbContext context)
        {
            loanType= new List<SelectListItem> {
                new SelectListItem {Text = "Advance" , Value ="Advance"},
                new SelectListItem { Text = "Bank Loan", Value = "Bank Loan"}
            };

            status = new List<SelectListItem>
            {
                new SelectListItem {Text = "Open" , Value = "Open"},
                new SelectListItem {Text = "Closed" , Value = "Closed"}
            };

            
            _context = context;
        }

        // GET: HumanResource/LoanRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LoanRequest.Include(l => l.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/LoanRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanRequest = await _context.LoanRequest
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (loanRequest == null)
            {
                return NotFound();
            }

            return View(loanRequest);
        }

        
        // GET: HumanResource/LoanRequests/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Name");
            ViewData["LoanType"] = new SelectList(loanType, "Value", "Text");
            ViewData["Status"] = new SelectList(status, "Value", "Text");

            //var request = new LoanRequest();
            //request.RequestNo = "0001";

            return View();
        }

        // POST: HumanResource/LoanRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanRequest loanRequest)
        {

            ModelState.Remove("RequestNo");

            if (loanRequest.LoanType == "Advance")
            {
                ModelState.Remove("Bank");
                loanRequest.Bank = "";
            }

            ModelState.Remove("Comment");
            ModelState.Remove("Employee");
            ViewData["LoanType"] = new SelectList(loanType, "Value", "Text",loanRequest.LoanType);
            ViewData["Status"] = new SelectList(status, "Value", "Text",loanRequest.Status);

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Name", loanRequest.EmployeeId);
            loanRequest.Status = "Applied";
            if (ModelState.IsValid)
            {
                try
                {
                    var trans = _context.Database.BeginTransaction();
                    var req = _context.LoanRequest.OrderBy(o => o.RequestNo);

                    if (req.Any())
                    {
                        int reqno = int.Parse(req.Last().RequestNo) + 1;
                        loanRequest.RequestNo = reqno.ToString().PadLeft(4,'0');
                    }
                    else {
                        loanRequest.RequestNo = "1".PadLeft(4, '0'); 
                    }

                    _context.Add(loanRequest);
                    await _context.SaveChangesAsync();
                    trans.Commit();

                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Data saved.");

                } catch(Exception) {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Unknown error");
                }
            }
            
            return View(loanRequest);
        }

        // GET: HumanResource/LoanRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Name");
            ViewData["LoanType"] = new SelectList(loanType, "Value", "Text");
            ViewData["Status"] = new SelectList(status, "Value", "Text");

            if (id == null)
            {
                return NotFound();
            }

            var loanRequest = await _context.LoanRequest.FindAsync(id);
            if (loanRequest == null)
            {
                return NotFound();
            }
            return View(loanRequest);
        }

        // POST: HumanResource/LoanRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoanRequest loanRequest)
        {
            if (id != loanRequest.RequestID)
            {
                return NotFound();
            }

            ModelState.Remove("RequestNo");

            if (loanRequest.LoanType == "Advance")
            {
                ModelState.Remove("Bank");
                loanRequest.Bank = "";
            }

            ModelState.Remove("Comment");
            ModelState.Remove("Employee");
            ViewData["LoanType"] = new SelectList(loanType, "Value", "Text", loanRequest.LoanType);
            ViewData["Status"] = new SelectList(status, "Value", "Text", loanRequest.Status);

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Name", loanRequest.EmployeeId);
            loanRequest.Status = "Applied";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanRequestExists(loanRequest.RequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Name", loanRequest.EmployeeId);
            return View(loanRequest);
        }

        // GET: HumanResource/LoanRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanRequest = await _context.LoanRequest
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (loanRequest == null)
            {
                return NotFound();
            }

            return View(loanRequest);
        }

        // POST: HumanResource/LoanRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanRequest = await _context.LoanRequest.FindAsync(id);
            if (loanRequest != null)
            {
                _context.LoanRequest.Remove(loanRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanRequestExists(int id)
        {
            return _context.LoanRequest.Any(e => e.RequestID == id);
        }

        public ActionResult Reject(int id, string comment) { 
        
            var reqst = _context.LoanRequest.FirstOrDefault(e => e.RequestID == id);

            reqst.Status = "Reject";
            reqst.Comment = comment;

            _context.LoanRequest.Update(reqst);
            _context.SaveChanges();

            return new EmptyResult ();
        }
    }
}
