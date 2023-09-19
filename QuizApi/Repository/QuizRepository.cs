using Microsoft.EntityFrameworkCore;

namespace QuizApi.Repository;

public class QuizRepository : GenericRepository<FlashCardSet>, IRepositoryQuiz
{
    public QuizRepository(QuizContext context) : base(context) {}

    public IEnumerable<FlashCardSet> GetAllIncludeQuestions() =>
        _context.Set<FlashCardSet>().Include(s => s.FlashCards);

    public IEnumerable<FlashCardSet> GetQuizByName(string name) =>
        _context.Set<FlashCardSet>().Where(s => s.Name == name).Include(s => s.FlashCards);

    public IEnumerable<FlashCardSet> GetQuizByQuestion(string question)
    {
        throw new NotImplementedException();
    }
}