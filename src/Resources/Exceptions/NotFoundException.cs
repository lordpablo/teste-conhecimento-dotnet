namespace SampleTest.Resources.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string error)
    {
        Error = error;
    }
    public string Error { get; }
}