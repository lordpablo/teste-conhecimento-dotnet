namespace SampleTest.Resources.Exceptions;

public class TooManyRequestException : Exception
{
    public TooManyRequestException(string error)
    {
        Error = error;
    }
    public string Error { get; }
}