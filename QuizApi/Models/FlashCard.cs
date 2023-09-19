using System.Text.Json.Serialization;

namespace QuizApi;

public class FlashCard : IEntity
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    
    [JsonIgnore]
    public FlashCardSet Set { get; set; }
    public Guid SetId { get; set; }
}
