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
    public class LeaveAllocationDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveAllocationDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/LeaveAllocationDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeaveAllocationDetails.ToListAsync());
        }

        
        public async Task<ContentResult> NewAllocation(int Year,int days, int headid)
        {
            //_context.Database.ExecuteSqlRaw("update employee set Discriminator = 'Resigned'");

            var staff = _context.Employee.ToList();
            string Status = "";
            int Total = staff.Count() ;
            int counter = 0;
            foreach (var employee in staff)
            {
                try
                {
                    var allocation = new LeaveAllocationDetails();
                    allocation.StartDate = new DateTime(Year, 1, 1);
                    allocation.EndDate = new DateTime(Year, 12, 31);
                    allocation.EmployeeID = employee.EmployeeID;
                    allocation.Closed = false;
                    allocation.HeadID = headid;
                    allocation.Days = days;
                    _context.LeaveAllocationDetails.Add(allocation);
                    await _context.SaveChangesAsync();
                    counter++;
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }

            Status = "Total : " + Total + ", Updated : " + counter;

            return new ContentResult { ContentType = "text/plain", Content = Status };
        }

        [HttpGet]
        public async Task<IActionResult> AllocationDetails(int year,int headid) {

            var used = from attd in _context.AttendanceDetails
                       group attd by attd.HeadID into attdGroup
                       select new
                       {
                           id = attdGroup.FirstOrDefault().EmployeeID,  // Get the first EmployeeId in the group
                           used = attdGroup.Sum(f => f.Half1 + f.Half2)  // Sum of Half1 and Half2 for the group
                       };

            var allocs = _context.LeaveAllocationDetails
                .Include(x => x.Employee)
                .Include(x => x.Head)
                .Where(l => l.HeadID == headid && l.StartDate.Year == year)
                .ToList();

            var accrual_with_used = (from all in allocs
                         join acc_used in used on all.EmployeeID equals acc_used.id into acc_used_Group
                         from acc_used_item in acc_used_Group.DefaultIfEmpty() // Left join handling
                         select new LeaveAllocationWithUsage
                         {
                             Allocation = all,
                             Used = acc_used_item != null ? acc_used_item.used : 0, // Handling null cases
                             Balance = all.Days - (acc_used_item != null ? acc_used_item.used : 0) // Balance calculation
                         }).ToList();



            return View(accrual_with_used);
        }
        // GET: HumanResource/LeaveAllocationDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveAllocationDetails = await _context.LeaveAllocationDetails
                .FirstOrDefaultAsync(m => m.ID == id);
            if (leaveAllocationDetails == null)
            {
                return NotFound();
            }

            return View(leaveAllocationDetails);
        }

        // GET: HumanResource/LeaveAllocationDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HumanResource/LeaveAllocationDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Days,Closed")] LeaveAllocationDetails leaveAllocationDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveAllocationDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveAllocationDetails);
        }

        // GET: HumanResource/LeaveAllocationDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveAllocationDetails = await _context.LeaveAllocationDetails.FindAsync(id);
            if (leaveAllocationDetails == null)
            {
                return NotFound();
            }
            return View(leaveAllocationDetails);
        }

        // POST: HumanResource/LeaveAllocationDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Days,ID")] LeaveAllocationDetails leaveAllocationDetails)
        {
            if (id != leaveAllocationDetails.ID)
            {
                return NotFound();
            }
            
            var alloc = _context.LeaveAllocationDetails.Where(l => l.ID == id).FirstOrDefault();

            alloc.Days = leaveAllocationDetails.Days;

            var vals = new RouteValueDictionary ();
            vals.Add ("headid", alloc.HeadID);
            vals.Add("year", alloc.StartDate.Year);

                try
                {
                    _context.Update(alloc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveAllocationDetailsExists(leaveAllocationDetails.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllocationDetails), vals);
            
        }

        // GET: HumanResource/LeaveAllocationDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveAllocationDetails = await _context.LeaveAllocationDetails
                .FirstOrDefaultAsync(m => m.ID == id);
            if (leaveAllocationDetails == null)
            {
                return NotFound();
            }

            return View(leaveAllocationDetails);
        }

        // POST: HumanResource/LeaveAllocationDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveAllocationDetails = await _context.LeaveAllocationDetails.FindAsync(id);
            if (leaveAllocationDetails != null)
            {
                _context.LeaveAllocationDetails.Remove(leaveAllocationDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveAllocationDetailsExists(int id)
        {
            return _context.LeaveAllocationDetails.Any(e => e.ID == id);
        }
    }
}
