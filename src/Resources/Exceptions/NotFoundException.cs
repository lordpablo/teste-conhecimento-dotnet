namespace SampleTest.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string error)
    {
        this.Error = error;
    }
    public string Error { get; }
}