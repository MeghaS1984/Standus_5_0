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
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/LeaveApplications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LeaveApplication.Include(l => l.Employee).Include(l => l.Head);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> LeaveApproved()
        {
            var applicationDbContext = _context.LeaveApplication.Include(l => l.Employee).Include(l => l.Head)
                .Where(f=> f.status == "Approved");
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> LeaveRequest()
        {
            var applicationDbContext = _context.LeaveApplication.Include(l => l.Employee).Include(l => l.Head)
                .Where(f => f.status == "Pending For Approval");
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> LeaveRejected()
        {
            var applicationDbContext = _context.LeaveApplication.Include(l => l.Employee).Include(l => l.Head)
                .Where(f => f.status == "reject");
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: HumanResource/LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplication
                .Include(l => l.Employee)
                .Include(l => l.Head)
                .FirstOrDefaultAsync(m => m.id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: HumanResource/LeaveApplications/Create
        public IActionResult Create()
        {
            var employees = _context.Employee.OfType<Resigned>().Select(e => new {
                id = e.EmployeeID,
                name = e.Name,
                resigned = e.resigned
            }).Where(f=> f.resigned == 0);

            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name");
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType");
            return View();
        }

        // POST: HumanResource/LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,EmployeeID,Date,Days,Description,StartDate,EndDate,HeadID")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var employees = _context.Employee.Select(e => new { 
                id = e.EmployeeID ,
                name = e.Name
            });

            ViewData["EmployeeID"] = new SelectList(employees, "id", "name");
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType", leaveApplication.HeadID);
            return View(leaveApplication);
        }

        // GET: HumanResource/LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplication.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", leaveApplication.EmployeeID);
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType", leaveApplication.HeadID);
            return View(leaveApplication);
        }

        // POST: HumanResource/LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,EmployeeID,Date,Days,Description,StartDate,EndDate,HeadID")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.id))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", leaveApplication.EmployeeID);
            ViewData["HeadID"] = new SelectList(_context.AttendanceHead, "ID", "HeadType", leaveApplication.HeadID);
            return View(leaveApplication);
        }

        // GET: HumanResource/LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplication
                .Include(l => l.Employee)
                .Include(l => l.Head)
                .FirstOrDefaultAsync(m => m.id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: HumanResource/LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplication.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplication.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplication.Any(e => e.id == id);
        }
    }
}
