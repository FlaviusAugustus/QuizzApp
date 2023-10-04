namespace QuizApi.Services;

public interface IParserFactory<out T> where T : class
{
    public IFileParser<T> GetParser(IFormFile file);
}