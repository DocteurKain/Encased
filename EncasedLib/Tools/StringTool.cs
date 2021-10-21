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
    }
}