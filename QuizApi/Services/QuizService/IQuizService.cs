using LanguageExt;
using LanguageExt.Common;

namespace QuizApi.Services.QuizService;

public interface IQuizService
{
    public Task CreateQuizAsync(FlashCardSet set);
    public Task<Result<FlashCardSet>> GetQuizAsync(Guid id);
    public Task<IEnumerable<FlashCardSet>> GetAllQuizzesAsync();
    public Task<Result<FlashCardSet>> CreateQuizFromFileAsync(IFormFile file);
    public Task<Result<Unit>> RemoveQuizAsync(Guid id);
}