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
    public class HolidaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HolidaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Holidays
        public async Task<IActionResult> Index()
        {
            return View(await _context.Holidays.ToListAsync());
        }

        // GET: HumanResource/Holidays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays
                .FirstOrDefaultAsync(m => m.ID == id);
            if (holidays == null)
            {
                return NotFound();
            }

            return View(holidays);
        }

        // GET: HumanResource/Holidays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/Holidays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,sDate,Reason,Reminder,Paid,HeadID")] Holidays holidays)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holidays);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(holidays);
        }

        // GET: HumanResource/Holidays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays.FindAsync(id);
            if (holidays == null)
            {
                return NotFound();
            }
            return View(holidays);
        }

        // POST: HumanResource/Holidays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,sDate,Reason,Reminder,Paid,HeadID")] Holidays holidays)
        {
            if (id != holidays.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holidays);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidaysExists(holidays.ID))
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
            return View(holidays);
        }

        // GET: HumanResource/Holidays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays
                .FirstOrDefaultAsync(m => m.ID == id);
            if (holidays == null)
            {
                return NotFound();
            }

            return View(holidays);
        }

        // POST: HumanResource/Holidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var holidays = await _context.Holidays.FindAsync(id);
            if (holidays != null)
            {
                _context.Holidays.Remove(holidays);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HolidaysExists(int id)
        {
            return _context.Holidays.Any(e => e.ID == id);
        }
    }
}
