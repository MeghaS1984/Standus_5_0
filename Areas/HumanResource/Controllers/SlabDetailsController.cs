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
    public class SlabDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SlabDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/SlabDetails
        public async Task<IActionResult> Index(int slabid, int categoryid)
        {
            var applicationDbContext = _context.SlabDetails.Include(s => s.Category).
                Where(f => f.SlabID == slabid && f.CategoryID == categoryid);

            //var applicationDbContext = from slbd in _context.SlabDetails
            //                           where 

            return PartialView("index",await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/SlabDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDetails = await _context.SlabDetails
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slabDetails == null)
            {
                return NotFound();
            }

            return View(slabDetails);
        }

        // GET: HumanResource/SlabDetails/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName");
            
            return PartialView();
        }

        // POST: HumanResource/SlabDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlabID,FromAmount,ToAmount,Type,Employee,Employer,ID,CategoryID")] SlabDetails slabDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slabDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabDetails.CategoryID);
            return View(slabDetails);
        }

        // GET: HumanResource/SlabDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDetails = await _context.SlabDetails.FindAsync(id);
            if (slabDetails == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabDetails.CategoryID);
            return View(slabDetails);
        }

        // POST: HumanResource/SlabDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SlabID,FromAmount,ToAmount,Type,Employee,Employer,ID,CategoryID")] SlabDetails slabDetails)
        {
            if (id != slabDetails.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slabDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlabDetailsExists(slabDetails.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabDetails.CategoryID);
            return View(slabDetails);
        }

        // GET: HumanResource/SlabDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDetails = await _context.SlabDetails
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slabDetails == null)
            {
                return NotFound();
            }

            return View(slabDetails);
        }

        // POST: HumanResource/SlabDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slabDetails = await _context.SlabDetails.FindAsync(id);
            if (slabDetails != null)
            {
                _context.SlabDetails.Remove(slabDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlabDetailsExists(int id)
        {
            return _context.SlabDetails.Any(e => e.ID == id);
        }
    }
}
