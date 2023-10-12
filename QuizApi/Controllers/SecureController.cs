using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApi.Constants;

namespace QuizApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetData()
    {
        return Ok("So secure omg");
    }

    [HttpPost]
    [Authorize(Policy = nameof(Policies.CanAccessSecureController))]
    public async Task<IActionResult> PostData()
    {
        return Ok("secure data bro");
    }
}