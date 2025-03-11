using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using Standus_5_0.Enums;
using Standus_5_0.Services;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class EAndDSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EAndDSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/EAndDSettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.EAndDSetting.Include(e => e.Allowance).Include(d=> d.Deduction).ToListAsync());
        }

        // GET: HumanResource/EAndDSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eAndDSetting = await _context.EAndDSetting
                .Include(e => e.Allowance).Include(f => f.Deduction).Include(p => p.EAndDSettingParams)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eAndDSetting == null)
            {
                return NotFound();
            }

            return View(eAndDSetting);
        }

        // GET: HumanResource/EAndDSettings/Create
        [HttpGet]
        public IActionResult Create()
        {
            DisplayEAndD(new EAndDSetting());
            return View();
        }

        public void DisplayEAndD(EAndDSetting model)
        {

            int selectedEID = model ?.AllowanceID ?? 0;
            int SelectedDID = model?.DeductionID ?? 0;

            var items = new List<SelectListItem>
                        {
                            new SelectListItem{ Text = "Select Allowance", Value = "0" }
            };

            items.AddRange(_context.Allowance.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.ID.ToString()
            }));

            ViewData["AllowanceID"] = new SelectList(items, "Value", "Text",selectedEID);

            items = new List<SelectListItem> { new SelectListItem { Text = "Select Deduction", Value = "0" } };
            items.AddRange(_context.Deduction.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.ID.ToString()
            }));

            ViewData["DeductionID"] = new SelectList(items, "Value", "Text", SelectedDID );
        }

        // POST: HumanResource/EAndDSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EAndDSetting model)
        {

            bool isError = false;

            ModelState.Remove("Allowance");
            ModelState.Remove("Deduction");

            if (model.AllowanceID > 0 && model.DeductionID > 0)
            {
                ModelState.AddModelError("AllowanceID", "Select Allowance or Deduction.");
                isError = true;
            }
            else if (model.AllowanceID == 0 && model.DeductionID == 0)
            {
                ModelState.AddModelError("AllowanceID", "Select Allowance or Deduction.");
                isError = true;
            }

            if (isError)
            {
                //return View(model);
                DisplayEAndD(new EAndDSetting());
                return View(model);
            }

            if (ModelState.IsValid)
            {
            
                          // Create the parent entity
                var eAndDSetting = new EAndDSetting
                {
                    Query = model.Query,
                    AllowanceID = model.AllowanceID,
                    DeductionID = model.DeductionID
                };

                if (model.EAndDSettingParams == null) {
                    model.EAndDSettingParams = new List<EAndDSettingParam>();
                }

                // Loop through the query parameters and ensure each one is treated as a new entity
                foreach (var queryParam in model.EAndDSettingParams)
                {
                    // Create a new instance of EAndDSettingParam
                    var newQueryParam = new EAndDSettingParam
                    {
                        QueryParam = queryParam.QueryParam,
                        EAndDSetting = eAndDSetting  // Set the reference to the parent entity
                    };

                    // Add the new query parameter to the parent's collection
                    eAndDSetting.EAndDSettingParams.Add(newQueryParam);
                }

                // Add the parent entity to the context
                _context.EAndDSetting.Add(eAndDSetting);

                await _context.SaveChangesAsync();

                if (eAndDSetting.ID > 0)
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                }
                else
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                }
                // Save changes to the database

            //return RedirectToAction(nameof(Index));  // Redirect after saving
                return RedirectToAction(nameof(Create));
            }
            
            DisplayEAndD(new EAndDSetting());
            return View(model);  // Return view if model is not valid
        }




        // GET: HumanResource/EAndDSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eAndDSetting = await _context.EAndDSetting.FindAsync(id);

            
            //if (eAndDSetting.EAndDSettingParams == null || eAndDSetting.EAndDSettingParams.Count == 0)
            //{
            //    eAndDSetting.EAndDSettingParams = new List<EAndDSettingParam>();
            //    eAndDSetting.EAndDSettingParams.Add(new EAndDSettingParam { QueryParam = "" });
            //}

            if (eAndDSetting == null)
            {
                return NotFound();
            }
            DisplayEAndD(eAndDSetting);
            return View(eAndDSetting);
        }

        // POST: HumanResource/EAndDSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  EAndDSetting eAndDSetting)
        {

            bool isError = false;

            ModelState.Remove("Allowance");
            ModelState.Remove("Deduction");

            if (eAndDSetting.AllowanceID > 0 && eAndDSetting.DeductionID > 0)
            {
                ModelState.AddModelError("AllowanceID", "Select Allowance or Deduction.");
                isError = true;
            }
            else if (eAndDSetting.AllowanceID == 0 && eAndDSetting.DeductionID == 0)
            {
                ModelState.AddModelError("AllowanceID", "Select Allowance or Deduction.");
                isError = true;
            }

            if (isError)
            {
                //return View(model);
                DisplayEAndD(new EAndDSetting());
                return View(eAndDSetting);
            }

            if (id != eAndDSetting.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int qid = id;
                    var EAndDParams = _context.EAndDSettingParam.Where(p => p.QueryId == qid);

                    _context.EAndDSettingParam.RemoveRange(EAndDParams);

                    _context.Update(eAndDSetting);
                    await _context.SaveChangesAsync();

                    if (eAndDSetting.ID > 0)
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                    }
                    else
                    {
                        TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                    }
                    // Save changes to the database
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EAndDSettingExists(eAndDSetting.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                DisplayEAndD(new EAndDSetting());
                return RedirectToAction(nameof(Edit));
            }
            
            return View(eAndDSetting);
        }

        // GET: HumanResource/EAndDSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eAndDSetting = await _context.EAndDSetting
                .Include(e => e.Allowance).Include(f => f.Deduction).Include(p => p.EAndDSettingParams)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (eAndDSetting == null)
            {
                return NotFound();
            }

            return View(eAndDSetting);
        }

        // POST: HumanResource/EAndDSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eAndDSetting = await _context.EAndDSetting.FindAsync(id);
            if (eAndDSetting != null)
            {
                _context.EAndDSetting.Remove(eAndDSetting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EAndDSettingExists(int id)
        {
            return _context.EAndDSetting.Any(e => e.ID == id);
        }
    }
}
