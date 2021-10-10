namespace EncasedLib.Models
{
    using System;

    public class Element
    {
        public String Address { get; set; }
        public String Category { get; set; }
        public String Text { get; set; }

        public Element() { }

        public Element(String address, String category, String text)
        {
            Address = address;
            Category = category;
            Text = text;
        }

        public override String ToString()
        {
            if (Text.Length == 0)
                return $"[{Address}] empty";
            if (Text.Length > 40)
                return $"[{Address}] {Text.Substring(0, 40)}";
            return $"[{Address}] {Text}";
        }
    }
}