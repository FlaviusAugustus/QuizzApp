using QuizApi.Exceptions;

namespace QuizApi.Services;

public class ParserFactory<T> : IParserFactory<T> where T : class, new()
{
    public IFileParser<T> GetParser(IFormFile file) => Path.GetExtension(file.FileName) switch 
    { 
        ".json" => new JsonParser<T>(),
        ".toml" => new TomlParser<T>(),
        ".yaml" => new YamlParser<T>(),
        _ => throw new ArgumentException(file.FileName)
    };
}
