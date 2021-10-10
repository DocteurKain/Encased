namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using Newtonsoft.Json;

    public class Glossary
    {
        [JsonProperty("glossary_id")]
        public String GlossaryId { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("ready")]
        public Boolean IsEnabled { get; set; }

        [JsonProperty("source_lang")]
        public String SourceLanguage { get; set; }

        [JsonProperty("target_lang")]
        public String TargetLanguage { get; set; }

        [JsonProperty("creation_time")]
        public DateTime CreationTime { get; set; }

        [JsonProperty("entry_count")]
        public Int32 EntryCount { get; set; }
    }
}