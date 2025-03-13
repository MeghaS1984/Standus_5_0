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
using Standus_5_0.Migrations.Allowance;
using Standus_5_0.Services;

namespace Standus_5_0.Areas.HumanResource.Controllers
{
    [Area("HumanResource")]
    public class PositionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/Positions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Position.Include(p => p.Department);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.PositionID == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: HumanResource/Positions/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: HumanResource/Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            ViewData["DepartmentID"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName", position.DepartmentID);

            if (ModelState.IsValid)
            {
                var positionType = _context.Position.Where(p => p.PositionName == position.PositionName)
                    .Select(n => n.PositionName).FirstOrDefault();

                if (positionType != null) {
                    ModelState.AddModelError("PositionName", "This Position Exists !");
                    return View(position);
                }

                _context.Add(position);
                await _context.SaveChangesAsync();
                if (position.PositionID  > 0)
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Data Saved.");
                }
                else
                {
                    TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Unknown error.");
                }
                return RedirectToAction(nameof(Create));
            }
            
            return View(position);
        }

        // GET: HumanResource/Positions/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           
            var position = await _context.Position.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName", position.DepartmentID);
            return View(position);
        }

        // POST: HumanResource/Positions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Position position)
        {
            ViewData["DepartmentID"] = new SelectList(_context.Department, "DepartmentID", "DepartmentName", position.DepartmentID);

            if (id != position.PositionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                   // var positionType = _context.Position.Where(p => p.PositionName == position.PositionName)
                   //.Select(n => n.PositionName).FirstOrDefault();

                   // if (positionType != null)
                   // {
                   //     ModelState.AddModelError("PositionName", "This Position Exists !");
                   //     return View(position);
                   // }

                    _context.Update(position);
                    await _context.SaveChangesAsync();

                    if (position.PositionID > 0)
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
                    if (!PositionExists(position.PositionID))
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
            
            return View(position);
        }

        // GET: HumanResource/Positions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = await _context.Position
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.PositionID == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: HumanResource/Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var position = await _context.Position.FindAsync(id);

            var employ = _context.Employee.OfType<EmployementDetails>().FirstOrDefault(f => f.PositionID == id);

            if (employ != null)
            {
                TempData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, position.PositionName + " is in use. Select Inactivate !");
                return RedirectToAction(nameof(Delete));
            }

            if (position != null)
            {
                _context.Position.Remove(position);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return _context.Position.Any(e => e.PositionID == id);
        }
    }
}
