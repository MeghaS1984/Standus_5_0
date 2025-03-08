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
    public class EmployementDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployementDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/EmployementDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employee.OfType<EmployementDetails>();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/EmployementDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employementDetails = await _context.Employee.OfType<BankDetails>().FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employementDetails == null)
            {
                return NotFound();
            }

            return View(employementDetails);
        }

        // GET: HumanResource/EmployementDetails/Create
        public IActionResult Create()
        {
            ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionName");
            return View();
        }

        // POST: HumanResource/EmployementDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Gender,MaritalStatus,PermanantAddress,Pincode,WeeklyHoliday,GradeID,ShiftType,WagesType,PaymentType,EmployeeID,Name,Email,Phone,Address,DateOfBirth,HireDate,Salary,DepartmentID,PositionID")] EmployementDetails employementDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employementDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionName", employementDetails.PositionID);
            return View(employementDetails);
        }

        // GET: HumanResource/EmployementDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = id;
            _context.Database.ExecuteSqlRaw("update employee set Discriminator = 'EmployementDetails' where employeeid=" + id);

            var employementDetails = await _context.Employee.OfType<EmployementDetails>().FirstOrDefaultAsync(m => m.EmployeeID == id);
            
            string paymentType="";
            string weeklyHoliday = "";
            string shiftType = "";
            string wagesType = "";
            string gender = "";
            string maritalStatus = "";

            if (employementDetails == null)
            {
                paymentType = "Monthly";
                weeklyHoliday = "Sunday";
                shiftType = "General";
                wagesType = "Monthly";
                gender = "Male";
                maritalStatus = "Unmarried";
            }
            else {
                paymentType = employementDetails.PaymentType;
                weeklyHoliday = employementDetails.WeeklyHoliday;
                shiftType = employementDetails.ShiftType;
                wagesType = employementDetails.WagesType;
                gender = employementDetails.Gender;
                maritalStatus = employementDetails.MaritalStatus;
            }
            ViewData["PaymentType"] = new SelectList(new List<string> { "Monthly", "Daily", "Weekly" },paymentType);
            ViewData["WagesType"] = new SelectList(new List<string> { "Monthly", "Daily", "Weekly" }, wagesType);
            ViewData["weeklyHoliday"] = new SelectList(new List<string> { "Monday","Tuesday","Wednesday",
                "Thursday", "Friday","Saturday","Sunday" }, weeklyHoliday);
            ViewData["ShiftType"] = new SelectList(new List<string> { "general Shift", "Rotation" }, shiftType);
            ViewData["Gender"] = new SelectList(new List<string> { "Male", "Female" }, gender);
            ViewData["MaritalStatus"] = new SelectList(new List<string> { "Married", "Unmarried" }, maritalStatus);
            if (employementDetails == null)
            {
                return NotFound();
            }
            //ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionName", employementDetails.PositionID);
            return View(employementDetails);
        }

        // POST: HumanResource/EmployementDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Code,Gender,MaritalStatus,PermanantAddress,Pincode1,WeeklyHoliday,GradeID,ShiftType,WagesType,PaymentType")] EmployementDetails employementDetails)
        {
            //if (id != employementDetails.EmployeeID)
            //{
            //    return NotFound();
            //}
            var existing_Employee = await _context.Employee.OfType<EmployementDetails>().FirstOrDefaultAsync(m => m.EmployeeID == id);
            //employementDetails.
            //if (ModelState.IsValid)
            //{
            try
                {
                    
                    if (existing_Employee != null)
                    {
                        existing_Employee.Code = employementDetails.Code;
                        existing_Employee.Gender = employementDetails.Gender;
                        existing_Employee.PermanantAddress= employementDetails.PermanantAddress;
                        existing_Employee.MaritalStatus = employementDetails.MaritalStatus;
                        existing_Employee.Pincode1 = employementDetails.Pincode1;
                        existing_Employee.WeeklyHoliday = employementDetails.WeeklyHoliday;
                        existing_Employee.GradeID = employementDetails.GradeID;
                        existing_Employee.ShiftType = employementDetails.ShiftType;
                        existing_Employee.WagesType = employementDetails.WagesType;
                        existing_Employee.PaymentType = employementDetails.PaymentType;
                        _context.Employee.Update(existing_Employee);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                    var _Employee = _context.Employee.Find(id);
                    //var new_Details = new EmployementDetails();
                    employementDetails.Name = _Employee.Name;
                    employementDetails.Address = _Employee.Address;
                    employementDetails.Email = _Employee.Email;
                    employementDetails.DateOfBirth = _Employee.DateOfBirth;
                    employementDetails.HireDate = _Employee.HireDate;
                    employementDetails.Phone = _Employee.Phone;
                    employementDetails.Pincode1 = "";
                    //new_Details.Code = employementDetails.Code;
                    //new_Details.Gender = employementDetails.Gender;
                    //new_Details.MaritalStatus = employementDetails.MaritalStatus;
                    //new_Details.Pincode1 = employementDetails.Pincode1;
                    //new_Details.WeeklyHoliday = employementDetails.WeeklyHoliday;
                    //new_Details.GradeID = employementDetails.GradeID;
                    //new_Details.ShiftType = employementDetails.ShiftType;
                    //new_Details.WagesType = employementDetails.WagesType;
                    //new_Details.PaymentType = employementDetails.PaymentType;
                    _context.Update(employementDetails);
                        await _context.SaveChangesAsync();
                        return View(employementDetails);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployementDetailsExists(employementDetails.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            //    return RedirectToAction(nameof(Index));
            //}
            return View(employementDetails);
        }

        // GET: HumanResource/EmployementDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employementDetails = await _context.Employee.OfType<BankDetails>().FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employementDetails == null)
            {
                return NotFound();
            }

            return View(employementDetails);
        }

        // POST: HumanResource/EmployementDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employementDetails = await _context.Employee.OfType<BankDetails>().FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employementDetails != null)
            {
                _context.Remove(employementDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployementDetailsExists(int id)
        {
            return _context.Employee.OfType<BankDetails>().Any(m => m.EmployeeID == id);
        }
    }
}
