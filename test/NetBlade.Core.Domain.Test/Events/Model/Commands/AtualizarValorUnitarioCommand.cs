using NetBlade.Core.Commands;

namespace NetBlade.Core.Domain.Test.Events.Model.Commands
{
    public class AtualizarValorUnitarioCommand : Command
    {
        public int Codigo { get; set; }

        public double Valor { get; set; }
    }
}
