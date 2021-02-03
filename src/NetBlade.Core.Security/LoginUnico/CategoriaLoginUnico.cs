using System;

namespace NetBlade.Core.Security.LoginUnico
{
    public class CategoriaLoginUnico
    {
        /// <summary>
        /// Identifica��o para reconhecer a categoria.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Identifica qual n�vel pertence a categoria adicionada pelo cidad�o.
        /// </summary>
        public string Nivel { get; set; }

        /// <summary>
        /// Identifica��o da categoria em tela para o cidad�o.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Descri��o padr�o do significado da categoria.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Mostra a data e hora que ocorreu atualiza��o da categoria na conta do usu�rio.
        /// </summary>
        public DateTime? DataUltimaAtualizacao { get; set; }

    }
}
