using System;

namespace NetBlade.Core.Security.LoginUnico
{
    public class CategoriaLoginUnico
    {
        /// <summary>
        /// Identificação para reconhecer a categoria.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Identifica qual nível pertence a categoria adicionada pelo cidadão.
        /// </summary>
        public string Nivel { get; set; }

        /// <summary>
        /// Identificação da categoria em tela para o cidadão.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição padrão do significado da categoria.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Mostra a data e hora que ocorreu atualização da categoria na conta do usuário.
        /// </summary>
        public DateTime? DataUltimaAtualizacao { get; set; }

    }
}
