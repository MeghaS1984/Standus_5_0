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
    public class EmploymentHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmploymentHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/EmploymentHistories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EmploymentHistory.Include(e => e.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/EmploymentHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employmentHistory = await _context.EmploymentHistory
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employmentHistory == null)
            {
                return NotFound();
            }

            return View(employmentHistory);
        }

        // GET: HumanResource/EmploymentHistories/Create
        public IActionResult Create(int id)
        {
            ViewData["EmployeeID"] = id;//new SelectList(_context.Employee, "EmployeeID", "Email");
            return View();
        }

        // POST: HumanResource/EmploymentHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmploymentHistoryID,EmployeeID,CompanyName,JobTitle,StartDate,EndDate,Responsibilities")] EmploymentHistory employmentHistory)
        {

            //if (ModelState.IsValid)
            //{
                _context.Add(employmentHistory);
                await _context.SaveChangesAsync();
                return View(employmentHistory);
            //}
            //ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", employmentHistory.EmployeeID);

        }

        // GET: HumanResource/EmploymentHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employmentHistory = await _context.EmploymentHistory.FindAsync(id);
            if (employmentHistory == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", employmentHistory.EmployeeID);
            return View(employmentHistory);
        }

        // POST: HumanResource/EmploymentHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmploymentHistoryID,EmployeeID,CompanyName,JobTitle,StartDate,EndDate,Responsibilities")] EmploymentHistory employmentHistory)
        {
            if (id != employmentHistory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employmentHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploymentHistoryExists(employmentHistory.ID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", employmentHistory.EmployeeID);
            return View(employmentHistory);
        }

        // GET: HumanResource/EmploymentHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employmentHistory = await _context.EmploymentHistory
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employmentHistory == null)
            {
                return NotFound();
            }

            return View(employmentHistory);
        }

        // POST: HumanResource/EmploymentHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employmentHistory = await _context.EmploymentHistory.FindAsync(id);
            if (employmentHistory != null)
            {
                _context.EmploymentHistory.Remove(employmentHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmploymentHistoryExists(int id)
        {
            return _context.EmploymentHistory.Any(e => e.ID == id);
        }
    }
}
