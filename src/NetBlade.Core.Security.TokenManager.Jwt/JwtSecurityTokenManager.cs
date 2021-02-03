using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetBlade.Core.Security.Options;
using NetBlade.Core.Security.Principal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace NetBlade.Core.Security.TokenManager.Jwt
{
    public class JwtSecurityTokenManager
    {
        private readonly IOptions<TokenOption> _options;

        public JwtSecurityTokenManager(IOptions<TokenOption> options)
        {
            this._options = options;
        }

        public int ExpiresInMinutes
        {
            get => this._options.Value.ExpiresInMinutes;
        }

        public string Issuer
        {
            get => this._options.Value.Issuer;
        }

        public string PrivateKey
        {
            get => this._options.Value.PrivateKey;
        }

        public string SecurityAlgorithms
        {
            get => this._options.Value.SecurityAlgorithms ?? Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256;
        }

        public static List<Claim> GetClaims(UserInfo user)
        {
            return user.GetClaims();
        }

        public static ClaimsPrincipal GetClaimsPrincipal(string token, string issuer, string privateKey)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters tokenValidationParameters = JwtSecurityTokenManager.GetTokenValidationParameters(issuer, privateKey);
            ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            return claimsPrincipal;
        }

        public static string GetToken(IEnumerable<Claim> claims, string issuer, string privateKey, int expiresInMinutes, string securityAlgorithms)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            SigningCredentials creds = new SigningCredentials(key, securityAlgorithms);
            JwtSecurityToken token = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(expiresInMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static TokenValidationParameters GetTokenValidationParameters(string issuer, string privateKey)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = key
            };

            return tokenValidationParameters;
        }

        public virtual ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            ClaimsPrincipal claimsPrincipal = JwtSecurityTokenManager.GetClaimsPrincipal(token, this.Issuer, this.PrivateKey);
            return claimsPrincipal;
        }

        public virtual string GetToken(IEnumerable<Claim> claims)
        {
            return JwtSecurityTokenManager.GetToken(claims, this.Issuer, this.PrivateKey, this.ExpiresInMinutes, this.SecurityAlgorithms);
        }

        public virtual string GetToken(IUserPrincipal user, params Claim[] claimsAdditional)
        {
            List<Claim> claims = user.GetClaims();
            return this.GetToken(claims);
        }

        public virtual string GetToken(UserInfo user, params Claim[] claimsAdditional)
        {
            List<Claim> claims = (claimsAdditional ?? new Claim[0]).ToList();
            claims.AddRange(JwtSecurityTokenManager.GetClaims(user));

            return this.GetToken(claims);
        }

        public virtual TokenValidationParameters GetTokenValidationParameters()
        {
            TokenValidationParameters tokenValidationParameters = JwtSecurityTokenManager.GetTokenValidationParameters(this.Issuer, this.PrivateKey);
            return tokenValidationParameters;
        }

        public virtual UserInfo GetUserInfo(IUserPrincipal userPrincipal)
        {
            return userPrincipal.GetUserInfo();
        }

        public virtual UserInfo GetUserInfo(string token)
        {
            ClaimsPrincipal claimsPrincipal = this.GetClaimsPrincipal(token);
            IUserPrincipal userPrincipal = this.GetUserPrincipal(claimsPrincipal);
            UserInfo userInfo = this.GetUserInfo(userPrincipal);

            return userInfo;
        }

        public virtual IUserPrincipal GetUserPrincipal(ClaimsPrincipal userClaims, params Claim[] claimsAdditional)
        {
            ClaimsPrincipal userClaimsNew = userClaims ?? new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity(string.Empty), claimsAdditional));
            if (userClaims != null && claimsAdditional.Any())
            {
                userClaims.AddIdentity(new ClaimsIdentity(claimsAdditional));
            }

            UserPrincipal user = new UserPrincipal(userClaimsNew, (IEnumerable<Claim> claims) => this.GetToken(claims));
            return user;
        }
    }
}
