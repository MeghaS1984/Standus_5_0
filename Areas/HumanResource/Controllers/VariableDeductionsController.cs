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
    public class VariableDeductionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariableDeductionsController(ApplicationDbContext context)
        {
            _context = context;
        }

		// GET: HumanResource/VariableDeductions
		public async Task<IActionResult> Index(int payid, int deductionid, int categoryid)
		{
			var applicationDbContext = from emp in _context.Employee
									   let matchedPayroll = (from pd in _context.PayrollDetails
															 where pd.EmployeeID == emp.EmployeeID
																   && pd.PayID == payid
																   && pd.DeductionID == deductionid
																   && pd.CategoryID == categoryid
															 select pd).FirstOrDefault()
									   select new VariableDeductionModel
									   {
										   employeeid = emp.EmployeeID,
										   employeename = emp.Name,
										   deductionid = matchedPayroll != null ? (int)matchedPayroll.DeductionID : 0,
										   amount = matchedPayroll != null && matchedPayroll.Employer != 0
													   ? (double)matchedPayroll.Employer : 0,
										   categoryid = matchedPayroll != null ? (int)matchedPayroll.CategoryID : 0,
										   payid = matchedPayroll != null ? (int)matchedPayroll.PayID : 0
									   };

			//var paydetails = applicationDbContext.ToList();
			//.Where (a => a.payid == payid && a.allowanceid == allowanceid && a.categoryid == categoryid);

			ViewBag.Deduction = new SelectList(_context.Deduction.Where(a => a.Fixed == false)
				.OrderBy(o => o.Name), "ID", "Name", deductionid);

			ViewBag.Category = new SelectList(_context.Category.OrderBy(c => c.CategoryName), "ID", "CategoryName", categoryid);

			ViewBag.Payroll = new SelectList(_context.Payroll.OrderBy(p => p.ForTheMonth), "PayID", "Date", payid);

			return View(await applicationDbContext.ToListAsync());
		}

		// GET: HumanResource/VariableDeductions/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollDetails = await _context.PayrollDetails
                .Include(p => p.Allowance)
                .Include(p => p.Category)
                .Include(p => p.Deduction)
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payrollDetails == null)
            {
                return NotFound();
            }

            return View(payrollDetails);
        }

        // GET: HumanResource/VariableDeductions/Create
        public IActionResult Create()
        {
            ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID");
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName");
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator");
            return View();
        }

		// POST: HumanResource/VariableDeductions/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public ActionResult Create([FromBody] PayrollData data)
		{
			//int id = data.payid;

			//var payDetails = _context.PayrollDetails.Where(pd => pd.PayID == id);

			//_context.PayrollDetails.RemoveRange(payDetails);
			//_context.SaveChanges();

			var payrollDetails = new PayrollDetails();

			payrollDetails.PayID = data.payid;
			payrollDetails.EmployeeID = data.employeeid;
			payrollDetails.AllowanceID = 0;
			payrollDetails.DeductionID = data.deductionid;
			payrollDetails.Employer = (decimal)data.employer;
			payrollDetails.CategoryID = data.categoryid;
			payrollDetails.Type = "";

			_context.PayrollDetails.Add(payrollDetails);
			_context.SaveChanges();

			ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID", payrollDetails.AllowanceID);
			ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", payrollDetails.CategoryID);
			ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", payrollDetails.DeductionID);
			ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", payrollDetails.EmployeeID);

			return new EmptyResult();
		}

		// GET: HumanResource/VariableDeductions/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollDetails = await _context.PayrollDetails.FindAsync(id);
            if (payrollDetails == null)
            {
                return NotFound();
            }
            ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID", payrollDetails.AllowanceID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", payrollDetails.CategoryID);
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", payrollDetails.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", payrollDetails.EmployeeID);
            return View(payrollDetails);
        }

        // POST: HumanResource/VariableDeductions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayID,EmployeeID,EmployeeAmount,AllowanceID,DeductionID,Employer,CategoryID,Paid,UnPaid,FromAmount,ToAmount,Fixed,Amount,EmployeeContribution,EmployerContribution,Type,ID")] PayrollDetails payrollDetails)
        {
            if (id != payrollDetails.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payrollDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollDetailsExists(payrollDetails.ID))
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
            ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID", payrollDetails.AllowanceID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", payrollDetails.CategoryID);
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", payrollDetails.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", payrollDetails.EmployeeID);
            return View(payrollDetails);
        }

        // GET: HumanResource/VariableDeductions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payrollDetails = await _context.PayrollDetails
                .Include(p => p.Allowance)
                .Include(p => p.Category)
                .Include(p => p.Deduction)
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payrollDetails == null)
            {
                return NotFound();
            }

            return View(payrollDetails);
        }

		// POST: HumanResource/VariableDeductions/Delete/5
		[HttpPost]
		public ActionResult DeleteConfirmed(int id, int categoryid, int deductionid)
		{
			var payDetails = _context.PayrollDetails
				.Where(pd => pd.PayID == id && pd.CategoryID == categoryid && pd.DeductionID == deductionid);

			if (payDetails != null)
			{
				_context.PayrollDetails.RemoveRange(payDetails);
			}

			_context.SaveChanges();
			return new EmptyResult();
		}

		private bool PayrollDetailsExists(int id)
        {
            return _context.PayrollDetails.Any(e => e.ID == id);
        }
    }
}
