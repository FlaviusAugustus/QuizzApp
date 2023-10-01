using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Services.UserService;

namespace QuizApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService) =>
        _userService = userService;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync(RegisterModel registerModel)
    {
        var result = await _userService.RegisterAsync(registerModel);
        return result.Match<IActionResult>(
            success => Ok(success),
            fail => BadRequest(fail.Message));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync(TokenRequestModel requestModel)
    {
        var result = await _userService.GetTokenAsync(requestModel);
        return result.Match<IActionResult>(
            success => success is null ? NotFound() : Ok(success),
            fail => BadRequest(fail.Message));
    }

    [HttpPost]
    [Route("add-role")]
    public async Task<IActionResult> AddToRole(AddRoleModel roleModel)
    {
        var result = await _userService.AddToRoleAsync(roleModel);
        return result.Match<IActionResult>(
            success => Ok(),
            fail => BadRequest(fail.Message));
    }
}