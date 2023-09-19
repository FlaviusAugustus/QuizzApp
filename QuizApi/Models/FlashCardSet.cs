namespace QuizApi;

public class FlashCardSet : IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Name { get; set; }
    public int FlashCardCount => FlashCards.Count();
    public IEnumerable<FlashCard> FlashCards { get; set; } = new List<FlashCard>();

}