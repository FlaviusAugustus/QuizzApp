using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApi;

public class Quiz
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int QuestionCount => Questions.Count();
    public IEnumerable<Question> Questions { get; set; } = new List<Question>();

}