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
    public class SlabDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SlabDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/SlabDetails
        public async Task<IActionResult> Index(int slabid, int categoryid)
        {
            var slab = _context.Slab.Where(s => s.SlabID == slabid).FirstOrDefault();

            if (slab != null)
            {
                ViewData["AllowanceID"] = slab.AllowanceID;
            }
            var applicationDbContext = _context.SlabDetails.Include(s => s.Category).
                Where(f => f.SlabID == slabid && f.CategoryID == categoryid);

            return PartialView("index",await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/SlabDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDetails = await _context.SlabDetails
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slabDetails == null)
            {
                return NotFound();
            }
            return View(slabDetails);
        }

        // GET: HumanResource/SlabDetails/Create
        public IActionResult Create(int SlabID,int CategoryID, int AllowanceID)
        {
            //var slab_details = _context.SlabDetails.Where(s => s.ID == SlabID && s.CategoryID == CategoryID);
            ViewData["CategoryID"] = CategoryID; //new SelectList(_context.Category, "ID", "CategoryName");
            ViewData["SlabID"] = SlabID;
            ViewData["AllowanceID"] = AllowanceID;
            return PartialView();
        }

        // POST: HumanResource/SlabDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlabID,FromAmount,ToAmount,Type,Employee,Employer,ID,CategoryID")] SlabDetails slabDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(slabDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create), new { slabid = slabDetails.SlabID,
                categoryid = slabDetails.CategoryID});
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabDetails.CategoryID);
            return View(slabDetails);
        }

        // GET: HumanResource/SlabDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDetails = await _context.SlabDetails.FindAsync(id);
            if (slabDetails == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabDetails.CategoryID);
            return View(slabDetails);
        }

        // POST: HumanResource/SlabDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SlabID,FromAmount,ToAmount,Type,Employee,Employer,ID,CategoryID")] SlabDetails slabDetails)
        {
            if (id != slabDetails.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(slabDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlabDetailsExists(slabDetails.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "CategoryName", slabDetails.CategoryID);
            return View(slabDetails);
        }

        // GET: HumanResource/SlabDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slabDetails = await _context.SlabDetails
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (slabDetails == null)
            {
                return NotFound();
            }

            return View(slabDetails);
        }

        // POST: HumanResource/SlabDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slabDetails = await _context.SlabDetails.FindAsync(id);
            if (slabDetails != null)
            {
                _context.SlabDetails.Remove(slabDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlabDetailsExists(int id)
        {
            return _context.SlabDetails.Any(e => e.ID == id);
        }

        public IActionResult SlabSchedule(int slabid, int detailsid,int categoryid, int allowanceID)
        {
            ViewData["categoryid"] = categoryid;
            ViewData["slabid"] = slabid;
            ViewData["detailsid"] = detailsid;
            ViewData["allowanceid"] = allowanceID;
            return View();
        }

        public ContentResult DeleteSlabSchedule(int slabid, int detailsid)
        {
            var schedule = _context.SlabSchedule.Where(s => s.SlabID == slabid && s.DetailsID == detailsid);

            try
            {
                _context.SlabSchedule.RemoveRange(schedule);
                _context.SaveChanges();

                return Content("Success", "text/plain");
            } catch (Exception)
            {
				return Content("Error", "text/plain");
			}
        }
        public ContentResult UpdateSlabSchedule(int slabid, int detailsid, string month, bool ischecked) {
            string success = "", failure ="";

			try
            {
                if (ischecked)
                {
                    var schedule = new SlabSchedule() { SlabID = slabid, DetailsID = detailsid, Month = month };

                    _context.SlabSchedule.Add(schedule);
                    _context.SaveChanges();
                }
				success = CommonServices.ShowAlert(Alerts.Success, "Schedule updated.");
				return Content(success, "text/plain");
			} catch(Exception )
            {
                failure = CommonServices.ShowAlert(Alerts.Danger, "Error..."); 
                return Content("Error", "text/plain");
            }
			
		}

        public JsonResult Get_Schedules(int slabid, int detailsid)
        {
			var schedule = _context.SlabSchedule.Where(s => s.SlabID == slabid && s.DetailsID == detailsid)
                .Select(s => s.Month);

            return Json(schedule);
		}

        public IActionResult SlabCalculation(int slabid, int detailsid, int categoryid, int allowanceID)
        {
            ViewData["categoryid"] = categoryid;
            ViewData["slabid"] = slabid;
            ViewData["detailsid"] = detailsid;
            ViewData["allowanceid"] = allowanceID;

            var allowance = from allw in _context.Allowance
                            join slbcalc in _context.SlabCalculation
                            on allw.ID equals slbcalc.AllowanceID into allowanceGroup
                            from slbcalc in allowanceGroup.DefaultIfEmpty() // Left join
                            where allw.ID == allowanceID &&
                                  (slbcalc == null || (slbcalc.SlabID == slabid && 
                                  slbcalc.DetailsID == detailsid)) // Check null and other conditions
                            select new
                            {
                                AllowanceID = allw.ID,
                                AllowanceName = allw.Name,
                                SlabID = slabid,
                                DetailsID = detailsid,
                                OnIncome = (slbcalc.OnIncome == null ? "Monthly" : slbcalc.OnIncome ) // Use null-conditional operator to safely access properties
                            };


            return View(allowance);
        }
    }
}
