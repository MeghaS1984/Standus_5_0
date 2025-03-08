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
    public class FriengeAssignsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriengeAssignsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/FriengeAssigns
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FriengeAssign.Include(f => f.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/FriengeAssigns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friengeAssign = await _context.FriengeAssign
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(m => m.FriengeID == id);
            if (friengeAssign == null)
            {
                return NotFound();
            }

            return View(friengeAssign);
        }

        // GET: HumanResource/FriengeAssigns/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator");
            return View();
        }

        // POST: HumanResource/FriengeAssigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FriengeID,EmployeeID,Amount")] FriengeAssign friengeAssign)
        {
            if (ModelState.IsValid)
            {
                _context.Add(friengeAssign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", friengeAssign.EmployeeID);
            return View(friengeAssign);
        }

        // GET: HumanResource/FriengeAssigns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friengeAssign = await _context.FriengeAssign.FindAsync(id);
            if (friengeAssign == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", friengeAssign.EmployeeID);
            return View(friengeAssign);
        }

        // POST: HumanResource/FriengeAssigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FriengeID,EmployeeID,Amount")] FriengeAssign friengeAssign)
        {
            if (id != friengeAssign.FriengeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friengeAssign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriengeAssignExists(friengeAssign.FriengeID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", friengeAssign.EmployeeID);
            return View(friengeAssign);
        }

        // GET: HumanResource/FriengeAssigns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friengeAssign = await _context.FriengeAssign
                .Include(f => f.Employee)
                .FirstOrDefaultAsync(m => m.FriengeID == id);
            if (friengeAssign == null)
            {
                return NotFound();
            }

            return View(friengeAssign);
        }

        // POST: HumanResource/FriengeAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friengeAssign = await _context.FriengeAssign.FindAsync(id);
            if (friengeAssign != null)
            {
                _context.FriengeAssign.Remove(friengeAssign);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriengeAssignExists(int id)
        {
            return _context.FriengeAssign.Any(e => e.FriengeID == id);
        }
    }
}
