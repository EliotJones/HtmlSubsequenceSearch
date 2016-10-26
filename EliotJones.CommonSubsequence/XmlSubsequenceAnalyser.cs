using System.Collections.Generic;
using AngleSharp.Dom.Html;
using System.Linq;

namespace EliotJones.CommonSubsequence
{
    public class XmlSubsequenceAnalyser : IXmlSubsequenceAnalyser
    {
        private readonly ITextIndexer textIndexer;

        public XmlSubsequenceAnalyser(ITextIndexer textIndexer)
        {
            this.textIndexer = textIndexer;
        }

        public IReadOnlyList<string> GetCommonRegions(IHtmlDocument documentOne, IHtmlDocument documentTwo)
        {
            var expressionOne = NodeExpression.Generate(documentOne, textIndexer);
            var expressionTwo = NodeExpression.Generate(documentTwo, textIndexer);

            var commonNodes = new NodeExpressionGraphComparer().Compare(expressionOne, expressionTwo);

            var result = commonNodes.SelectMany(x => x.Get()).Select(x => x.Element.OuterHtml).ToList();

            return result;

            if(!WhiteSpaceEquality.EqualsWhiteSpaceInvariant(documentOne.Body.InnerHtml, documentTwo.Body.InnerHtml))
            {
                return new List<string>();
            }

            return new List<string>
            {
                documentOne.Body.InnerHtml,
                documentTwo.Body.InnerHtml
            };
        }
    }
}
