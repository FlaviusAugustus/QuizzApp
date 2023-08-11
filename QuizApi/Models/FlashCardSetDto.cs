namespace QuizApi;

public class FlashCardSetDto
{
    public string Name { get; set; }
    public IEnumerable<FlashCardDto> FlashCards { get; set; } = new List<FlashCardDto>();
}