namespace EliotJones.CommonSubsequence
{
    internal class WhiteSpaceEquality
    {
        public static bool EqualsWhiteSpaceInvariant(string x, string y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if(x == null || y == null)
            {
                return false;
            }
            
            var yIndex = 0;

            for (int i = 0; i < x.Length; i++)
            {
                var xChar = x[i];

                if (char.IsWhiteSpace(xChar))
                {
                    continue;
                }
                
                for (int j = yIndex; j < y.Length; j++)
                {
                    yIndex = j + 1;
                    var yChar = y[j];

                    if (char.IsWhiteSpace(yChar))
                    {
                        continue;
                    }

                    if (!char.ToUpperInvariant(xChar).Equals(char.ToUpperInvariant(yChar)))
                    {
                        return false;
                    }

                    break;
                }
            }

            return true;
        }
    }
}
