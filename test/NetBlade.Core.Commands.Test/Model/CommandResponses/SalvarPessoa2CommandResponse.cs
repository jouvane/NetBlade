namespace NetBlade.Core.Commands.Test.Model.CommandResponses
{
    public class SalvarPessoa2CommandResponse : CommandResponse
    {
        protected SalvarPessoa2CommandResponse(bool success)
            : base(success)
        {
        }

        public int Codigo { get; set; }

        public string Contrato { get; set; }
    }
}
