namespace QuizApi.Repository;

public interface IRepositoryQuiz : IGenericRepository<FlashCardSet>
{ 
    public Task<IEnumerable<FlashCardSet>> GetAllIncludeQuestions();
    public Task<IEnumerable<FlashCardSet>> GetQuizByName(string name);
    public Task<IEnumerable<FlashCardSet>> GetQuizByQuestion(string question);
}
