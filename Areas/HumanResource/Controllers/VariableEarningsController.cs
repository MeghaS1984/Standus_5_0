using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class VariableEarningsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariableEarningsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/VariableEarnings
        public async Task<IActionResult> Index(int payid,int allowanceid, int categoryid)
        {
			var applicationDbContext =from emp in _context.Employee
	                let matchedPayroll = (from pd in _context.PayrollDetails
						                  where pd.EmployeeID == emp.EmployeeID
								                && pd.PayID == payid
								                && pd.AllowanceID == allowanceid
								                && pd.CategoryID == categoryid
						                  select pd).FirstOrDefault()
	                select new VariableAllowanceModel
	                {
		                employeeid = emp.EmployeeID,
		                employeename = emp.Name,
		                allowanceid = matchedPayroll != null ? (int)matchedPayroll.AllowanceID : 0,
		                amount = matchedPayroll != null && matchedPayroll.Employer != 0
					                ? (double)matchedPayroll.Employer : 0,
		                categoryid = matchedPayroll != null ? (int)matchedPayroll.CategoryID : 0,
		                payid = matchedPayroll != null ? (int)matchedPayroll.PayID : 0
	                };





			//var paydetails = applicationDbContext.ToList();
			//.Where (a => a.payid == payid && a.allowanceid == allowanceid && a.categoryid == categoryid);

			ViewBag.Allowance = new SelectList(_context.Allowance.Where(a => a.Fixed == false)
                .OrderBy(o => o.Name),"ID","Name",allowanceid);

            ViewBag.Category = new SelectList(_context.Category.OrderBy(c => c.CategoryName),"ID","CategoryName",categoryid);            

            ViewBag.Payroll = new SelectList( _context.Payroll.OrderBy(p => p.ForTheMonth) ,"PayID","Date",payid);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/VariableEarnings/Details/5
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

        // GET: HumanResource/VariableEarnings/Create
        public IActionResult Create()
        {
            ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID");
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName");
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator");
            return View();
        }

        // POST: HumanResource/VariableEarnings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public  ActionResult Create([FromBody] PayrollData data)
        {
            //int id = data.payid;

            //var payDetails = _context.PayrollDetails.Where(pd => pd.PayID == id);

            //_context.PayrollDetails.RemoveRange(payDetails);
            //_context.SaveChanges();

            var payrollDetails = new PayrollDetails();

            payrollDetails.PayID = data.payid;
            payrollDetails.EmployeeID = data.employeeid;
            payrollDetails.AllowanceID = data.allowanceid;
            payrollDetails.DeductionID = 0;
            payrollDetails.Employer = (decimal)data.employer ;
            payrollDetails.CategoryID = data.categoryid;
            payrollDetails.Type = "";

            _context.PayrollDetails.Add(payrollDetails);
            _context.SaveChanges();                
            
            ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID", payrollDetails.AllowanceID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", payrollDetails.CategoryID);
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", payrollDetails.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", payrollDetails.EmployeeID);
			
			return new EmptyResult ();
		}

        // GET: HumanResource/VariableEarnings/Edit/5
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

        // POST: HumanResource/VariableEarnings/Edit/5
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

        // GET: HumanResource/VariableEarnings/Delete/5
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

        // POST: HumanResource/VariableEarnings/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id,int categoryid,int allowanceid)
        {
			var payDetails = _context.PayrollDetails
                .Where(pd => pd.PayID == id && pd.CategoryID == categoryid && pd.AllowanceID == allowanceid);

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

     public class PayrollData { 

        public int payid { get; set; }
        public int employeeid { get; set; }
        public int allowanceid { get; set; }
        public double employer { get; set; }
        public int categoryid { get; set; }

        public int deductionid { get; set; }    
    } 
}
