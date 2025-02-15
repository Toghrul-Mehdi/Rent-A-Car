using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.Services.Abstract;
using Rent_A_Car.BL.Services.Implement;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database context configuration
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));

// Configure JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

// Add application services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IBrandService, BrandService>();

// Authentication configuration (fixing the missing cookie authentication)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Handle 404 errors
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 404)
    {
        response.Redirect("/Home/NotFound");
    }
});


// Enable routing
app.UseRouting();


app.UseAuthorization();   // Add authorization middleware

// Map controller routes
app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// User seeding middleware (if defined in an extension method)
app.UseUserSeed();

app.Run();
