using System;
using System.Collections.Generic;
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
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employee.Include(e => e.Position);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: HumanResource/Employees/Create
        public IActionResult Create()
        {
            ViewData["PositionID"] = new SelectList(_context.Set<Position>(), "PositionID", "PositionName");
            return View();
        }

        // POST: HumanResource/Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Name,Email,Phone,Address,DateOfBirth,HireDate,Salary,DepartmentID,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                //employee.Name = employee.FirstName + " " + employee.LastName;
                _context.Add(employee);
                await _context.SaveChangesAsync();

                if (employee.EmployeeID > 0)
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Employee added");
                }
                else
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Unknown error");
                }

                //return RedirectToAction(nameof(Index));
                return View(employee);
            }
            else {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Unknown error");
            }
            ViewData["PositionID"] = new SelectList(_context.Set<Position>(), "PositionID", "PositionName", employee.PositionID);
            return View(employee);
        }

        // GET: HumanResource/Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = id;
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["PositionID"] = new SelectList(_context.Set<Position>(), "PositionID", "PositionName", employee.PositionID);
            return View(employee);
        }

        // POST: HumanResource/Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Email,Phone,Address,DateOfBirth,HireDate,Salary,DepartmentID,PositionID")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
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
            ViewData["PositionID"] = new SelectList(_context.Set<Position>(), "PositionID", "PositionName", employee.PositionID);
            return View(employee);
        }

        // GET: HumanResource/Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: HumanResource/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeID == id);
        }
    }
}
