namespace SampleTest.Resources.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string error)
    {
        this.Error = error;
    }
    public string Error { get; }
}