using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Areas.Report.Data;
using Standus_5_0.Areas.Identity;
using Standus_5_0.Areas.Report.Models;
using Standus_5_0.Data;
using Standus_5_0.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Standus_5_0.Areas.Identity.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser,ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<Standus_5_0.Areas.Identity.Model.ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddIdentity<Standus_5_0.Areas.Identity.Models.ApplicationUser, IdentityRole>(
//    options =>
//    {
//        options.Password.RequireDigit = true;
//        options.Password.RequiredLength = 8;
//        options.Password.RequireNonAlphanumeric = true;
//        options.Password.RequireUppercase = true;
//        options.Password.RequireLowercase = true;
//        options.Password.RequiredUniqueChars = 4;
//    }).AddEntityFrameworkStores<ApplicationDbContext>();

var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

builder.Services.AddSingleton(smtpSettings);
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();


// Configure the Application Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    // If the LoginPath isn't set, ASP.NET Core defaults the path to /Account/Login.
    options.LoginPath = "/Account/Login"; // Set your login path here

    //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<ExcelImport>();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    
});

builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "ModuleArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();
//app.MapRazorPages();

app.Run();
