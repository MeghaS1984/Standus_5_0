using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    public class BankDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BankDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/BankDetails
        [HttpGet]
        public async Task<IActionResult> Index(int? id) {

            _context.Database.ExecuteSqlRaw("update employee set Discriminator = 'BankDetails' where employeeid=" + id);

            var applicationDbContext = _context.Employee.OfType<BankDetails>()
            .Where(e => e.EmployeeID == id);

            return PartialView("Index" , await applicationDbContext.ToListAsync());
            
        }

        // GET: HumanResource/BankDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankDetails = await _context.Employee.OfType<BankDetails>()
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (bankDetails == null)
            {
                return NotFound();
            }
            return View(bankDetails);
        }

        // GET: HumanResource/BankDetails/Create
        public IActionResult Create(int id)
        {
            ViewData["EmployeeId"] = id;
            return View();
        }

        // POST: HumanResource/BankDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,BankName,AccountNo,IFSCCode")] BankDetails bankDetails)
        {
            _context.Database.ExecuteSqlRaw("update employee set Discriminator = 'BankDetails' where employeeid=" + bankDetails.EmployeeID);
            var bank_Details = await _context.Employee.OfType<BankDetails>().FirstOrDefaultAsync(m => m.EmployeeID == bankDetails.EmployeeID);
            
            ////_context.Database.ExecuteSqlRaw("update employee set Discriminator = 'EmployementDetails' where employeeid=" + bankDetails.EmployeeID);
            //employee.Discriminator = "BankDetails";

            bank_Details.BankName = bankDetails.BankName;
            bank_Details.AccountNo = bankDetails.AccountNo;
            bank_Details.IFSCCode = bankDetails.IFSCCode;
                
            _context.Employee.Update(bank_Details);
            await _context.SaveChangesAsync();
           
            return View(bank_Details);
        }
        //public async Task<IActionResult> Create([Bind("EmployeeID,BankName,AccountNo,IFSCCode,Discriminator")] BankDetails bankDetails)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(bankDetails);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionName", bankDetails.PositionID);
        //    return View(bankDetails);
        //}

        // GET: HumanResource/BankDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankDetails = _context.Employee.OfType<BankDetails>().FirstOrDefault(m => m.EmployeeID == id);
            if (bankDetails == null)
            {
                return View();
            }
            return View(bankDetails);
        }

        // POST: HumanResource/BankDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,BankName,AccountNo,IFSCCode")] BankDetails bankDetails)
        {
            //if (id != bankDetails.EmployeeID)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
                try
                {
                    var existing_Employee = await _context.Employee.OfType<BankDetails>().FirstOrDefaultAsync(m => m.EmployeeID == id);
                if (existing_Employee != null)
                    {
                        existing_Employee.BankName = bankDetails.BankName;
                        existing_Employee.AccountNo = bankDetails.AccountNo;
                        existing_Employee.IFSCCode = bankDetails.IFSCCode;
                        _context.Update(existing_Employee);
                        await _context.SaveChangesAsync();
                    }
                    else {
                        //var new_BankDetails = new BankDetails();
                        //new_BankDetails.BankName = bankDetails.BankName;
                        //new_BankDetails.AccountNo = bankDetails.AccountNo;
                        //new_BankDetails.IFSCCode = bankDetails.IFSCCode;
                        //_context.Employee.OfType<BankDetails>().ExecuteUpdate<>
                        await _context.SaveChangesAsync();
                        //ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionName", bankDetails.PositionID);
                        return View(bankDetails);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankDetailsExists(bankDetails.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
            }
            //return RedirectToAction(nameof(Index));
            //}
            ViewData["PositionID"] = new SelectList(_context.Position, "PositionID", "PositionName", bankDetails.PositionID);
            return View(bankDetails);
        }

        // GET: HumanResource/BankDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankDetails = await _context.Employee.OfType<BankDetails>()
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (bankDetails == null)
            {
                return NotFound();
            }

            return View(bankDetails);
        }

        // POST: HumanResource/BankDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankDetails = await _context.Employee.OfType<BankDetails>().FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (bankDetails != null)
            {
                _context.Remove(bankDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankDetailsExists(int id)
        {
            return _context.Employee.OfType<BankDetails>().Any(m => m.EmployeeID == id);
        }
    }
}
