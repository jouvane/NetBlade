using NetBlade.CrossCutting.Mask;

namespace NetBlade.Core.Test.Helper.Models
{
    public class PessoaModel
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public string Nome { get; set; }

        [Mask("(99) 99999-9999")]
        public string Telefone { get; set; }
    }
}
