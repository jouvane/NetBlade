using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.CrossCutting.Helpers.Test
{
    public class StringHelperTest : IClassFixture<Startup>
    {
        public StringHelperTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public async Task StringHelperOnlyNumbersNullTest()
        {
            string cpf = StringHelper.OnlyNumbers(null);
            Assert.Equal(string.Empty, cpf);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task StringHelperOnlyNumbersTest()
        {
            string cpf = "079.924.746-43".OnlyNumbers();
            Assert.Equal("07992474643", cpf);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task StringHelperRemoveSpecialCharacterNullTest()
        {
            string value = StringHelper.RemoveSpecialCharacter(null);
            Assert.Equal(string.Empty, value);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task StringHelperRemoveSpecialCharacterTest()
        {
            string value = "Ge0V@Ne 4l%e!.º´".RemoveSpecialCharacter();
            Assert.Equal("Ge0VNe4le", value);

            await Task.CompletedTask;
        }
    }
}
