namespace NetBlade.Core.Security.LoginUnico
{
    public class TokenLoginUnico
    {
        /// <summary>
        /// Token de acesso a recursos protegidos do autenticador, bem como serviços do Login Único.
        /// </summary>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// Listagem dos fatores de autenticação do usuário.
        /// Pode ser “passwd” se o mesmo logou fornecendo a senha, ou “x509” se o mesmo utilizou certificado digital ou certificado em nuvem.
        /// </summary>
        public virtual string[] Amr { get; set; }

        /// <summary>
        /// Categoria que cidadão possui.
        /// </summary>
        public virtual CategoriaLoginUnico[] Categorias { get; set; }

        /// <summary>
        /// CNPJ vinculado ao usuário autenticado.
        /// Atributo será preenchido quando autenticação ocorrer por certificado digital de pessoal jurídica.
        /// </summary>
        public virtual string Cnpj { get; set; }

        /// <summary>
        /// Selos de confiabilidade o cidadão possui.
        /// </summary>
        public virtual ConfiabilidadeLoginUnico[] Confiabilidades { get; set; }

        /// <summary>
        /// CPF do usuário autenticado.
        /// </summary>
        public virtual string Cpf { get; set; }

        /// <summary>
        /// Endereço de e-mail cadastrado no Gov.br do usuário autenticado.
        /// Caso o atributo EmailVerified tiver o valor false, o atributo email não virá.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Token de autenticação com informações básicas do usuário
        /// </summary>
        public virtual string IdToken { get; set; }

        /// <summary>
        /// Identificador único do token, reconhecido internamente pelo provedor de autenticação.
        /// </summary>
        public virtual string Jti { get; set; }

        /// <summary>
        /// Nome cadastrado no Gov.br do usuário autenticado.
        /// </summary>
        public virtual string Nome { get; set; }

        /// <summary>
        /// Representantes vinculado ao usuário autenticado.
        /// Atributo será preenchido quando autenticação ocorrer por certificado digital de pessoal jurídica.
        /// </summary>
        public virtual RepresentanteLoginUnico[] Representantes { get; set; }

        /// <summary>
        /// Número de telefone cadastrado no Gov.br do usuário autenticado.
        /// Caso o atributo PhoneNumberVerified tiver o valor false, o atributo PhoneNumberVerified não virá.
        /// </summary>
        public virtual string Telefone { get; set; }

        /// <summary>
        /// O tipo do token gerado. 
        /// Padrão: Bearer.
        /// </summary>
        public virtual string TokenType { get; set; }
    }
}
