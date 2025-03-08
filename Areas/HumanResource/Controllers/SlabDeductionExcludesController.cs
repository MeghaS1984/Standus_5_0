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
    public class SlabDeductionExcludesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SlabDeductionExcludesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/SlabDeductionExcludes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SlabDeductionExclude.Include(s => s.Deduction).Include(s => s.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/SlabDeductionExcludes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDeductionExclude = await _context.SlabDeductionExclude
                .Include(s => s.Deduction)
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (slabDeductionExclude == null)
            {
                return NotFound();
            }

            return View(slabDeductionExclude);
        }

        // GET: HumanResource/SlabDeductionExcludes/Create
        public IActionResult Create()
        {
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name");
            return View();
        }

        // POST: HumanResource/SlabDeductionExcludes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,DeductionID,Exclude")] SlabDeductionExclude slabDeductionExclude)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slabDeductionExclude);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", slabDeductionExclude.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name", slabDeductionExclude.EmployeeID);
            return View(slabDeductionExclude);
        }

        // GET: HumanResource/SlabDeductionExcludes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDeductionExclude = await _context.SlabDeductionExclude.FindAsync(id);
            if (slabDeductionExclude == null)
            {
                return NotFound();
            }
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "CutOffType", slabDeductionExclude.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", slabDeductionExclude.EmployeeID);
            return View(slabDeductionExclude);
        }

        // POST: HumanResource/SlabDeductionExcludes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,DeductionID,Exclude")] SlabDeductionExclude slabDeductionExclude)
        {
            if (id != slabDeductionExclude.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slabDeductionExclude);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlabDeductionExcludeExists(slabDeductionExclude.EmployeeID))
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
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "CutOffType", slabDeductionExclude.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", slabDeductionExclude.EmployeeID);
            return View(slabDeductionExclude);
        }

        // GET: HumanResource/SlabDeductionExcludes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDeductionExclude = await _context.SlabDeductionExclude
                .Include(s => s.Deduction)
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (slabDeductionExclude == null)
            {
                return NotFound();
            }

            return View(slabDeductionExclude);
        }

        // POST: HumanResource/SlabDeductionExcludes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slabDeductionExclude = await _context.SlabDeductionExclude.FindAsync(id);
            if (slabDeductionExclude != null)
            {
                _context.SlabDeductionExclude.Remove(slabDeductionExclude);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlabDeductionExcludeExists(int id)
        {
            return _context.SlabDeductionExclude.Any(e => e.EmployeeID == id);
        }
    }
}
