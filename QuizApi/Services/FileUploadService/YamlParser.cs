﻿using QuizApi.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace QuizApi.Services;

public class YamlParser<T> : IFileParser<T> where T : class
{
    public IFormFile File { get; set; }

    public YamlParser(IFormFile file) =>
        File = file;

    public T Parse()
    {
        T model;
        var deserializer = new DeserializerBuilder().Build();
        try
        {
            using var reader = new StreamReader(File.OpenReadStream());
            model = deserializer.Deserialize<T>(reader);
        }
        catch (YamlException e)
        {
            throw new IncorrectFileContentException(File.FileName, e);
        }
        return model;
    }
}