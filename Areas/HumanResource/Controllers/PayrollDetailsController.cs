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
    public class PayrollDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PayrollDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/PayrollDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PayrollDetails.Include(p => p.Allowance).Include(p => p.Category).Include(p => p.Deduction).Include(p => p.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/PayrollDetails/Details/5
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

        // GET: HumanResource/PayrollDetails/Create
        public IActionResult Create()
        {
            ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID");
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName");
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator");
            return View();
        }

        // POST: HumanResource/PayrollDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PayID,EmployeeID,EmployeeAmount,AllowanceID,DeductionID,Employer,CategoryID,Paid,UnPaid,FromAmount,ToAmount,Fixed,Amount,EmployeeContribution,EmployerContribution,Type,ID")] PayrollDetails payrollDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payrollDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AllowanceID"] = new SelectList(_context.Allowance, "ID", "ID", payrollDetails.AllowanceID);
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", payrollDetails.CategoryID);
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", payrollDetails.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", payrollDetails.EmployeeID);
            return View(payrollDetails);
        }

        // GET: HumanResource/PayrollDetails/Edit/5
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

        // POST: HumanResource/PayrollDetails/Edit/5
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

        // GET: HumanResource/PayrollDetails/Delete/5
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

        // POST: HumanResource/PayrollDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payrollDetails = await _context.PayrollDetails.FindAsync(id);
            if (payrollDetails != null)
            {
                _context.PayrollDetails.Remove(payrollDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayrollDetailsExists(int id)
        {
            return _context.PayrollDetails.Any(e => e.ID == id);
        }
    }
}
