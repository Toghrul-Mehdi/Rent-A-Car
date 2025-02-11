using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Rent_A_Car.BL.Services.Abstract;
using Rent_A_Car.BL.Services.Implement;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using Rent_A_Car.MVC.Extension;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddIdentity<User, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IBrandService, BrandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseUserSeed();
app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();


app.MapControllerRoute(name: "areas",
pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStaticFiles();

app.Run();
