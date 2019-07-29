using System.Security.Claims;
using Jobs.Core.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Jobs.Core.Application
{
    public class AuthService:IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void SignIn(string name)
        {
            ClaimsIdentity identity = new ClaimsIdentity("Forms");
            identity.AddClaim(new Claim(ClaimTypes.Name, name));
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            _httpContextAccessor.HttpContext.SignInAsync(CookieJobsAuthInfo.AdminAuthCookieScheme, principal);
        }
    }
}