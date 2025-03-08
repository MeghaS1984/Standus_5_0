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
            return View(await _context.EAndDSetting.ToListAsync());
        }

        // GET: HumanResource/EAndDSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eAndDSetting = await _context.EAndDSetting
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
            var items = new List<SelectListItem>
            {
                new SelectListItem{ Text = "--Select Allowance--", Value = "0" }
            };

            items.AddRange(_context.Allowance.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.ID.ToString()
            }));

            ViewData["AllowanceID"] = new SelectList(items, "Value", "Text");

            items = new List<SelectListItem> { new SelectListItem { Text = "Select Deduction" , Value = "0" } };
            items.AddRange(_context.Deduction.Select(a => new SelectListItem{ 
                Text = a.Name,
                Value = a.ID.ToString() 
            }));

            ViewData["DeductionID"] = new SelectList(items,"Value", "Text");
            return View();
        }

        // POST: HumanResource/EAndDSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EAndDSetting model)
        {
            
            if (ModelState.IsValid)
            {
                if (model.AllowanceID > 0 && model.DeductionID > 0) {
                    ModelState.AddModelError("AllowanceID","Select Allowance or Deduction.");
                }
                // Create the parent entity
                var eAndDSetting = new EAndDSetting
                {
                    Query = model.Query,
                };

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

                if (eAndDSetting.ID > 0)
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                }
                else
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                }
                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));  // Redirect after saving
            }
            else
            {
                TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error");
            }
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
            if (eAndDSetting == null)
            {
                return NotFound();
            }
            return View(eAndDSetting);
        }

        // POST: HumanResource/EAndDSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Query")] EAndDSetting eAndDSetting)
        {
            if (id != eAndDSetting.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eAndDSetting);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
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
