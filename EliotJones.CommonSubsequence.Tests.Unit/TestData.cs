using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EliotJones.CommonSubsequence.Tests.Unit
{
    internal class TestData
    {
        public static IEnumerable<object[]> Data()
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            var files = Directory.GetFiles(directory);

            if (files.Length == 0)
            {
                return new object[0][];
            }

            var fileIdentifiers = files.Select(x => new FileDataIdentifier(x))
                .Where(x => x.IsValid);

            var groups = fileIdentifiers.GroupBy(x => x.GroupNumber).ToList();

            var results = new List<object[]>();

            foreach (var group in groups)
            {
                var a = group.FirstOrDefault(x => x.Identifier == 'A');
                var b = group.FirstOrDefault(x => x.Identifier == 'B');
                var result = group.FirstOrDefault(x => x.IsResult);

                if (a == null || b == null || result == null)
                {
                    continue;
                }

                var resultText = File.ReadAllText(result.FilePath);

                var resultTexts = resultText.Split(new[] { "===" }, StringSplitOptions.RemoveEmptyEntries);

                if (resultTexts.Length != 2)
                {
                    continue;
                }

                results.Add(new object[]
                {
                    File.ReadAllText(a.FilePath),
                    File.ReadAllText(b.FilePath),
                    resultTexts[0],
                    resultTexts[1]
                });
            }

            return results;
        }
    }
}
