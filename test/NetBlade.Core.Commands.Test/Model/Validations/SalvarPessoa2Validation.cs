using FluentValidation;
using NetBlade.Core.Commands.Test.Model.Commands;

namespace NetBlade.Core.Commands.Test.Model.Validations
{
    public class SalvarPessoa2Validation : AbstractValidator<SalvarPessoa2Command>
    {
        public SalvarPessoa2Validation()
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
