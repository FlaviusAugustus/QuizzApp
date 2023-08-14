using QuizApi.Exceptions;

namespace QuizApi.Services;

public static class ParserFactory<T> where T : class, new()
{
    public static IFileParser<T> GetParser(IFormFile file) => Path.GetExtension(file.FileName) switch 
    { 
        ".json" => new JsonParser<T>(),
        ".toml" => new TomlParser<T>(),
        ".yaml" => new YamlParser<T>(),
        _ => throw new IncorrectFileContentException(file.FileName)
    };
}
