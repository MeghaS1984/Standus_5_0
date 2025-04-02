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
    public class StandardDeductionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StandardDeductionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/StandardDeductions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StandardDeduction.Include(s => s.DeductionDetails).Include(s => s.EmployeeDetails);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/StandardDeductions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standardDeduction = await _context.StandardDeduction
                .Include(s => s.DeductionDetails)
                .Include(s => s.EmployeeDetails)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (standardDeduction == null)
            {
                return NotFound();
            }

            return View(standardDeduction);
        }

        // GET: HumanResource/StandardDeductions/Create
        public IActionResult Create()
        {
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name");
            return View();
        }

        // POST: HumanResource/StandardDeductions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Type,Employee,Employer,DeductionID")] StandardDeduction standardDeduction)
        {
            ModelState.Remove("EmployeeDetails");
            ModelState.Remove("DeductionDetails");
            if (ModelState.IsValid)
            {
                _context.Add(standardDeduction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "CutOffType", standardDeduction.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", standardDeduction.EmployeeID);
            return View(standardDeduction);
        }

        [HttpPost]
        public ActionResult Create([FromBody] StandardDeductionData data)
        {
            //Handle the parent - child data
            //The formData.Entries contains the addresses as an array of objects
            var std = new StandardDeduction ();
            std.EmployeeID = data.employeeid;
            std.Type = data.type;
            std.Employee = data.employee;
            std.Employer = data.employer;
            std.DeductionID = data.deductionid;

            _context.StandardDeduction.Add(std);
            _context.SaveChanges();

            foreach (var allw in data.allowances)
            {
                var std_Allowance = new StandardDeductionCalculation();
                std_Allowance.DeductionID = data.deductionid;
                std_Allowance.AllowanceID = allw.allowanceid;

                _context.StandardDeductionCalculation.Add(std_Allowance);
                _context.SaveChanges();
            }

            return Json(new { success = true, message = "Data submitted successfully!" });
        }

        // GET: HumanResource/StandardDeductions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standardDeduction = await _context.StandardDeduction.FindAsync(id);
            if (standardDeduction == null)
            {
                return NotFound();
            }
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "CutOffType", standardDeduction.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", standardDeduction.EmployeeID);
            return View(standardDeduction);
        }

        // POST: HumanResource/StandardDeductions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Type,Employee,Employer,DeductionID")] StandardDeduction standardDeduction)
        {
            if (id != standardDeduction.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(standardDeduction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StandardDeductionExists(standardDeduction.EmployeeID))
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
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "CutOffType", standardDeduction.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", standardDeduction.EmployeeID);
            return View(standardDeduction);
        }

        // GET: HumanResource/StandardDeductions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var standardDeduction = await _context.StandardDeduction
                .Include(s => s.DeductionDetails)
                .Include(s => s.EmployeeDetails)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (standardDeduction == null)
            {
                return NotFound();
            }

            return View(standardDeduction);
        }

        // POST: HumanResource/StandardDeductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var standardDeduction = await _context.StandardDeduction.FindAsync(id);
            if (standardDeduction != null)
            {
                _context.StandardDeduction.Remove(standardDeduction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StandardDeductionExists(int id)
        {
            return _context.StandardDeduction.Any(e => e.EmployeeID == id);
        }
    }

    public class StandardDeductionData { 
        public int employeeid {  get; set; }
        public string type { get; set; }
        public double employee {  get; set; }
        public double employer {  get; set; }
        public int deductionid { get; set; }

        public List<DeductionOn> allowances { get; set; }
    }

    public class DeductionOn
    {
        public int allowanceid { get; set; }
    }
}
