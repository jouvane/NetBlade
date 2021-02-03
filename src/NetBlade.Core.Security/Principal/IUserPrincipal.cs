using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace NetBlade.Core.Security.Principal
{
    public interface IUserPrincipal : IPrincipal
    {
        string Env { get; }

        int Id { get; }

        string Identifier { get; }

        bool IsAuthenticated { get; }

        bool Master { get; }

        string SessionId { get; }

        TipoUsuarioEnum UserType { get; }

        string UserCpf { get; }

        string UserEmail { get; }

        string UserLogin { get; }

        string UserName { get; }

        string UserPhone { get; }

        string[] UserStamps { get; }

        string RepresentedCpfCnpj { get; }

        string RepresentedName { get; }

        IEnumerable<Claim> FindAll(Predicate<Claim> match);

        IEnumerable<Claim> FindAll(string type);

        Claim FindFirst(Predicate<Claim> match);

        Claim FindFirst(string type);

        bool HasClaim(Predicate<Claim> match);

        bool HasClaim(string type, string value);
    }
}
