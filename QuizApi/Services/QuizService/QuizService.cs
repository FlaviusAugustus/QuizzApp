using LanguageExt;
using LanguageExt.Common;
using QuizApi.Extensions;
using QuizApi.Repository;

namespace QuizApi.Services.QuizService;

public class QuizService : IQuizService
{
    private readonly IRepositoryQuiz _repository;
    private readonly IParserFactory<FlashCardSet> _quizParser;

    public QuizService(IRepositoryQuiz repository, IParserFactory<FlashCardSet> parser)
    {
        _repository = repository;
        _quizParser = parser;
    }

    public async Task CreateQuizAsync(FlashCardSet set)
    {
        _repository.Add(set);
        await _repository.SaveAsync();
    }

    public async Task<Result<FlashCardSet>> GetQuizAsync(Guid id)
    {
        var quiz = await _repository.GetByIdAsync(id);
        if (quiz is null)
        {
            var notFound = new ArgumentException($"No quiz with id {id}");
            return new Result<FlashCardSet>(notFound);
        }
        return new Result<FlashCardSet>(quiz);
    }

    public async Task<IEnumerable<FlashCardSet>> GetAllQuizzesAsync()
    {
        return await _repository.GetAllIncludeQuestions();
    }

    public async Task<Result<FlashCardSet>> CreateQuizFromFileAsync(IFormFile file)
    {
        var quiz = _quizParser.GetParser(file).Parse(file);
        if (quiz is null)
        {
            var error = new ArgumentException("Incorrect file");
            return new Result<FlashCardSet>(error);
        }
        _repository.Add(quiz);
        await _repository.SaveAsync();
        
        return new Result<FlashCardSet>(quiz);
    }

    public async Task<Result<Unit>> RemoveQuizAsync(Guid id)
    {
        var quiz = await  _repository.GetByIdAsync(id);
        if (quiz is null)
        {
            var error = new ArgumentException($"No quiz with id {id}");
            return new Result<Unit>(error);
        }
        return new Result<Unit>();
    }
}