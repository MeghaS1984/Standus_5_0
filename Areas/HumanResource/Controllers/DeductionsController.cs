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
    public class DeductionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeductionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Deductions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deduction.ToListAsync());
        }

        public ActionResult SlabSetup()
        {
            var slabs = from deduction in _context.Deduction 
                        join slb in _context.Slab on deduction.ID equals slb.DeductionID
                        into slabGroup
                        from slb in slabGroup.DefaultIfEmpty()
                        select new
                        {
                            Deductionid = deduction.ID,
                            Deduction = deduction.Name,
                            Status = slb != null ? "Done" : "Not Done",
                            SlabID = slb != null ? slb.SlabID : 0
                        };
            return PartialView("_DeductionSlabSetup", slabs);
        }

        // GET: HumanResource/Deductions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deduction = await _context.Deduction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (deduction == null)
            {
                return NotFound();
            }

            return View(deduction);
        }

        // GET: HumanResource/Deductions/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/Deductions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Deduction deduction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deduction);
                await _context.SaveChangesAsync();
                if (deduction.ID > 0)
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                }
                else
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                }
                return RedirectToAction(nameof(Create));
            }
            return View(deduction);
        }

        // GET: HumanResource/Deductions/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deduction = await _context.Deduction.FindAsync(id);
            if (deduction == null)
            {
                return NotFound();
            }
            return View(deduction);
        }

        // POST: HumanResource/Deductions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Period,CutOffType,CutOff,RoundOf,Month,Day,Variable,AccountID,OnYearlyIncome,PayRollSlNo,InActive,Fixed,DebitTo,CreditTo,EmployerDebitTo")] Deduction deduction)
        {
            if (id != deduction.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deduction);
                    await _context.SaveChangesAsync();

                    if (deduction.ID > 0)
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                    }
                    else
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                    }
                    return RedirectToAction(nameof(Edit));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeductionExists(deduction.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(deduction);
        }

        // GET: HumanResource/Deductions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deduction = await _context.Deduction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (deduction == null)
            {
                return NotFound();
            }

            return View(deduction);
        }

        // POST: HumanResource/Deductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deduction = await _context.Deduction.FindAsync(id);

            var slab = _context.Slab.FirstOrDefault(f => f.DeductionID == id);

            if (slab != null)
            {
                TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, deduction.Name + " is in use. Select Inactivate !");
                return RedirectToAction(nameof(Delete));
            }
            if (deduction != null)
            {
                _context.Deduction.Remove(deduction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeductionExists(int id)
        {
            return _context.Deduction.Any(e => e.ID == id);
        }
    }
}
