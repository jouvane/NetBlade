using System;

namespace NetBlade.Core.Security.LoginUnico
{
    public class RepresentanteLoginUnico
    {
        /// <summary>
        /// Papel do CPF na empresa na Receita Federal.
        /// O conte�do ser� SOCIO, CONTADOR, REPRESENTANTE_LEGAL ou NAO_ATUANTE.
        /// O NAO_ATUANTE representa CPF possui certificado digital de pessoa jur�dica, por�m n�o possui um papel na empresa na base da Receita Federal.
        /// Se CPF for colaborador, atributo atuacao n�o aparecer�.
        /// </summary>
        public string Atuacao { get; set; }

        /// <summary>
        /// Identifica se o CPF pode realizar cadastro de colaboradores para CNPJ.
        /// O conte�do false determinar que o CPF � um colaborador da empresa.
        /// O conte�do true determina CPF � representante da empresa com certificado digital de pessoal jur�dica.
        /// </summary>
        public string Cadastrador { get; set; }

        /// <summary>
        /// N�mero de CNPJ da empresa vinculada.
        /// </summary>
        public string Cnpj { get; set; }

        /// <summary>
        /// N�mero do CPF que pode atuar com empresa.
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// CPF respons�vel por realizar cadastro do Colaborador.
        /// Se CPF apresentar atributo cadastrador com conte�do true, o atributo cpfCadastrador n�o aparecer�.
        /// </summary>
        public string CpfCadastrador { get; set; }

        /// <summary>
        /// Mostra a data e hora da vincula��o do CPF ao CNPJ.
        /// </summary>
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// Mostra a data e hora que o CPF poder� atuar com CNPJ.
        /// </summary>
        public DateTime? DataExpiracao { get; set; }

        /// <summary>
        /// Raz�o Social (Nome da empresa) cadastrada na Receita Federal.
        /// </summary>
        public string Nome { get; set; }
    }
}
