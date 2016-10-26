using System.Collections.Generic;

namespace EliotJones.CommonSubsequence
{
    internal struct NodeExpressionMatchPair
    {
        public NodeExpression NodeOne { get; }

        public NodeExpression NodeTwo { get; }

        public NodeExpressionMatchPair(NodeExpression nodeOne, NodeExpression nodeTwo)
        {
            NodeOne = nodeOne;
            NodeTwo = nodeTwo;
        }

        public IEnumerable<NodeExpression> Get()
        {
            yield return NodeOne;
            yield return NodeTwo;
        }
    }
}
