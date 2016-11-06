using System.Collections.Generic;

namespace EliotJones.CommonSubsequence
{
    internal class NodeExpressionGraphComparer
    {
        public List<NodeExpressionMatchPair> Compare(NodeExpression graph1, NodeExpression graph2)
        {
            var graph2Leaves = FetchLeaves(graph2);

            var currentNode1 = graph1;
            while (currentNode1.Children.Count > 0)
            {
                currentNode1 = currentNode1.Children[0];
            }

            var results = new List<NodeExpressionMatchPair>();

            while (currentNode1 != null)
            {
                SearchLeafNode(currentNode1, graph2Leaves, results);

                currentNode1 = NextLeafNode(false, currentNode1);
            }            

            return results;
        }

        private NodeExpression NextLeafNode(bool goingUp, NodeExpression currentNode)
        {
            if (currentNode == null)
            {
                return null;
            }

            if (currentNode.Children.Count == 0)
            {
                if (currentNode.Visited)
                {
                    return NextLeafNode(true, currentNode.Parent);
                }

                return currentNode;
            }

            currentNode.Visit(false);

            if (goingUp)
            {
                return TryGoDown(currentNode);
            }

            for (int i = 0; i < currentNode.Children.Count; i++)
            {
                var child = currentNode.Children[i];

                if (!child.Visited)
                {
                    return NextLeafNode(false, child);
                }
            }

            return NextLeafNode(true, currentNode.Parent);
        }

        public NodeExpression TryGoDown(NodeExpression node)
        {
            if (node.Children.Count == 1)
            {
                return NextLeafNode(true, node.Parent);
            }

            for (int i = 0; i < node.Children.Count; i++)
            {
                var child = node.Children[i];

                if (!child.Visited)
                {
                    return NextLeafNode(false, child);
                }
            }

            return NextLeafNode(true, node.Parent);
        }

        private void SearchLeafNode(NodeExpression currentLeafNode, IReadOnlyList<NodeExpression> allLeaves, List<NodeExpressionMatchPair> results)
        {
            currentLeafNode.Visit();

            NodeExpression match = null;
            for (int i = 0; i < allLeaves.Count; i++)
            {
                if (allLeaves[i].Equals(currentLeafNode))
                {
                    match = allLeaves[i];
                    break;
                }
            }

            if (match == null)
            {
                return;
            }

            match.Visit();
            
            while (match.Parent?.Equals(currentLeafNode?.Parent) == true)
            {
                if (match.Parent.Children.Count != currentLeafNode.Parent.Children.Count)
                {
                    break;
                }

                match.Visit();
                currentLeafNode.Visit();

                match = match.Parent;
                currentLeafNode = currentLeafNode.Parent;
            }

            results.Add(new NodeExpressionMatchPair(match, currentLeafNode));
        }

        private IReadOnlyList<NodeExpression> FetchLeaves(NodeExpression graph)
        {
            var results = new List<NodeExpression>();

            FetchLeaf(results, graph);

            return results;
        }

        public void FetchLeaf(List<NodeExpression> results, NodeExpression graph)
        {
            if (graph.Children.Count == 0)
            {
                results.Add(graph);
                return;
            }

            for (int i = 0; i < graph.Children.Count; i++)
            {
                FetchLeaf(results, graph.Children[i]);
            }
        }
    }
}
