namespace SampleTest.Application.Exceptions;

public class NotImplementedException : Exception
{
    public NotImplementedException(string error)
    {
        this.Error = error;
    }
    public string Error { get; }
}