using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetBlade.API.Security.Authorizations;
using NetBlade.API.Security.JwtBearers;
using NetBlade.Core.Security.Options;
using NetBlade.Core.Security.Principal;
using NetBlade.Core.Security.TokenManager.Jwt;
using System;
using System.Security.Claims;

namespace NetBlade.API.Security
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration, string sectionTokenOption = "NetBlade:TokenOption", Func<IServiceProvider, ClaimsPrincipal, Claim[]> fnClaimsAdditional = null, Type netBladeJwtBearerEvents = null, Action<TokenOption> configureTokenOption = null)
        {
            netBladeJwtBearerEvents = netBladeJwtBearerEvents ?? typeof(NetBladeJwtBearerEvents);
            if (configureTokenOption == null)
            {
                configureTokenOption = (TokenOption c) =>
                {
                    configuration.Bind(sectionTokenOption, c);
                };
            }

            services.TryAddSingleton<JwtSecurityTokenManager>();
            services.TryAddSingleton<NetBladeJwtBearerEvents>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationProvider, AuthorizationProvider>();
            services.TryAddSingleton(netBladeJwtBearerEvents);

            services
                .Configure<TokenOption>(configureTokenOption)
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.EventsType = netBladeJwtBearerEvents;
                    options.TokenValidationParameters = JwtSecurityTokenManager.GetTokenValidationParameters(configuration[$"{sectionTokenOption}:Issuer"], configuration[$"{sectionTokenOption}:PrivateKey"]);
                });

            services.TryAddScoped<IUserPrincipal>(s =>
            {
                JwtSecurityTokenManager jwtSecurityTokenManager = s.GetRequiredService<JwtSecurityTokenManager>();
                HttpContext httpContext = s.GetService<IHttpContextAccessor>().HttpContext;
                ClaimsPrincipal userClaims = httpContext?.User;
                Claim[] claimsAdditional = fnClaimsAdditional != null ? fnClaimsAdditional(s, userClaims) ?? Array.Empty<Claim>() : Array.Empty<Claim>();

                IUserPrincipal user = jwtSecurityTokenManager.GetUserPrincipal(userClaims, claimsAdditional);
                return user;
            });

            return services;
        }
    }
}
