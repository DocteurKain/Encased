namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the usage statistics of the DeepL API plan.
    /// </summary>
    public class UsageStatistics
    {
        /// <summary>
        /// Gets the number of characters that have been translated so far.
        /// </summary>
        [JsonProperty("character_count")]
        public Int64 CharacterCount { get; private set; }

        /// <summary>
        /// Gets the quota of characters that can be translated.
        /// </summary>
        [JsonProperty("character_limit")]
        public Int64 CharacterLimit { get; private set; }
    }
}