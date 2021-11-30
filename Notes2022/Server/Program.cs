// Server side

using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

// database providers

using Microsoft.EntityFrameworkCore;

//using Npgsql.EntityFrameworkCore.PostgreSQL;
//using IBM.EntityFrameworkCore;
//using MySql.Data.EntityFrameworkCore;
//using Pomelo.EntityFrameworkCore.MySql;

// end database providers

using Notes2022.Server;
using Notes2022.Server.Data;
using Notes2022.Server.Models;
using Notes2022.Server.Services;

/*
 *      Partial list of possible database providers
 * 
Database 	            Package
SQLite	                Microsoft.EntityFrameworkCore.Sqlite
Microsoft SQL Server	Microsoft.EntityFrameworkCore.SqlServer
Npgsql (PostgreSQL)     Npgsql.EntityFrameworkCore.PostgreSQL
IBM Data Servers	    IBM.EntityFrameworkCore
MySQL (Official)	    MySql.Data.EntityFrameworkCore
Pomelo (MySQL)	        Pomelo.EntityFrameworkCore.MySql
InMemory (for Testing)	Microsoft.EntityFrameworkCore.InMemory * 
 * 
 * See full list at: https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli
 */

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Later we could add a switch to select from the above databases via
// appsettings.json config value OR eaasier, just select the appropriate 
// commented out using above and modify the statement below...
// ... need to add the NuGet Package to the project too.

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseSqlServer(connectionString));

// end of switchable db section

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NotesDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, NotesDbContext>();

//builder.Services.AddSignalR();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});

builder.Services.AddHangfireServer();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization();

builder.Services.AddRazorPages();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

Globals.StartupDateTime = DateTime.Now.ToUniversalTime();

Globals.ProductionUrl = builder.Configuration["ProductionUrl"];

Globals.ImportRoot = builder.Configuration["ImportRoot"];

Globals.TimeZoneDefaultID = 54; // not needed with blazor

Globals.SendGridApiKey = builder.Configuration["SendGridApiKey"];
Globals.SendGridEmail = builder.Configuration["SendGridEmail"];
Globals.SendGridName = builder.Configuration["SendGridName"];

Globals.DBConnectString = builder.Configuration.GetConnectionString("DefaultConnection");

Globals.PrimeAdminName = "Dale Sinder";
Globals.PrimeAdminEmail = "sinder@illinois.edu";

Globals.HangfireLoc = Guid.NewGuid().ToString();

try  // replace default with config values,  fails during migration so try
{
    Globals.PrimeAdminName = builder.Configuration["PrimeAdminName"];
    Globals.PrimeAdminEmail = builder.Configuration["PrimeAdminEmail"];
}
catch
{
    //ignore
}

var app = builder.Build();

//app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.UsePathBase(Globals.PathBase);

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/" + Globals.HangfireLoc, new DashboardOptions
{
    Authorization = new[] { new MyAuthorizationFilter() }
});

app.MapRazorPages();
app.MapControllers();

app.MapFallbackToFile("index.html");

//EmailSender s = new EmailSender();
//BackgroundJob.Enqueue(() => s.SendEmailAsync("sinder@illinois.edu", "Notes 2022", "Startup"));

app.Run();


public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var cookies = context.GetHttpContext().Request.Cookies;
        bool test = cookies.ContainsKey("IsAdmin");

        return test;
    }
}