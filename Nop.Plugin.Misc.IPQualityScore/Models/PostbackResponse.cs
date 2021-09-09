using Newtonsoft.Json;

namespace Nop.Plugin.Misc.IPQualityScore.Models
{
    /// <summary>
    /// Represents a postback response model
    /// </summary>
    public class PostbackResponse : ApiResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets a a value for the likelihood that this device is to commit fraud or engage in abusive behavior. Fraud Scores >= 75 are suspicious, but not necessarily fraudulent. We recommend flagging or blocking traffic with Fraud Scores >= 85, but you may find it beneficial to use a higher or lower threshold.
        /// </summary>
        [JsonProperty("fraud_chance")]
        public double FraudChance { get; set; }

        #endregion
    }
}
