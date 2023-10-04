using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Exceptions;
using QuizApi.Repository;
using QuizApi.Services;
using QuizApi.Services.QuizService;

namespace QuizApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService service)
    {
        _quizService = service;
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetAll()
    {
        var allQuizzes = await _quizService.GetAllQuizzesAsync();
        return Ok(allQuizzes);
    }

    [HttpGet]
    [Route("get/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _quizService.GetQuizAsync(id);
        return result.Match<IActionResult>(
            success => Ok(success),
            fail => NotFound(fail.Message)
        );
    } 
    
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _quizService.CreateQuizFromFileAsync(file);
        return result.Match<IActionResult>(
            success => CreatedAtAction(nameof(GetAll), success),
            fail => BadRequest(fail.Message)
        );
    }

    [HttpDelete]
    [Route("remove/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _quizService.RemoveQuizAsync(id);
        return result.Match<IActionResult>(
            success => Ok(),
            fail => NotFound(fail.Message)
            );
    }
}