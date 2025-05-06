using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class LoanSchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoanSchedulesController> _logger;

        public LoanSchedulesController(ApplicationDbContext context, ILogger<LoanSchedulesController> logger)
        {
            _context = context;
            _logger = logger;  // Initialize the logger
        }

        // GET: HumanResource/LoanSchedules
        public ActionResult Index(int sanctionid)
        {
            var applicationDbContext = _context.LoanSchedule;

            ViewData["Employee"] = applicationDbContext.Select(ap => ap.Sanction.Request.Employee.Name).FirstOrDefault();
            ViewData["Request"] = applicationDbContext.Select(ap => ap.Sanction.Request.RequestNo).FirstOrDefault();
            //.Where(ls => ls.SanctionID == sanctionid);

            //var data = applicationDbContext.ToList();

            //_logger.LogInformation($"Returned {data.Count} rows for SanctionID {sanctionid}");

            return View(applicationDbContext.ToList());
        }

        // GET: HumanResource/LoanSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanSchedule = await _context.LoanSchedule
                .FirstOrDefaultAsync(m => m.SanctionID == id);
            if (loanSchedule == null)
            {
                return NotFound();
            }

            return View(loanSchedule);
        }

        // GET: HumanResource/LoanSchedules/Create
        public IActionResult Create()
        {
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId");
            return View();
        }

        // POST: HumanResource/LoanSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SanctionID,Installment,Amount,Paid,Forward,Date,SalaryDate,Skip")] LoanSchedule loanSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId", loanSchedule.SanctionID);
            return View(loanSchedule);
        }

        // GET: HumanResource/LoanSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanSchedule = await _context.LoanSchedule.FindAsync(id);
            if (loanSchedule == null)
            {
                return NotFound();
            }
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId", loanSchedule.SanctionID);
            return View(loanSchedule);
        }

        // POST: HumanResource/LoanSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SanctionID,Installment,Amount,Paid,Forward,Date,SalaryDate,Skip")] LoanSchedule loanSchedule)
        {
            if (id != loanSchedule.SanctionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanScheduleExists(loanSchedule.SanctionID))
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
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId", loanSchedule.SanctionID);
            return View(loanSchedule);
        }

        // GET: HumanResource/LoanSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanSchedule = await _context.LoanSchedule
                .FirstOrDefaultAsync(m => m.SanctionID == id);
            if (loanSchedule == null)
            {
                return NotFound();
            }

            return View(loanSchedule);
        }

        // POST: HumanResource/LoanSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanSchedule = await _context.LoanSchedule.FindAsync(id);
            if (loanSchedule != null)
            {
                _context.LoanSchedule.Remove(loanSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanScheduleExists(int id)
        {
            return _context.LoanSchedule.Any(e => e.SanctionID == id);
        }

        public ActionResult updateSchedule(int id, int sanctionid) {

            var sched = _context.LoanSchedule.Where(ls => ls.ID == id).FirstOrDefault() ;
            
            sched.Forward = true ;

            DateTime schDate = sched.Date;

            _context.Update(sched);
            _context.SaveChanges();

            var newSched = new LoanSchedule();
            schDate = schDate.AddMonths(1);
            newSched = sched;
            newSched.Forward = false;
            newSched.Date = schDate;
            _context.Add(newSched);
            _context.SaveChanges();

            var updtSched = _context.LoanSchedule.Where(ls => ls.SanctionID == sanctionid && ls.ID > id && ls.ID != newSched.ID);

            foreach (var sch in updtSched)
            {
                sch.Date = schDate.AddMonths(1);
                _context.Update(sch);
                _context.SaveChanges();
            }

            return new EmptyResult(); 
        }
    }
}
