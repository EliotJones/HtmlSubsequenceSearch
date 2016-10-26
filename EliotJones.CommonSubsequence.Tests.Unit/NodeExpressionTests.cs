using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using FakeItEasy;
using System.Collections.Generic;
using Shouldly;

namespace EliotJones.CommonSubsequence.Tests.Unit
{
    public class NodeExpressionTests
    {
        public void CreateLinear()
        {
            var paragraph = Generate("p", "big");
            var div = Generate("div", "big", new[] { paragraph });
            var body = Generate("body", "big testing", new[] { div });
            var document = A.Fake<IDocument>();
            A.CallTo(() => document.Body).Returns(body);

            var result = NodeExpression.Generate(document, new TextIndexer());

            var divExpression = result.Children[0];
            var paragraphExpression = divExpression.Children[0];
            
            paragraphExpression.Children.ShouldBeEmpty();
            paragraphExpression.Content.ShouldBe(divExpression.Content);
            paragraphExpression.NodeType.ShouldNotBe(divExpression.NodeType);
            divExpression.Children.Count.ShouldBe(1);

            result.Content.ShouldNotBe(divExpression.Content);
        }

        public void CreateBranched()
        {
            var paragraphOne = Generate("p", "one");
            var paragraphTwo = Generate("p", "two");

            var div = Generate("div", "one\n\ntwo", new[] 
            {
                paragraphOne,
                paragraphTwo
            });

            var body = Generate("body", "one\n\ntwo", new[] { div });

            var document = A.Fake<IDocument>();
            A.CallTo(() => document.Body).Returns(body);

            var result = NodeExpression.Generate(document, new TextIndexer());

            var divExpression = result.Children[0];

            var paragraphOneExpression = divExpression.Children[0];
            var paragraphTwoExpression = divExpression.Children[1];

            result.Children.Count.ShouldBe(1);
            divExpression.Children.Count.ShouldBe(2);

            result.Content.ShouldBe(divExpression.Content);
            result.NodeType.ShouldNotBe(divExpression.NodeType);

            paragraphOneExpression.NodeType.ShouldBe(paragraphTwoExpression.NodeType);
            paragraphOneExpression.Content.ShouldNotBe(paragraphTwoExpression.NodeType);
        }

        private static IHtmlElement Generate(string tag, string text, IEnumerable<IElement> children = null)
        {
            var element = A.Fake<IHtmlElement>();

            A.CallTo(() => element.TagName).Returns(tag);
            A.CallTo(() => element.TextContent).Returns(text);

            A.CallTo(() => element.Children).Returns(new TestHtmlCollection(children));

            return element;
        }
    }
}
