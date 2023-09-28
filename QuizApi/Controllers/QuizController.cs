using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Exceptions;
using QuizApi.Repository;
using QuizApi.Services;

namespace QuizApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IRepositoryQuiz _context;
    private readonly IMapper _mapper;
    private readonly ILogger<QuizController> _logger;

    public QuizController(IRepositoryQuiz context, ILogger<QuizController> logger
    , IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("get")]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Fetching all sets");
        var allQuizzes = _context.GetAllIncludeQuestions();
        return Ok(allQuizzes);
    }

    [HttpGet]
    [Route("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Fetching a set by Id");
        var quiz = await _context.GetByIdAsync(id);
        if (quiz is null) 
        {
            return NotFound(quiz);
        }
        return Ok(quiz);
    } 
    
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        _logger.LogInformation("Importing set");
        var model = ParserFactory<FlashCardSetDto>.GetParser(file)
                .Parse(file);
        _context.Add(_mapper.Map<FlashCardSet>(model));
        await _context.SaveAsync();
        return CreatedAtAction(nameof(GetAll), model);
    }

    [HttpDelete]
    [Route("remove/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Removing set {Id}", id);
        var entity = await _context.GetByIdAsync(id);
        if (entity is null)
        {
            _logger.LogError("Failed to remove set. Set {Id} not found", id);
            return BadRequest(entity);
        }
        _context.Remove(entity);
        await _context.SaveAsync();
        return Ok(entity);
    }
}