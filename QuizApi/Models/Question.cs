using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuizApi;

public class Question
{
    public Guid Id { get; set; }
    public string Content { get; set; } 
    public string CorrectAnwser { get; set; }
    public ICollection<Answer> WrongAnwsers { get; set; } = new List<Answer>();
    
    public Guid QuizId { get; set; }
    [JsonIgnore]
    public Quiz Quiz { get; set; }
}