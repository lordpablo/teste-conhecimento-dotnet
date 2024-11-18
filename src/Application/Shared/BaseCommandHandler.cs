using SampleTest.Resources.Configuration.Interfaces;

namespace SampleTest.Application.Shared
{
    public abstract class BaseCommandHandler
    {
        protected readonly IAuthenticateUser AuthenticateUser;

        protected BaseCommandHandler(IAuthenticateUser authenticateUser)
        {
            AuthenticateUser = authenticateUser;
        }
    }

}
