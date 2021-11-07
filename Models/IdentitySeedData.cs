using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BurgerShop.Models {
    public static class IdentitySeedData {
        private const string adminEmail = "admin@example.com";
        private const string adminPassword = "1234";

        private const string customerEmail = "customer@example.com";
        private const string customerPassword = "1234";

        public static async void SeedDatabase(IApplicationBuilder app) {
            IdentityContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();
            if (context.Database.GetPendingMigrations().Any()) {
                context.Database.Migrate();
            }

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityRole role = new IdentityRole() {Name = "admin"};
            await roleManager.CreateAsync(role);

            UserManager<IdentityUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null) {
                admin = new IdentityUser(adminEmail);
                admin.Email = adminEmail;
                await userManager.CreateAsync(admin, adminPassword);
                await userManager.AddToRoleAsync(admin, "admin");
            }

            IdentityUser customer = await userManager.FindByEmailAsync(customerEmail);
            if (customer == null) {
                customer = new IdentityUser(customerEmail);
                customer.Email = customerEmail;
                await userManager.CreateAsync(customer, customerPassword);
            }
        }
    }
}
