using System.Text.Json;
using QuizApi.Exceptions;

namespace QuizApi.Services;

public class JsonParser<T> : IFileParser<T> where T : class
{
    public T Parse(IFormFile file)
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
            using var reader = new StreamReader(file.OpenReadStream());
            model = JsonSerializer.Deserialize<T>(reader.ReadToEnd(), options);
        }
        catch (JsonException)
        {
            throw new IncorrectFileContentException(file.FileName);
        }
        return model ?? throw new IncorrectFileContentException();
    }
}
