using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.ValueObject;
using Xunit;

namespace NetBlade.Core.Test.ValueObject
{
    public class CnpjValueObjectTest : IClassFixture<Startup>
    {
        public CnpjValueObjectTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public void CnpjValueObjectCastCnpjValueObjectToStringTest()
        {
            CnpjValueObject cnpj = new CnpjValueObject("02.179.636/0001-78");
            Assert.Equal("02179636000178", cnpj);
        }

        [Fact]
        public void CnpjValueObjectCastStringToCnpjValueObjectTest()
        {
            CnpjValueObject cnpj = (CnpjValueObject)"02.179.636/0001-78";
            Assert.Equal("02179636000178", cnpj);
        }

        [Fact]
        public void CnpjValueObjectIsNotValidTest()
        {
            CnpjValueObject cnpj = (CnpjValueObject)"02.179.636/0001-71";
            Assert.False(cnpj.IsValid);
        }

        [Fact]
        public void CnpjValueObjectIsValidTest()
        {
            CnpjValueObject cnpj = (CnpjValueObject)"02.179.636/0001-78";
            Assert.True(cnpj.IsValid);
        }

        [Fact]
        public void CnpjValueObjectToStringTest()
        {
            CnpjValueObject cnpj = (CnpjValueObject)"02.179.636/0001-78";
            Assert.Equal("02179636000178", cnpj.ToString());
        }
    }
}
