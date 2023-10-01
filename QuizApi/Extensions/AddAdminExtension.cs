using Microsoft.AspNetCore.Identity;
using QuizApi.Constants;
using QuizApi.Services.UserService;

namespace QuizApi.Extensions;

public static class AddAdminExtension
{
    public static void AddAdmin(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetService<IUserService>();
        var config = app.Configuration.GetSection("AdminDefaults");
        var admin = new RegisterModel
        {
            UserName = config["UserName"],
            Email = config["Email"],
            Password = config["Password"]
        };
        service.RegisterAsync(admin);
        var roleAdmin = new AddRoleModel
        {
            UserName = config["UserName"],
            Role = Roles.Admin.ToString()
        };
    }
}