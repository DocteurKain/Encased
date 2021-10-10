namespace EncasedLib.Tools
{
    using System;
    using System.Text;

    public static class StringTool
    {
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