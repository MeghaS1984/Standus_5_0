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
    public class LoanRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HumanResource/LoanRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LoanRequest.Include(l => l.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HumanResource/LoanRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanRequest = await _context.LoanRequest
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (loanRequest == null)
            {
                return NotFound();
            }

            return View(loanRequest);
        }

        // GET: HumanResource/LoanRequests/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator");
            return View();
        }

        // POST: HumanResource/LoanRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestID,RequestNo,EmployeeId,Date,Amount,Reason,LoanType,Bank,Status,Comments")] LoanRequest loanRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loanRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", loanRequest.EmployeeId);
            return View(loanRequest);
        }

        // GET: HumanResource/LoanRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanRequest = await _context.LoanRequest.FindAsync(id);
            if (loanRequest == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", loanRequest.EmployeeId);
            return View(loanRequest);
        }

        // POST: HumanResource/LoanRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,RequestNo,EmployeeId,Date,Amount,Reason,LoanType,Bank,Status,Comments")] LoanRequest loanRequest)
        {
            if (id != loanRequest.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanRequestExists(loanRequest.RequestID))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeID", "Discriminator", loanRequest.EmployeeId);
            return View(loanRequest);
        }

        // GET: HumanResource/LoanRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanRequest = await _context.LoanRequest
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (loanRequest == null)
            {
                return NotFound();
            }

            return View(loanRequest);
        }

        // POST: HumanResource/LoanRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanRequest = await _context.LoanRequest.FindAsync(id);
            if (loanRequest != null)
            {
                _context.LoanRequest.Remove(loanRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanRequestExists(int id)
        {
            return _context.LoanRequest.Any(e => e.RequestID == id);
        }
    }
}
