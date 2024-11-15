using Microsoft.AspNetCore.Http;
using SampleTest.Resources.Configuration.Interfaces;
using System.Security.Authentication;

namespace SampleTest.Resources.Configuration.Impl
{
    public class AuthenticateUser : IAuthenticateUser
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly int _companyId;
        public AuthenticateUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private int? UserId
        {
            get
            {
                var userClaim = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "user.id");
                return int.TryParse(userClaim?.Value, out var userId) ? userId : null;
            }
        }
        public int GetUserId()
        {
            if (UserId.HasValue)
                return UserId.Value;

            throw new AuthenticationException("Usuário não identificado.");
        }

        public bool IsAuthenticated()
        {
            return UserId.HasValue;
        }
    }
}
