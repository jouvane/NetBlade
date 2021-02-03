using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.Domain.Test.Models;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.Core.Domain.Test
{
    public class EntityTest : IClassFixture<Startup>
    {
        public EntityTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public async Task EntityGetHashCodeTest()
        {
            PessoaModel p = new PessoaModel
            {
                ID = 1
            };

            Assert.IsType<int>(p.GetHashCode());
            await Task.CompletedTask;
        }

        [Fact]
        public async Task EntityIsNewTest()
        {
            PessoaModel p = new PessoaModel();
            Assert.True(p.IsNew);
            await Task.CompletedTask;
        }

        [Fact]
        public async Task EntityIsNotNewTest()
        {
            PessoaModel p = new PessoaModel
            {
                ID = 1
            };

            Assert.False(p.IsNew);
            await Task.CompletedTask;
        }

        [Fact]
        public async Task EntityNotHasEventsTest()
        {
            PessoaModel p = new PessoaModel();
            Assert.False(p.HasEvents());
            await Task.CompletedTask;
        }
    }
}
