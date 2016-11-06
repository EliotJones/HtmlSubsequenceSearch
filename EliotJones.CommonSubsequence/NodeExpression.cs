using AngleSharp.Dom;
using System.Collections.Generic;
using System.Linq;

namespace EliotJones.CommonSubsequence
{
    internal class NodeExpression
    {
        public bool Visited { get; private set; }

        public NodeExpression Parent { get; }

        public uint NodeType { get; }

        public uint Content { get; }

        public IReadOnlyList<NodeExpression> Children { get; }

        public IElement Element { get; }

        private readonly string _hash;

        private NodeExpression(NodeExpression parent, IElement element, ITextIndexer textIndexer)
        {
            NodeType = textIndexer.GetIndex(element.TagName);
            Content = textIndexer.GetIndex(element.TextContent);
            Parent = parent;
            Element = element;

            var children = new NodeExpression[element.Children.Length];

            for (int i = 0; i < element.Children.Length; i++)
            {
                children[i] = new NodeExpression(this, element.Children[i], textIndexer);
            }

            Children = children;
            _hash = $"[{NodeType}.{Content}.{string.Join(string.Empty, Children.Select(x => x.ToString()))}]";
        }

        public static NodeExpression Generate(IDocument element, ITextIndexer textIndexer)
        {
            return new NodeExpression(null, element.Body, textIndexer);
        }

        public void Visit(bool includeChildren = true)
        {
            Visited = true;

            if (includeChildren)
            {
                VisitChildren();
            }
        }

        private void VisitChildren()
        {
            foreach (var child in Children)
            {
                if (child.Visited)
                {
                    continue;
                }

                child.Visit();
            }
        }

        public bool Equals(NodeExpression node)
        {
            if(node == null)
            {
                return false;
            }

            if (ReferenceEquals(node, this))
            {
                return true;
            }

            if (Children.Count != node.Children.Count)
            {
                return false;
            }

            return _hash == node._hash;
        }

        public override string ToString()
        {
            return _hash;
        }
    }
}
