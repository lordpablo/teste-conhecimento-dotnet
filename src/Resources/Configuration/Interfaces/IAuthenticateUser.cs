namespace SampleTest.Resources.Configuration.Interfaces
{
    public interface IAuthenticateUser
    {
        bool IsAuthenticated();
        int GetUserId();
    }
}
