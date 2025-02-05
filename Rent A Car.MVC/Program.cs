using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.Services.Abstract;
using Rent_A_Car.BL.Services.Implement;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<ICategoryService, CategoryService>();



builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
     name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
