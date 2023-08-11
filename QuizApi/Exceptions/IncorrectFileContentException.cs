namespace QuizApi.Exceptions;

public class IncorrectFileContentException : Exception
{
    public IncorrectFileContentException()
    {
    }

    public IncorrectFileContentException(string message) 
        : base(message)
    {
    }

    public IncorrectFileContentException(string message, Exception inner)
        : base(message, inner)
    {
    }
}