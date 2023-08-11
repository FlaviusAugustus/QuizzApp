namespace QuizApi.Services;

public interface IFileParser<out T> where T : class
{
    public T Parse();
}