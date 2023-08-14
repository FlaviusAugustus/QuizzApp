using QuizApi.Exceptions;
using Tomlyn;

namespace QuizApi.Services;

public class TomlParser<T> : IFileParser<T> where T : class, new()
{
    public T Parse(IFormFile file)
    {
        T model;
        var options = new TomlModelOptions
        {
            ConvertPropertyName = s => s
        };
        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            model = Toml.ToModel<T>(reader.ReadToEnd(), null, options);
        }
        catch (TomlException e)
        {
            throw new IncorrectFileContentException(file.FileName, e);
        }
        return model;
    }
}