using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute.ReturnsExtensions;
using QuizApi.Controllers;
using QuizApi.Repository;

namespace QuizApi.Tests;

public class QuizControllerTests
{
    private readonly IRepositoryQuiz _service = Substitute.For<IRepositoryQuiz>();
    private readonly ILogger<QuizUploadController> _logger = Substitute.For<ILogger<QuizUploadController>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly QuizUploadController _sut;

    public QuizControllerTests()
    {
        _sut = new(_service, _logger, _mapper);
    }
    
    [Fact]
    public void GetAll_Returns_AllQuizzes()
    {
        // Arrange
        _service.GetAllIncludeQuestions().Returns(SampleQuizzes.Sets);
        
        // Act
        var result = (OkObjectResult)_sut.GetAll();
        
        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(SampleQuizzes.Sets, (List<FlashCardSet>)result.Value);
    }

    [Fact]
    public void GetById_ProvidedInputIsCorrect_ShouldReturn200Status()
    {
        // Arrange
        var quiz = SampleQuizzes.Sets.First();
        _service.GetById(quiz.Id).Returns(quiz);
        
        // Act
        var result = (OkObjectResult)_sut.GetById(quiz.Id);
        
        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(quiz, (FlashCardSet)result.Value);
    }

    [Fact]
    public void GetById_WithIncorrectInput_ShouldReturn404Status()
    {
        // Arrange
        _service.GetById(Arg.Any<Guid>()).ReturnsNull();
        
        // Act
        var result = (NotFoundObjectResult)_sut.GetById(Guid.NewGuid());
        
        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Delete_ProvidedInputIsCorrect_ShouldReturn200StatusAndRemoveQuiz()
    {
        // Arrange
        var quizList = new List<FlashCardSet>(SampleQuizzes.Sets);
        var toRemove = quizList.First();
        _service.GetById(toRemove.Id).Returns(toRemove);
        _service.When(x => x.Remove(toRemove))
                .Do(_ => quizList.Remove(toRemove));
        
        // Act
        var result = (OkObjectResult)_sut.Delete(toRemove.Id);
        
        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.DoesNotContain(quizList, x => x == toRemove);
    }

    [Fact]
    public void Delete_WithIncorrectInput_ShouldReturn400Status()
    {
        // Arrange
        _service.GetById(Arg.Any<Guid>()).ReturnsNull();
        
        // Act
        var result = (BadRequestObjectResult)_sut.Delete(Guid.NewGuid());
        
        // Assert
        Assert.Equal(400, result.StatusCode);
    }
}