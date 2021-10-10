namespace EncasedLib.Libs.DeepL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json;
    using Models;

    public abstract class DeepLClient : IDisposable
    {
        #region Properties
        internal HttpClient _httpClient;

        public Boolean IsDisposed { get; private set; }
        #endregion
        #region Internal Methods
        internal async Task CheckResponseStatusCodeAsync(HttpResponseMessage responseMessage)
        {
            // Validates the arguments
            if (responseMessage == null)
                throw new ArgumentNullException(nameof(responseMessage));

            // When the status code represents success, then nothing is done
            if (responseMessage.IsSuccessStatusCode)
                return;

            // Checks which error occurred and throws an exception accordingly
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    try
                    {
                        var content = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                        ErrorResult errorResult = JsonConvert.DeserializeObject<ErrorResult>(content);
                        throw new DeepLException($"Bad request. Please check error message and your parameters. {errorResult.Message}");
                    }
                    catch (JsonReaderException) { }
                    throw new DeepLException("Bad request. Please check error message and your parameters.");
                case HttpStatusCode.Forbidden:
                    throw new DeepLException("Authorization failed. Please supply a valid authentication key.");
                case HttpStatusCode.NotFound:
                    throw new DeepLException("The requested resource could not be found.");
                case HttpStatusCode.RequestEntityTooLarge:
                    throw new DeepLException("The request size exceeds the limit.");
                case (HttpStatusCode)429:
                    throw new DeepLException("Too many requests. Please wait and resend your request.");
                case (HttpStatusCode)456:
                    throw new DeepLException("Quota exceeded. The character limit has been reached.");
                case HttpStatusCode.InternalServerError:
                    throw new DeepLException("An internal server error occurred.");
                case HttpStatusCode.ServiceUnavailable:
                    throw new DeepLException("Resource currently unavailable. Try again later.");
                default:
                    throw new DeepLException("An unknown error occurred.");
            }
        }
        #endregion
        #region IDisposable Implementation
        public void Dispose()
        {
            // Calls the dispose method, which can be overridden by sub-classes to dispose of further resources
            this.Dispose(true);

            // Suppresses the finalization of this object by the garbage collector, because the resources have already been disposed of
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposingManagedResources)
        {
            // Checks if the DeepL API client has already been disposed of
            if (this.IsDisposed)
                throw new ObjectDisposedException("The DeepL API client has already been disposed of.");

            this.IsDisposed = true;

            // Checks if unmanaged resources should be disposed of
            if (disposingManagedResources)
            {
                // Checks if the HTTP client has already been disposed of, if not then it is disposed of
                if (this._httpClient != null)
                    this._httpClient.Dispose();
            }
        }
        #endregion
    }
}