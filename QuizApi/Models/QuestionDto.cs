namespace QuizApi;

public class QuestionDto
{
    public string Content { get; set; }
    public string CorrectAnwser { get; set; }
    public IEnumerable<string> WrongAnswers { get; set; } = new List<string>();
}