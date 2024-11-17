namespace SampleTest.Resources.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string error, Dictionary<string, string[]> errors)
    {
        this.Error = error;
        this.Errors = errors;
    }
    public string Error { get; }
    public Dictionary<string, string[]> Errors { get; }
}