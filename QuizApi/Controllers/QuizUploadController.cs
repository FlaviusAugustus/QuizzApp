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
    private readonly ILogger<QuizUploadController> _logger;

    public QuizUploadController(QuizContext context, ILogger<QuizUploadController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [HttpGet]
    [Route("get")]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Fetching all sets");
        var query = _context.Sets
            .Include(x => x.FlashCards);
        return Ok(query);
    }

    [HttpGet]
    [Route("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Fetching a set by Id");
        var quiz = await _context.Sets.FindAsync(id);
        if (quiz is null) 
        {
            return NotFound();
        }
        return Ok(quiz);
    } 
    
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        _logger.LogInformation("Importing set");
        FlashCardSetDto model;
        try
        {
            model = ParserFactory<FlashCardSetDto>.GetParser(file)
                .Parse(file);
        }
        catch (IncorrectFileContentException e)
        {
            _logger.LogError(e, "Incorrect file sent: {FileName}", file.FileName);
            return BadRequest();
        }
        await _context.AddAsync(model);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), model);
    }

    [HttpDelete]
    [Route("remove/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Removing set {Id}", id);
        var entity = await _context.Sets.FindAsync(id);
        if (entity is null)
        {
            _logger.LogError("Failed to remove set. Set {Id} not found", id);
            return BadRequest();
        }
        _context.Sets.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok();
    }
}