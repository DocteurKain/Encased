namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Glossaries
    {
        [JsonProperty("glossaries")]
        public List<Glossary> GlossaryList { get; set; }
    }
}