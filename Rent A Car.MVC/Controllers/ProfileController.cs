﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.BL.DTOs.Profile;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;
using System.Security.Claims;

namespace Rent_A_Car.MVC.Controllers
{
    public class ProfileController(UserManager<User> _userManager,AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index(string username)
        {

            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var favouriteList = await _context.WishLists
                .Where(x => x.UserId == userId)
                .Include(x => x.Advertisement)
                .ThenInclude(x=>x.Brand)
                .Include(x=>x.Advertisement)
                .ThenInclude(x=>x.Category)
                .Include(x => x.User)
                .ToListAsync();

            if (favouriteList == null || !favouriteList.Any())  
            {
                ViewBag.Favourite = new List<WishList>();  
            }
            else
            {
                ViewBag.Favourite = favouriteList; 
            }

            var model = new ProfileDTO
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "/images/profile/photo.jpg" : user.ImageUrl
            };

            return View(model);
        }
        public async Task<IActionResult> FavouriteCar(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var favouriteList = await _context.WishLists
               .Where(x => x.UserId == userId)
               .Include(x => x.Advertisement)
               .ThenInclude(x => x.Brand)
               .Include(x => x.Advertisement)
               .ThenInclude(x => x.Category)
               .Include(x => x.User)
               .ToListAsync();

            if (favouriteList == null || !favouriteList.Any())
            {
                ViewBag.Favourite = new List<WishList>();
            }
            else
            {
                ViewBag.Favourite = favouriteList;
            }

            var model = new ProfileDTO
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "/images/profile/photo.jpg" : user.ImageUrl
            };

            return View(model);
        }
    }
}
