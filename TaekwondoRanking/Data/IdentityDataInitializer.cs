using Microsoft.AspNetCore.Identity;
using TaekwondoRanking.Models;

namespace TaekwondoRanking.Data
{
    public class IdentityDataInitializer
    {
        public static async Task SeedRolesAndAdminUserAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string adminRoleName = "Admin";
            string adminEmail = "admin@taekwondo.com";
            string adminPassword = "AdminPassword123!"; // IMPORTANT: Change this for a real application!

            // 1. Create the "Admin" role if it doesn't exist
            if (await roleManager.FindByNameAsync(adminRoleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            // 2. Create the admin user if it doesn't exist
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                // 3. Assign the new user to the "Admin" role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRoleName);
                }
            }
        }
    }
}