using System;

namespace NetBlade.Core.Security.LoginUnico
{
    public class ConfiabilidadeLoginUnico
    {
        /// <summary>
        /// Identifica��o para reconhecer o selo.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Identifica qual categoria pertence o selo adquirido pelo cidad�o.
        /// </summary>
        public string Categoria { get; set; }

        /// <summary>
        /// Identifica��o do selo em tela para o cidad�o.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descri��o padr�o do significado do selo.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Mostra a data e hora da cria��o do selo na conta do usu�rio.
        /// </summary>
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// Mostra a data e hora que ocorreu atualiza��o do selo na conta do usu�rio.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; }
    }
}
