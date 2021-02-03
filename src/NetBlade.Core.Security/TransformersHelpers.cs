using NetBlade.Core.Security.LoginUnico;
using NetBlade.Core.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NetBlade.Core.Security
{
    public static class TransformersHelpers
    {
        public static List<Claim> GetClaims(this UserInfo user)
        {
            List<Claim> claims = new List<Claim>();
            if (!string.IsNullOrEmpty(user.Env))
            {
                claims.Add(new Claim("env", user.Env));
            }

            if (!string.IsNullOrEmpty(user.Identifier))
            {
                claims.Add(new Claim("identifier", user.Identifier));
            }

            if (!string.IsNullOrEmpty(user.RepresentedCpfCnpj))
            {
                claims.Add(new Claim("representedCpfCnpj", user.RepresentedCpfCnpj));
            }

            if (!string.IsNullOrEmpty(user.RepresentedName))
            {
                claims.Add(new Claim("representedName", user.RepresentedName));
            }

            if (!string.IsNullOrEmpty(user.SessionId))
            {
                claims.Add(new Claim("sessionId", user.SessionId));
            }

            if (!string.IsNullOrEmpty(user.UserCpf))
            {
                claims.Add(new Claim("userCpf", user.UserCpf));
            }

            if (!string.IsNullOrEmpty(user.UserEmail))
            {
                claims.Add(new Claim("userEmail", user.UserEmail));
            }

            if (!string.IsNullOrEmpty(user.UserLogin))
            {
                claims.Add(new Claim("userLogin", user.UserLogin));
            }

            if (!string.IsNullOrEmpty(user.UserName))
            {
                claims.Add(new Claim("userName", user.UserName));
            }

            if (!string.IsNullOrEmpty(user.UserPhone))
            {
                claims.Add(new Claim("userPhone", user.UserPhone));
            }

            claims.Add(new Claim("userType", user.UserType.ToString()));
            claims.Add(new Claim("master", user.Master.ToString()));
            claims.Add(new Claim("id", user.Id.ToString()));

            foreach (string stamps in user.UserStamps)
            {
                claims.Add(new Claim(ClaimTypes.Role, $"stamp:{stamps}"));
            }

            return claims;
        }

        public static List<Claim> GetClaims(this IUserPrincipal user)
        {
            List<Claim> claims = new List<Claim>();
            if (!string.IsNullOrEmpty(user.Env))
            {
                claims.Add(new Claim("env", user.Env));
            }

            if (!string.IsNullOrEmpty(user.Identifier))
            {
                claims.Add(new Claim("identifier", user.Identifier));
            }

            if (!string.IsNullOrEmpty(user.RepresentedCpfCnpj))
            {
                claims.Add(new Claim("representedCpfCnpj", user.RepresentedCpfCnpj));
            }

            if (!string.IsNullOrEmpty(user.RepresentedName))
            {
                claims.Add(new Claim("representedName", user.RepresentedName));
            }

            if (!string.IsNullOrEmpty(user.SessionId))
            {
                claims.Add(new Claim("sessionId", user.SessionId));
            }

            if (!string.IsNullOrEmpty(user.UserCpf))
            {
                claims.Add(new Claim("userCpf", user.UserCpf));
            }

            if (!string.IsNullOrEmpty(user.UserEmail))
            {
                claims.Add(new Claim("userEmail", user.UserEmail));
            }

            if (!string.IsNullOrEmpty(user.UserLogin))
            {
                claims.Add(new Claim("userLogin", user.UserLogin));
            }

            if (!string.IsNullOrEmpty(user.UserName))
            {
                claims.Add(new Claim("userName", user.UserName));
            }

            if (!string.IsNullOrEmpty(user.UserPhone))
            {
                claims.Add(new Claim("userPhone", user.UserPhone));
            }

            claims.Add(new Claim("userType", user.UserType.ToString()));
            claims.Add(new Claim("master", user.Master.ToString()));
            claims.Add(new Claim("id", user.Id.ToString()));

            foreach (string stamps in user.UserStamps)
            {
                claims.Add(new Claim(ClaimTypes.Role, $"stamp:{stamps}"));
            }

            return claims;
        }

        public static UserInfo[] GetRepresentantesUserInfo(this TokenLoginUnico user)
        {
            if (user?.Representantes != null)
            {
                List<UserInfo> representanteResult = new List<UserInfo>();
                foreach (RepresentanteLoginUnico representante in user.Representantes)
                {
                    List<string> stamps = user.Categorias?.Select(s => s.Id.ToLower().Trim()).Distinct().ToList();
                    List<string> stampsLevel = user.Categorias?.Select(s => $"level:{s.Nivel.ToLower().Trim()}").Distinct().ToList();

                    if (!string.IsNullOrEmpty(representante.Atuacao))
                    {
                        stamps.Add($"companyRepresentation:{representante.Atuacao.ToUpper().Trim()}");
                    }
                    else
                    {
                        stamps.Add($"companyRepresentation:COLABORADOR");
                    }

                    if (stampsLevel.Any())
                    {
                        stamps.AddRange(stampsLevel);
                    }

                    UserInfo userInfo = new UserInfo(
                        env: null,
                        id: 0,
                        identifier: null,
                        master: false,
                        sessionId: null,
                        representedCpfCnpj: representante.Cnpj,
                        representedName: representante.Nome,
                        userCpf: user.Cpf,
                        userEmail: user.Email,
                        userLogin: user.Cpf,
                        userName: user.Nome,
                        userPhone: user.Telefone,
                        userStamps: stamps.OrderBy(o => o).ToArray(),
                        userType: TipoUsuarioEnum.LoginUnicoPF_PJ);

                    representanteResult.Add(userInfo);
                }

                return representanteResult.ToArray();
            }
            else
            {
                return null;
            }
        }

        public static UserInfo GetUserInfo(this IUserPrincipal user)
        {
            UserInfo userInfo = new UserInfo(
                env: user.Env,
                id: user.Id,
                identifier: user.Identifier,
                master: user.Master,
                representedCpfCnpj: user.RepresentedCpfCnpj,
                representedName: user.RepresentedName,
                sessionId: user.SessionId,
                userCpf: user.UserCpf,
                userEmail: user.UserEmail,
                userLogin: user.UserLogin,
                userName: user.UserName,
                userPhone: user.UserPhone,
                userStamps: user.UserStamps,
                userType: user.UserType);

            return userInfo;
        }

        public static UserInfo GetUserInfo(this TokenLoginUnico user)
        {
            List<string> stamps = user.Categorias?.Select(s => s.Id.ToLower().Trim()).Distinct().ToList();
            List<string> stampsLevel = user.Categorias?.Select(s => $"level:{s.Nivel.ToLower().Trim()}").Distinct().ToList();

            if (stampsLevel.Any())
            {
                stamps.AddRange(stampsLevel);
            }

            UserInfo userInfo = new UserInfo(
                env: null,
                id: 0,
                identifier: null,
                master: false,
                representedCpfCnpj: null,
                representedName: null,
                sessionId: null,
                userCpf: user.Cpf,
                userEmail: user.Email,
                userLogin: user.Cpf,
                userName: user.Nome,
                userPhone: user.Telefone,
                userStamps: stamps.OrderBy(o => o).ToArray(),
                userType: TipoUsuarioEnum.LoginUnicoPF);

            return userInfo;
        }
    }
}
