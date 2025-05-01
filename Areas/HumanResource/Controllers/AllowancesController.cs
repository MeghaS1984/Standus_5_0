using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class AllowancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllowancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Allowances
        public async Task<IActionResult> Index()
        {
            return View(await _context.Allowance.ToListAsync());
        }

        public ActionResult SlabSetup() {

            var slabs = from earning in _context.Allowance
                        join slb in _context.Slab on earning.ID equals slb.AllowanceID
                        into slabGroup
                        from slb in slabGroup.DefaultIfEmpty()
                        select new
                        {
                            Allowanceid = earning.ID,
                            Allowance = earning.Name,
                            Status = slb != null ? "Done": "Not Done"
                        };

            return PartialView("_AllowanceSlabSetup",slabs);
        }
        // GET: HumanResource/Allowances/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowance = await _context.Allowance
                .FirstOrDefaultAsync(m => m.ID == id);
            if (allowance == null)
            {
                return NotFound();
            }

            return View(allowance);
        }

        // GET: HumanResource/Allowances/Create
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

        // POST: HumanResource/Allowances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        //[ValidateAntiForgeryToken]
        public ActionResult Create([FromBody] Allowance_Data data)
        {
            var allowance_data = new Allowance();

            allowance_data.Name = data.name;
            allowance_data.Description = data.description;
            allowance_data.PayrollSlNO = data.payrollslno == "" ? 0 : Convert.ToInt16(data.payrollslno);
            allowance_data.Type = data.allowance_type;
            allowance_data.RoundOff = data.roundoff;
            allowance_data.OnYearlyIncome = data.onyearlyincome;
            allowance_data.Fixed = data.allowance_fixed;
            allowance_data.InActive = data.inactive;
            allowance_data.Variable = 0;
            _context.Add(allowance_data);
            _context.SaveChanges();

            if (allowance_data.ID > 0)
            {
                for (int i = 0; i < data.allowances.Count; i++)
                {
                    var sdc = new StandardDeductionCalculation();
                    sdc.For_AllowanceID = allowance_data.ID;
                    sdc.AllowanceID = data.allowances[i];
                    _context.StandardDeductionCalculation.Add(sdc);
                    _context.SaveChanges();
                }

                foreach (string mon in data.months)
                {
                    var sch = new SlabSchedule();
                    sch.AllowanceID = allowance_data.ID;
                    sch.DeductionID = 0;
                    sch.Month = mon;
                    _context.SlabSchedule.Add(sch);
                    _context.SaveChanges();
                }
                string success = CommonServices.ShowAlert(Alerts.Success, "Data saved.");
                return Content(success, "text/plain");
            }
            else
            {

                string failure = CommonServices.ShowAlert(Alerts.Danger, "Error.");
                return Content(failure, "text/plain");
            }
        }

        // GET: HumanResource/Allowances/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowance = await _context.Allowance.FindAsync(id);
            if (allowance == null)
            {
                return NotFound();
            }

            var type = new List<SelectListItem>{
                    new SelectListItem {Text = "Monthly", Value= "Monthly" },
                    new SelectListItem {Text = "Yearly", Value = "Yearly" }
                };

            ViewData["TypeList"] = new SelectList(type, "Value", "Text",allowance.Type);

            return View(allowance);
        }

        // POST: HumanResource/Allowances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult Edit([FromBody] Allowance_Data data)
        {
            var allowance_data = _context.Allowance .Where(d => d.ID == data.allowanceid).FirstOrDefault();

            try
            {
                allowance_data.Name = data.name;
                allowance_data.Description = data.description;
                allowance_data.PayrollSlNO  = data.payrollslno == "" ? 0 : Convert.ToInt16(data.payrollslno);
                allowance_data.Type = data.allowance_type;
                allowance_data.RoundOff = data.roundoff;
                allowance_data.OnYearlyIncome = data.onyearlyincome;
                allowance_data.Fixed = data.allowance_fixed;
                allowance_data.InActive = data.inactive;
                allowance_data.Variable = 0;

                _context.Update(allowance_data);
                _context.SaveChanges();

                var sdc_ext = _context.StandardDeductionCalculation.Where(sc => sc.For_AllowanceID == data.allowanceid);

                _context.StandardDeductionCalculation.RemoveRange(sdc_ext);
                _context.SaveChanges();

                var slb_ext = _context.SlabSchedule.Where(sc => sc.AllowanceID == data.allowanceid);
                _context.SlabSchedule.RemoveRange(slb_ext);
                _context.SaveChanges();

                if (allowance_data.ID > 0)
                {
                    for (int i = 0; i < data.allowances.Count; i++)
                    {
                        var sdc = new StandardDeductionCalculation();
                        sdc.For_AllowanceID = allowance_data.ID;
                        sdc.AllowanceID = data.allowances[i];
                        _context.StandardDeductionCalculation.Add(sdc);
                        _context.SaveChanges();
                    }

                    foreach (string mon in data.months)
                    {
                        var sch = new SlabSchedule();
                        sch.AllowanceID = allowance_data.ID;
                        sch.DeductionID = 0;
                        sch.Month = mon;
                        _context.SlabSchedule.Add(sch);
                        _context.SaveChanges();
                    }
                }

                string success = CommonServices.ShowAlert(Alerts.Success, "Data saved.");
                return Content(success, "text/plain");
            }
            catch (Exception)
            {

                string success = CommonServices.ShowAlert(Alerts.Danger, "Error.");
                return Content(success, "text/plain");
            }
        }

        // GET: HumanResource/Allowances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allowance = await _context.Allowance
                .FirstOrDefaultAsync(m => m.ID == id);
            if (allowance == null)
            {
                return NotFound();
            }

            return View(allowance);
        }

        // POST: HumanResource/Allowances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allowance = await _context.Allowance.FindAsync(id);

            var slab = _context.Slab.FirstOrDefault(f => f.AllowanceID == id);

            if (slab != null)
            {
                TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, allowance.Name  + " is in use. Select Inactivate !");
                return RedirectToAction(nameof(Delete));
            }

            if (allowance != null)
            {
                _context.Allowance.Remove(allowance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllowanceExists(int id)
        {
            return _context.Allowance.Any(e => e.ID == id);
        }

        private ActionResult AllowanceExists(string Name)
        {
            var allw = _context.Allowance.Any(e => e.Name == Name);
            return Json(allw) ;
        }
    }

    public class Allowance_Data
    {
        public string name { get; set; }
        public string description { get; set; }
        public string payrollslno { get; set; }
        public string allowance_type { get; set; }
        public string roundoff { get; set; }
        public bool onyearlyincome { get; set; }

        public bool allowance_fixed { get; set; }
        public bool inactive { get; set; }
        public List<int> allowances { get; set; }
        public List<string> months { get; set; }

        public int allowanceid { get; set; }
    }

}
