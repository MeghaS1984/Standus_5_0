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
    public class FamiliesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FamiliesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Families
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Family.Include(f => f.Employee)
                .Where(f => f.EmployeeID == id);
            return PartialView("Index",await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/Families/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var family = await _context.Family
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (family == null)
            {
                return NotFound();
            }

            return View(family);
        }

        // GET: HumanResource/Families/Create
        public IActionResult Create(int id)
        {
            ViewData["EmployeeID"] = id;
            return View();
        }

        // POST: HumanResource/Families/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Relation,Name,Age,ID")] Family family)
        {
            family.EmployeeID = family.ID;
            family.ID = 0;
                _context.Add(family);
                await _context.SaveChangesAsync();
                return View(family);            
        }

        // GET: HumanResource/Families/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var family = await _context.Family.FindAsync(id);
            if (family == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", family.EmployeeID);
            return View(family);
        }

        // POST: HumanResource/Families/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Relation,Name,Age,ID")] Family family)
        {
            if (id != family.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(family);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FamilyExists(family.ID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", family.EmployeeID);
            return View(family);
        }

        // GET: HumanResource/Families/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var family = await _context.Family
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (family == null)
            {
                return NotFound();
            }

            return View(family);
        }

        // POST: HumanResource/Families/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var family = await _context.Family.FindAsync(id);
            if (family != null)
            {
                _context.Family.Remove(family);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FamilyExists(int id)
        {
            return _context.Family.Any(e => e.ID == id);
        }
    }
}
