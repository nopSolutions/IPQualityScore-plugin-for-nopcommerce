using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Plugin.Misc.IPQualityScore.Models;

namespace Nop.Plugin.Misc.IPQualityScore.Services
{
    /// <summary>
    /// Provides an abstraction for the HTTP client to interact with the endpoint(s).
    /// </summary>
    public abstract class BaseHttpClient
    {
        #region Properties

        /// <summary>
        /// Gets or sets the HTTP client
        /// </summary>
        protected HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets the API key
        /// </summary>
        public string ApiKey { get; set; }

        #endregion

        #region Ctor

        public BaseHttpClient(IPQualityScoreSettings settings, HttpClient httpClient)
        {
            ApiKey = settings.ApiKey;

            httpClient.BaseAddress = new Uri(Defaults.IPQualityScore.Api.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(Defaults.IPQualityScore.Api.DefaultTimeout);
            httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, Defaults.IPQualityScore.Api.UserAgent);
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
            HttpClient = httpClient;
        }

        #endregion

        #region Methods

        protected async Task<TResponse> GetAsync<TResponse>(string requestUri, IDictionary<string, string> queryString = null, [CallerMemberName] string callerName = "")
        {
            if (queryString != null)
                requestUri = QueryHelpers.AddQueryString(requestUri, queryString);

            return await CallAsync<TResponse>(() => HttpClient.GetAsync(requestUri), callerName);
        }

        protected async Task<TResponse> PostAsync<TResponse>(string requestUri, object request = null, [CallerMemberName] string callerName = "")
        {
            HttpContent body = null;
            if (request != null)
            {
                var content = JsonConvert.SerializeObject(request);
                body = new StringContent(content, Encoding.UTF8, MimeTypes.ApplicationJson);
            }

            return await CallAsync<TResponse>(() => HttpClient.PostAsync(requestUri, body), callerName);
        }

        protected async Task<TResponse> CallAsync<TResponse>(Func<Task<HttpResponseMessage>> requestFunc, [CallerMemberName] string callerName = "")
        {
            HttpResponseMessage response = null;
            try
            {
                response = await requestFunc();
            }
            catch (Exception exception)
            {
                throw new ApiException(500, $"Error when calling '{callerName}'. HTTP status code - 500. {exception.Message}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var statusCode = (int)response.StatusCode;
            if (statusCode >= 400)
            {
                // throw exception with deserialized error
                var errorResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);
                var message = $"Error when calling '{callerName}'. HTTP status code - {statusCode}. ";
                if (errorResponse != null)
                {
                    message += @$"
                            Success - '{errorResponse.Success}'.
                            Message - '{errorResponse.Message}'.
                            Request id - '{errorResponse.RequestId}'.";
                }

                throw new ApiException(statusCode, message, errorResponse);
            }

            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        #endregion
    }
}
