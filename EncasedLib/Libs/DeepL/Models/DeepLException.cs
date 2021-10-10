namespace EncasedLib.Libs.DeepL.Models
{
    using System;

    /// <summary>
    /// Represents an exception, which is thrown by the <see cref="DeepLTranslate"/> to signal any errors during the parsing process. Having a
    /// single exception type makes error handling much easier.
    /// </summary>
    public class DeepLException : Exception
    {
        /// <summary>
        /// Initializes a new <see cref="DeepLException"/> instance.
        /// </summary>
        public DeepLException() { }

        /// <summary>
        /// Initializes a new <see cref="DeepLException"/> instance.
        /// </summary>
        /// <param name="message">The error message, which describes what went wrong during the parsing.</param>
        public DeepLException(String message)
            : base(message) { }

        /// <summary>
        /// Initializes a new <see cref="DeepLException"/> instance.
        /// </summary>
        /// <param name="message">The error message, which describes what went wrong during the parsing.</param>
        /// <param name="innerException">The original exception, which caused this exception to be thrown.</param>
        public DeepLException(String message, Exception innerException):
            base (message, innerException) { }
    }
}