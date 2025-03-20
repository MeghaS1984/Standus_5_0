using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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
            var applicationDbContext = _context.Employee
                .Include(e => e.Position)
                .Include(e => e.Department);
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
            PopulateSelect();
            return View();
        }

        public void PopulateSelect(int PostionID = 0,int DepartmentID = 0) {

            var pos = new List<SelectListItem>
            {
                new SelectListItem{Text = "Select Position" , Value = "0" }
            };

            pos.AddRange(_context.Position.Select(p => new SelectListItem
            {
                Text = p.PositionName,
                Value = p.PositionID.ToString()
            }).OrderBy(d => d.Text) );


            ViewData["PositionID"] = new SelectList(pos, "Value", "Text",PostionID);

            var dept = new List<SelectListItem> {
                new SelectListItem { Text = "Select Department" , Value = "0" }
            };

            dept.AddRange(_context.Department.Select(d => new SelectListItem
            {
                Text = d.DepartmentName,
                Value = d.DepartmentID.ToString()
            }).OrderBy(d => d.Text));

            //var dept = _context.Department.OrderBy(d => d.DepartmentName);              
            ViewData["DepartmentID"] = new SelectList(dept, "Value", "Text", DepartmentID);
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
            PopulateSelect(employee.PositionID, employee.DepartmentID);
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
                
            }
            PopulateSelect(employee.PositionID,employee.DepartmentID );
            return RedirectToAction(nameof(Index),new { id = employee.EmployeeID});
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
        public async Task<IActionResult> Allowance(int employeeid, int categoryid)
        {

            

            var allowance = from A in _context.Allowance
                         join B in _context.Slab on A.ID equals B.AllowanceID
                         join C in _context.SlabCategory on B.SlabID equals C.SlabID
                         join D in _context.SlabCalculation on B.SlabID equals D.SlabID
                         join E in _context.SlabDetails  on D.DetailsID equals E.ID 
                         where C.CategoryID == (categoryid == null ? 0 : Convert.ToInt32(categoryid))
                         && B.SlabID == E.SlabID
                         orderby A.PayrollSlNO
                            select new AllowanceDetails
                            {
                                ID = A.ID,
                                Name = A.Name,
                                Amount = 0,
                                Fixed = A.Fixed,
                                FromAmount = 0,
                                ToAmount = 0,
                                Employee = 0,
                                Employer = 0,
                                DetailsID = E.ID
                            };


            var slab = from A in _context.SlabAllowance
                         join B in _context.Allowance on A.AllowanceID equals B.ID
                         where A.EmployeeID == (employeeid == null ? 0 : Convert.ToInt32(employeeid))
                         orderby B.PayrollSlNO
                         select new
                         {
                             A.AllowanceID,
                             B.Name,
                             Amount = 0,
                             B.Fixed,
                             A.FromAmount,
                             A.ToAmount,
                             A.Employee,
                             A.Employer,
                             A.Type
                         };

            // Merging the two lists based on DeductionID
            var allowance_set = allowance.ToList().Join(slab,
                                d => d.ID,
                                s => s.AllowanceID,
                                (d, s) =>
                                {
                                    d.Amount = s.Amount;
                                    d.Fixed = s.Fixed;
                                    d.FromAmount = s.FromAmount;
                                    d.ToAmount = s.ToAmount;
                                    d.Employee = s.Employee;
                                    d.Employer = s.Employer;
                                    return d;
                                }).ToList();

            return View(allowance_set);
        }
        public async Task<IActionResult> Deduction(int employeeid, int categoryid)
        {

            var deduction = from A in _context.Deduction
                         join B in _context.Slab on A.ID equals B.DeductionID
                         join C in _context.SlabCategory on B.SlabID equals C.SlabID
                            join D in _context.SlabCalculation on B.SlabID equals D.SlabID
                            join E in _context.SlabDetails on D.DetailsID equals E.ID
                            where C.CategoryID == (categoryid == null ? 0 : Convert.ToInt32(categoryid))
                            && B.SlabID == E.SlabID
                            orderby A.PayRollSlNo
                            select new DeductionDetails
                            {
                                ID = A.ID,
                                Name = A.Name,
                                Amount = 0,
                                Fixed = A.Fixed,
                                FromAmount = 0,
                                ToAmount = 0,
                                Employee = 0,
                                Employer = 0,
                                DetailsID = E.ID
                            };

            var slab = from A in _context.SlabDeduction
                            join B in _context.Deduction on A.DeductionID equals B.ID
                            where A.EmployeeID == (employeeid == null ? 0 : Convert.ToInt32(employeeid))
                            orderby B.PayRollSlNo
                            select new
                            {
                                A.DeductionID,
                                B.Name,
                                Amount = 0,
                                B.Fixed,
                                A.FromAmount,
                                A.ToAmount,
                                A.Employee,
                                A.Employer,
                                A.Type,
                                A.DetailsID
                            };

            // Merging the two lists based on AllowanceID
            var deduction_set = deduction.ToList().Join(slab,
                                d => d.ID,
                                s => s.DeductionID,
                                (d, s) =>
                                {
                                    d.Amount = s.Amount;
                                    d.Fixed = s.Fixed;
                                    d.FromAmount = s.FromAmount;
                                    d.ToAmount = s.ToAmount;
                                    d.Employee = s.Employee;
                                    d.Employer = s.Employer;
                                    return d;
                                }).ToList();

            return View(deduction_set);
        }
    }
}
