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
    public class PaySheetSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PaySheetSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/PaySheetSettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.paySheetSetting.ToListAsync());
        }

        // GET: HumanResource/PaySheetSettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paySheetSetting = await _context.paySheetSetting
                .FirstOrDefaultAsync(m => m.ID == id);
            if (paySheetSetting == null)
            {
                return NotFound();
            }

            return View(paySheetSetting);
        }

        // GET: HumanResource/PaySheetSettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/PaySheetSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Query,Design")] PaySheetSetting paySheetSetting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paySheetSetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paySheetSetting);
        }

        // GET: HumanResource/PaySheetSettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paySheetSetting = await _context.paySheetSetting.FindAsync(id);
            if (paySheetSetting == null)
            {
                return NotFound();
            }
            return View(paySheetSetting);
        }

        // POST: HumanResource/PaySheetSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Query,Design")] PaySheetSetting paySheetSetting)
        {
            if (id != paySheetSetting.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paySheetSetting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaySheetSettingExists(paySheetSetting.ID))
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
            return View(paySheetSetting);
        }

        // GET: HumanResource/PaySheetSettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paySheetSetting = await _context.paySheetSetting
                .FirstOrDefaultAsync(m => m.ID == id);
            if (paySheetSetting == null)
            {
                return NotFound();
            }

            return View(paySheetSetting);
        }

        // POST: HumanResource/PaySheetSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paySheetSetting = await _context.paySheetSetting.FindAsync(id);
            if (paySheetSetting != null)
            {
                _context.paySheetSetting.Remove(paySheetSetting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaySheetSettingExists(int id)
        {
            return _context.paySheetSetting.Any(e => e.ID == id);
        }
    }
}
