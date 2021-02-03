using Microsoft.Extensions.DependencyInjection;
using NetBlade.Core.ValueObject;
using Xunit;

namespace NetBlade.Core.Test.ValueObject
{
    public class CpfValueObjectTest : IClassFixture<Startup>
    {
        public CpfValueObjectTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public void CpfValueObjectCastCpfValueObjectToStringTest()
        {
            CpfValueObject Cpf = new CpfValueObject("079.924.746-43");
            Assert.Equal("07992474643", Cpf);
        }

        [Fact]
        public void CpfValueObjectCastStringToCpfValueObjectTest()
        {
            CpfValueObject Cpf = (CpfValueObject)"079.924.746-43";
            Assert.Equal("07992474643", Cpf);
        }

        [Fact]
        public void CpfValueObjectIsNotValidTest()
        {
            CpfValueObject Cpf = (CpfValueObject)"079.924.746-41";
            Assert.False(Cpf.IsValid);
        }

        [Fact]
        public void CpfValueObjectIsValidTest()
        {
            CpfValueObject Cpf = (CpfValueObject)"079.924.746-43";
            Assert.True(Cpf.IsValid);
        }

        [Fact]
        public void CpfValueObjectToStringTest()
        {
            CpfValueObject Cpf = (CpfValueObject)"079.924.746-43";
            Assert.Equal("07992474643", Cpf.ToString());
        }
    }
}
