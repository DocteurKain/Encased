namespace EncasedLib.Models
{
    using System;
    using System.Collections.Generic;

    public class Locale
    {
        public String Filename { get; set; }
        public Language Language { get; set; }
        public List<Byte> Header { get; set; } = new();
        public List<Element> Elements { get; set; } = new();

        public Locale() { }

        public override String ToString()
        {
            if(Elements.Count < 2)
                return $"[Loc] {Elements.Count} element";
            return $"[Loc] {Elements.Count} elements";
        }
    }
}