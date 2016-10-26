using AngleSharp.Dom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EliotJones.CommonSubsequence.Tests.Unit
{
    internal class TestHtmlCollection : IHtmlCollection<IElement>
    {
        private readonly IReadOnlyList<IElement> elements;

        public TestHtmlCollection(IEnumerable<IElement> elements)
        {
            if (elements != null)
            {
                this.elements = elements.ToList();
            }
            else
            {
                this.elements = new IElement[0];
            }
        }

        public IElement this[string id]
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public IElement this[int index] => elements[index];

        public int Length => elements.Count;

        public IEnumerator<IElement> GetEnumerator() => elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => elements.GetEnumerator();
    }
}
