namespace NetBlade.Core.Commands.Test.Model.CommandResponses
{
    public class SalvarPessoaCommandResponse : CommandResponse
    {
        protected SalvarPessoaCommandResponse(bool success)
            : base(success)
        {
        }

        public int Codigo { get; set; }

        public string Contrato { get; set; }
    }
}
