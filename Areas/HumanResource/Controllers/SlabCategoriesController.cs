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
    public class SlabCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SlabCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/SlabCategories
        public async Task<IActionResult> Index(int SlabID)
        {
            var applicationDbContext = from catg in _context.Category
                                       join slb in _context.SlabCategory on catg.ID equals slb.CategoryID
                                       into slabgroup
                                       from slb in slabgroup.DefaultIfEmpty()
                                       where (slb == null || slb.SlabID == SlabID)
                                       select new { 
                                           CategoryID= catg.ID,
                                           Category = catg.CategoryName,
                                           SlabID = slb != null ? slb.SlabID : 0
                                       };
                                        
            //var applicationDbContext = _context.SlabCategory.Include(s => s.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/SlabCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabCategory = await _context.SlabCategory
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SlabID == id);
            if (slabCategory == null)
            {
                return NotFound();
            }

            return View(slabCategory);
        }

        // GET: HumanResource/SlabCategories/Create
        public IActionResult Create()
        {

            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName");
            return View();
        }

        // POST: HumanResource/SlabCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlabID,CategoryID")] SlabCategory slabCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slabCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabCategory.CategoryID);
            return View(slabCategory);
        }

        // GET: HumanResource/SlabCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabCategory = await _context.SlabCategory.FindAsync(id);
            if (slabCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabCategory.CategoryID);
            return View(slabCategory);
        }

        // POST: HumanResource/SlabCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SlabID,CategoryID")] SlabCategory slabCategory)
        {
            if (id != slabCategory.SlabID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slabCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlabCategoryExists(slabCategory.SlabID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabCategory.CategoryID);
            return View(slabCategory);
        }

        // GET: HumanResource/SlabCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabCategory = await _context.SlabCategory
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SlabID == id);
            if (slabCategory == null)
            {
                return NotFound();
            }

            return View(slabCategory);
        }

        // POST: HumanResource/SlabCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slabCategory = await _context.SlabCategory.FindAsync(id);
            if (slabCategory != null)
            {
                _context.SlabCategory.Remove(slabCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlabCategoryExists(int id)
        {
            return _context.SlabCategory.Any(e => e.SlabID == id);
        }
    }
}
