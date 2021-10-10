namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a language that is supported by the DeepL API. While the <see cref="Language"/> enumeration contains the languages that
    /// are supported at the writing of this .NET implementation, a <see cref="SupportedLanguage"/> comes directly from the DeepL API itself
    /// and may therefore change over time.
    /// </summary>
    public class SupportedLanguage
    {
        /// <summary>
        /// The language code, which can be used as a parameter for translation.
        /// </summary>
        [JsonProperty("language")]
        public String LanguageCode { get; private set; }

        /// <summary>
        /// The English name of the language.
        /// </summary>
        [JsonProperty("name")]
        public String Name { get; private set; }
    }
}