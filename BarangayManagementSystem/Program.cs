using BarangayCMS.BLL.Interfaces;
using BarangayCMS.BLL.Services;
using BarangayCMS.DAL.Context;
using BarangayCMS.DAL.Repository;
using BarangayCMS.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// DB CONTEXT SETUP
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// REGISTRATION OF REPOSITORIES (DAL)
// ==========================================
builder.Services.AddScoped<IResidentRepository, ResidentRepository>();
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IDisasterRepository, DisasterRepository>();
builder.Services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();
builder.Services.AddScoped<IHealthRepository, HealthRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ==========================================
// REGISTRATION OF SERVICES (BLL)
// ==========================================
builder.Services.AddScoped<IResidentService, ResidentService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IDisasterService, DisasterService>();
builder.Services.AddScoped<IComplaintService, ComplaintService>();
builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserService, UserService>();

// ==========================================
// IDENTITY & AUTHENTICATION SETUP
// ==========================================
// In-update ang Identity setup para luwagan ang login at password rules
builder.Services.AddIdentity<BarangayCMS.Entities.ApplicationUser, IdentityRole>(options =>
{
    // 🔑 SOLUSYON: Pinatay ang RequireConfirmedAccount para makapasok agad ang bagong rehistro
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    // 🔑 BONUS: Inalis ang striktong password rules para hindi ka ma-error sa special characters
    options.Password.RequireNonAlphanumeric = false; // Hindi na kailangan ng @, #, !, atbp.
    options.Password.RequireDigit = false;           // Hindi na kailangan ng numero
    options.Password.RequireUppercase = false;       // Hindi na kailangan ng Capital letter
    options.Password.RequiredLength = 6;             // Maintain sa minimum 6 characters
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Dito babaguhin ang Login at Access Denied paths ng Identity nang walang duplicate scheme error
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";               // Kung saan itatapon ang user kapag hindi naka-login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Kung saan itatapon kapag walang tamang Role (hal. Staff pumasok sa Admin)
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ==========================================
// MIDDLEWARES & ROUTING CONFIGURATION
// ==========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Para gumana ang CSS, Images, at JS mula sa wwwroot

app.UseRouting(); // 1. Tukuyin kung anong URL ang tinatawag ng user

// CRITICAL AT MAGKASUNOD: Ito ang magbabantay sa mga [Authorize] attributes mo
app.UseAuthentication(); // 2. Kilalanin kung sino ang naka-login
app.UseAuthorization();  // 3. I-verify kung may karapatan ba siya sa page na pupuntahan niya

// ==========================================
// ROUTE MAPPING (ISANG BESES LANG DAPAT BAWAT ROUTE)
// ==========================================
// Route para sa Areas (Admin at Staff Dashboards)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default Route para sa mga Public Controllers (gaya ng HomeController)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Patakbuhin ang Web Application
app.Run();