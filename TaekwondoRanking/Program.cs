using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Data;
using TaekwondoRanking.Mappings;
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
//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//})
//.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        context.Response.Redirect("/Error/401");
        return Task.CompletedTask;
    };
});


// Add MVC support
builder.Services.AddControllersWithViews();

// Register your application services
builder.Services.AddScoped<ICompetitionService, CompetitionService>();
builder.Services.AddScoped<IAthleteService, AthleteService>();
builder.Services.AddScoped<IRegionService, RegionService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddResponseCaching(); // <--- ADD THIS LINE

// Configure authentication options (example, adjust if you have specific needs)
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Error/401"; // Redirect to your custom 401 error page
    options.SlidingExpiration = true;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await IdentityDataInitializer.SeedRolesAndAdminUserAsync(scope.ServiceProvider);
}

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

app.UseResponseCaching(); // <--- ADD THIS LINE

app.UseAuthentication(); // Make sure this is before UseAuthorization
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}");


// Routing
app.MapDefaultControllerRoute();
app.MapRazorPages(); // Required for Identity pages to work

app.Run();