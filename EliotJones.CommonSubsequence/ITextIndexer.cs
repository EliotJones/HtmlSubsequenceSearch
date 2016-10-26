using System;
using System.Collections.Generic;

namespace EliotJones.CommonSubsequence
{
    public interface ITextIndexer
    {
        uint GetIndex(string text);
    }

    internal class TextIndexer : ITextIndexer
    {
        private static readonly object _lock = new object();

        private readonly Dictionary<string, uint> _indices = new Dictionary<string, uint>();

        private uint current;

        public uint GetIndex(string text)
        {
            var normalised = TextNormalizer.Normalize(text);

            lock(_lock)
            {
                uint value;
                if (_indices.TryGetValue(normalised, out value))
                {
                    return value;
                }
                
                _indices.Add(normalised, current);

                value = current;
                current++;
                return value;
            }
        }
    }
}
