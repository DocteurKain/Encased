namespace EncasedLib.Models
{
    using System;

    public class EtfLine
    {
        public String Address { get; set; }
        public String Category { get; set; }
        public String Source { get; set; }
        public String Target { get; set; }

        public EtfLine() { }

        public EtfLine(String address, String category, String source, String target)
        {
            Address = address;
            Category = category;
            Source = source;
            Target = target;
        }

        public override String ToString()
        {
            if (Source.Length == 0)
                return $"[{Address}] empty";
            if (Source.Length > 40)
                return $"[{Address}] {Source.Substring(0, 40)}";
            return $"[{Address}] {Source}";
        }
    }
}