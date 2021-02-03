namespace NetBlade.Core.Security.LoginUnico
{
    public class TokenLoginUnico
    {
        /// <summary>
        /// Token de acesso a recursos protegidos do autenticador, bem como servi�os do Login �nico.
        /// </summary>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// Listagem dos fatores de autentica��o do usu�rio.
        /// Pode ser �passwd� se o mesmo logou fornecendo a senha, ou �x509� se o mesmo utilizou certificado digital ou certificado em nuvem.
        /// </summary>
        public virtual string[] Amr { get; set; }

        /// <summary>
        /// Categoria que cidad�o possui.
        /// </summary>
        public virtual CategoriaLoginUnico[] Categorias { get; set; }

        /// <summary>
        /// CNPJ vinculado ao usu�rio autenticado.
        /// Atributo ser� preenchido quando autentica��o ocorrer por certificado digital de pessoal jur�dica.
        /// </summary>
        public virtual string Cnpj { get; set; }

        /// <summary>
        /// Selos de confiabilidade o cidad�o possui.
        /// </summary>
        public virtual ConfiabilidadeLoginUnico[] Confiabilidades { get; set; }

        /// <summary>
        /// CPF do usu�rio autenticado.
        /// </summary>
        public virtual string Cpf { get; set; }

        /// <summary>
        /// Endere�o de e-mail cadastrado no Gov.br do usu�rio autenticado.
        /// Caso o atributo EmailVerified tiver o valor false, o atributo email n�o vir�.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Token de autentica��o com informa��es b�sicas do usu�rio
        /// </summary>
        public virtual string IdToken { get; set; }

        /// <summary>
        /// Identificador �nico do token, reconhecido internamente pelo provedor de autentica��o.
        /// </summary>
        public virtual string Jti { get; set; }

        /// <summary>
        /// Nome cadastrado no Gov.br do usu�rio autenticado.
        /// </summary>
        public virtual string Nome { get; set; }

        /// <summary>
        /// Representantes vinculado ao usu�rio autenticado.
        /// Atributo ser� preenchido quando autentica��o ocorrer por certificado digital de pessoal jur�dica.
        /// </summary>
        public virtual RepresentanteLoginUnico[] Representantes { get; set; }

        /// <summary>
        /// N�mero de telefone cadastrado no Gov.br do usu�rio autenticado.
        /// Caso o atributo PhoneNumberVerified tiver o valor false, o atributo PhoneNumberVerified n�o vir�.
        /// </summary>
        public virtual string Telefone { get; set; }

        /// <summary>
        /// O tipo do token gerado. 
        /// Padr�o: Bearer.
        /// </summary>
        public virtual string TokenType { get; set; }
    }
}
