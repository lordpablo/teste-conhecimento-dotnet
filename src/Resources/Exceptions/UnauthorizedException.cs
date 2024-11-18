namespace SampleTest.Resources.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string error)
    {
        Error = error;
    }
    public string Error { get; }
}