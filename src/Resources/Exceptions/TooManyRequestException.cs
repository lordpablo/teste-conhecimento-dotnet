namespace SampleTest.Application.Exceptions;

public class TooManyRequestException : Exception
{
    public TooManyRequestException(string error)
    {
        this.Error = error;
    }
    public string Error { get; }
}