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
    public class IncentiveSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncentiveSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/IncentiveSettings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IncentiveSetting.Include(i => i.EmployeeDetails);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/IncentiveSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incentiveSetting = await _context.IncentiveSetting
                .Include(i => i.EmployeeDetails)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (incentiveSetting == null)
            {
                return NotFound();
            }

            return View(incentiveSetting);
        }

        // GET: HumanResource/IncentiveSettings/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator");
            return View();
        }

        // POST: HumanResource/IncentiveSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,Incentive,AddnIncentive")] IncentiveSetting incentiveSetting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incentiveSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name", incentiveSetting.EmployeeID);
            return View(incentiveSetting);
        }

        // GET: HumanResource/IncentiveSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incentiveSetting = await _context.IncentiveSetting.FindAsync(id);
            if (incentiveSetting == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Name", incentiveSetting.EmployeeID);
            return View(incentiveSetting);
        }

        // POST: HumanResource/IncentiveSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Incentive,AddnIncentive")] IncentiveSetting incentiveSetting)
        {
            if (id != incentiveSetting.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incentiveSetting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncentiveSettingExists(incentiveSetting.EmployeeID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", incentiveSetting.EmployeeID);
            return View(incentiveSetting);
        }

        // GET: HumanResource/IncentiveSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incentiveSetting = await _context.IncentiveSetting
                .Include(i => i.EmployeeDetails)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (incentiveSetting == null)
            {
                return NotFound();
            }

            return View(incentiveSetting);
        }

        // POST: HumanResource/IncentiveSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incentiveSetting = await _context.IncentiveSetting.FindAsync(id);
            if (incentiveSetting != null)
            {
                _context.IncentiveSetting.Remove(incentiveSetting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncentiveSettingExists(int id)
        {
            return _context.IncentiveSetting.Any(e => e.EmployeeID == id);
        }
    }
}
