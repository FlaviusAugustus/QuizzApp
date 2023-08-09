using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QuizApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class QuizUploadController : ControllerBase
{
    private readonly QuizContext _context;

    public QuizUploadController(QuizContext context) =>
        _context = context;
    
    [HttpGet]
    public IActionResult GetAllQuizzes()
    {
        var query = _context.Quizzes.Include(x => x.Questions);
        return Ok(query);
    }

    [HttpGet]
    [Route("/{id:int}")]
    public async Task<IActionResult> GetQuizById(int id)
    {
        var quiz = await _context.Quizzes.FindAsync(id);
        if (quiz is null) 
        {
            return NotFound();
        }
        return Ok(quiz);
    } 
    
    [HttpPost]
    public async Task<IActionResult> UploadQuiz(IFormFile file)
    {
        var json = await JsonSerializer.DeserializeAsync<QuizDto>(file.OpenReadStream());
        if (json is null) 
        {
            return BadRequest();
        }
        await _context.AddAsync(json);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllQuizzes), json);
    }

    
    

}