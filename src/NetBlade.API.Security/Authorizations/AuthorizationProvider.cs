using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NetBlade.Core.Security.TokenManager.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetBlade.API.Security.Authorizations
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly JwtSecurityTokenManager _jwtSecurityTokenManager;

        public AuthorizationProvider(IHttpContextAccessor httpContextAccessor, JwtSecurityTokenManager jwtSecurityTokenManager)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._jwtSecurityTokenManager = jwtSecurityTokenManager;
        }

        public virtual Task<string[]> GetRoles()
        {
            if (this._httpContextAccessor?.HttpContext?.Request?.Headers?.TryGetValue("Roles", out StringValues rolesToken) ?? false && !string.IsNullOrEmpty(rolesToken))
            {
                ClaimsPrincipal claims = null;//TODO: ajustar probelama. this._jwtSecurityTokenManager.GetClaimsPrincipal(rolesToken);
                return Task.FromResult(claims?.FindAll(f => ClaimTypes.Role.Equals(f.Type))?.Select(s => s.Value)?.ToArray() ?? Array.Empty<string>());
            }
            else
            {
                return Task.FromResult(Array.Empty<string>());
            }
        }

        public virtual Task<string> GetToken(string identificadorAutenticacao)
        {
            return Task.FromResult<string>(null);
        }
    }
}
