using System.Text.Json;
using QuizApi.Exceptions;

namespace QuizApi.Services;

public class JsonParser<T> : IFileParser<T> where T : class, new()
{
    public IFormFile File { get; set; }

    public JsonParser(IFormFile file) =>
        File = file;
        
    public T Parse()
    {
        T model;
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };
        try
        {
            using var reader = new StreamReader(File.OpenReadStream());
            model = JsonSerializer.Deserialize<T>(reader.ReadToEnd(), options);
        }
        catch (JsonException e)
        {
            throw new IncorrectFileContentException(File.FileName, e);
        }
        return model ?? throw new IncorrectFileContentException();
    }
}
