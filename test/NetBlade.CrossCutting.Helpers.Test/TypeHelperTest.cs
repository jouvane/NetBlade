using NetBlade.Core.Test.Helper.Models;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.CrossCutting.Helpers.Test
{
    public class TypeHelperTest
    {
        [Fact]
        public async Task TypeHelperCheckIsDerivedTest()
        {
            Assert.True(TypeHelper.CheckIsDerived<PessoaModel, PessoaFisicaModel>());
            Assert.False(TypeHelper.CheckIsDerived<PessoaFisicaModel, PessoaModel>());
            Assert.False(TypeHelper.CheckIsDerived<PessoaModel, TypeHelperTest>());

            await Task.CompletedTask;
        }
    }
}
