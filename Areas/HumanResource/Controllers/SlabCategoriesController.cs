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
        public async Task<IActionResult> Index(int SlabID, int AllowanceID)
        {
            var applicationDbContext = from catg in _context.Category
                                       join slb in _context.SlabCategory on catg.ID equals slb.CategoryID
                                       where slb.SlabID == SlabID
                                       select new { 
                                           CategoryID= catg.ID,
                                           Category = catg.CategoryName,
                                           SlabID = slb.SlabID
                                       };
            ViewData["AllowanceID"] = AllowanceID;                         
            //var applicationDbContext = _context.SlabCategory.Include(s => s.Category);
            return PartialView("_Index",await applicationDbContext.ToListAsync());
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
        public IActionResult Create(int? id)
        {
            var allw = _context.Allowance.Where(a => a.ID  == id).Include(s => s.slab).FirstOrDefault() ;
            
            ViewData["SlabID"] = allw.slab.SlabID;
            ViewData["AllowanceID"] = allw.ID;
            ViewData["Allowance"] = allw.Name;
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
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {             
                var slb = _context.SlabCategory
                    .Where(s => s.SlabID == slabCategory.SlabID && s.CategoryID == slabCategory.CategoryID).FirstOrDefault();

                if (slb != null) {
                    var allw = _context.Allowance.Include(s => s.slab)
                        .Where(a => a.slab.SlabID == slabCategory.SlabID)
                        .FirstOrDefault();

                    ModelState.AddModelError("CategoryID","Category assigned.");
                    return RedirectToAction(nameof(Create),new { id = allw.ID });
                }

                _context.Add(slabCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
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

        
        public async Task<IActionResult> DeleteConfirmed(int slabid , int categoryid, int allowanceid)
        {
            var slabCategory = await _context.SlabCategory
                .Where(c => c.SlabID == slabid && c.CategoryID == categoryid)
                .FirstOrDefaultAsync();

            if (slabCategory != null)
            {
                _context.SlabCategory.Remove(slabCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create), new { id = allowanceid});
        }

        private bool SlabCategoryExists(int id)
        {
            return _context.SlabCategory.Any(e => e.SlabID == id);
        }
    }
}
