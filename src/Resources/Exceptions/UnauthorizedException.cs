namespace SampleTest.Application.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string error)
    {
        this.Error = error;
    }
    public string Error { get; }
}