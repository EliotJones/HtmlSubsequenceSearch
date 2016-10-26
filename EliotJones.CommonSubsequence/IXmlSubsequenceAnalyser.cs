using AngleSharp.Dom.Html;
using System.Collections.Generic;

namespace EliotJones.CommonSubsequence
{
    public interface IXmlSubsequenceAnalyser
    {
        IReadOnlyList<string> GetCommonRegions(IHtmlDocument documentOne, IHtmlDocument documentTwo);
    }
}
