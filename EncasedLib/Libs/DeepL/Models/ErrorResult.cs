namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the result, when an error occurs in the DeepL API.
    /// </summary>
    internal class ErrorResult
    {
        /// <summary>
        /// Gets the error message.
        /// </summary>
        [JsonProperty("message")]
        public String Message { get; private set; }
    }
}