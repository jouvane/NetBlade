using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBlade.Core.Security.LoginUnico;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NetBlade.CrossCutting.Security.BrasilCidadao
{
    public class LoginUnicoProvider
    {
        protected readonly ILogger<LoginUnicoProvider> _logger;
        protected readonly IOptions<LoginUnicoProviderOption> _loginUnicoProviderOption;

        public LoginUnicoProvider(IOptions<LoginUnicoProviderOption> loginUnicoProviderOption, ILogger<LoginUnicoProvider> logger)
        {
            this._loginUnicoProviderOption = loginUnicoProviderOption;
            this._logger = logger;
        }

        public string DefaultDateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        public virtual async Task<CategoriaLoginUnico[]> GetCategorias(TokenLoginUnico token)
        {
            List<CategoriaLoginUnico> itens = new List<CategoriaLoginUnico>();
            JObject[] responseObject = await this.CreateRequetWithAuthorization<JObject[]>($"{this._loginUnicoProviderOption.Value.UrlApiService}/confiabilidades/v2/contas/{token.Cpf}/categorias", token);

            if (responseObject != null)
            {
                foreach (JObject item in responseObject)
                {
                    if (item.ContainsKey("categoria"))
                    {
                        JObject categoria = item["categoria"].Value<JObject>();

                        string id = categoria.ContainsKey("id") ? categoria["id"].Value<string>() : null;
                        string nivel = categoria.ContainsKey("nivel") ? categoria["nivel"].Value<string>() : null;
                        string titulo = categoria.ContainsKey("titulo") ? categoria["titulo"].Value<string>() : null;
                        string descricao = categoria.ContainsKey("descricao") ? categoria["descricao"].Value<string>() : null;
                        string dataUltimaAtualizacao = item.ContainsKey("dataUltimaAtualizacao") ? item["dataUltimaAtualizacao"].Value<string>() : null;

                        itens.Add(new CategoriaLoginUnico()
                        {
                            Id = id,
                            Nivel = nivel,
                            Titulo = titulo,
                            Descricao = descricao,
                            DataUltimaAtualizacao = this.TryParseExact(dataUltimaAtualizacao),
                        });
                    }
                }
            }

            return itens.ToArray();
        }

        public virtual async Task<ConfiabilidadeLoginUnico[]> GetConfiabilidades(TokenLoginUnico token)
        {
            List<ConfiabilidadeLoginUnico> itens = new List<ConfiabilidadeLoginUnico>();

            try
            {

                JObject[] responseObject = await this.CreateRequetWithAuthorization<JObject[]>($"{this._loginUnicoProviderOption.Value.UrlApiService}/confiabilidades/v2/contas/{token.Cpf}/confiabilidades", token);
                if (responseObject != null)
                {
                    foreach (JObject item in responseObject)
                    {
                        if (item.ContainsKey("confiabilidade"))
                        {
                            JObject confiabilidade = item["confiabilidade"].Value<JObject>();

                            string id = confiabilidade.ContainsKey("id") ? confiabilidade["id"].Value<string>() : null;
                            string categoria = confiabilidade.ContainsKey("categoria") ? confiabilidade["categoria"].Value<string>() : null;
                            string titulo = confiabilidade.ContainsKey("titulo") ? confiabilidade["titulo"].Value<string>() : null;
                            string descricao = confiabilidade.ContainsKey("descricao") ? confiabilidade["descricao"].Value<string>() : null;
                            string dataCriacao = item.ContainsKey("dataCriacao") ? item["dataCriacao"].Value<string>() : null;
                            string dataAtualizacao = item.ContainsKey("dataAtualizacao") ? item["dataAtualizacao"].Value<string>() : null;

                            itens.Add(new ConfiabilidadeLoginUnico()
                            {
                                Categoria = categoria,
                                DataCriacao = this.TryParseExact(dataCriacao),
                                DataAtualizacao = this.TryParseExact(dataAtualizacao),
                                Descricao = descricao,
                                Id = id,
                                Titulo = titulo
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, $"Falha ao obter Confiabilidades para {token.Cpf}");
            }

            return itens.ToArray();
        }

        public virtual async Task<RepresentanteLoginUnico> GetRepresentante(TokenLoginUnico token, string cnpj, string razaoSocial)
        {
            JObject responseObject = await this.CreateRequetWithAuthorization<JObject>($"{this._loginUnicoProviderOption.Value.UrlApiService}/empresas/v2/empresas/{cnpj}/participantes/{token.Cpf}", token);
            if (responseObject != null)
            {
                string cpf = responseObject.ContainsKey("cpf") ? responseObject["cpf"].Value<string>()?.PadLeft(11, '0') : null;
                string atuacao = responseObject.ContainsKey("atuacao") ? responseObject["atuacao"].Value<string>() : null;
                string cadastrador = responseObject.ContainsKey("cadastrador") ? responseObject["cadastrador"].Value<string>() : null;
                string cpfCadastrador = responseObject.ContainsKey("cpfCadastrador") ? responseObject["cpfCadastrador"].Value<string>()?.PadLeft(11, '0') : null;
                string dataCriacao = responseObject.ContainsKey("dataCriacao") ? responseObject["dataCriacao"].Value<string>() : null;
                string dataExpiracao = responseObject.ContainsKey("dataExpiracao") ? responseObject["dataExpiracao"].Value<string>() : null;

                return new RepresentanteLoginUnico()
                {
                    Cpf = cpf,
                    Nome = razaoSocial,
                    Cnpj = cnpj,
                    Atuacao = atuacao,
                    Cadastrador = cadastrador,
                    CpfCadastrador = cpfCadastrador,
                    DataCriacao = this.TryParseExact(dataCriacao),
                    DataExpiracao = this.TryParseExact(dataExpiracao),
                };
            }

            return null;
        }

        public virtual async Task<RepresentanteLoginUnico[]> GetRepresentantes(TokenLoginUnico token)
        {
            ConcurrentDictionary<string, RepresentanteLoginUnico> itens = new ConcurrentDictionary<string, RepresentanteLoginUnico>();
            JObject[] responseObject = await this.CreateRequetWithAuthorization<JObject[]>($"{this._loginUnicoProviderOption.Value.UrlApiService}/empresas/v2/empresas?filtrar-por-participante={token.Cpf}", token);
            if (responseObject != null)
            {
                List<dynamic> cnpjs = new List<dynamic>();
                foreach (JObject item in responseObject)
                {
                    if (item.ContainsKey("cnpj") && item.ContainsKey("razaoSocial"))
                    {
                        string cnpj = item["cnpj"].Value<string>().PadLeft(14, '0');
                        string razaoSocial = item["razaoSocial"].Value<string>();

                        cnpjs.Add(new { cnpj, razaoSocial });
                    }
                }

                _ = Parallel.ForEach(cnpjs, (dynamic item) =>
                 {
                     Task<RepresentanteLoginUnico> representante = this.GetRepresentante(token, item.cnpj, item.razaoSocial);
                     representante.Wait();
                     if (representante.Result != null)
                     {
                         itens.TryAdd(item.cnpj, representante.Result);
                     }
                 });
            }

            RepresentanteLoginUnico[] representantes = itens.Select(s => s.Value).ToArray();
            return await Task.Factory.StartNew(() =>
            {
                return representantes.OrderBy(o => o.Nome).ToArray();
            });
        }

        public virtual async Task<TokenLoginUnico> GetTokenLoginUnico(string code, string redirectUri)
        {
            Dictionary<string, dynamic> responseObject = await this.GetToken(code, redirectUri);
            if (responseObject != null)
            {
                string accessToken = responseObject["access_token"];
                string idToken = responseObject["id_token"];
                string tokenType = responseObject.ContainsKey("token_type") ? responseObject["token_type"] : "Bearer";
                return await this.CreateTokenLoginUnico(accessToken, idToken, tokenType);
            }

            return null;
        }

        protected virtual async Task<T> CreateRequetWithAuthorization<T>(string url, TokenLoginUnico token)
        {
            Uri uri = new Uri(url);
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token.Authorization());

            using HttpResponseMessage response = await client.GetAsync(uri);
            string json = await response.Content.ReadAsStringAsync() ?? "{}";

            this._logger?.LogDebug(
                @$"{{
                        ""authorization"": ""{token.Authorization()}"",
                        ""cpf"": ""{token.Cpf}"",
                        ""nome"": ""{token.Nome}"",
                        ""email"": ""{token.Email}"",
                        ""telefone"": ""{token.Telefone}"",
                        ""uri"": ""{uri}"",
                        ""json"": ""{json}"",
                        ""responseStatusCode"": ""{response.StatusCode}""
                   }}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                this._logger?.LogError(
                    @$"{{ 
                            ""authorization: ""{token.Authorization()}"",
                            ""cpf: ""{token.Cpf}"",
                            ""nome: ""{token.Nome}"",
                            ""email: ""{token.Email}"",
                            ""telefone: ""{token.Telefone}"",
                            ""uri: ""{uri}"",
                            ""json: ""{json}"",
                            ""responseStatusCode: ""{response.StatusCode}""
                       }}");

                return default;
            }
        }

        protected virtual async Task<TokenLoginUnico> CreateTokenLoginUnico(string accessToken, string idToken, string tokenType)
        {
            TokenLoginUnico token = await TokenLoginUnicoFactory.CreateTokenLoginUnico(accessToken, idToken, tokenType, this.GetRepresentantes, this.GetCategorias, this.GetConfiabilidades);
            return token;
        }

        protected virtual async Task<Dictionary<string, dynamic>> GetToken(string code, string redirectUri)
        {
            Uri uri = new Uri(this._loginUnicoProviderOption.Value.UrlGetToken);
            UriBuilder uriBuilder = new UriBuilder(uri);

            using HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", this._loginUnicoProviderOption.Value.Authorization);

            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["grant_type"] = this._loginUnicoProviderOption.Value.GrantType;
            query["code"] = code;
            query["redirect_uri"] = redirectUri;
            uriBuilder.Query = query.ToString();

            using HttpResponseMessage response = await client.PostAsync(uriBuilder.Uri, new StringContent(string.Empty, Encoding.UTF8, "application/json"));
            string json = await response.Content.ReadAsStringAsync() ?? "{}";

            this._logger?.LogDebug(
                @$"{{
                        ""code"": ""{code}"",
                        ""authorization"": ""{this._loginUnicoProviderOption.Value.Authorization}"",
                        ""redirectUri"": ""{redirectUri}"",
                        ""urlGetToken"": ""{this._loginUnicoProviderOption.Value.UrlGetToken}"",
                        ""json"": ""{json}"",
                        ""responseStatusCode"": ""{response.StatusCode}""
                   }}");

            Dictionary<string, dynamic> responseObject = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            if (!responseObject.ContainsKey("error"))
            {
                return responseObject;
            }
            else
            {
                this._logger?.LogError(
                    @$"{{
                            ""code"": ""{code}"",
                            ""authorization"": ""{this._loginUnicoProviderOption.Value.Authorization}"",
                            ""redirectUri"": ""{redirectUri}"",
                            ""urlGetToken"": ""{this._loginUnicoProviderOption.Value.UrlGetToken}"",
                            ""json"": ""{json}"",
                            ""responseStatusCode"": ""{response.StatusCode}"",
                            ""error"": ""{responseObject["error"]}""
                       }}");
            }

            return null;
        }

        private DateTime? TryParseExact(string s)
        {
            if (!string.IsNullOrEmpty(s) && s.EndsWith("-0300"))
            {
                s = s.Substring(0, s.IndexOf("-0300"));
            }

            DateTimeFormatInfo dateTimeFormat = CultureInfo.CreateSpecificCulture("pt-BR").DateTimeFormat;

            if (DateTime.TryParseExact(s, this.DefaultDateTimeFormat, dateTimeFormat, DateTimeStyles.AssumeLocal, out DateTime dt))
            {
                return dt;
            }
            else if (DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm:ss", dateTimeFormat, DateTimeStyles.AssumeLocal, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm", dateTimeFormat, DateTimeStyles.AssumeLocal, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParseExact(s, "yyyy-MM-dd HH", dateTimeFormat, DateTimeStyles.AssumeLocal, out dt))
            {
                return dt;
            }
            else if (DateTime.TryParseExact(s, "yyyy-MM-dd", dateTimeFormat, DateTimeStyles.AssumeLocal, out dt))
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
    }
}
