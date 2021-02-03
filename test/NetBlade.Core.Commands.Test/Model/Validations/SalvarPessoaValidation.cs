using FluentValidation;
using NetBlade.Core.Commands.Test.Model.Commands;

namespace NetBlade.Core.Commands.Test.Model.Validations
{
    public class SalvarPessoaValidation : AbstractValidator<SalvarPessoaCommand>
    {
        public SalvarPessoaValidation()
        {
            this.ValidarNome();
        }

        private void ValidarNome()
        {
            this.RuleFor(c => c.Nome)
               .MaximumLength(200)
               .WithMessage("O campo \"Nome\" nao pode ser maior que 200.");
        }
    }
}
