using Microsoft.AspNetCore.Identity;
using QuizApi.Services.UserService;

namespace QuizApi.Extensions;

public static class IdentityConfigExtensions
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<QuizContext>();
        services.Configure<IdentityOptions>(opts =>
        {
            opts.Password.RequireDigit = false;
            opts.Password.RequiredLength = 0;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequiredUniqueChars = 0;
            opts.Password.RequireNonAlphanumeric = false;
        });
        return services;
    }
}