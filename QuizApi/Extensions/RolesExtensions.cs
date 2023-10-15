using Microsoft.AspNetCore.Identity;
using QuizApi.Constants;

namespace QuizApi.Extensions;

public static class RolesExtensions
{
    public static async Task AddRoles(this IServiceCollection services)
    {
        var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        foreach (var role in Enum.GetNames<Role>())
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}