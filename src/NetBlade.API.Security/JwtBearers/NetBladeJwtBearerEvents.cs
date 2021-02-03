using Microsoft.AspNetCore.Authentication.JwtBearer;
using NetBlade.API.Security.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetBlade.API.Security.JwtBearers
{
    public class NetBladeJwtBearerEvents : JwtBearerEvents
    {
        private readonly IAuthorizationProvider authorizationServices;

        public NetBladeJwtBearerEvents(IAuthorizationProvider authorizationServices)
        {
            this.authorizationServices = authorizationServices;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            ClaimsPrincipal user = context?.Principal;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                string[] roles = await this.authorizationServices.GetRoles() ?? Array.Empty<string>();
                IEnumerable<Claim> claimsAdditional = roles.Select(s => new Claim(ClaimTypes.Role, s));

                if (claimsAdditional.Any())
                {
                    user.AddIdentity(new ClaimsIdentity(claimsAdditional));
                }
            }
            else
            {
                await base.TokenValidated(context);
            }
        }

        public override async Task MessageReceived(MessageReceivedContext context)
        {
            if (string.IsNullOrEmpty(context.Token) && context.HttpContext.Request.Cookies.ContainsKey("SCA"))
            {
                string scaCookie = context.HttpContext.Request.Cookies["SCA"];
                context.Token = await this.authorizationServices.GetToken(scaCookie);
            }
        }
    }
}
