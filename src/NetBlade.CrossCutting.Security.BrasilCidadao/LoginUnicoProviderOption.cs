using System;
using System.Text;

namespace NetBlade.CrossCutting.Security.BrasilCidadao
{
    public class LoginUnicoProviderOption
    {
        public string Authorization
        {
            get => $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ this.ClientID}:{this.ClientSecret}"))}";
        }

        public string ClientID { get; set; } = "devapp.NetBlade.gov.br";

        public string ClientSecret { get; set; } = "NerU3AMjF0iPTUtr_7pSbxsQUFkVLjb9R-3iSW16ZJhjBM1Xvc_HA0XoTBF8YzQJCMojiyMsoXs0BbCIVp5qig";

        public string ContentType { get; set; } = "application/x-www-form-urlencoded";

        public string GrantType { get; set; } = "authorization_code";

        public string UrlApiService { get; set; } = "https://api.staging.acesso.gov.br";

        public string UrlGetToken { get; set; } = "https://sso.staging.acesso.gov.br/token";
    }
}
