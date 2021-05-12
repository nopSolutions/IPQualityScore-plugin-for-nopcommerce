using Newtonsoft.Json;

namespace Nop.Plugin.Misc.IPQualityScore.Models
{
    /// <summary>
    /// Represents a IP reputation response model
    /// </summary>
    public class IPReputationResponse : ApiResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the overall fraud score of the user based on the IP, user agent, language, and any other optionally passed variables. Fraud Scores >= 75 are suspicious, but not necessarily fraudulent. We recommend flagging or blocking traffic with Fraud Scores >= 85, but you may find it beneficial to use a higher or lower threshold.
        /// </summary>
        [JsonProperty("fraud_score")]
        public double FraudScore { get; set; }

        /// <summary>
        /// Gets or sets the rate of abuse of an IP address in the IPQS threat network. Values can be "high", "medium", "low", or "none". Can be used in combination with the Fraud Score to identify bad behavior.
        /// </summary>
        [JsonProperty("abuse_velocity")]
        public string AbuseVelocity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to the IP address suspected to be a proxy
        /// </summary>
        [JsonProperty("proxy")]
        public bool IsProxy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to the IP address suspected to be a VPN
        /// </summary>
        [JsonProperty("vpn")]
        public bool IsVpn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to the IP address suspected to be a TOR
        /// </summary>
        [JsonProperty("tor")]
        public bool IsTor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if there has been any recently verified abuse across IPQS network
        /// </summary>
        [JsonProperty("recent_abuse")]
        public bool RecentAbuse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to this IP associated with being a confirmed crawler from a mainstream search engine such as Googlebot, Bingbot, Yandex, etc. based on hostname or IP address verification.
        /// </summary>
        [JsonProperty("isCrawler")]
        public bool IsCrawler { get; set; }

        /// <summary>
        /// Gets or sets the additional scoring variables for risk analysis are available when transaction scoring data is passed through the API request.
        /// </summary>
        [JsonProperty("transaction_details")]
        public TransactionDetails TransactionDetails { get; set; }

        #endregion
    }
}
