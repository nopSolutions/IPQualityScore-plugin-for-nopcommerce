using Newtonsoft.Json;

namespace Nop.Plugin.Misc.IPQualityScore.Models
{
    /// <summary>
    /// Represents a email reputation response model
    /// </summary>
    public class EmailReputationResponse : ApiResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to the email is valid
        /// </summary>
        [JsonProperty("valid")]
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to the email is suspected of belonging to a temporary or disposable mail service. Usually associated with fraudsters and scammers.
        /// </summary>
        [JsonProperty("disposable")]
        public bool Disposable { get; set; }

        /// <summary>
        /// Gets or sets the overall Fraud Score of the user based on the email's reputation and recent behavior across the IPQS threat network. Fraud Scores >= 75 are suspicious, but not necessarily fraudulent.
        /// </summary>
        [JsonProperty("fraud_score")]
        public double FraudScore { get; set; }

        /// <summary>
        /// Gets or sets a value will indicate if there has been any recently verified abuse across our network for this email address. Abuse could be a confirmed chargeback, fake signup, compromised device, fake app install, or similar malicious behavior within the past few days.
        /// </summary>
        [JsonProperty("recent_abuse")]
        public bool RecentAbuse { get; set; }

        #endregion
    }
}
