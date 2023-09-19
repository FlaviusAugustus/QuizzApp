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
        return Ok(result);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync(TokenRequestModel requestModel)
    {
        var result = await _userService.GetTokenAsync(requestModel);
        return Ok(result.Token);
    }

    [HttpPost]
    [Route("add-role")]
    public async Task<IActionResult> AddToRole(AddRoleModel roleModel)
    {
        var result = await _userService.AddToRoleAsync(roleModel);
        return Ok(result);
    }
}