using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Data;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services; // <-- Add this if your services are in a "Services" folder

var builder = WebApplication.CreateBuilder(args);

// Connection string for both contexts
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the Identity DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register your domain-specific context (Competition)
builder.Services.AddDbContext<CompetitionDbContext>(options =>
    options.UseSqlServer(connectionString));

// Helpful error pages for development
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add ASP.NET Identity with default UI and EF store
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Add MVC support
builder.Services.AddControllersWithViews();

// Register your application services
builder.Services.AddScoped<ICompetitionService, CompetitionService>();
builder.Services.AddScoped<IAthleteService, AthleteService>();
builder.Services.AddScoped<IRegionService, RegionService>();



var app = builder.Build();

// Development environment config
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Make sure this is before UseAuthorization
app.UseAuthorization();

// Routing
app.MapDefaultControllerRoute();
app.MapRazorPages(); // Required for Identity pages to work

app.Run();
