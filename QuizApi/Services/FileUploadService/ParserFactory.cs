namespace QuizApi.Services;

public static class ParserFactory<T> where T : class, new()
{
    public static IFileParser<T> GetParser(IFormFile file) => Path.GetExtension(file.FileName) switch 
    { 
        ".json" => new JsonParser<T>(file),
        ".toml" => new TomlParser<T>(file),
        ".yaml" => new YamlParser<T>(file),
        _ => throw new ArgumentException(file.FileName)
    };
}
