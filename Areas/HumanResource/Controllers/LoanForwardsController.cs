using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class LoanForwardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanForwardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/LoanForwards
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LoanForward.Include(l => l.Sanction);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/LoanForwards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanForward = await _context.LoanForward
                .Include(l => l.Sanction)
                .FirstOrDefaultAsync(m => m.ForwardId == id);
            if (loanForward == null)
            {
                return NotFound();
            }

            return View(loanForward);
        }

        // GET: HumanResource/LoanForwards/Create
        public IActionResult Create()
        {
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId");
            return View();
        }

        // POST: HumanResource/LoanForwards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ForwardId,SanctionID,Installment,Type,Reason,Date")] LoanForward loanForward)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanForward);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId", loanForward.SanctionID);
            return View(loanForward);
        }

        // GET: HumanResource/LoanForwards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanForward = await _context.LoanForward.FindAsync(id);
            if (loanForward == null)
            {
                return NotFound();
            }
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId", loanForward.SanctionID);
            return View(loanForward);
        }

        // POST: HumanResource/LoanForwards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ForwardId,SanctionID,Installment,Type,Reason,Date")] LoanForward loanForward)
        {
            if (id != loanForward.ForwardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanForward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanForwardExists(loanForward.ForwardId))
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
            ViewData["SanctionID"] = new SelectList(_context.LoanSanction, "SanctionId", "SanctionId", loanForward.SanctionID);
            return View(loanForward);
        }

        // GET: HumanResource/LoanForwards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanForward = await _context.LoanForward
                .Include(l => l.Sanction)
                .FirstOrDefaultAsync(m => m.ForwardId == id);
            if (loanForward == null)
            {
                return NotFound();
            }

            return View(loanForward);
        }

        // POST: HumanResource/LoanForwards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanForward = await _context.LoanForward.FindAsync(id);
            if (loanForward != null)
            {
                _context.LoanForward.Remove(loanForward);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanForwardExists(int id)
        {
            return _context.LoanForward.Any(e => e.ForwardId == id);
        }
    }
}
