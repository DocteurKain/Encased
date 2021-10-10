namespace EncasedLib.Libs.DeepL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Models;
    using System.Net.Http.Headers;
    using System.Web;

    public class DeepLGlossary : DeepLClient
    {
        #region Properties
        private String _sourceLanguageCode = "en";
        private String _targetLanguageCode = "fr";
        #endregion
        #region Constructor
        public DeepLGlossary(String authKey)
        {
            base._httpClient = new HttpClient();
            base._httpClient.DefaultRequestHeaders.Add("User-Agent", $"DeepL.NET/{Assembly.GetExecutingAssembly().GetName().Version}");
            base._httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            base._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DeepL-Auth-Key", authKey);
        }
        #endregion
        #region Public Methods
        public String GetGlossaryIdByName(String glossaryName)
        {
            var task = GetGlossaries();

            task.Wait();

            var glossaries = task.Result;

            var glossary = glossaries.GlossaryList.FirstOrDefault(a => a.Name == glossaryName);

            if (glossary == null)
                return String.Empty;

            return glossary.GlossaryId;
        }

        public async Task<Glossaries> GetGlossaries(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var responseMessage = await this._httpClient.GetAsync(
                this.BuildUrl(Constants.GlossariesPath),
                cancellationToken
            ).ConfigureAwait(false);

            await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

            var translationResultContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Glossaries>(translationResultContent);
        }

        public async Task<Glossary> GetGlossary(
            String glossaryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var responseMessage = await this._httpClient.GetAsync(
                this.BuildUrl($"{Constants.GlossariesPath}/{glossaryId}"),
                cancellationToken
            ).ConfigureAwait(false);

            await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

            var translationResultContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<Glossary>(translationResultContent);
        }

        public async Task<Dictionary<String, String>> GetGlossaryEntries(
            String glossaryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var responseMessage = await this._httpClient.GetAsync(
                this.BuildUrl($"{Constants.GlossariesPath}/{glossaryId}/entries"),
                cancellationToken
            ).ConfigureAwait(false);

            await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

            var translationResultContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return translationResultContent
                .Split("\n")
                .ToDictionary(a => a.Split('\t')[0], a => a.Split('\t')[1]);
        }

        public async Task<Boolean> SetGlossary(
            String glossaryName, 
            Dictionary<String, String> values,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new List<KeyValuePair<String, String>>();

            parameters.Add(new KeyValuePair<String, String>("name", glossaryName));
            parameters.Add(new KeyValuePair<String, String>("source_lang", _sourceLanguageCode));
            parameters.Add(new KeyValuePair<String, String>("target_lang", _targetLanguageCode));
            parameters.Add(new KeyValuePair<String, String>("entries_format", "tsv"));

            var sb = new StringBuilder();

            foreach (var v in values)
                sb.AppendLine($"{v.Key}	{v.Value}");

            parameters.Add(new KeyValuePair<String, String>("entries", sb.ToString()));

            using (var httpContent = new FormUrlEncodedContent(parameters))
            {
                var responseMessage = await this._httpClient.PostAsync(
                    this.BuildUrl(Constants.GlossariesPath),
                    httpContent,
                    cancellationToken
                ).ConfigureAwait(false);

                await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

                var a = responseMessage;
            }

            return true;
        }

        public async Task<Boolean> DeleteGlossary(
            String glossaryId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var responseMessage = await this._httpClient.DeleteAsync(
                this.BuildUrl($"{Constants.GlossariesPath}/{glossaryId}"),
                cancellationToken
            ).ConfigureAwait(false);

            await this.CheckResponseStatusCodeAsync(responseMessage).ConfigureAwait(false);

            return true;
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
                return url;

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