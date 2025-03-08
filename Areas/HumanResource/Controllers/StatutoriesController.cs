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
    public class StatutoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatutoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Statutories
        public async Task<IActionResult> Index()
        {
            var statutories = _context.Statutory.Include(s => s.Employee);
            return PartialView("_List",await statutories.ToListAsync());
        }

        // GET: HumanResource/Statutories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statutory = await _context.Statutory
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (statutory == null)
            {
                return NotFound();
            }

            return View(statutory);
        }

        // GET: HumanResource/Statutories/Create
        public IActionResult Create(int id)
        {
            ViewData["EmployeeID"] = id;
            return View();
        }

        // POST: HumanResource/Statutories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Type,Details,ID")] Statutory statutory)
        {
            statutory.EmployeeID = statutory.ID;
            statutory.ID = 0;
            //if (ModelState.IsValid)
            //{
                _context.Add(statutory);
                await _context.SaveChangesAsync();
                return View(statutory);
            //}
            ////ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", statutory.EmployeeID);
            //return View(statutory);
        }

        // GET: HumanResource/Statutories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statutory = await _context.Statutory.FindAsync(id);
            if (statutory == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", statutory.EmployeeID);
            return View(statutory);
        }

        // POST: HumanResource/Statutories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Type,Details,ID")] Statutory statutory)
        {
            if (id != statutory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statutory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatutoryExists(statutory.ID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Email", statutory.EmployeeID);
            return View(statutory);
        }

        // GET: HumanResource/Statutories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statutory = await _context.Statutory
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (statutory == null)
            {
                return NotFound();
            }

            return View(statutory);
        }

        // POST: HumanResource/Statutories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statutory = await _context.Statutory.FindAsync(id);
            if (statutory != null)
            {
                _context.Statutory.Remove(statutory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatutoryExists(int id)
        {
            return _context.Statutory.Any(e => e.ID == id);
        }
    }
}
