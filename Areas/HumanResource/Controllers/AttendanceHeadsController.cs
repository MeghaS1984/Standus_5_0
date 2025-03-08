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
    public class AttendanceHeadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceHeadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/AttendanceHeads
        public async Task<IActionResult> Index()
        {
            return View(await _context.AttendanceHead.ToListAsync());
        }

        // GET: HumanResource/AttendanceHeads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceHead = await _context.AttendanceHead
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attendanceHead == null)
            {
                return NotFound();
            }

            return View(attendanceHead);
        }

        // GET: HumanResource/AttendanceHeads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/AttendanceHeads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,HeadType,Description,Paid,IsLeave,CarryForward,MaxAllowed,Reimburse,FromDate,ToDate,IsInactive,IsDefault,IsHoliday,IsHalfDay,IsHalfDayLeave,Priority,IsEncashment")] AttendanceHead attendanceHead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceHead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendanceHead);
        }

        // GET: HumanResource/AttendanceHeads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceHead = await _context.AttendanceHead.FindAsync(id);
            if (attendanceHead == null)
            {
                return NotFound();
            }
            return View(attendanceHead);
        }

        // POST: HumanResource/AttendanceHeads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,HeadType,Description,Paid,IsLeave,CarryForward,MaxAllowed,Reimburse,FromDate,ToDate,IsInactive,IsDefault,IsHoliday,IsHalfDay,IsHalfDayLeave,Priority,IsEncashment")] AttendanceHead attendanceHead)
        {
            if (id != attendanceHead.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceHead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceHeadExists(attendanceHead.ID))
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
            return View(attendanceHead);
        }

        // GET: HumanResource/AttendanceHeads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceHead = await _context.AttendanceHead
                .FirstOrDefaultAsync(m => m.ID == id);
            if (attendanceHead == null)
            {
                return NotFound();
            }

            return View(attendanceHead);
        }

        // POST: HumanResource/AttendanceHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendanceHead = await _context.AttendanceHead.FindAsync(id);
            if (attendanceHead != null)
            {
                _context.AttendanceHead.Remove(attendanceHead);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceHeadExists(int id)
        {
            return _context.AttendanceHead.Any(e => e.ID == id);
        }
    }
}
