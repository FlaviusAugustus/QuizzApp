using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuizApi.Exceptions.Filters;

public class ParsingExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _hostEnvironment;

    public ParsingExceptionFilter(IHostEnvironment hostEnvironment) =>
        _hostEnvironment = hostEnvironment;

    public void OnException(ExceptionContext context)
    {
        context.Result = context.Exception switch
        {
            IncorrectFileContentException or 
            ArgumentException => new BadRequestResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}