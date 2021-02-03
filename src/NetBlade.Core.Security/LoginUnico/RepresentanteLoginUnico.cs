using System;

namespace NetBlade.Core.Security.LoginUnico
{
    public class RepresentanteLoginUnico
    {
        /// <summary>
        /// Papel do CPF na empresa na Receita Federal.
        /// O conteúdo será SOCIO, CONTADOR, REPRESENTANTE_LEGAL ou NAO_ATUANTE.
        /// O NAO_ATUANTE representa CPF possui certificado digital de pessoa jurídica, porém não possui um papel na empresa na base da Receita Federal.
        /// Se CPF for colaborador, atributo atuacao não aparecerá.
        /// </summary>
        public string Atuacao { get; set; }

        /// <summary>
        /// Identifica se o CPF pode realizar cadastro de colaboradores para CNPJ.
        /// O conteúdo false determinar que o CPF é um colaborador da empresa.
        /// O conteúdo true determina CPF é representante da empresa com certificado digital de pessoal jurídica.
        /// </summary>
        public string Cadastrador { get; set; }

        /// <summary>
        /// Número de CNPJ da empresa vinculada.
        /// </summary>
        public string Cnpj { get; set; }

        /// <summary>
        /// Número do CPF que pode atuar com empresa.
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// CPF responsável por realizar cadastro do Colaborador.
        /// Se CPF apresentar atributo cadastrador com conteúdo true, o atributo cpfCadastrador não aparecerá.
        /// </summary>
        public string CpfCadastrador { get; set; }

        /// <summary>
        /// Mostra a data e hora da vinculação do CPF ao CNPJ.
        /// </summary>
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// Mostra a data e hora que o CPF poderá atuar com CNPJ.
        /// </summary>
        public DateTime? DataExpiracao { get; set; }

        /// <summary>
        /// Razão Social (Nome da empresa) cadastrada na Receita Federal.
        /// </summary>
        public string Nome { get; set; }
    }
}
