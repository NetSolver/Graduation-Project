using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Enums;

namespace Smile_Simulation.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider) 
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserApp>>();

        
            string[] roles = { Roles.Admin.ToString(), Roles.Doctor.ToString(), Roles.Patient.ToString() };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

       
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new UserApp
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin User",
                    gender = Gender.Male
                };

                await userManager.CreateAsync(adminUser, "AdminPassword123!");
                await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
            }
        }
    }
}
