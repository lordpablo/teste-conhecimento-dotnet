namespace SampleTest.Resources.Exceptions;

public class MethodNotAllowedException : Exception
{
    public MethodNotAllowedException(string error)
    {
        this.Error = error;
    }
    public string Error { get; }
}