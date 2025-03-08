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
    public class LeaveAllocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveAllocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/LeaveAllocations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LeaveAllocation.Include(l => l.Category).Include(l => l.Head);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/LeaveAllocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveAllocation = await _context.LeaveAllocation
                .Include(l => l.Category)
                .Include(l => l.Head)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (leaveAllocation == null)
            {
                return NotFound();
            }

            return View(leaveAllocation);
        }

        // GET: HumanResource/LeaveAllocations/Create
        public IActionResult Create()
        {
            var categories = _context.Category.Select(c => new
            {
                id=c.ID,
                name=c.CategoryName
            }) ;

            
            ViewData["CategoryID"] = new SelectList(categories, "id", "name");
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType");
            return View();
        }

        // POST: HumanResource/LeaveAllocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryID,StartDate,EndDate,HeadID")] LeaveAllocation leaveAllocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveAllocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", leaveAllocation.CategoryID);
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType", leaveAllocation.HeadID);
            return View(leaveAllocation);
        }

        // GET: HumanResource/LeaveAllocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveAllocation = await _context.LeaveAllocation.FindAsync(id);
            if (leaveAllocation == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", leaveAllocation.CategoryID);
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType", leaveAllocation.HeadID);
            return View(leaveAllocation);
        }

        // POST: HumanResource/LeaveAllocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryID,StartDate,EndDate,HeadID")] LeaveAllocation leaveAllocation)
        {
            if (id != leaveAllocation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveAllocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveAllocationExists(leaveAllocation.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", leaveAllocation.CategoryID);
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType", leaveAllocation.HeadID);
            return View(leaveAllocation);
        }

        // GET: HumanResource/LeaveAllocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveAllocation = await _context.LeaveAllocation
                .Include(l => l.Category)
                .Include(l => l.Head)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (leaveAllocation == null)
            {
                return NotFound();
            }

            return View(leaveAllocation);
        }

        // POST: HumanResource/LeaveAllocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveAllocation = await _context.LeaveAllocation.FindAsync(id);
            if (leaveAllocation != null)
            {
                _context.LeaveAllocation.Remove(leaveAllocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveAllocationExists(int id)
        {
            return _context.LeaveAllocation.Any(e => e.ID == id);
        }
    }
}
