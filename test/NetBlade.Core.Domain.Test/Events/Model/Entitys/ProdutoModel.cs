using NetBlade.Core.Domain.Test.Events.Model.DomainEvents;

namespace NetBlade.Core.Domain.Test.Events.Model.Entitys
{
    public class ProdutoModel : EntityInt
    {
        public string Nome { get; set; }

        public double ValorUnitario { get; set; }

        public void AtualizarValorUnitario(double valorUnitario)
        {
            double oldValorUnitario = this.ValorUnitario;
            this.ValorUnitario = valorUnitario;
            if (this.ValorUnitario != oldValorUnitario)
            {
                this.RegisterEvent(new AtualizarValorUnitarioEvent(this, oldValorUnitario));
            }
        }
    }
}
