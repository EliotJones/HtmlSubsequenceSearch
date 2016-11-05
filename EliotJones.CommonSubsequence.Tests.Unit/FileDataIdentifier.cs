using System;
using System.Text.RegularExpressions;

namespace EliotJones.CommonSubsequence.Tests.Unit
{
    internal class FileDataIdentifier
    {
        private static readonly Regex _validator =
            new Regex(@"TestCase(?<int>\d)(?<identifier>[AB]|Result).html");

        public char Identifier { get; }

        public string FilePath { get; }

        public int GroupNumber { get; }

        public bool IsResult { get; }

        public bool IsValid { get; } = true;

        public FileDataIdentifier(string filePath)
        {
            FilePath = filePath;

            if (string.IsNullOrWhiteSpace(filePath))
            {
                IsValid = false;
                return;
            }

            var match = _validator.Match(filePath);

            if (!match.Success)
            {
                IsValid = false;
                return;
            }

            GroupNumber = Convert.ToInt32(match.Groups["int"].Value);

            var identity = match.Groups["identifier"];

            if (identity.Value == "Result")
            {
                IsResult = true;
                Identifier = 'R';
                return;
            }

            Identifier = identity.Value[0];
        }
    }
}
