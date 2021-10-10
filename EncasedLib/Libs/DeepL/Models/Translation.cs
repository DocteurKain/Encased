namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a single translation, which is part of a <see cref="TranslationResult"/>.
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// Gets the language detected in the source text. It reflects the value of the source language parameter, when specified.
        /// </summary>
        [JsonProperty("detected_source_language")]
        public String DetectedSourceLanguage { get; private set; }

        /// <summary>
        /// Gets the translated text.
        /// </summary>
        [JsonProperty("text")]
        public String Text { get; private set; }
    }
}