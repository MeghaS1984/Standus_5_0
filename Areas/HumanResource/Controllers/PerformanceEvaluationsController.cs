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
    public class PerformanceEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformanceEvaluationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/PerformanceEvaluations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PerformanceEvaluation.Include(p => p.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/PerformanceEvaluations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceEvaluation = await _context.PerformanceEvaluation
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.EvaluationID == id);
            if (performanceEvaluation == null)
            {
                return NotFound();
            }

            return View(performanceEvaluation);
        }

        // GET: HumanResource/PerformanceEvaluations/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email");
            return View();
        }

        // POST: HumanResource/PerformanceEvaluations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvaluationID,EmployeeID,EvaluationDate,Rating,Comments")] PerformanceEvaluation performanceEvaluation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(performanceEvaluation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", performanceEvaluation.EmployeeID);
            return View(performanceEvaluation);
        }

        // GET: HumanResource/PerformanceEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceEvaluation = await _context.PerformanceEvaluation.FindAsync(id);
            if (performanceEvaluation == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", performanceEvaluation.EmployeeID);
            return View(performanceEvaluation);
        }

        // POST: HumanResource/PerformanceEvaluations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvaluationID,EmployeeID,EvaluationDate,Rating,Comments")] PerformanceEvaluation performanceEvaluation)
        {
            if (id != performanceEvaluation.EvaluationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performanceEvaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceEvaluationExists(performanceEvaluation.EvaluationID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", performanceEvaluation.EmployeeID);
            return View(performanceEvaluation);
        }

        // GET: HumanResource/PerformanceEvaluations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceEvaluation = await _context.PerformanceEvaluation
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.EvaluationID == id);
            if (performanceEvaluation == null)
            {
                return NotFound();
            }

            return View(performanceEvaluation);
        }

        // POST: HumanResource/PerformanceEvaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performanceEvaluation = await _context.PerformanceEvaluation.FindAsync(id);
            if (performanceEvaluation != null)
            {
                _context.PerformanceEvaluation.Remove(performanceEvaluation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformanceEvaluationExists(int id)
        {
            return _context.PerformanceEvaluation.Any(e => e.EvaluationID == id);
        }
    }
}
