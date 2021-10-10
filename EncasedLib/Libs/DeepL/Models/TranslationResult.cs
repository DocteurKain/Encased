namespace EncasedLib.Libs.DeepL.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the result of the translation of a text using the DeepL API, which contains one or more translations.
    /// </summary>
    internal class TranslationResult
    {
        /// <summary>
        /// Gets the translations (one for each text that was fed to the translation engine).
        /// </summary>
        [JsonProperty("translations")]
        public IEnumerable<Translation> Translations { get; private set; }
    }
}