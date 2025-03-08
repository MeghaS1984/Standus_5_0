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
    public class FriengesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriengesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Frienges
        public async Task<IActionResult> Index()
        {
            return View(await _context.Frienge.ToListAsync());
        }

        // GET: HumanResource/Frienges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frienge = await _context.Frienge
                .FirstOrDefaultAsync(m => m.Id == id);
            if (frienge == null)
            {
                return NotFound();
            }

            return View(frienge);
        }

        // GET: HumanResource/Frienges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/Frienges/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FriengeType,PayrollSlNO,InActive")] Frienge frienge)
        {
            if (ModelState.IsValid)
            {
                _context.Add(frienge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(frienge);
        }

        // GET: HumanResource/Frienges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frienge = await _context.Frienge.FindAsync(id);
            if (frienge == null)
            {
                return NotFound();
            }
            return View(frienge);
        }

        // POST: HumanResource/Frienges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FriengeType,PayrollSlNO,InActive")] Frienge frienge)
        {
            if (id != frienge.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(frienge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriengeExists(frienge.Id))
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
            return View(frienge);
        }

        // GET: HumanResource/Frienges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var frienge = await _context.Frienge
                .FirstOrDefaultAsync(m => m.Id == id);
            if (frienge == null)
            {
                return NotFound();
            }

            return View(frienge);
        }

        // POST: HumanResource/Frienges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var frienge = await _context.Frienge.FindAsync(id);
            if (frienge != null)
            {
                _context.Frienge.Remove(frienge);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriengeExists(int id)
        {
            return _context.Frienge.Any(e => e.Id == id);
        }
    }
}
