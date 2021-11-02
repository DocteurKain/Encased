namespace EncasedLib.Models
{
    using System;
    using System.Collections.Generic;

    public class Locale
    {
        public String Filename { get; set; }
        public List<Byte> Header { get; set; } = new();
        public List<LocaleLine> Lines { get; set; } = new();

        public Locale() { }

        public override String ToString()
        {
            if(Lines.Count < 2)
                return $"[Loc] {Lines.Count} line";
            return $"[Loc] {Lines.Count} lines";
        }
    }
}