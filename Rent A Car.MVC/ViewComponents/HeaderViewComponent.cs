using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rent_A_Car.BL.DTOs.Profile;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.DAL.Context;

namespace Rent_A_Car.MVC.ViewComponents
{
    public class HeaderViewComponent(UserManager<User> _userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View(null);
            }

            var model = new ProfileDTO
            {
                Fullname = user.Fullname, 
                Username = user.UserName,
                Email = user.Email,
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "/images/profile/1.jpg" : user.ImageUrl
            };

            return View(model);
        }
    }
}
