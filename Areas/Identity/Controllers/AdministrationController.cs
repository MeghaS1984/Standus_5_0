using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.Identity.Models;
using Standus_5_0.Data;
using System.Security.Claims;

namespace Standus_5_0.Areas.Identity.Controllers    
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AdministrationController(RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the role already exists
                bool roleExists = await _roleManager.RoleExistsAsync(roleModel?.RoleName);
                if (roleExists)
                {
                    ModelState.AddModelError("", "Role Already Exists");
                }
                else
                {
                    // Create the role
                    // We just need to specify a unique role name to create a new role
                    ApplicationRole identityRole = new ApplicationRole
                    {
                        Name = roleModel?.RoleName
                    };

                    // Saves the role in the underlying AspNetRoles table
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(roleModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            List<ApplicationRole> roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            //First Get the role information from the database
            ApplicationRole? role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Handle the scenario when the role is not found
                return View("Error");
            }

            //Populate the EditRoleViewModel from the data retrived from the database
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                Users = new List<string>()
                // You can add other properties here if needed
            };

            // Retrieve all the Users
            foreach (var user in _userManager.Users.ToList())
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. 
                // This model object is then passed to the view for display
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    // Handle the scenario when the role is not found
                    ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    // Update other properties if needed

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles"); // Redirect to the roles list
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Role not found, handle accordingly
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                // Role deletion successful
                return RedirectToAction("ListRoles"); // Redirect to the roles list page
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // If we reach here, something went wrong, return to the view
            return View("ListRoles", await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            ViewBag.RollName = role.Name;
            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            //First check whether the Role Exists or not
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult? result;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    //If IsSelected is true and User is not already in this role, then add the user
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    //If IsSelected is false and User is already in this role, then remove the user
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    //Don't do anything simply continue the loop
                    continue;
                }

                //If you add or remove any user, please check the Succeeded of the IdentityResult
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { roleId = roleId });
                }
            }

            return RedirectToAction("EditRole", new { roleId = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string UserId)
        {
            //First Fetch the User Details by UserId
            var user = await _userManager.FindByIdAsync(UserId);

            //Check if User Exists in the Database
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // GetRolesAsync returns the list of user Roles
            var userRoles = await _userManager.GetRolesAsync(user);

            //Store all the information in the EditUserViewModel instance
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            //Pass the Model to the View
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            //First Fetch the User by Id from the database
            var user = await _userManager.FindByIdAsync(model.Id);

            //Check if the User Exists in the database
            if (user == null)
            {
                //If the User does not exists in the database, then return Not Found Error View
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                //If the User Exists, then proceed and update the data
                //Populate the user instance with the data from EditUserViewModel
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                //UpdateAsync Method will update the user data in the AspNetUsers Identity table
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    //Once user data updated redirect to the ListUsers view
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    //In case any error, stay in the same view and show the model validation error
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            //First Fetch the User you want to Delete
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                // Handle the case where the user wasn't found
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }
            else
            {
                //Delete the User Using DeleteAsync Method of UserManager Service
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    // Handle a successful delete
                    return RedirectToAction("ListUsers");
                }
                else
                {
                    // Handle failure
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View("ListUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string UserId)
        {
            //First Fetch the User Information from the Identity database by user Id
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                //handle if User Not Found in the Database
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            //Store the UserId in the ViewBag which is required while updating the Data
            //Store the UserName in the ViewBag which is used for displaying purpose
            ViewBag.UserId = UserId;
            ViewBag.UserName = user.UserName;

            //Create a List to Hold all the Roles Information
            var model = new List<UserRolesViewModel>();

            //Loop Through Each role and populate the model 
            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Description = role.Description
                };

                //Check if the Role is already assigned to this user
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                //Add the userRolesViewModel to the model
                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            //fetch the list of roles the specified user belongs to
            var roles = await _userManager.GetRolesAsync(user);

            //Then remove all the assigned roles for this user
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            List<string> RolesToBeAssigned = model.Where(x => x.IsSelected).Select(y => y.RoleName).ToList();

            //If At least 1 Role is assigned, Any Method will return true
            if (RolesToBeAssigned.Any())
            {
                //add a user to multiple roles simultaneously

                result = await _userManager.AddToRolesAsync(user, RolesToBeAssigned);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot Add Selected Roles to User");
                    return View(model);
                }
            }

            return RedirectToAction("EditUser", new { UserId = UserId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string UserId)
        {
            //First, fetch the User Details Based on the UserId
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                //handle if the User is not Exists in the database
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            //Storing the UserName in the ViewBag for Display Purpose
            ViewBag.UserName = user.UserName;

            //Create UserClaimsViewModel Instance
            var model = new UserClaimsViewModel
            {
                UserId = UserId
            };

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            // Loop through each claim we have in our application
            // Call the GetAllClaims Static Method ClaimsStore Class
            foreach (Claim claim in ClaimsStore.GetAllClaims())
            {
                //Create an Instance of UserClaim class
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                //By default the IsSelected is False, no need to set as false

                //Add the userClaim to UserClaimsViewModel Instance 
                model.Cliams.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            //First fetch the User Details
            var user = await _userManager.FindByIdAsync(model.UserId);


            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            // Add all the claims that are selected on the UI
            var AllSelectedClaims = model.Cliams.Where(c => c.IsSelected)
                        .Select(c => new Claim(c.ClaimType, c.ClaimType))
                        .ToList();

            //If At least 1 Claim is assigned, Any Method will return true
            if (AllSelectedClaims.Any())
            {
                //add a user to multiple claims simultaneously
                result = await _userManager.AddClaimsAsync(user, AllSelectedClaims);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected claims to user");
                    return View(model);
                }
            }

            return RedirectToAction("EditUser", new { UserId = model.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageAccessClaims(string UserId) {

            var allClaims = _context.UserClaims
            .Select(c => new { c.Id, c.ClaimType, c.ClaimValue })
            .ToList();

            ViewBag.userId = UserId;

            var query =
            from menus in _context.Menu
                join accessClaim in _context.AccessClaims
                    .Where(ac => ac.UserId == UserId)
                    on menus.ID equals accessClaim.MenuId
                    into accessClaimsJoin

                from ac in accessClaimsJoin.DefaultIfEmpty()

                join userClaim in _context.UserClaims
                    on ac.ClaimId equals userClaim.Id into userClaimsJoin

                from uc in userClaimsJoin.DefaultIfEmpty()

                select new
                {
                    menus.GroupName,
                    menus.ID,
                    menus.MenuName,
                    menus.UrlPath,
                    AccessClaimId = (int?)ac.Id,
                    AccessClaimUserId = ac.UserId,
                    Claim = uc
                };

            var queryResults = query.ToList(); // fetch from DB

            var grouped = queryResults
                .GroupBy(x => x.GroupName)
                .Select(g => new MenuGroupDto
                {
                    GroupName = g.Key,
                    Menus = g.GroupBy(x => new { x.ID, x.MenuName, x.UrlPath })
                             .Select(menuGroup => new MenuAccessDto
                             {
                                 MenuId = menuGroup.Key.ID,
                                 MenuName = menuGroup.Key.MenuName,
                                 UrlPath = menuGroup.Key.UrlPath,
                                 AccessClaimId = null,
                                 AccessClaimUserId = null,
                                 Claims = allClaims
                                     .Select(claim => new ClaimDto
                                     {
                                         ClaimId = claim.Id,
                                         ClaimType = claim.ClaimType!,
                                         ClaimValue = claim.ClaimValue!,
                                         IsSelected = menuGroup.Any(x => x.Claim != null && x.Claim.Id == claim.Id)
                                     })
                                     .ToList()
                             })
                             .ToList()
                });



            return View(grouped);
        }

        public IActionResult UpdateAccess(string userid,int claimid, int menuid,bool select) {
            if (select)
            {
                AccessClaims accessclaim = new AccessClaims()
                {
                    UserId = userid,
                    ClaimId = claimid,
                    MenuId = menuid
                };

                _context.Add(accessclaim);
                _context.SaveChanges();
            }
            else { 
                AccessClaims accessClaims = _context.AccessClaims.Where(ac => ac.UserId == userid && ac.ClaimId == claimid 
                    && ac.MenuId == menuid).FirstOrDefault();
                    if (accessClaims != null)
                {
                    _context.Remove (accessClaims);
                    _context.SaveChanges(); 
                }
            }
            return new EmptyResult();
        }

        [HttpGet]
        public async Task<IActionResult> ManageReportAccessClaims(string UserId)
        {

            var allClaims = _context.UserClaims
                .Where(uc => uc.ClaimType == "View Role")
            .Select(c => new { c.Id, c.ClaimType, c.ClaimValue })
            .ToList();

            ViewBag.userId = UserId;

            var query =
            from menus in _context.Reports
            join grp in _context.Groups on menus.GroupID equals grp.GroupID
            join accessClaim in _context.AccessClaims
                .Where(ac => ac.UserId == UserId)
                on menus.ReportID  equals accessClaim.ReportId
                into accessClaimsJoin

            from ac in accessClaimsJoin.DefaultIfEmpty()

            join userClaim in _context.UserClaims
                on ac.ClaimId equals userClaim.Id into userClaimsJoin

            from uc in userClaimsJoin.DefaultIfEmpty()

            select new
            {
                menus.@group.Groupname, 
                menus.ReportName,
                menus.ReportID ,
                menus.ReportType,
                AccessClaimId = (int?)ac.Id,
                AccessClaimUserId = ac.UserId,
                Claim = uc
            };

            var queryResults = query.ToList(); // fetch from DB

            var grouped = queryResults
                .GroupBy(x => x.Groupname)
                .Select(g => new MenuGroupDto
                {
                    GroupName = g.Key,
                    Menus = g.GroupBy(x => new { x.ReportID,x.Groupname , x.ReportName, x.ReportType})
                             .Select(menuGroup => new MenuAccessDto
                             {
                                 MenuId = menuGroup.Key.ReportID,
                                 MenuName = menuGroup.Key.ReportName,
                                 UrlPath = menuGroup.Key.ReportType,
                                 AccessClaimId = null,
                                 AccessClaimUserId = null,
                                 Claims = allClaims
                                     .Select(claim => new ClaimDto
                                     {
                                         ClaimId = claim.Id,
                                         ClaimType = claim.ClaimType!,
                                         ClaimValue = claim.ClaimValue!,
                                         IsSelected = menuGroup.Any(x => x.Claim != null && x.Claim.Id == claim.Id)
                                     })
                                     .ToList()
                             })
                             .ToList()
                });



            return View(grouped);
        }

        public IActionResult UpdateReportAccess(string userid, int claimid, int menuid, bool select)
        {
            if (select)
            {
                AccessClaims accessclaim = new AccessClaims()
                {
                    UserId = userid,
                    ClaimId = claimid,
                    ReportId = menuid
                };

                _context.Add(accessclaim);
                _context.SaveChanges();
            }
            else
            {
                AccessClaims accessClaims = _context.AccessClaims.Where(ac => ac.UserId == userid && ac.ClaimId == claimid
                    && ac.ReportId == menuid).FirstOrDefault();
                if (accessClaims != null)
                {
                    _context.Remove(accessClaims);
                    _context.SaveChanges();
                }
            }
            return new EmptyResult();
        }
    }
}