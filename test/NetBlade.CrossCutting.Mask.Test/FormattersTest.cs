using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace NetBlade.CrossCutting.Mask.Test
{
    public sealed class FormattersTest : IClassFixture<Startup>
    {
        public FormattersTest(Startup startup)
        {
            this._serviceProvider = startup.ServiceProvider;
        }

        private readonly ServiceProvider _serviceProvider;

        [Fact]
        public void FormattersCalculoDigitoVerificadorNumeroProcessoTest()
        {
            Assert.Equal("27211815250198536", "272118152501985".CalculoDigitoVerificadorNumeroProcesso());
        }

        [Fact]
        public void FormattersCleNetBladeaskGuidTest()
        {
            Assert.Equal("5109E3FAB1004912B3CF9E189B8BC63F", "5109E3FA-B100-4912-B3CF-9E189B8BC63F".CleNetBladeaskGuid());
        }

        [Fact]
        public void FormattersCleNetBladeaskNumeroProcessoNetBladeTest()
        {
            Assert.Equal("11111222222333344", "11111.222222/3333-44".CleNetBladeaskNumeroProcessoNetBlade());
        }

        [Fact]
        public void FormattersMaskCEPTest()
        {
            Assert.Equal("30.620-190", "30620190".MaskCEP());
        }

        [Fact]
        public void FormattersMaskCnpjTest()
        {
            Assert.Equal("08.837.346/0001-82", "08837346000182".MaskCnpj());
        }

        [Fact]
        public void FormattersMaskCpfCnpjParaCnpjTest()
        {
            Assert.Equal("08.837.346/0001-82", "08837346000182".MaskCpfCnpj());
        }

        [Fact]
        public void FormattersMaskCpfCnpjParaCpfTest()
        {
            Assert.Equal("079.924.746-43", "07992474643".MaskCpfCnpj());
        }

        [Fact]
        public void FormattersMaskCpfTest()
        {
            Assert.Equal("079.924.746-43", "07992474643".MaskCpf());
        }

        [Fact]
        public void FormattersMaskGuidTest()
        {
            Assert.Equal("5109E3FA-B100-4912-B3CF-9E189B8BC63F", "5109E3FAB1004912B3CF9E189B8BC63F".MaskGuid());
        }

        [Fact]
        public void FormattersMaskNumeroProcessoNetBladeTest()
        {
            Assert.Equal("27211.815250/1985-36", "27211815250198536".MaskNumeroProcessoNetBlade());
        }

        [Fact]
        public void FormattersObterAnoProcessoNetBladeTest()
        {
            Assert.Equal("1985", Formatters.ObterAnoProcessoNetBlade("27211.815250/1985-36"));
        }

        [Fact]
        public void FormattersObterNumeroSemAnoTest()
        {
            Assert.Equal("815250", Formatters.ObterNumeroSemAno("815250/1985"));
        }

        [Fact]
        public void FormattersOnlyNumbersTest()
        {
            Assert.Equal("8152501985", "A8C1E/*5&º2!5.0/1985".OnlyNumbers());
        }
    }
}
