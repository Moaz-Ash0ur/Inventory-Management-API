
using Microsoft.AspNetCore.Identity;
using Inventory.DAL.Data;
using Inventory.DAL.Models;

namespace InventoryApi.Helper
{
    public class ContextConfig
    {
        public static async Task SeedDataAsync(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedUserAsync(userManager, roleManager);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist
            var roles = new[]
            {
              SystemRoles.Admin,
              SystemRoles.InventoryManager,
              SystemRoles.Salesperson,
              SystemRoles.PurchasingOfficer,
              SystemRoles.Accountant
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Ensure admin user exists
            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "01060528766"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, SystemRoles.Admin);
                }
            }


            //Other Roles
            await CreateUserIfNotExists(userManager, SystemRoles.InventoryManager, "inventory@gmail.com.com", "Inventory123", "Inventory", "Manager", "01022223334");
            await CreateUserIfNotExists(userManager, SystemRoles.Salesperson, "sales@gmail.com.com", "Sales123!", "Sales", "Person", "01033334445");
            await CreateUserIfNotExists(userManager, SystemRoles.PurchasingOfficer, "purchasing@gmail.com.com", "Purchase123", "Purchasing", "Officer", "01044445556");
            await CreateUserIfNotExists(userManager, SystemRoles.Accountant, "accountant@gmail.com.com", "Account123", "Account", "Ant", "01055556667");

        }


        private static async Task CreateUserIfNotExists(
        UserManager<ApplicationUser> userManager,string role,string email,
        string password,string firstName,string lastName,string phone)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phone
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, role); 
                   Console.WriteLine("Done Sucess");

            }
        }



    }


}


