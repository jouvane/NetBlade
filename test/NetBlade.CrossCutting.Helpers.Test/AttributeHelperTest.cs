using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Test.Helper.Models;
using NetBlade.CrossCutting.Mask;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.CrossCutting.Helpers.Test
{
    public class AttributeHelperTest : IClassFixture<Startup>
    {
        public AttributeHelperTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public async Task AttributeHelperExtractAttributeExpressionNullTest()
        {
            MaskAttribute attr = AttributeHelper.ExtractAttribute<MaskAttribute, PessoaModel, string>(e => e.Nome);
            Assert.Null(attr);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task AttributeHelperExtractAttributeExpressionTest()
        {
            MaskAttribute attr = AttributeHelper.ExtractAttribute<MaskAttribute, PessoaModel, string>(e => e.Telefone);
            Assert.NotNull(attr);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task AttributeHelperExtractAttributeNullTest()
        {
            MaskAttribute attr = AttributeHelper.ExtractAttribute<MaskAttribute>(typeof(PessoaModel).GetProperty(nameof(PessoaModel.Nome)));
            Assert.Null(attr);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task AttributeHelperExtractAttributeTest()
        {
            MaskAttribute attr = AttributeHelper.ExtractAttribute<MaskAttribute>(typeof(PessoaModel).GetProperty(nameof(PessoaModel.Telefone)));
            Assert.NotNull(attr);

            await Task.CompletedTask;
        }
    }
}
