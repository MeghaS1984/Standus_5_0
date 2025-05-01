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
    public class LoanSanctionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanSanctionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/LoanSanctions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LoanSanction
                .Include(l => l.Request)
                .Include(l => l.Deduction)
                .Include(l => l.Request.Employee);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/LoanSanctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanSanction = await _context.LoanSanction
                .Include(l => l.Request)
                .FirstOrDefaultAsync(m => m.SanctionId == id);
            if (loanSanction == null)
            {
                return NotFound();
            }

            return View(loanSanction);
        }

        // GET: HumanResource/LoanSanctions/Create
        public IActionResult Create(int requestid)
        {
            ViewData["RequestID"] = new SelectList(_context.LoanRequest, "RequestID", "RequestID");
            ViewData["DeductionID"] = new SelectList(_context.Deduction,"ID","Name");

            var request = _context.LoanRequest.Where(r => r.RequestID == requestid).Include(r => r.Employee).FirstOrDefault();

            var sanction = new LoanSanction();

            sanction.Request = request;
            sanction.Date = DateTime.Now; 
            sanction.InstallmentDate = DateTime.Now;
            return View(sanction);
        }

        // POST: HumanResource/LoanSanctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LoanSanction loanSanction)
        {
            ModelState.Remove("Deduction");
            ModelState.Remove("Request");
            if (ModelState.IsValid)
            {
                loanSanction.DeductionType = "";
                _context.Add(loanSanction);
                _context.SaveChanges();

                // load loan schedule

                var loanSchedule = new LoanSchedule();
                int installmentNo = 1;
                decimal loanAmount = 0;
                DateTime schDate = loanSanction.InstallmentDate;
                while (loanAmount < loanSanction.Amount) {
                    loanAmount = loanAmount + loanSanction.Installment;
                    loanSchedule.SanctionID = loanSanction.SanctionId;
                    loanSchedule.Installment = installmentNo;
                    loanSchedule.Amount = loanSanction.Installment;
                    loanSchedule.Paid = 0;
                    loanSchedule.Forward = false;
                    loanSchedule.Date = schDate;

                    _context.LoanSchedule.Add(loanSchedule);
                    _context.SaveChanges();
                    schDate = schDate.AddMonths(1); 
                    installmentNo++;
                }

                loanSanction.Installment = installmentNo;
                _context.Update(loanSanction);
                _context.SaveChanges();

                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Data saved.");

                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestID"] = new SelectList(_context.LoanRequest, "RequestID", "RequestID", loanSanction.RequestID);
            return View(loanSanction);
        }

        // GET: HumanResource/LoanSanctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanSanction = await _context.LoanSanction.FindAsync(id);
            if (loanSanction == null)
            {
                return NotFound();
            }
            ViewData["RequestID"] = new SelectList(_context.LoanRequest, "RequestID", "RequestID", loanSanction.RequestID);
            return View(loanSanction);
        }

        // POST: HumanResource/LoanSanctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,Date,Amount,Interest,DeductionType,Installment,InstallmentNo,InstallmentDate,SanctionId,DeductionId")] LoanSanction loanSanction)
        {
            if (id != loanSanction.SanctionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanSanction);
                    await _context.SaveChangesAsync();
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Data saved.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanSanctionExists(loanSanction.SanctionId))
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
            ViewData["RequestID"] = new SelectList(_context.LoanRequest, "RequestID", "RequestID", loanSanction.RequestID);
            return View(loanSanction);
        }

        // GET: HumanResource/LoanSanctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanSanction = await _context.LoanSanction
                .Include(l => l.Request)
                .FirstOrDefaultAsync(m => m.SanctionId == id);
            if (loanSanction == null)
            {
                return NotFound();
            }

            return View(loanSanction);
        }

        // POST: HumanResource/LoanSanctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanSanction = await _context.LoanSanction.FindAsync(id);
            if (loanSanction != null)
            {
                _context.LoanSanction.Remove(loanSanction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanSanctionExists(int id)
        {
            return _context.LoanSanction.Any(e => e.SanctionId == id);
        }

        public ActionResult Pending() {

            var sanctionedIds = _context.LoanSanction.Select(s => s.RequestID);
            var pending = _context.LoanRequest.Where(r => !sanctionedIds.Contains(r.RequestID))
                .Where(r => r.Status != "Reject")
                .Include(p => p.Employee)
                .ToList();

            return PartialView("Pending",pending);
        }
    }
}
