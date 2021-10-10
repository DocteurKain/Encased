namespace EncasedLib.Libs.DeepL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json;
    using Enums;
    using Models;

    public class DeepLTranslate : DeepLClient
    {
        #region Properties
        private String _authenticationKey = String.Empty;
        #endregion
        #region Constructor
        public DeepLTranslate(String authKey)
        {
            this._authenticationKey = authKey;

            base._httpClient = new HttpClient();
            base._httpClient.DefaultRequestHeaders.Add("User-Agent", $"DeepL.NET/{Assembly.GetExecutingAssembly().GetName().Version}");
            base._httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
        }
        #endregion
        #region Public Methods
        public async Task<IEnumerable<Translation>> TranslateAsync(
            IEnumerable<String> texts,
            String sourceLanguageCode,
            String targetLanguageCode,
            Boolean moreFormality,
            String glossaryId = "",
            Splitting splitting = Splitting.InterpunctionAndNewLines,
            Boolean preserveFormatting = false,
            XmlHandling xmlHandling = default(XmlHandling),
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Validates the parameters
            if (texts == null)
                throw new ArgumentNullException(nameof(texts));
            if (!texts.Any())
                throw new ArgumentException("No texts were provided for translation.", nameof(texts));
            if (texts.Any(text => text == null))
                throw new ArgumentException("One or more texts are null.");
            if (targetLanguageCode == null)
                throw new ArgumentNullException(nameof(targetLanguageCode));
            if (String.IsNullOrWhiteSpace(targetLanguageCode))
                throw new ArgumentException("The target language code must not be empty or only consist of white spaces.");

            // Prepares the parameters for the HTTP POST request
            var parameters = new List<KeyValuePair<String, String>>();

            foreach (var text in texts)
                parameters.Add(new KeyValuePair<String, String>("text", text));

            if (!String.IsNullOrWhiteSpace(sourceLanguageCode))
                parameters.Add(new KeyValuePair<String, String>("source_lang", sourceLanguageCode));

            parameters.Add(new KeyValuePair<String, String>("target_lang", targetLanguageCode));

            if (moreFormality)
                parameters.Add(new KeyValuePair<String, String>("formality", "more"));
            else
                parameters.Add(new KeyValuePair<String, String>("formality", "less"));

            if (glossaryId.Length > 0)
                parameters.Add(new KeyValuePair<String, String>("glossary_id", glossaryId));

            switch (splitting)
            {
                case Splitting.None:
                    parameters.Add(new KeyValuePair<String, String>("split_sentences", "0"));
                    break;
                case Splitting.InterpunctionAndNewLines:
                    parameters.Add(new KeyValuePair<String, String>("split_sentences", "1"));
                    break;
                case Splitting.Interpunction:
                    parameters.Add(new KeyValuePair<String, String>("split_sentences", "nonewlines"));
                    break;
            }

            parameters.Add(new KeyValuePair<String, String>("preserve_formatting", preserveFormatting ? "1" : "0"));

            if (xmlHandling != null)
            {
                parameters.Add(new KeyValuePair<String, String>("tag_handling", "xml"));
                if (xmlHandling.NonSplittingTags != null && xmlHandling.NonSplittingTags.Any())
                    parameters.Add(new KeyValuePair<String, String>("non_splitting_tags", String.Join(",", xmlHandling.NonSplittingTags)));
                if (xmlHandling.SplittingTags != null && xmlHandling.SplittingTags.Any())
                    parameters.Add(new KeyValuePair<String, String>("splitting_tags", String.Join(",", xmlHandling.SplittingTags)));
                if (xmlHandling.IgnoreTags != null && xmlHandling.IgnoreTags.Any())
                    parameters.Add(new KeyValuePair<String, String>("ignore_tags", String.Join(",", xmlHandling.IgnoreTags)));
                parameters.Add(new KeyValuePair<String, String>("outline_detection", xmlHandling.OutlineDetection ? "1" : "0"));
            }

            // Sends a request to the DeepL API to translate the text
            using (var httpContent = new FormUrlEncodedContent(parameters))
            {
                var responseMessage = await this._httpClient.PostAsync(
                    this.BuildUrl(Constants.TranslatePath),
                    httpContent,
                    cancellationToken
                ).ConfigureAwait(false);

                await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

                // Retrieves the returned JSON and parses it into a .NET object
                var translationResultContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<TranslationResult>(translationResultContent).Translations;
            }
        }

        public async Task<Translation> TranslateAsync(
            String text,
            String sourceLanguageCode,
            String targetLanguageCode,
            Boolean moreFormality,
            String glossaryId = "",
            Splitting splitting = Splitting.InterpunctionAndNewLines,
            Boolean preserveFormatting = false,
            XmlHandling xmlHandling = default(XmlHandling),
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Validates the arguments
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("The text must not be empty or only consist of white spaces.");

            // Translates the text
            IEnumerable<Translation> translations = await this.TranslateAsync(
                new List<String> { text },
                sourceLanguageCode,
                targetLanguageCode,
                moreFormality,
                glossaryId,
                splitting,
                preserveFormatting,
                xmlHandling,
                cancellationToken
            ).ConfigureAwait(false);

            // Since only one text was translated, the first translation is returned
            return translations.FirstOrDefault();
        }

        public async Task<UsageStatistics> GetUsageStatisticsAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Sends a request to the DeepL API to retrieve the usage statistics
            var responseMessage = await this._httpClient.GetAsync(
                this.BuildUrl(Constants.UsageStatisticsPath),
                cancellationToken
            ).ConfigureAwait(false);

            await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

            // Retrieves the returned JSON and parses it into a .NET object
            var usageStatisticsContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<UsageStatistics>(usageStatisticsContent);
        }

        public async Task<IEnumerable<SupportedLanguage>> GetSupportedLanguagesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Sends a request to the DeepL API to retrieve the supported languages
            var responseMessage = await this._httpClient.GetAsync(
                this.BuildUrl(Constants.SupportedLanguagesPath),
                cancellationToken
            ).ConfigureAwait(false);

            await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

            // Retrieves the returned JSON and parses it into a .NET object
            var supportedLanguagesContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<IEnumerable<SupportedLanguage>>(supportedLanguagesContent);
        }
        #endregion
        #region Private Methods
        private String BuildUrl(String path, IEnumerable<String> pathParameters, IDictionary<String, String> queryParameters = null)
        {
            // Validates the parameters
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path must not be empty.");

            // Concatenates the path to the base URL
            var url = $"{Constants.FreeApiBaseUrl}/{path}";

            // Adds the path parameters
            if (pathParameters != null && pathParameters.Any())
                url = String.Concat(url, "/", String.Join("/", pathParameters));

            // Adds the authentication key to the query parameters
            if (queryParameters == null)
                queryParameters = new Dictionary<String, String>();

            queryParameters.Add("auth_key", _authenticationKey);

            // Converts the query parameters to a string and appends them to the URL
            String queryString = String.Join("&", queryParameters.Select(keyValuePair => $"{keyValuePair.Key}={HttpUtility.HtmlEncode(keyValuePair.Value)}"));

            url = String.Concat(url, "?", queryString);

            // Returns the built URL
            return url;
        }

        private String BuildUrl(String path, IDictionary<String, String> queryParameters = null)
            => this.BuildUrl(path, null, queryParameters);
        #endregion
    }
}