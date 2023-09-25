namespace QuizApi.Tests;

public static class SampleQuizzes
{
    public static List<FlashCardSet> Sets = new List<FlashCardSet>()
    {
        new ()
        {
            CreatedAt = DateTime.Parse("12.02.2022"),
            Id = new Guid("4c939673-7f1a-4595-aff1-1a458455ce76"),
            Name = "Quiz1"
        },
        new ()
        {
            CreatedAt = DateTime.Parse("11.01.2023"),
            Id = new Guid("4f15bf7c-7ce8-47d7-b57f-f60978f6455d"),
            Name = "Quiz2"
        },
        new () 
        {
            CreatedAt = DateTime.Parse("02.02.2011"),
            Id = new Guid("ea5e4673-19e1-4fa1-9f66-729c0dc1c275"),
            Name = "Quiz3"
        }
    };
}
