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
            var type = new List<SelectListItem>{
                    new SelectListItem {Text = "Monthly", Value= "Monthly" },
                    new SelectListItem {Text = "Yearly", Value = "Yearly" }
                };

            ViewData["TypeList"] = new SelectList(type, "Value", "Text");

			return View();
        }

        // POST: HumanResource/Deductions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Create(Deduction deduction)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        _context.Add(deduction);
        ////        await _context.SaveChangesAsync();
        ////        if (deduction.ID > 0)
        ////        {
        ////            TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
        ////        }
        ////        else
        ////        {
        ////            TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
        ////        }
        ////        return RedirectToAction(nameof(Create));
        ////    }
        ////    return View(deduction);
        ////}
        [HttpPost]
        public ActionResult Create([FromBody] Deduction_Data data) {

            var deduction_data = new Deduction();

            deduction_data.Name = data.name;
            deduction_data.Description = data.description;
            deduction_data.PayRollSlNo = data.payrollslno == "" ? 0 : Convert.ToInt16(data.payrollslno);
            deduction_data.Type = data.deduction_type;
            deduction_data.RoundOff = data.roundoff;
            deduction_data.OnYearlyIncome = data.onyearlyincome;
            deduction_data.Fixed = data.deduction_fixed;
            deduction_data.InActive = data.inactive;
            deduction_data.Variable = false;
			_context.Add(deduction_data);
            _context.SaveChanges();

            if (deduction_data.ID > 0) {
                for (int i = 0; i < data.allowances.Count; i++) {
                    var sdc = new StandardDeductionCalculation();
                    sdc.DeductionID = deduction_data .ID;
                    sdc.AllowanceID = data.allowances[i];
                    _context.StandardDeductionCalculation.Add(sdc);
                    _context.SaveChanges();
                }

                foreach (string mon in data.months) { 
                    var sch = new SlabSchedule ();
                    sch.DeductionID = deduction_data.ID;
                    sch.AllowanceID = 0;
                    sch.Month = mon;
                    _context.SlabSchedule.Add(sch);
                    _context.SaveChanges();
                }
            }

            return Json(new { success = true, message = "Data save" });
        }

        // GET: HumanResource/Deductions/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var type = new List<SelectListItem>{
                    new SelectListItem {Text = "Monthly", Value= "Monthly" },
                    new SelectListItem {Text = "Yearly", Value = "Yearly" }
                };

            ViewData["TypeList"] = new SelectList(type, "Value", "Text");

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
        public async Task<IActionResult> Edit([FromBody] Deduction_Data data)
        {
            var deduction_data = new Deduction();

            deduction_data.ID = data.deductionid;
            deduction_data.Name = data.name;
            deduction_data.Description = data.description;
            deduction_data.PayRollSlNo = data.payrollslno == "" ? 0 : Convert.ToInt16(data.payrollslno);
            deduction_data.Type = data.deduction_type;
            deduction_data.RoundOff = data.roundoff;
            deduction_data.OnYearlyIncome = data.onyearlyincome;
            deduction_data.Fixed = data.deduction_fixed;
            deduction_data.InActive = data.inactive;
            deduction_data.Variable = false;

            _context.Update(deduction_data);
            _context.SaveChanges();

            var sdc_ext = _context.StandardDeductionCalculation.Where(sc => sc.DeductionID == data.deductionid);

            _context.StandardDeductionCalculation.RemoveRange(sdc_ext);
            _context.SaveChanges();

            var slb_ext = _context.SlabSchedule.Where(sc => sc.DeductionID == data.deductionid);
            _context.SlabSchedule.RemoveRange(slb_ext);

            if (deduction_data.ID > 0)
            {
                for (int i = 0; i < data.allowances.Count; i++)
                {
                    var sdc = new StandardDeductionCalculation();
                    sdc.DeductionID = deduction_data.ID;
                    sdc.AllowanceID = data.allowances[i];
                    _context.StandardDeductionCalculation.Add(sdc);
                    _context.SaveChanges();
                }

                foreach (string mon in data.months)
                {
                    var sch = new SlabSchedule();
                    sch.DeductionID = deduction_data.ID;
                    sch.AllowanceID = 0;
                    sch.Month = mon;
                    _context.SlabSchedule.Add(sch);
                    _context.SaveChanges();
                }
            }

            return Json(new { success = true, message = "Data save" });
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

    public class Deduction_Data { 
        public string name {  get; set; }
        public string description { get; set; }
        public string payrollslno { get; set; }
        public string deduction_type { get; set; }
        public string roundoff { get; set; }
        public bool onyearlyincome {  get; set; }

        public bool deduction_fixed { get; set; }
        public bool inactive { get; set; }
        public List<int> allowances { get; set; }
        public List<string> months { get; set; }

        public int deductionid { get; set; }
    }

    public class AllowanceID { 
        public int allowanceid { get; set; }

    }

    public class MonthData { 
        public string month { get; set; }
    }
}
