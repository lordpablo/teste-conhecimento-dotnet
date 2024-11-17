namespace SampleTest.Resources.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string error)
    {
        this.Error = error;
    }
    public string Error { get; }
}