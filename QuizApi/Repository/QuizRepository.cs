using Microsoft.EntityFrameworkCore;

namespace QuizApi.Repository;

public class QuizRepository : GenericRepository<FlashCardSet>, IRepositoryQuiz
{
    public QuizRepository(QuizContext context) : base(context) {}

    public async Task<IEnumerable<FlashCardSet>> GetAllIncludeQuestions() =>
        await _context.Set<FlashCardSet>().Include(s => s.FlashCards).ToListAsync();

    public async Task<IEnumerable<FlashCardSet>> GetQuizByName(string name) =>
        await _context.Set<FlashCardSet>().Where(s => s.Name == name).Include(s => s.FlashCards).ToListAsync();

    public async Task<IEnumerable<FlashCardSet>> GetQuizByQuestion(string question)
    {
        throw new NotImplementedException();
    }
}