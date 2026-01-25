using Microsoft.AspNetCore.Identity;
using stepik.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Db
{
    public class IdentityInitializer
    {
        public static void Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@gmail.com";
            var password = "_Aa123456";
            if (roleManager.FindByNameAsync(Consts.AdminRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Consts.AdminRoleName)).Wait();
            }
            if (roleManager.FindByNameAsync(Consts.UserRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Consts.UserRoleName)).Wait();
            }
            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail , FirstName = "Admin", LastName = "Admin" };
                var result = userManager.CreateAsync(admin, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, Consts.AdminRoleName).Wait();
                }
            }
        }
    }
}
