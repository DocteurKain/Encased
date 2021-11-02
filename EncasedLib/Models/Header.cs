namespace EncasedLib.Models
{
    using System;
    using System.Collections.Generic;

    public class Header
    {
        public String Filename { get; set; }
        public List<Byte> Data { get; set; } = new();

        public Header() { }

        public Header(String filename, List<Byte> data)
        {
            Filename = filename;
            Data = data;
        }

        public override String ToString()
        {
            if (Filename.Length == 0)
                return "[-] empty";
            return $"{Filename} sz:{Data.Count}";
        }
    }
}