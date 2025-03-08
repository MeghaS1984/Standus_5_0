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
    public class SlabsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SlabsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Slabs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Slab.ToListAsync());
        }

        public ActionResult SlabSetting() {
            return View();
        }
        // GET: HumanResource/Slabs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slab = await _context.Slab
                .FirstOrDefaultAsync(m => m.SlabID == id);
            if (slab == null)
            {
                return NotFound();
            }

            return View(slab);
        }

        // GET: HumanResource/Slabs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/Slabs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlabID,DeductionID,AllowanceID")] Slab slab)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slab);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slab);
        }

        // GET: HumanResource/Slabs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slab = await _context.Slab.FindAsync(id);
            if (slab == null)
            {
                return NotFound();
            }
            return View(slab);
        }

        // POST: HumanResource/Slabs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SlabID,DeductionID,AllowanceID")] Slab slab)
        {
            if (id != slab.SlabID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slab);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlabExists(slab.SlabID))
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
            return View(slab);
        }

        // GET: HumanResource/Slabs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slab = await _context.Slab
                .FirstOrDefaultAsync(m => m.SlabID == id);
            if (slab == null)
            {
                return NotFound();
            }

            return View(slab);
        }

        // POST: HumanResource/Slabs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slab = await _context.Slab.FindAsync(id);
            if (slab != null)
            {
                _context.Slab.Remove(slab);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlabExists(int id)
        {
            return _context.Slab.Any(e => e.SlabID == id);
        }
    }
}
