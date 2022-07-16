namespace EncasedLib.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringTool
    {
        private static Char[] sElements = new Char[] { '.', '!', '?' };

        public static List<String> ToSentences(this String text)
        {
            var r = new List<String>();
            var sb = new StringBuilder();

            foreach (var c in text)
            {
                sb.Append(c);

                if (sElements.Contains(c))
                {
                    r.Add(sb.ToString());
                    sb.Clear();
                }
            }

            return r;
        }

        public static String[] SplitInSentences(this String value)
        {
            return Regex
                .Split(value, @"(?<=[.?!])")
                .Where(a => !String.IsNullOrEmpty(a))
                .ToArray();
        }

        public static String ToOneLine(this String text)
        {
            text = text.Replace("\r", "[r]");
            text = text.Replace("\n", "[n]");
            text = text.Replace("\t", "[t]");

            return text;
        }

        public static String ToMultiLine(this String text)
        {
            text = text.Replace("[r]", "\r");
            text = text.Replace("[n]", "\n");
            text = text.Replace("[t]", "\t");

            return text;
        }

        public static Int32 LevenshteinDistance(String source1, String source2)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new Int32[source1Length + 1, source2Length + 1];

            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[source1Length, source2Length];
        }
    }
}