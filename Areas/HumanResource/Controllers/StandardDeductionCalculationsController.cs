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
    public class StandardDeductionCalculationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StandardDeductionCalculationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/StandardDeductionCalculations
        public async Task<IActionResult> Index(int deductionid, int allowanceid)
        {
            //var applicationDbContext = from earning in _context.Allowance
            //                           join stdcal in _context.StandardDeductionCalculation on earning.ID equals stdcal.AllowanceID
            //                           into stdcalgroup
            //                           from stdcal in stdcalgroup.DefaultIfEmpty()
            //                           where (stdcal == null || stdcal.DeductionID == deductionid)
            //                           select new
            //                           {
            //                               AllowanceID = earning.ID,
            //                               Allowance = earning.Name,
            //                               Deductionid = stdcal == null ? (int?)null : stdcal.DeductionID
            //                           };

            IQueryable<dynamic> applicationDbContext = null;

            
                applicationDbContext = from earning in _context.Allowance
                                       select new
                                       {
                                           AllowanceID = earning.ID,
                                           Allowance = earning.Name,
                                           DeductionID = 0
                                       };
            //}

            //if (allowanceid > 0)
            //{
            //    applicationDbContext = from earning in _context.Allowance
            //                               select new
            //                               {
            //                                   AllowanceID = earning.ID,
            //                                   Allowance = earning.Name,
            //                                   DeductionID = 0
            //                               };
            //}


            var result = await applicationDbContext.ToListAsync();
            return PartialView("index", result);
        }

        // GET: HumanResource/StandardDeductionCalculations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var StandardDeductionCalculation = await _context.StandardDeductionCalculation
                .FirstOrDefaultAsync(m => m.DeductionID == id);
            if (StandardDeductionCalculation == null)
            {
                return NotFound();
            }

            return View(StandardDeductionCalculation);
        }

        // GET: HumanResource/StandardDeductionCalculations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/StandardDeductionCalculations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeductionID,AllowanceID")] StandardDeductionCalculation deductionCalculation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deductionCalculation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deductionCalculation);
        }

        // GET: HumanResource/StandardDeductionCalculations/Edit/5
        public async Task<IActionResult> Edit(int deductionid, int allowanceid)
        {
           

            IQueryable<dynamic> applicationDbContext = null;

            if (deductionid > 0)
            {
                var stdcl = _context.StandardDeductionCalculation.Where(s => s.DeductionID == deductionid);

                applicationDbContext = from earning in _context.Allowance
                                       join stdcal in stdcl on earning.ID equals stdcal.AllowanceID into stdcalgroup
                                       from stdcal in stdcalgroup.DefaultIfEmpty() // This ensures the left join behavior
                                                                                   //where (stdcal == null || stdcal.DeductionID == deductionid)
                                       select new
                                       {
                                           AllowanceID = earning.ID,
                                           Allowance = earning.Name,
                                           ID = stdcal == null ? (int?)0 : stdcal.DeductionID
                                       };

            } else if (allowanceid > 0)
            {
                var stdcl = _context.StandardDeductionCalculation.Where(s => s.For_AllowanceID  == allowanceid);

                applicationDbContext = from earning in _context.Allowance
                                       join stdcal in stdcl on earning.ID equals stdcal.AllowanceID into stdcalgroup
                                       from stdcal in stdcalgroup.DefaultIfEmpty() // This ensures the left join behavior
                                                                                   //where (stdcal == null || stdcal.DeductionID == deductionid)
                                       select new
                                       {
                                           AllowanceID = earning.ID,
                                           Allowance = earning.Name,
                                           ID = stdcal == null ? (int?)0 : stdcal.AllowanceID
                                       };

            }
            return PartialView("Edit", await applicationDbContext.ToListAsync());
        }

        // POST: HumanResource/StandardDeductionCalculations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeductionID,AllowanceID")] StandardDeductionCalculation StandardDeductionCalculation)
        {
            if (id != StandardDeductionCalculation.DeductionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(StandardDeductionCalculation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StandardDeductionCalculationExists(StandardDeductionCalculation.DeductionID))
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
            return View(StandardDeductionCalculation);
        }

        // GET: HumanResource/StandardDeductionCalculations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var StandardDeductionCalculation = await _context.StandardDeductionCalculation
                .FirstOrDefaultAsync(m => m.DeductionID == id);
            if (StandardDeductionCalculation == null)
            {
                return NotFound();
            }

            return View(StandardDeductionCalculation);
        }

        // POST: HumanResource/StandardDeductionCalculations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var StandardDeductionCalculation = await _context.StandardDeductionCalculation.FindAsync(id);
            if (StandardDeductionCalculation != null)
            {
                _context.StandardDeductionCalculation.Remove(StandardDeductionCalculation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StandardDeductionCalculationExists(int id)
        {
            return _context.StandardDeductionCalculation.Any(e => e.DeductionID == id);
        }
    }
}
