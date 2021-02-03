using NetBlade.Core.Test.Helper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NetBlade.CrossCutting.Helpers.Test
{
    public class EnumHelpersTest
    {
        [Fact]
        public async Task EnumHelpersGetDescriptionTest()
        {
            string descA = EnumHelpers.GetDescription(TestEnum.AA);
            string descB = EnumHelpers.GetDescription(TestEnum.BB);
            string descC = EnumHelpers.GetDescription(TestEnum.CC);

            Assert.Equal("Test AA", descA);
            Assert.Equal("Test BB", descB);
            Assert.Equal("Test CC", descC);
            await Task.CompletedTask;
        }

        [Fact]
        public async Task EnumHelpersGetItemByValueTest()
        {
            TestEnum a = EnumHelpers.GetItemByValue<TestEnum>(typeof(TestEnum), "0");
            TestEnum b = EnumHelpers.GetItemByValue<TestEnum>(typeof(TestEnum), "2");
            TestEnum c = EnumHelpers.GetItemByValue<TestEnum>(typeof(TestEnum), "1");

            Assert.Equal(TestEnum.AA, a);
            Assert.Equal(TestEnum.BB, b);
            Assert.Equal(TestEnum.CC, c);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task EnumHelpersListAllItemEnumTest()
        {
            List<Tuple<string, string>> values = EnumHelpers.ListAllItemEnum<TestEnum>();

            Assert.NotNull(values);
            Assert.Equal(3, values.Count);

            Assert.Equal("0", values[0].Item1);
            Assert.Equal("2", values[1].Item1);
            Assert.Equal("1", values[2].Item1);

            Assert.Equal("Test AA", values[0].Item2);
            Assert.Equal("Test BB", values[1].Item2);
            Assert.Equal("Test CC", values[2].Item2);

            await Task.CompletedTask;
        }
    }
}
