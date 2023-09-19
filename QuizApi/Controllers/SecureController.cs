using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PostData()
    {
        return Ok("secure data bro");
    }
}