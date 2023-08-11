using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Exceptions;
using QuizApi.Services;

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
        var query = _context.Sets.Include(x => x.FlashCards);
        return Ok(query);
    }

    [HttpGet]
    [Route("/{id:int}")]
    public async Task<IActionResult> GetQuizById(int id)
    {
        var quiz = await _context.Sets.FindAsync(id);
        if (quiz is null) 
        {
            return NotFound();
        }
        return Ok(quiz);
    } 
    
    [HttpPost]
    public async Task<IActionResult> UploadQuiz(IFormFile file)
    {
        FlashCardSetDto model;
        try
        {
            model = ParserFactory<FlashCardSetDto>.GetParser(file)
                .Parse();
        }
        catch (IncorrectFileContentException e)
        {
            return BadRequest(e);
        }
        _context.Add(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllQuizzes), model);
    }
}