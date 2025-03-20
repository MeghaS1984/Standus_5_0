using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;

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

            var holidays = _context.Holidays
                .Include(f => f.Head).ToList();

            var filter_holidays = holidays.Where(d => d.sDate.Year == DateTime.Now.Year ).ToList();

            return View(filter_holidays);
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
        [HttpGet]
        public IActionResult Create()
        {
            int headid = _context.AttendanceHead
                .Where(f => f.IsHoliday)
                .Select(f => f.ID).FirstOrDefault();  

            var items = new List<SelectListItem> { new SelectListItem { Text = "Select Head Type", Value = "0" } };
            items.AddRange(_context.AttendanceHead.Select(s => new SelectListItem { 
                Text = s.HeadType  ,
                Value = s.ID.ToString () 
            }));
            ViewData["HeadID"] = new SelectList(items, "Value", "Text",headid);
            return View();
        }

        // POST: HumanResource/Holidays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,sDate,Reason,Reminder,Paid,HeadID")] Holidays holidays)
        {
            ModelState.Remove("Head");

            // set head select list before executing any further code to help populate heads if there are any error found 
            // during execution

            var items = new List<SelectListItem> { new SelectListItem { Text = "Select Head Type", Value = "0" } };
            items.AddRange(_context.AttendanceHead.Select(s => new SelectListItem
            {
                Text = s.HeadType,
                Value = s.ID.ToString()
            }));
            ViewData["HeadID"] = new SelectList(items, "Value", "Text", holidays.HeadID);

            if (ModelState.IsValid)
            {
                if (holidays.Reminder > holidays.sDate)
                {
                    ModelState.AddModelError("Reminder", "Set Reminder Date on or before Holiday Date.");
                    return View(holidays);
                }
                else if (holidays.sDate == DateTime.Now.Date && holidays.Reminder < holidays.sDate)
                {
                    ModelState.AddModelError("Reminder", "Set Reminder Date to Holiday Date.");
                    return View(holidays);
                }
                else if (holidays.sDate < DateTime.Now.Date ) {
                    ModelState.AddModelError("sDate", "Holidays allowed for today or future dates.");
                    return View(holidays);
                }
                
                _context.Add(holidays);
                await _context.SaveChangesAsync();
                if (holidays.ID > 0)
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                }
                else
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                }
                return RedirectToAction(nameof(Create));
            }
            

            return View(holidays);
        }

        // GET: HumanResource/Holidays/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays.FindAsync(id);

            var items = new List<SelectListItem> { new SelectListItem { Text = "Select Head Type", Value = "0" } };
            items.AddRange(_context.AttendanceHead.Select(s => new SelectListItem
            {
                Text = s.HeadType,
                Value = s.ID.ToString()
            }));
            ViewData["HeadID"] = new SelectList(items, "Value", "Text",holidays.HeadID);

            
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

                    if (holidays.ID > 0)
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
                    if (!HolidaysExists(holidays.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            var items = new List<SelectListItem> { new SelectListItem { Text = "Select Head Type", Value = "0" } };
            items.AddRange(_context.AttendanceHead.Select(s => new SelectListItem
            {
                Text = s.HeadType,
                Value = s.ID.ToString()
            }));
            ViewData["HeadID"] = new SelectList(items, "Value", "Text", holidays.HeadID);
            return View(holidays);
        }

        // GET: HumanResource/Holidays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays.Include(h => h.Head)
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

            DateTime holiday;
            holiday = holidays.sDate;
                
            var attnd = _context.AttendanceDetails.Where(f => f.Date == holiday).FirstOrDefault ();

            if (attnd != null)
            {
                TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, holiday + " is expired!");
                return RedirectToAction(nameof(Delete));
            }

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
