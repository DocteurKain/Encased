namespace EncasedLib.Tools
{
    using System;
    using System.Text;

    public static class StringTool
    {
        public static String BytesToData(this Byte[] value)
        {
            var text = Encoding.UTF8.GetString(value);

            /*text = text.Replace("\r", "[r]");
            text = text.Replace("\n", "[n]");
            text = text.Replace("\t", "[t]");*/

            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            text = text.Replace("\t", "");

            return text;
        }
    }
}