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
        }

        public static NodeExpression Generate(IDocument element, ITextIndexer textIndexer)
        {
            return new NodeExpression(null, element.Body, textIndexer);
        }

        public void Visit()
        {
            Visited = true;
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

            var childrenEqual = ChildrenEqual(this, node);

            return childrenEqual && Content == node.Content && NodeType == node.NodeType;
        }

        private static bool ChildrenEqual(NodeExpression nodeOne, NodeExpression nodeTwo)
        {
            foreach (var child in nodeOne.Children)
            {
                if (child.Visited)
                {
                    continue;
                }

                NodeExpression matchingChild = null;
                for (int i = 0; i < nodeTwo.Children.Count; i++)
                {
                    var nodeChild = nodeTwo.Children[i];

                    if (nodeChild.Visited)
                    {
                        continue;
                    }

                    if (nodeChild.Equals(child))
                    {
                        matchingChild = nodeChild;
                        break;
                    }
                }

                if (matchingChild == null)
                {
                    return false;
                }

                var childMatches = ChildrenEqual(child, matchingChild);

                if (!childMatches)
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"[{NodeType}.{Content}.{string.Join(string.Empty, Children.Select(x => x.ToString()))}]";
        }
    }
}
