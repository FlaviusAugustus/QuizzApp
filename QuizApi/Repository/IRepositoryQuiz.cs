namespace QuizApi.Repository;

public interface IRepositoryQuiz : IGenericRepository<FlashCardSet>
{ 
    public IEnumerable<FlashCardSet> GetAllIncludeQuestions();
    public IEnumerable<FlashCardSet> GetQuizByName(string name);
    public IEnumerable<FlashCardSet> GetQuizByQuestion(string question);
}
