using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Standus_5_0.Areas.Identity.Models;
using Standus_5_0.Data;
using Standus_5_0.Models;
using System.Diagnostics;

namespace Standus_5_0.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context,SignInManager<ApplicationUser> userManager )
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string userid)
        {
            List<MenuAccessViewModel> menus = null;

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (User.IsInRole("Superadmin")) {
                menus =  _context.Menu
                         .GroupBy(m => m.GroupName)
                         .Select(g => new MenuAccessViewModel
                         {
                             GroupName = g.Key,
                             Menus = g.GroupBy(x => new { x.MenuName,x.UrlPath })
                             .Select(mg => new MenuAccessDto { 
                                MenuName = mg.Key.MenuName ,
                                UrlPath = mg.Key.UrlPath 
                             }).ToList(),
                             AccessClaim = null }).ToList();
            }
            else
            {
                //menus = (from menu in _context.Menu
                //            join clm in _context.AccessClaims on menu.ID equals clm.MenuId
                //            where clm.UserId == userId
                //            select new MenuAccessViewModel
                //            { Menu = menu, AccessClaim = clm }).ToList();
            }
            return View(menus) ;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
