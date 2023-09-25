using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Exceptions;
using QuizApi.Repository;
using QuizApi.Services;

namespace QuizApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class QuizUploadController : ControllerBase
{
    private readonly IRepositoryQuiz _context;
    private readonly IMapper _mapper;
    private readonly ILogger<QuizUploadController> _logger;

    public QuizUploadController(IRepositoryQuiz context, ILogger<QuizUploadController> logger
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
    public IActionResult GetById(Guid id)
    {
        _logger.LogInformation("Fetching a set by Id");
        var quiz = _context.GetById(id);
        if (quiz is null) 
        {
            return NotFound(quiz);
        }
        return Ok(quiz);
    } 
    
    [HttpPost]
    [Route("upload")]
    public IActionResult Upload(IFormFile file)
    {
        _logger.LogInformation("Importing set");
        var model = ParserFactory<FlashCardSetDto>.GetParser(file)
                .Parse(file);
        _context.Add(_mapper.Map<FlashCardSet>(model));
        _context.Save();
        return CreatedAtAction(nameof(GetAll), model);
    }

    [HttpDelete]
    [Route("remove/{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _logger.LogInformation("Removing set {Id}", id);
        var entity = _context.GetById(id);
        if (entity is null)
        {
            _logger.LogError("Failed to remove set. Set {Id} not found", id);
            return BadRequest(entity);
        }
        _context.Remove(entity);
        _context.Save();
        return Ok(entity);
    }
}