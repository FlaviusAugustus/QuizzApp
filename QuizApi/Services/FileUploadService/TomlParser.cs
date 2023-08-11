using QuizApi.Exceptions;
using Tomlyn;

namespace QuizApi.Services;

public class TomlParser<T> : IFileParser<T> where T : class, new()
{
    public IFormFile File { get; set; }

    public TomlParser(IFormFile file) =>
        File = file;
        
    public T Parse()
    {
        T model;
        var options = new TomlModelOptions
        {
            ConvertPropertyName = s => s
        };
        try
        {
            using var reader = new StreamReader(File.OpenReadStream());
            model = Toml.ToModel<T>(reader.ReadToEnd(), null, options);
        }
        catch (TomlException e)
        {
            throw new IncorrectFileContentException(File.FileName, e);
        }
        return model;
    }
}