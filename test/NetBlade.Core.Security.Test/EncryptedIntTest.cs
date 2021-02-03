using Xunit;

namespace NetBlade.Core.Security.Test
{
    public class EncryptedIntTest
    {
        [Theory, InlineData(1, "TSD2rwBJkRY="), InlineData(2, "j8XxjspMDVc="), InlineData(3, "mPVX3deRc1k="), InlineData(4, "NE6cYEmXSp8="), InlineData(5, "8qnxMfasrT0="), InlineData(6, "8ok/0Xsq61o="),
         InlineData(7, "i+PoKDPUS7A="), InlineData(8, "bizP3ZcnYNs="), InlineData(9, "wWziIKorGrw="), InlineData(10, "/VAitPO7Erw="), InlineData(11, "sAZYPC1Bcl8="), InlineData(12, "Xx3aec9gVTM="),
         InlineData(21, "mVTVOiOig2g="), InlineData(35, "zg56nBefaCI="), InlineData(44, "rBH6Ll+Zuqc="), InlineData(50, "XqRe2YG8s7c="), InlineData(62, "FZGw9MZUvLM="), InlineData(73, "X+LQATusu0I="),
         InlineData(84, "90Th+7a1UkI="), InlineData(95, "32N6wa/6N2g="), InlineData(100, "lZFuIXAGiZY="), InlineData(116, "qoL86QT87h4="), InlineData(127, "x6t+wGZ7re8="), InlineData(138, "no48wjstjao="),
         InlineData(149, "ICnB9sEYJXE="), InlineData(150, "VUKiHvfjzxk="), InlineData(190, "TOn/vdZrYIg="), InlineData(1000, "+y/4ls6MLw0="), InlineData(1001, "ROj+wv2SEqE="), InlineData(1120, "8VVvcOkftXs="),
         InlineData(1555, "P6TLOQUR/zo="), InlineData(5555, "BBdErJPXahQ="), InlineData(10000, "ELe75OHTEME="), InlineData(11111, "oWuDQcPWLUE="), InlineData(22222, "ENfiStgcN88="), InlineData(99999, "vVib3DNDOJ4="),
         InlineData(100000, "ra3CG60A3q0="), InlineData(900000, "KMTnJgSI9E8="), InlineData(1000000, "vxO2jyJmLlg="), InlineData(9999999, "8U0rdX59q7k="), InlineData(10000000, "te6rlKu5jHMJNFhAGMhndQ=="),
         InlineData(99999999, "7D+akWOK+hJ9IzsSGAULxA=="), InlineData(111111111, "iS1jL1L2PDeC7nWlxzkasw=="), InlineData(999999999, "7D+akWOK+hLpffcO8bZifQ=="), InlineData(1111111111, "iS1jL1L2PDcgV0yr+HXK+g=="),
         InlineData(2147483647, "vBsOS1Yx9PhZ68VNqTQLkw=="), InlineData(-21483647, "JnTsrkvF4IH2vj+1mEMoDg==")]
        public void EncryptedIntInlineDataDesencryptTest(int value, string encryptedValue)
        {
            Assert.Equal(value, new EncryptedInt(encryptedValue));
        }

        [Theory, InlineData(1, "TSD2rwBJkRY="), InlineData(2, "j8XxjspMDVc="), InlineData(3, "mPVX3deRc1k="), InlineData(4, "NE6cYEmXSp8="), InlineData(5, "8qnxMfasrT0="), InlineData(6, "8ok/0Xsq61o="),
         InlineData(7, "i+PoKDPUS7A="), InlineData(8, "bizP3ZcnYNs="), InlineData(9, "wWziIKorGrw="), InlineData(10, "/VAitPO7Erw="), InlineData(11, "sAZYPC1Bcl8="), InlineData(12, "Xx3aec9gVTM="),
         InlineData(21, "mVTVOiOig2g="), InlineData(35, "zg56nBefaCI="), InlineData(44, "rBH6Ll+Zuqc="), InlineData(50, "XqRe2YG8s7c="), InlineData(62, "FZGw9MZUvLM="), InlineData(73, "X+LQATusu0I="),
         InlineData(84, "90Th+7a1UkI="), InlineData(95, "32N6wa/6N2g="), InlineData(100, "lZFuIXAGiZY="), InlineData(116, "qoL86QT87h4="), InlineData(127, "x6t+wGZ7re8="), InlineData(138, "no48wjstjao="),
         InlineData(149, "ICnB9sEYJXE="), InlineData(150, "VUKiHvfjzxk="), InlineData(190, "TOn/vdZrYIg="), InlineData(1000, "+y/4ls6MLw0="), InlineData(1001, "ROj+wv2SEqE="), InlineData(1120, "8VVvcOkftXs="),
         InlineData(1555, "P6TLOQUR/zo="), InlineData(5555, "BBdErJPXahQ="), InlineData(10000, "ELe75OHTEME="), InlineData(11111, "oWuDQcPWLUE="), InlineData(22222, "ENfiStgcN88="), InlineData(99999, "vVib3DNDOJ4="),
         InlineData(100000, "ra3CG60A3q0="), InlineData(900000, "KMTnJgSI9E8="), InlineData(1000000, "vxO2jyJmLlg="), InlineData(9999999, "8U0rdX59q7k="), InlineData(10000000, "te6rlKu5jHMJNFhAGMhndQ=="),
         InlineData(99999999, "7D+akWOK+hJ9IzsSGAULxA=="), InlineData(111111111, "iS1jL1L2PDeC7nWlxzkasw=="), InlineData(999999999, "7D+akWOK+hLpffcO8bZifQ=="), InlineData(1111111111, "iS1jL1L2PDcgV0yr+HXK+g=="),
         InlineData(2147483647, "vBsOS1Yx9PhZ68VNqTQLkw=="), InlineData(-21483647, "JnTsrkvF4IH2vj+1mEMoDg==")]
        public void EncryptedIntInlineDataEncryptedTest(int value, string encryptedValue)
        {
            Assert.Equal(encryptedValue, ((EncryptedInt)value).ToString());
        }
    }
}
