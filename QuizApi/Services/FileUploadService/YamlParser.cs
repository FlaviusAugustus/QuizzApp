using QuizApi.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace QuizApi.Services;

public class YamlParser<T> : IFileParser<T> where T : class
{
    public T Parse(IFormFile file)
    {
        T model;
        var deserializer = new DeserializerBuilder().Build();
        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            model = deserializer.Deserialize<T>(reader);
        }
        catch (YamlException e)
        {
            throw new IncorrectFileContentException(file.FileName, e);
        }
        return model;
    }
}