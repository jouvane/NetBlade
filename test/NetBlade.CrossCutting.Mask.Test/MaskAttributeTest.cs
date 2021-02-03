using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace NetBlade.CrossCutting.Mask.Test
{
    public sealed class MaskAttributeTest : IClassFixture<Startup>
    {
        public MaskAttributeTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public void CleanValue_CNPJ_Test()
        {
            MaskAttribute mask = new MaskAttribute("99.999.999/9999-99");
            Assert.Equal("68837346000182", mask.CleanValue("68.837.346/0001-82"));
        }

        [Fact]
        public void CleanValue_CPF_Test()
        {
            MaskAttribute mask = new MaskAttribute("999.999.999-99");
            Assert.Equal("07992474643", mask.CleanValue("079.924.746-43"));
        }

        [Fact]
        public void CleanValue_MultMask_CNPJ_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "99.999.999/9999-99", "999.999.999-99" });
            Assert.Equal("68837346000182", mask.CleanValue("68.837.346/0001-82"));
        }

        [Fact]
        public void CleanValue_MultMask_CPF_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "99.999.999/9999-99", "999.999.999-99" });
            Assert.Equal("07992474643", mask.CleanValue("079.924.746-43"));
        }

        [Fact]
        public void CleanValue_MultMask_Telefone_8_Dig_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "(99) 9999-9999", "(31) 99999-9999" });
            Assert.Equal("3187423236", mask.CleanValue("(31) 8742-3236"));
        }

        [Fact]
        public void CleanValue_MultMask_Telefone_9_Dig_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "(99) 9999-9999", "(31) 99999-9999" });
            Assert.Equal("31987423236", mask.CleanValue("(31) 98742-3236"));
        }

        [Fact]
        public void Format_CNPJ_Test()
        {
            MaskAttribute mask = new MaskAttribute("99.999.999/9999-99");
            Assert.Equal("68.837.346/0001-82", mask.Format("68837346000182"));
        }

        [Fact]
        public void Format_CPF_Test()
        {
            MaskAttribute mask = new MaskAttribute("999.999.999-99");
            Assert.Equal("079.924.746-43", mask.Format("07992474643"));
        }

        [Fact]
        public void Format_End_Test()
        {
            MaskAttribute mask = new MaskAttribute("99.99+++++");
            Assert.Equal("12.34+++++", mask.Format("1234"));
        }

        [Fact]
        public void Format_INT_Test()
        {
            MaskAttribute mask = new MaskAttribute("999");
            Assert.Equal("001", mask.Format("1"));
        }

        [Fact]
        public void Format_Lat_Long_Test()
        {
            MaskAttribute mask = new MaskAttribute("+00°00'00.000\"");
            Assert.Equal("+60°58'35.100\"", mask.Format("605835100"));
        }

        [Fact]
        public void Format_MultMask_CNPJ_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "99.999.999/9999-99", "999.999.999-99" });
            Assert.Equal("68.837.346/0001-82", mask.Format("68837346000182"));
        }

        [Fact]
        public void Format_MultMask_CPF_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "99.999.999/9999-99", "999.999.999-99" });
            Assert.Equal("079.924.746-43", mask.Format("07992474643"));
        }

        [Fact]
        public void Format_MultMask_Telefone_8_Dig_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "(99) 9999-9999", "(99) 99999-9999" });
            Assert.Equal("(31) 8742-3236", mask.Format("3187423236"));
        }

        [Fact]
        public void Format_MultMask_Telefone_9_Dig_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "(99) 9999-9999", "(99) 99999-9999" });
            Assert.Equal("(31) 98742-3236", mask.Format("31987423236"));
        }

        [Fact]
        public void Format_MultMask_Value_Null_Test()
        {
            MaskAttribute mask = new MaskAttribute(new[] { "(99) 9999-9999", "(99) 99999-9999" });
            Assert.Equal(string.Empty, mask.Format(null));
        }
    }
}
