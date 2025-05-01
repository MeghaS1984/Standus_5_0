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

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class StandardDeductionExcludesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StandardDeductionExcludesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/StandardDeductionExcludes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StandardDeductionExclude
                .Include(s => s.Deduction)
                .Include(s => s.Employee)
                .Select(ex => new StandardDeductionExclude
                { 
                    EmployeeID = ex.EmployeeID ,
                    DeductionID = ex.DeductionID ,
                    Exclude = (ex.Exclude == true ? true : false),
                    Include = (ex.Include == true ? true : false),
                    Employee = ex.Employee ,
                    Deduction = ex.Deduction 
                });
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/StandardDeductionExcludes/Details/5
        public async Task<IActionResult> Details(int? eid, int did)
        {
            
            var standardDeductionExclude = await _context.StandardDeductionExclude
                .Include(s => s.Deduction)
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.EmployeeID == eid && m.DeductionID ==  did);

            if (standardDeductionExclude == null)
            {
                return NotFound();
            }

            return View(standardDeductionExclude);
        }

        // GET: HumanResource/StandardDeductionExcludes/Create
        public IActionResult Create()
        {
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID","Name");
            return View();
        }

        // POST: HumanResource/StandardDeductionExcludes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,DeductionID,Exclude,Include")] StandardDeductionExclude standardDeductionExclude)
        {
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", standardDeductionExclude.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name", standardDeductionExclude.EmployeeID);

            ModelState.Remove("Employee");
            ModelState.Remove("Deduction");

            var isExists = _context.StandardDeductionExclude
                .Where(ex => ex.EmployeeID == standardDeductionExclude.EmployeeID &&
                ex.DeductionID == standardDeductionExclude.DeductionID).ToList();

            if (isExists.Count > 0) {
                ModelState.AddModelError("EmployeeID","Entry for selected employee and " +
                    "selected deduction type exists.");
                return View(standardDeductionExclude);
            }
 

            try {
                _context.Add(standardDeductionExclude);
                await _context.SaveChangesAsync();
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Data saved..");
            } catch(Exception) {
                ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Error..");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: HumanResource/StandardDeductionExcludes/Edit/5
        public async Task<IActionResult> Edit(int? eid, int did)
        {
            var standardDeductionExclude = await _context.StandardDeductionExclude
                            .Include(s => s.Deduction)
                            .Include(s => s.Employee)
                            .Where(ex => ex.EmployeeID == eid && ex.DeductionID == did)
                            .FirstOrDefaultAsync();
            
            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", did);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name", eid);

            return View(standardDeductionExclude);
        }

        // POST: HumanResource/StandardDeductionExcludes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,DeductionID,Exclude,Include")] StandardDeductionExclude standardDeductionExclude)
        {
            var excpt = _context.StandardDeductionExclude
                .Where(ex => ex.EmployeeID == standardDeductionExclude.EmployeeID
                && ex.DeductionID == standardDeductionExclude.DeductionID);

            _context.StandardDeductionExclude.RemoveRange(excpt);
            _context.SaveChanges();

                try
                {
                    _context.StandardDeductionExclude.Add(standardDeductionExclude);
                    await _context.SaveChangesAsync();

                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Success, "Data saved..");
                }
                catch (DbUpdateConcurrencyException)
                {
                    ViewBag.Alert = CommonServices.ShowAlert(Alerts.Danger, "Error..");
                if (!StandardDeductionExcludeExists(standardDeductionExclude.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            ViewData["DeductionID"] = new SelectList(_context.Deduction, "ID", "Name", standardDeductionExclude.DeductionID);
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name", standardDeductionExclude.EmployeeID);

            return RedirectToAction(nameof(Index));
        }

        // GET: HumanResource/StandardDeductionExcludes/Delete/5
        public async Task<IActionResult> Delete(int? eid, int did)
        {

            var standardDeductionExclude = await _context.StandardDeductionExclude
            .Include(s => s.Deduction)
            .Include(s => s.Employee)
            .Where(m => m.EmployeeID == eid && m.DeductionID == did)
            .Select(ex => new StandardDeductionExclude
            {
                EmployeeID = ex.EmployeeID,
                DeductionID = ex.DeductionID,
                Exclude = ex.Exclude,
                Include = ex.Include,
                Employee = ex.Employee,
                Deduction = ex.Deduction
            })
            .FirstOrDefaultAsync();

            if (standardDeductionExclude == null)
            {
                return NotFound();
            }

            return View(standardDeductionExclude);
        }

        // POST: HumanResource/StandardDeductionExcludes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int eid, int did)
        {
            var standardDeductionExclude =_context.StandardDeductionExclude
                .Where(ex => ex.EmployeeID == eid && ex.DeductionID == did);

            if (standardDeductionExclude != null)
            {
                _context.StandardDeductionExclude.RemoveRange(standardDeductionExclude);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StandardDeductionExcludeExists(int id)
        {
            return _context.StandardDeductionExclude.Any(e => e.EmployeeID == id);
        }
    }
}
