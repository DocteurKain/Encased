namespace EncasedLib.Libs.DeepL.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the options for the handling of XML of the DeepL translation engine. At the time of writing this .NET implementation, XML
    /// handling for Japanese or Chinese texts is not supported by the DeepL API.
    /// </summary>
    public class XmlHandling
    {
        /// <summary>
        /// Gets or sets a list of XML tags which never split sentences.
        /// </summary>
        public IEnumerable<String> NonSplittingTags { get; set; }

        /// <summary>
        /// Gets or sets a list of XML tags which always cause splits.
        /// </summary>
        public IEnumerable<String> SplittingTags { get; set; }

        /// <summary>
        /// Gets or sets a list of XML tags that indicate text not to be translated.
        /// </summary>
        public IEnumerable<String> IgnoreTags { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether the outline of an XML document is automatically detected. Automatic outline
        /// detection may not always provide the best translation results, therefore it can make sense to disable it and provide a custom
        /// list of splitting and non-splitting tags.
        /// </summary>
        public Boolean OutlineDetection { get; set; } = true;
    }
}