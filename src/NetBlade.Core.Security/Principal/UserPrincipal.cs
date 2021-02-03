using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace NetBlade.Core.Security.Principal
{
    public class UserPrincipal : IUserPrincipal
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly Func<IEnumerable<Claim>, string> _fnGetToken;

        public UserPrincipal(ClaimsPrincipal claimsPrincipal)
            : this(claimsPrincipal, null)
        {
        }

        public UserPrincipal(ClaimsPrincipal claimsPrincipal, Func<IEnumerable<Claim>, string> _fnGetToken)
        {
            this._claimsPrincipal = claimsPrincipal;
            this._fnGetToken = _fnGetToken;
        }

        public string Env
        {
            get => this.FindFirst(c => "env".Equals(c.Type))?.Value;
        }

        public int Id
        {
            get => int.Parse(this.FindFirst(c => "id".Equals(c.Type))?.Value ?? "0");
        }

        public string Identifier
        {
            get => this.FindFirst(c => "identifier".Equals(c.Type))?.Value;
        }

        public IIdentity Identity
        {
            get => this._claimsPrincipal?.Identity;
        }

        public bool IsAuthenticated
        {
            get => this._claimsPrincipal?.Identity?.IsAuthenticated ?? false;
        }

        public bool Master
        {
            get => bool.Parse(this.FindFirst(c => "master".Equals(c.Type))?.Value ?? "False");
        }

        public string RepresentedCpfCnpj
        {
            get => this.FindFirst(c => "representedCpfCnpj".Equals(c.Type))?.Value;
        }

        public string RepresentedName
        {
            get => this.FindFirst(c => "representedName".Equals(c.Type))?.Value;
        }

        public string SessionId
        {
            get => this.FindFirst(c => "sessionId".Equals(c.Type))?.Value;
        }

        public string UserCpf
        {
            get => this.FindFirst(c => "userCpf".Equals(c.Type))?.Value;
        }

        public string UserEmail
        {
            get => this.FindFirst(c => "userEmail".Equals(c.Type))?.Value;
        }

        public string UserLogin
        {
            get => this.FindFirst(c => "userLogin".Equals(c.Type))?.Value;
        }

        public string UserName
        {
            get => this.FindFirst(c => "userName".Equals(c.Type))?.Value;
        }

        public string UserPhone
        {
            get => this.FindFirst(c => "userPhone".Equals(c.Type))?.Value;
        }

        public string[] UserStamps
        {
            get => this.FindAll(c => ClaimTypes.Role.Equals(c.Type) && c.Value.StartsWith("stamp:"))?.Select(s => s.Value.Substring("stamp:".Length))?.ToArray();
        }

        public TipoUsuarioEnum UserType
        {
            get => (TipoUsuarioEnum)Enum.Parse(typeof(TipoUsuarioEnum), this.FindFirst(c => "userType".Equals(c.Type))?.Value ?? TipoUsuarioEnum.Desconhecido.ToString());
        }

        public IEnumerable<Claim> FindAll(Predicate<Claim> match)
        {
            return this._claimsPrincipal?.FindAll(match) ?? Array.Empty<Claim>();
        }

        public IEnumerable<Claim> FindAll(string type)
        {
            return this._claimsPrincipal?.FindAll(type) ?? Array.Empty<Claim>();
        }

        public Claim FindFirst(Predicate<Claim> match)
        {
            return this._claimsPrincipal?.FindFirst(match);
        }

        public Claim FindFirst(string type)
        {
            return this._claimsPrincipal?.FindFirst(type);
        }

        public bool HasClaim(Predicate<Claim> match)
        {
            return this._claimsPrincipal?.HasClaim(match) ?? false;
        }

        public bool HasClaim(string type, string value)
        {
            return this._claimsPrincipal?.HasClaim(type, value) ?? false;
        }

        public bool IsInRole(string role)
        {
            return this._claimsPrincipal?.IsInRole(role) ?? false;
        }

        public override string ToString()
        {
            return this._fnGetToken != null ? this._fnGetToken(this._claimsPrincipal.Claims) : base.ToString();
        }
    }
}
