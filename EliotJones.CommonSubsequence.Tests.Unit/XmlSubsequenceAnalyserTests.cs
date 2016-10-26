using AngleSharp.Parser.Html;
using Shouldly;
using System.Collections.Generic;
using UglyToad.Fixie.DataDriven;

namespace EliotJones.CommonSubsequence.Tests.Unit
{
    public class XmlSubsequenceAnalyserTests
    {
        private readonly IXmlSubsequenceAnalyser subsequenceAnalyser = new XmlSubsequenceAnalyser(new TextIndexer());
        private readonly HtmlParser parser = new HtmlParser();

        [InlineData("", "")]
        [InlineData("<div>Malcolm <span>Tucker</span></div>", "<div>Malcolm <span>Tucker</span></div>")]
        [InlineData("<p>Red</p>", "<p>Red</p>")]
        public void SameString(string text1, string text2)
        {
            var documentOne = parser.Parse(text1);
            var documentTwo = parser.Parse(text2);

            var result = subsequenceAnalyser.GetCommonRegions(documentOne, documentTwo);

            result.Count.ShouldBe(2);
            result.ShouldContain($"<body>{text1}</body>");
        }

        [InlineData("", " ", "")]
        [InlineData("", " \n", "")]
        [InlineData("", " \n", "")]
        [InlineData("<p> Balthazar</p>", "\n<p>Balthazar</p>", "<p>Balthazar</p>")]
        public void SameStringWhiteSpaceDiffers(string text1, string text2, string expected)
        {
            var documentOne = parser.Parse(text1);
            var documentTwo = parser.Parse(text2);

            var result = subsequenceAnalyser.GetCommonRegions(documentOne, documentTwo);

            result.Count.ShouldBe(2);
            result.ShouldContain($"<body>{expected}</body>");
        }

        public void SameStringWhiteSpaceAndCaseDiffers()
        {
            var documentOne = parser.Parse("<p >Tony THE PONY</p>");
            var documentTwo = parser.Parse("<p>Tony The \n\nPony\n</p>");

            var result = subsequenceAnalyser.GetCommonRegions(documentOne, documentTwo);

            result.Count.ShouldBe(2);

            result.ShouldContain("<body><p>Tony THE PONY</p></body>");
            result.ShouldContain("<body><p>Tony The \n\nPony\n</p></body>");
        }

        [InlineData("<span>Mike</span>", "<p>Mike</p>")]
        [InlineData("<div>Mike</div>", "<p>Mike</p>")]
        [InlineData("<div><p>Mike</p></div>", "<span>Mike</span>")]
        public void DifferentTags(string text1, string text2)
        {
            var documentOne = parser.Parse(text1);
            var documentTwo = parser.Parse(text2);

            var result = subsequenceAnalyser.GetCommonRegions(documentOne, documentTwo);

            result.ShouldBeEmpty();
        }

        [InlineData("<div><p>Bertold</p></div>", "<p>Bertold</p>", "<p>Bertold</p>")]
        [InlineData("<div><p>Home <span>Menu</span></p></div>", "<nav><p>Home <span>Menu</span></p></div>", "<p>Home <span>Menu</span></p>")]
        public void NestedTagDetected(string text1, string text2, string expected)
        {
            var documentOne = parser.Parse(text1);
            var documentTwo = parser.Parse(text2);

            var result = subsequenceAnalyser.GetCommonRegions(documentOne, documentTwo);

            result.Count.ShouldBe(2);

            result.ShouldAllBe(x => x == expected);
        }

        private static readonly IEnumerable<object[]> ComplexCaseData = new[]
        {
            new object[]
            {
                "<div><p>Eric <span>Ericson</span></p><p>Funky</p></div>",
                "<div><p>Eric <span>Ericson</span></p></div>",
                "<p>Eric <span>Ericson</span></p>",
                "<p>Eric <span>Ericson</span></p>"
            },
            new object[]
            {
                "<div><p>Eric <span>Ericson</span></p><p>Funky</p></div>",
                "<div><p>Eric <span>Ericson</span></p><p>Dilated Peoples</p></div>",
                "<p>Eric <span>Ericson</span></p>",
                "<p>Eric <span>Ericson</span></p>"
            },
            new object[]
            {
                "<div><p>Nested <span>Stuff</span></p><img/></div>",
                "<div><p>Nested <span>STUFF</span></p><p></p></div>",
                "<p>Nested <span>Stuff</span></p>",
                "<p>Nested <span>STUFF</span></p>"
            },
            new object[] 
            {
                "<div></div><div><p>Eric <span>Ericson</span></p></div>",
                "<p>Eric<span>Ericson</span></p>",
                "<p>Eric <span>Ericson</span></p>",
                "<p>Eric<span>Ericson</span></p>"
            }
        };

        [MemberData(nameof(ComplexCaseData))]
        public void ComplexSearch(string text1, string text2, string expected1, string expected2)
        {
            var documentOne = parser.Parse(text1);
            var documentTwo = parser.Parse(text2);

            var result = subsequenceAnalyser.GetCommonRegions(documentOne, documentTwo);

            result.Count.ShouldBe(2);

            result.ShouldContain(expected1);
            result.ShouldContain(expected2);
        }
    }
}
