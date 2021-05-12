using Newtonsoft.Json;

namespace Nop.Plugin.Misc.IPQualityScore.Models
{
    /// <summary>
    /// Represents a API response.
    /// </summary>
    public class ApiResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to request is successful
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the generic status message, either success or some form of an error notice.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for this request that can be used to lookup the request details or send a postback conversion notice.
        /// </summary>
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        #endregion
    }
}
