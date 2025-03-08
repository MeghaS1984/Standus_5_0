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
    public class PaySheetSummarySettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaySheetSummarySettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/PaySheetSummarySettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.PaySheetSummarySetting.ToListAsync());
        }

        // GET: HumanResource/PaySheetSummarySettings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paySheetSummarySetting = await _context.PaySheetSummarySetting
                .FirstOrDefaultAsync(m => m.ID == id);
            if (paySheetSummarySetting == null)
            {
                return NotFound();
            }

            return View(paySheetSummarySetting);
        }

        // GET: HumanResource/PaySheetSummarySettings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/PaySheetSummarySettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Query,Design")] PaySheetSummarySetting paySheetSummarySetting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paySheetSummarySetting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paySheetSummarySetting);
        }

        // GET: HumanResource/PaySheetSummarySettings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paySheetSummarySetting = await _context.PaySheetSummarySetting.FindAsync(id);
            if (paySheetSummarySetting == null)
            {
                return NotFound();
            }
            return View(paySheetSummarySetting);
        }

        // POST: HumanResource/PaySheetSummarySettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Query,Design")] PaySheetSummarySetting paySheetSummarySetting)
        {
            if (id != paySheetSummarySetting.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paySheetSummarySetting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaySheetSummarySettingExists(paySheetSummarySetting.ID))
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
            return View(paySheetSummarySetting);
        }

        // GET: HumanResource/PaySheetSummarySettings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paySheetSummarySetting = await _context.PaySheetSummarySetting
                .FirstOrDefaultAsync(m => m.ID == id);
            if (paySheetSummarySetting == null)
            {
                return NotFound();
            }

            return View(paySheetSummarySetting);
        }

        // POST: HumanResource/PaySheetSummarySettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paySheetSummarySetting = await _context.PaySheetSummarySetting.FindAsync(id);
            if (paySheetSummarySetting != null)
            {
                _context.PaySheetSummarySetting.Remove(paySheetSummarySetting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaySheetSummarySettingExists(int id)
        {
            return _context.PaySheetSummarySetting.Any(e => e.ID == id);
        }
    }
}
