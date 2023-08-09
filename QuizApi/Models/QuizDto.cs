namespace QuizApi;

public class QuizDto
{
    public string Name { get; set; }
    public IEnumerable<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
}