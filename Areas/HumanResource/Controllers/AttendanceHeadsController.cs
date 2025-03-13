using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;
using YourNamespace.Models;

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

        public async Task<IActionResult> LeaveTypes()
        {
            var years = from accrual in _context.LeaveAllocationDetails
                        group accrual by accrual.HeadID into accrualGroup
                        select new
                        {
                            HeadId = accrualGroup.Key,  // Group key is the HeadID
                            cYear = accrualGroup.FirstOrDefault().StartDate
                        };

            var attendanceHeads = await _context.AttendanceHead
                .Where(a => a.IsLeave)
                .ToListAsync();

            // Here, you could merge the `years` projection with the attendanceHeads
            // For example, create a dictionary or a lookup for the `HeadId` and `cYear` values.
            var headIdToYearMap = years.ToDictionary(y => y.HeadId, y => y.cYear);

            // Now, if you want to access the year for each AttendanceHead:
            foreach (var head in attendanceHeads)
            {
                if (headIdToYearMap.TryGetValue(head.ID, out var year))
                {
                    // You can now use `year` for the corresponding `HeadID` for each `AttendanceHead`.
                    head.FromDate  = year; // Example: setting a property on AttendanceHead if required.
                }
            }

            // Return the view
            return View(attendanceHeads);


            //return View(await _context.AttendanceHead.Where(a => a.IsLeave).ToListAsync());
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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/AttendanceHeads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceHead attendanceHead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceHead);
                await _context.SaveChangesAsync();

                if (attendanceHead.ID > 0)
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                }
                else
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                }
                return RedirectToAction(nameof(Create));
            }
            return View(attendanceHead);
        }

        // GET: HumanResource/AttendanceHeads/Edit/5
        [HttpGet]
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
        public async Task<IActionResult> Edit(int id,  AttendanceHead attendanceHead)
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

                    if (attendanceHead.ID > 0)
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                    }
                    else
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                    }
                    return RedirectToAction(nameof(Edit));
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

            var dailyAttendance = _context.AttendanceDetails.Where(h => h.Half1 == id || h.Half2 == id).FirstOrDefault();

            if (dailyAttendance != null)
            {
                TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, attendanceHead.HeadType +  " is in use. Select Inactivate !");
                return RedirectToAction(nameof(Delete));
            }
            
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
