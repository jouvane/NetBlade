using NetBlade.Core.Security.LoginUnico;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace NetBlade.CrossCutting.Security.BrasilCidadao
{
    public static class TokenLoginUnicoFactory
    {
        public static string Authorization(this TokenLoginUnico token)
        {
            return $"{(string.IsNullOrEmpty(token.TokenType) ? "Bearer" : token.TokenType)} {token.AccessToken}";
        }

        public static async Task<TokenLoginUnico> CreateTokenLoginUnico(string accessToken, string idToken, string tokenType, Func<TokenLoginUnico, Task<RepresentanteLoginUnico[]>> fnGetRepresentanteLoginUnico, Func<TokenLoginUnico, Task<CategoriaLoginUnico[]>> fnGetCategorias, Func<TokenLoginUnico, Task<ConfiabilidadeLoginUnico[]>> fnGetConfiabilidades)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtAccessToken = handler.ReadJwtToken(accessToken);
            JwtSecurityToken jwtIdToken = handler.ReadJwtToken(idToken);
            IDictionary<string, string[]> dic = null;

            dic = jwtAccessToken.Claims.GroupBy(g => g.Type, claim => claim).ToDictionary(k => k.Key, v => v.Select(s => s.Value).ToArray());
            string cpf = dic.ContainsKey("sub") ? dic["sub"].First()?.PadLeft(11, '0') : null;
            string[] amr = dic.ContainsKey("amr") ? dic["amr"] : null;
            string jti = dic.ContainsKey("jti") ? dic["jti"].First() : null;
            string cnpj = dic.ContainsKey("cnpj") ? dic["cnpj"].First() : null;

            dic = jwtIdToken.Claims.GroupBy(g => g.Type, claim => claim).ToDictionary(k => k.Key, v => v.Select(s => s.Value).ToArray());
            cpf ??= dic.ContainsKey("sub") ? dic["sub"].First()?.PadLeft(11, '0') : null;
            string name = dic.ContainsKey("name") ? dic["name"].First() : null;
            string telefone = dic.ContainsKey("phone_number") ? dic["phone_number"].First() : null;
            string email = dic.ContainsKey("email") ? dic["email"].First() : null;

            TokenLoginUnico token = new TokenLoginUnico()
            {
                AccessToken = accessToken,
                TokenType = tokenType,
                IdToken = idToken,
                Cpf = cpf,
                Amr = amr,
                Jti = jti,
                Cnpj = cnpj,
                Nome = name,
                Email = email,
                Telefone = telefone,
            };

            Task<RepresentanteLoginUnico[]> representantes = fnGetRepresentanteLoginUnico(token);
            Task<CategoriaLoginUnico[]> categorias = fnGetCategorias(token);
            Task<ConfiabilidadeLoginUnico[]> confiabilidadeLoginUnico = fnGetConfiabilidades(token);

            return await Task.Factory.StartNew(() =>
            {
                Task.WaitAll(new[] { representantes, categorias, (Task)confiabilidadeLoginUnico });
                token.Representantes = representantes.Result;
                token.Categorias = categorias.Result;
                token.Confiabilidades = confiabilidadeLoginUnico.Result;

                return token;
            });
        }
    }
}
