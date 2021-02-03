using System;

namespace NetBlade.Core.Security.LoginUnico
{
    public class ConfiabilidadeLoginUnico
    {
        /// <summary>
        /// Identificação para reconhecer o selo.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Identifica qual categoria pertence o selo adquirido pelo cidadão.
        /// </summary>
        public string Categoria { get; set; }

        /// <summary>
        /// Identificação do selo em tela para o cidadão.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição padrão do significado do selo.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Mostra a data e hora da criação do selo na conta do usuário.
        /// </summary>
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// Mostra a data e hora que ocorreu atualização do selo na conta do usuário.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
    }
}
