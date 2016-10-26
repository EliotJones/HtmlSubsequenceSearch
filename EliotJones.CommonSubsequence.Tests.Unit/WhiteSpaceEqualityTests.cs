using Shouldly;
using UglyToad.Fixie.DataDriven;

namespace EliotJones.CommonSubsequence.Tests.Unit
{
    public class WhiteSpaceEqualityTests
    {
        public void BothNull()
        {
            var result = WhiteSpaceEquality.EqualsWhiteSpaceInvariant(null, null);

            result.ShouldBeTrue();
        }

        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("a poor test", "a poor test")]
        [InlineData("a POor test", "a POor test")]
        [InlineData("\n", "\n")]
        [InlineData("", "\n")]
        public void BothExactlyEqual(string s1, string s2)
        {
            var result = WhiteSpaceEquality.EqualsWhiteSpaceInvariant(s1, s2);

            result.ShouldBeTrue();
        }

        [InlineData("", " ")]
        [InlineData("apu", "apu\t")]
        [InlineData("\t\t\t  \napu", "\n\napu\t")]
        [InlineData("never forget\r\napu", "never forget apu  ")]
        public void BothEqualWithDifferingWhiteSpace(string x, string y)
        {
            var result = WhiteSpaceEquality.EqualsWhiteSpaceInvariant(x, y);

            result.ShouldBeTrue();
        }

        [InlineData("a", "A")]
        [InlineData("tony the POny", "TONY THE PONY")]
        [InlineData("tony the POny", "TONY\nTHE\r\nPONY")]
        public void CaseInsensitiveEqual(string x, string y)
        {
            var result = WhiteSpaceEquality.EqualsWhiteSpaceInvariant(x, y);

            result.ShouldBeTrue();
        }

        [InlineData("alfred", null)]
        [InlineData("alfred   ", "  golding")]
        [InlineData("go ulding", "  golding")]
        public void NotEqual(string x, string y)
        {
            var result = WhiteSpaceEquality.EqualsWhiteSpaceInvariant(x, y);

            result.ShouldBeFalse();
        }
    }
}
