namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an ongoing translation of a document that was uploaded to the DeepL API.
    /// </summary>
    public class DocumentTranslation
    {
        /// <summary>
        /// Gets a unique ID assigned to the uploaded document and the translation process. Must be used when referring to this particular
        /// document in subsequent API requests.
        /// </summary>
        [JsonProperty("document_id")]
        public String DocumentId { get; private set; }

        /// <summary>
        /// Gets a unique key that is used to encrypt the uploaded document as well as the resulting translation on the server side. Must be
        /// provided with every subsequent API request regarding this particular document.
        /// </summary>
        [JsonProperty("document_key")]
        public String DocumentKey { get; private set; }
    }
}