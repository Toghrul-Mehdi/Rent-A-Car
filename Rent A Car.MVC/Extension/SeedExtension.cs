﻿using Microsoft.AspNetCore.Identity;
using Rent_A_Car.CORE.Entities;
using Rent_A_Car.CORE.Enums;

namespace Rent_A_Car.MVC.Extension
{
    public static class SeedExtension
    {

        public async static void UseUserSeed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //rollarin elave edilmesi

                if (!roleManager.Roles.Any())
                {
                    foreach (Roles role in Enum.GetValues(typeof(Roles)))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                    }
                }


                //adminin elave edilmesi 

                if (!userManager.Users.Any(x => x.NormalizedUserName == "ADMIN"))
                {
                    User admin = new User
                    {
                        Fullname = "Admin",
                        UserName = "Admin",
                        Email = "admin@gmail.com",
                        ImageUrl = "admin.jpg"
                    };

                    await userManager.CreateAsync(admin, "Admin123.");
                    await userManager.AddToRoleAsync(admin, nameof(Roles.Admin));
                }
            }
        }

    }
}
