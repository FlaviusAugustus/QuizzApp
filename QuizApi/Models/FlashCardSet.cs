namespace QuizApi;

public class FlashCardSet
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int FlashCardCount => FlashCards.Count();
    public IEnumerable<FlashCard> FlashCards { get; set; } = new List<FlashCard>();

}