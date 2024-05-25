using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Q.Data;
using Q.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<User> userManager)
    {
        using (var context = new QuizDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<QuizDbContext>>()))
        {
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var user = new User
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Password123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}