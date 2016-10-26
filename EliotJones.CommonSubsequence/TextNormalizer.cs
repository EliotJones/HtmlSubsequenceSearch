namespace EliotJones.CommonSubsequence
{
    internal class TextNormalizer
    {
        public static string Normalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i]))
                {
                    count++;
                }
            }

            var result = new char[s.Length - count];
            var currentIndex = 0;

            for (int i = 0; i < s.Length; i++)
            {
                var currentChar = s[i];
                if(!char.IsWhiteSpace(s[i]))
                {
                    result[currentIndex] = char.ToUpperInvariant(currentChar);
                    currentIndex++;
                }
            }

            return new string(result);
        }
    }
}
