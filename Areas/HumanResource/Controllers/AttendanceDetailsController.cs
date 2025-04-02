using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;
using YourNamespace.Models;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class AttendanceDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/AttendanceDetails
        [HttpGet]
        public IActionResult Index(DateOnly? date)
        {
            if (date == null) {
                date = DateOnly.FromDateTime(DateTime.Now);
            }


            // Fetch all required data first into memory
            var half1 = _context.AttendanceHead.ToList();
            var half2 = _context.AttendanceHead.ToList();
            var shift = _context.Shift.ToList();

            var attendanceDetails =  _context.AttendanceDetails
                .Include(e => e.employee) // Include related employee data
                .Where(e => e.Date == date) // You can filter by date if needed
                .ToList(); // Now it's an in-memory collection

            // Perform the joins on the in-memory collections
            var result = attendanceDetails
                .Join(half1,
                    att => att.Half1,
                    head => head.ID,
                    (att, head) => new
                    {
                        att,
                        head1Name = head.HeadType,
                    })
                .Join(half2,
                    result => result.att.Half2,
                    head => head.ID,
                    (result, head) => new
                    {
                        result.att,
                        result.head1Name,
                        head2Name = head.HeadType
                    })
                .GroupJoin(shift, // This creates a left join for shift
                    result => result.att.ShiftID,
                    s => s.ShiftID,
                    (result, shifts) => new
                    {
                        result.att,
                        result.head1Name,
                        result.head2Name,
                        shifts = shifts.DefaultIfEmpty() // If no shift is found, use DefaultIfEmpty to return null
                    })
                .SelectMany(
                    result => result.shifts,
                    (result, s) => new AttendanceDetailsViewModel
                    {
                        AttendanceID = result.att.AttendanceID,
                        EmployeeID = result.att.EmployeeID,
                        Date = result.att.Date,
                        Head1Name = result.head1Name,
                        Head2Name = result.head2Name,
                        ShiftName = s != null ? s.ShiftNo : "No Shift", // If no shift is found, assign a default value
                        Name = result.att.employee.Name,
                        InTime = result.att.InTime,
                        OutTime = result.att.OutTime
                    })
                .ToList(); // Convert to list

            ViewData["Date"] = new DateTime(date.Value.Year,date.Value.Month,date.Value.Day).ToString("yyyy-MM-dd");
            return View(result);
        }

        // GET: HumanResource/AttendanceDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceDetails = await _context.AttendanceDetails
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendanceDetails == null)
            {
                return NotFound();
            }

            return View(attendanceDetails);
        }

        // GET: HumanResource/AttendanceDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/AttendanceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceID,Overtime,InTime,OutTime,Reason,Date,HeadID,EmployeeID,ShiftID,Half1,Half2,Status")] AttendanceDetails attendanceDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendanceDetails);
        }

        // GET: HumanResource/AttendanceDetails/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceDetails = await _context.AttendanceDetails
               .Include(e => e.employee)
               .Where(st => st.AttendanceID == id)
               .FirstOrDefaultAsync();

            ViewData["Half1"] = new SelectList(_context.AttendanceHead.ToList(), "ID", "HeadType",attendanceDetails.Half1);
            ViewData["Half2"] = new SelectList(_context.AttendanceHead.ToList(), "ID", "HeadType", attendanceDetails.Half2);
            ViewData["Shift"] = new SelectList(_context.Shift.ToList(), "ShiftID", "ShiftNo", attendanceDetails.ShiftID);

            if (attendanceDetails == null)
            {
                return NotFound();
            }
            return View(attendanceDetails);
        }

        // POST: HumanResource/AttendanceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  AttendanceDetails attendanceDetails)
        {
            if (id != attendanceDetails.AttendanceID)
            {
                return NotFound();
            }

            DateOnly date = DateOnly.FromDateTime(DateTime.Now);

            ViewData["Half1"] = new SelectList(_context.AttendanceHead.ToList(), "ID", "HeadType", attendanceDetails.Half1);
            ViewData["Half2"] = new SelectList(_context.AttendanceHead.ToList(), "ID", "HeadType", attendanceDetails.Half2);
            ViewData["Shift"] = new SelectList(_context.Shift.ToList(), "ShiftID", "ShiftNo", attendanceDetails.ShiftID);

            ModelState.Remove("Head");
            ModelState.Remove("employee");

            bool model_error = false;

            if (ModelState.IsValid)
            {
                if (attendanceDetails.Date > date)
                {
                    ModelState.AddModelError("Date", "Future date not allowed.");
                    model_error = true;
                }
                if (attendanceDetails.InTime  > date.ToDateTime(TimeOnly.MinValue))
                {
                    ModelState.AddModelError("InTime", "Future date not allowed.");
                    model_error = true;
                }
                if (attendanceDetails.OutTime < date.ToDateTime(TimeOnly.MinValue))
                {
                    ModelState.AddModelError("OutTime", "History date not allowed.");
                    model_error = true;
                }

                if (model_error)
                {
                    return View(attendanceDetails);
                }
                try
                {
                    attendanceDetails.Status = "";
                    if (attendanceDetails.Reason == null)
                    {
                        attendanceDetails.Reason = "";
                    }
                    _context.Update(attendanceDetails);
                    await _context.SaveChangesAsync();

                    if (attendanceDetails.AttendanceID > 0)
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                    }
                    else
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceDetailsExists(attendanceDetails.AttendanceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = attendanceDetails.AttendanceID });
            }
            return View(attendanceDetails);
        }

        // GET: HumanResource/AttendanceDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceDetails = await _context.AttendanceDetails
                .FirstOrDefaultAsync(m => m.AttendanceID == id);
            if (attendanceDetails == null)
            {
                return NotFound();
            }

            return View(attendanceDetails);
        }

        // POST: HumanResource/AttendanceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendanceDetails = await _context.AttendanceDetails.FindAsync(id);
            if (attendanceDetails != null)
            {
                _context.AttendanceDetails.Remove(attendanceDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceDetailsExists(int id)
        {
            return _context.AttendanceDetails.Any(e => e.AttendanceID == id);
        }

        public async Task<ContentResult> AddAttendance(DateTime date) {

            DateTime current_date = DateTime.Now;

            if ( date > current_date )
            {
                string failure = CommonServices.ShowAlert(Alerts.Danger, "Future date not allowed.");
                return Content(failure, "text/plain");
            }else if (date == DateTime.MinValue )
            {
                string failure = CommonServices.ShowAlert(Alerts.Danger, "Future date not allowed.");
                return Content(failure, "text/plain");
            }

            var employee = await _context.Employee.OfType<EmployementDetails>()
                .Where(e => e.resigned == 0 || e.resigned == null || e.date >= date || e.date == null)
                .Select(e => new
                {
                    EmployeeID = e.EmployeeID,
                    weekly_off = e.WeeklyHoliday
                }).ToListAsync();            
            
            foreach ( var emp in employee ) {
                try
                {
                    DateTime holiday_date = new DateTime(date.Year, date.Month, date.Day);
                var holiday = await _context.Holidays.Where(h => h.sDate.Date == holiday_date.Date).FirstOrDefaultAsync();
                bool is_Holiday = false;
                int head_id_half1 = 0;
                int head_id_half2 = 0;
                string week_day = date.DayOfWeek.ToString();

                if (holiday != null)
                {
                    var atthead = await _context.AttendanceHead.Where(h => h.IsHoliday == true).FirstOrDefaultAsync();
                    if (atthead != null)
                    {
                        head_id_half1 = atthead.ID;
                        head_id_half2 = atthead.ID;

                    }
                }else if (emp.weekly_off == week_day)
                {
                    var atthead = await _context.AttendanceHead.AsNoTracking().Where(a => a.IsHoliday == true).FirstOrDefaultAsync();
                    head_id_half1 = atthead.ID;
                    head_id_half2 = atthead.ID;
                }
                else
                {
                    var atthead = await _context.AttendanceHead.AsNoTracking().Where(h => h.IsDefault == true).FirstOrDefaultAsync();
                    head_id_half1 = atthead.ID;
                    head_id_half2 = atthead.ID;

                }

                
                var att = new AttendanceDetails();
                att.EmployeeID = emp.EmployeeID;
                att.Date = DateOnly.FromDateTime (date);
                att.InTime = date;
                att.OutTime = date;
                att.ShiftID = 0;
                att.Half1 = head_id_half1;
                att.Half2 = head_id_half2;
                att.Reason = "";
                    att.Status = "";
                TimeSpan timeDifference = att.OutTime - att.InTime;
                att.Overtime = (decimal)timeDifference.TotalMinutes;

                
                    _context.AttendanceDetails.Add(att);
                    _context.SaveChanges();
                    
                }
                catch(Exception ex)
                {
                    string failure = CommonServices.ShowAlert(Alerts.Danger, "Error.");
                    return Content(failure, "text/plain");
                }
            }

            string success = CommonServices.ShowAlert(Alerts.Success, "Attendance updated.");
            return Content(success, "text/plain");
        } 
    }
}
