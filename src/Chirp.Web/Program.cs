using Chirp.Infrastructure;
using Chirp.Core;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "";
// Check if code is running in production environment (like Azure)
if (builder.Environment.IsProduction())
{
    //ChatGPT help here
    connectionString = builder.Configuration.GetConnectionString("AzureSQL")
                       ?? throw new InvalidOperationException(
                           "AzureSQL connection string not found.  Configure it in Azure Portal.");

    builder.Services.AddDbContext<CheepDBContext>(options => options.UseSqlServer(connectionString));
} 
else 
{ 
    connectionString = builder. Configuration.GetConnectionString("DefaultConnection")
                       ??  "Data Source=Chirp.db";
    builder.Services.AddDbContext<CheepDBContext>(options => options.UseSqlite(connectionString));
} 

// Adds the Identity services to the DI container and uses a custom user type, ApplicationUser
builder.Services.AddDefaultIdentity<Author>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
        options.Lockout.AllowedForNewUsers = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_ ";
    })
    .AddEntityFrameworkStores<CheepDBContext>();

builder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
// HSTS necessary for the HSTS header for reasons
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromHours(1);
});
// github authentication
builder.Services.AddAuthentication()
    .AddGitHub(githubOptions =>
    {
        githubOptions.ClientId = builder.Configuration["authentication:github:clientId"];
        githubOptions.ClientSecret = builder.Configuration["authentication:github:clientSecret"];
        githubOptions.CallbackPath = "/signin-github";
        githubOptions.Scope.Add("user:email"); // Explicitly asking for Email as Github can be difficult to get Email from
        githubOptions.SaveTokens = true; // maybe

        githubOptions.CorrelationCookie.SameSite = SameSiteMode.None;
        githubOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
    })
    .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["authentication:google:clientId"];
            googleOptions.ClientSecret = builder.Configuration["authentication:google:clientSecret"];
            googleOptions.CallbackPath = "/signin-google";

            // Optional: get additional info like profile or email
            googleOptions.Scope.Add("profile");
            googleOptions.Scope.Add("email");


            googleOptions.SaveTokens = true;

            // Ensure cookies are secure for cross-site auth
            googleOptions.CorrelationCookie.SameSite = SameSiteMode.None;
            googleOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Create a disposable service scope
using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetRequiredService<CheepDBContext>();
    context.Database.Migrate();
    if (app.Environment.EnvironmentName != "Testing")
    {
        DbInitializer.SeedDatabase(context);
    }
}

if(app.Environment.IsProduction())
{
    app.UseHsts(); // Send HSTS headers, but only in production
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
