using System.Text.Json.Serialization;

namespace QuizApi;

public class FlashCard
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    
    [JsonIgnore]
    public FlashCardSet Set { get; set; }
    public Guid SetId { get; set; }
}
