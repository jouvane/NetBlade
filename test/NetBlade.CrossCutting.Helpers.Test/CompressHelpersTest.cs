using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Test.Helper.Models;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.CrossCutting.Helpers.Test
{
    public class CompressHelpersTest : IClassFixture<Startup>
    {
        public CompressHelpersTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public async Task CompressHelpersCompressAndDecompressObjectEqualTest()
        {
            PessoaModel p = new PessoaModel
            {
                Codigo = 1010,
                Descricao = "Descricao Descricao Descricao",
                Nome = "Geovane Alves Simões",
                Telefone = "31 987423236"
            };

            byte[] pCompress = CompressHelpers.SerializerJsonAndCompress(p);
            PessoaModel pDecompress = CompressHelpers.DeserializeJsonAndDecompress<PessoaModel>(pCompress);

            Assert.Equal(p.Codigo, pDecompress.Codigo);
            Assert.Equal(p.Descricao, pDecompress.Descricao);
            Assert.Equal(p.Nome, pDecompress.Nome);
            Assert.Equal(p.Telefone, pDecompress.Telefone);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task CompressHelpersCompressAndDecompressTest()
        {
            PessoaModel p = new PessoaModel
            {
                Codigo = 1010,
                Descricao = "Descricao Descricao Descricao",
                Nome = "Geovane Alves Simões",
                Telefone = "31 987423236"
            };

            byte[] pCompress = CompressHelpers.SerializerJsonAndCompress(p);
            Assert.NotNull(pCompress);

            PessoaModel pDecompress = CompressHelpers.DeserializeJsonAndDecompress<PessoaModel>(pCompress);
            Assert.NotNull(pDecompress);

            await Task.CompletedTask;
        }
    }
}
