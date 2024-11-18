namespace SampleTest.Resources.Exceptions;

public class NotImplementedException : Exception
{
    public NotImplementedException(string error)
    {
        Error = error;
    }
    public string Error { get; }
}