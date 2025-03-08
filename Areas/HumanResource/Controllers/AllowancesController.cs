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
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/Allowances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,CutOffType,Period,DaysPresent,AuthorisedLeave,GeneralHoliday,CutOff,RoundOf,AccountID,Month,Day,Variable,PayrollSlNO,InActive,Fixed,DebitTo,CreditTo")] Allowance allowance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allowance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allowance);
        }

        // GET: HumanResource/Allowances/Edit/5
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
            return View(allowance);
        }

        // POST: HumanResource/Allowances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,CutOffType,Period,DaysPresent,AuthorisedLeave,GeneralHoliday,CutOff,RoundOf,AccountID,Month,Day,Variable,PayrollSlNO,InActive,Fixed,DebitTo,CreditTo")] Allowance allowance)
        {
            if (id != allowance.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allowance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllowanceExists(allowance.ID))
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
            return View(allowance);
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
    }
}
