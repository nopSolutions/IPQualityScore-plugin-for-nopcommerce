using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.IPQualityScore
{
    /// <summary>
    /// Represents settings of the IPQualityScore plugin
    /// </summary>
    public class IPQualityScoreSettings : ISettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to log the request errors
        /// </summary>
        public bool LogRequestErrors { get; set; }

        #region IP Reputation

        /// <summary>
        /// Gets or sets the key to sign the API requests
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to the IP Reputation service is enabled
        /// </summary>
        public bool IPReputationEnabled { get; set; }

        /// <summary>
        /// Gets or sets the IP reputation / Order scoring fraud score for blocking
        /// </summary>
        public double IPReputationFraudScoreForBlocking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the proxy
        /// </summary>
        public bool ProxyBlockingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the VPN
        /// </summary>
        public bool VpnBlockingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the TOR
        /// </summary>
        public bool TorBlockingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the strictness. How in depth (strict) do you want this query to be? Higher values take longer to process and may provide a higher false-positive rate. We recommend starting at "0", the lowest strictness setting, and increasing to "1" or "2" depending on your levels of fraud.
        /// </summary>
        public int IPReputationStrictness { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to public access points are allowed. Bypasses certain checks for IP addresses from education and research institutions, schools, and some corporate connections to better accommodate audiences that frequently use public connections.
        /// </summary>
        public bool AllowPublicAccessPoints { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use lighter penalties. Enable this setting to lower detection rates and Fraud Scores for mixed quality IP addresses. If you experience any false-positives with your traffic then enabling this feature will provide better results.
        /// </summary>
        public bool LighterPenalties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip validation for crawlers
        /// </summary>
        public bool AllowCrawlers { get; set; }

        #region Order scoring

        /// <summary>
        /// Gets or sets a value indicating whether to the Order scoring service is enabled
        /// </summary>
        public bool OrderScoringEnabled { get; set; }

        /// <summary>
        /// Gets or sets the fraud score for blocking
        /// </summary>
        public double RiskScoreForBlocking { get; set; }

        /// <summary>
        /// Gets or sets the transaction strictness. Adjusts the weights for penalties applied due to irregularities and fraudulent patterns detected on order and transaction details that can be optionally provided on each API request. This feature is only beneficial if you are passing order and transaction details. A table is available further down the page with supported transaction variables.
        /// </summary>
        public int TransactionStrictness { get; set; }

        /// <summary>
        /// Gets or sets the order status ID when cusomer has been approved by IPQualityScore
        /// </summary>
        public int ApproveStatusId { get; set; }

        /// <summary>
        /// Gets or sets the order status ID when cusomer has been approved by IPQualityScore
        /// </summary>
        public int RejectStatusId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to inform the user about potential fraud
        /// </summary>
        public bool InformCustomerAboutFraud { get; set; }

        #endregion

        #endregion

        #region Email validation

        /// <summary>
        /// Gets or sets a value indicating whether to enable the email validation
        /// </summary>
        public bool EmailValidationEnabled { get; set; }

        /// <summary>
        /// Gets or sets the email reputation fraud score for blocking
        /// </summary>
        public double EmailReputationFraudScoreForBlocking { get; set; }

        /// <summary>
        /// Gets or sets the strictness. How strictly spam traps and honeypots are detected by our system, depending on how comfortable you are with identifying emails suspected of being a spam trap. 0 is the lowest level which will only return spam traps with high confidence. Strictness levels above 0 will return increasingly more strict results, with level 2 providing the greatest detection rates.
        /// </summary>
        public int EmailReputationStrictness { get; set; }

        /// <summary>
        /// Gets or sets the strictness level for machine learning pattern recognition of abusive email addresses with the "recent_abuse" data point. Default level of 0 provides good coverage, however if you are filtering account applications and facing advanced fraudsters then we recommend increasing this value to level 1 or 2.
        /// </summary>
        public int AbuseStrictness { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of seconds to wait for a reply from a mail service provider. If your implementation requirements do not need an immediate response, we recommend bumping this value to 20. Any results which experience a connection timeout will return the "timed_out" variable as true. Default value is 7 seconds.
        /// </summary>
        public int EmailReputationTimeout { get; set; }

        #endregion

        #region Device fingerprint

        /// <summary>
        /// Gets or sets a value indicating whether to enable the device fingerprint
        /// </summary>
        public bool DeviceFingerprintEnabled { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript snippet code
        /// </summary>
        public string DeviceFingerprintTrackingCode { get; set; }

        /// <summary>
        /// Gets or sets the value that this device is to commit fraud or engage in abusive behavior. 0 = not likely, 100 = very likely. 25 is the median result. Fraud Scores >= 75 are suspicious, but not necessarily fraudulent. We recommend flagging or blocking traffic with Fraud Scores >= 85, but you may find it beneficial to use a higher or lower threshold.
        /// </summary>
        public int DeviceFingerprintFraudChance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the user if script is blocked by that user
        /// </summary>
        public bool BlockUserIfScriptIsBlocked { get; set; }

        #endregion

        #endregion
    }
}
