using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineStoreInventory;
using OnlineStoreInventory.DataBase;

public class ApplicationDbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Roles.Any())
        {
            var roles = new[] { "Admin", "User", "Manager" };
            foreach (var role in roles)
            {
                roleManager.CreateAsync(new IdentityRole(role)).Wait();
            }
        }

        if (!context.Users.Any())
        {
            var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
            var result = userManager.CreateAsync(user, "Password123!").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }
}