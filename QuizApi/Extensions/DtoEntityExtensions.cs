namespace QuizApi.Extensions;

public static class DtoEntityExtensions
{
    public static FlashCardDto ToDto(this FlashCard set)
    {
        if (set == null)
        {
            return null;
        }

        return new FlashCardDto()
        {
            Question = set.Question,
            Answer = set.Answer,
        };
    }

    public static FlashCardSetDto ToDto(this FlashCardSet set)
    {
        if (set == null)
        {
            return null;
        }

        return new FlashCardSetDto()
        {
            Name = set.Name,
            FlashCards = set.FlashCards.Select(flashCard => flashCard.ToDto())
        };
    }
}