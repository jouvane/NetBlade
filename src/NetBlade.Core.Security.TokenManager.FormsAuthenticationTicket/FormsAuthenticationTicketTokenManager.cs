using AspNetCore.LegacyAuthCookieCompat;
using Microsoft.Extensions.Options;
using NetBlade.Core.Security.Options;
using NetBlade.Core.Security.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NetBlade.Core.Security.TokenManager.FormsAuthenticationTicket
{
    public class FormsAuthenticationTicketTokenManager
    {
        public readonly byte[] DecripitionKey;
        public readonly byte[] ValidationKey;
        private readonly IOptions<TokenOption> _options;

        public FormsAuthenticationTicketTokenManager(IOptions<TokenOption> options)
        {
            this._options = options;
            this.DecripitionKey = HexUtils.HexToBinary(this._options.Value.FormsDecripitionKey);
            this.ValidationKey = HexUtils.HexToBinary(this._options.Value.FormsValidationKey);
        }

        public virtual string GetEncryptedText(IUserPrincipal user)
        {
            string exp = user.FindFirst("exp")?.Value;
            DateTime dataLogin = !string.IsNullOrEmpty(exp) ? DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).LocalDateTime.AddHours(this._options.Value.ExpiresInMinutes * -1) : DateTime.Now;
            string login = user.UserLogin;

            AspNetCore.LegacyAuthCookieCompat.FormsAuthenticationTicket formsAuthenticationTicket = new AspNetCore.LegacyAuthCookieCompat.FormsAuthenticationTicket(1, login, dataLogin, dataLogin.AddHours(this._options.Value.ExpiresInMinutes), false, user.Identifier, "/");
            return this.EncryptFormsAuthTicket(formsAuthenticationTicket);
        }

        public virtual string GetUserData(string encryptedText)
        {
            if (!string.IsNullOrEmpty(encryptedText))
            {
                AspNetCore.LegacyAuthCookieCompat.FormsAuthenticationTicket formsAuthenticationTicket = this.DecryptFormsAuthTicket(encryptedText);
                return this.GetClaims(formsAuthenticationTicket).FirstOrDefault(f => f.Type == ClaimTypes.UserData)?.Value;
            }

            return null;
        }

        protected virtual AspNetCore.LegacyAuthCookieCompat.FormsAuthenticationTicket DecryptFormsAuthTicket(string encryptedText)
        {
            LegacyFormsAuthenticationTicketEncryptor legacyFormsAuthenticationTicketEncryptor = this.GetLegacyFormsAuthenticationTicketEncryptor();
            AspNetCore.LegacyAuthCookieCompat.FormsAuthenticationTicket decryptedTicket = legacyFormsAuthenticationTicketEncryptor.DecryptCookie(encryptedText);
            return decryptedTicket;
        }

        protected virtual string EncryptFormsAuthTicket(AspNetCore.LegacyAuthCookieCompat.FormsAuthenticationTicket ticket)
        {
            LegacyFormsAuthenticationTicketEncryptor legacyFormsAuthenticationTicketEncryptor = this.GetLegacyFormsAuthenticationTicketEncryptor();
            string token = legacyFormsAuthenticationTicketEncryptor.Encrypt(ticket);
            return token;
        }

        protected virtual List<Claim> GetClaims(AspNetCore.LegacyAuthCookieCompat.FormsAuthenticationTicket formsAuthenticationTicket)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Expiration, formsAuthenticationTicket.Expiration.ToString(), ClaimValueTypes.DateTime),
                new Claim(ClaimTypes.IsPersistent, formsAuthenticationTicket.IsPersistent.ToString(), ClaimValueTypes.Boolean),
                new Claim(ClaimTypes.Expired, formsAuthenticationTicket.Expired.ToString(), ClaimValueTypes.Boolean),
                new Claim(ClaimTypes.Version, formsAuthenticationTicket.Version.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimTypes.CookiePath, formsAuthenticationTicket.CookiePath),
                new Claim(ClaimTypes.UserData, formsAuthenticationTicket.UserData)
            };

            return claims;
        }

        protected virtual LegacyFormsAuthenticationTicketEncryptor GetLegacyFormsAuthenticationTicketEncryptor()
        {
            LegacyFormsAuthenticationTicketEncryptor legacyFormsAuthenticationTicketEncryptor = new LegacyFormsAuthenticationTicketEncryptor(this.DecripitionKey, this.ValidationKey, ShaVersion.Sha1);
            return legacyFormsAuthenticationTicketEncryptor;
        }
    }
}
