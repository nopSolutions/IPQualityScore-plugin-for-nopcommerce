using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Models
{
    /// <summary>
    /// Represents a model to configure plugin
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        #region IP reputation

        /// <summary>
        /// Gets or sets the key to sign the API requests
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.ApiKey")]
        public string ApiKey { get; set; }
        public bool ApiKey_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to the IP Reputation service is enabled
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.IPReputationEnabled")]
        public bool IPReputationEnabled { get; set; }
        public bool IPReputationEnabled_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the proxy
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.ProxyBlockingEnabled")]
        public bool ProxyBlockingEnabled { get; set; }
        public bool ProxyBlockingEnabled_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the VPN
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.VpnBlockingEnabled")]
        public bool VpnBlockingEnabled { get; set; }
        public bool VpnBlockingEnabled_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the TOR
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.TorBlockingEnabled")]
        public bool TorBlockingEnabled { get; set; }
        public bool TorBlockingEnabled_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the fraud score for blocking
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.IPReputationFraudScoreForBlocking")]
        public double IPReputationFraudScoreForBlocking { get; set; }
        public bool IPReputationFraudScoreForBlocking_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the strictness. How in depth (strict) do you want this query to be? Higher values take longer to process and may provide a higher false-positive rate. We recommend starting at "0", the lowest strictness setting, and increasing to "1" or "2" depending on your levels of fraud.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.IPReputationStrictness")]
        public int IPReputationStrictness { get; set; }
        public bool IPReputationStrictness_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to public access points are allowed. Bypasses certain checks for IP addresses from education and research institutions, schools, and some corporate connections to better accommodate audiences that frequently use public connections.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.AllowPublicAccessPoints")]
        public bool AllowPublicAccessPoints { get; set; }
        public bool AllowPublicAccessPoints_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use lighter penalties. Enable this setting to lower detection rates and Fraud Scores for mixed quality IP addresses. If you experience any false-positives with your traffic then enabling this feature will provide better results.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.LighterPenalties")]
        public bool LighterPenalties { get; set; }
        public bool LighterPenalties_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip validation for crawlers
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.AllowCrawlers")]
        public bool AllowCrawlers { get; set; }
        public bool AllowCrawlers_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a group IDs to check IP quality.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.IPQualityGroupIds")]
        public IList<int> IPQualityGroupIds { get; set; }
        public bool IPQualityGroupIds_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the available group system names to check IP quality.
        /// </summary>
        public IList<SelectListItem> AvailableIPQualityGroupIds { get; set; }

        /// <summary>
        /// Gets or sets a IP block notification type
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.IPBlockNotificationTypeId")]
        public int IPBlockNotificationTypeId { get; set; }
        public bool IPBlockNotificationTypeId_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the available IP block notification types
        /// </summary>
        public IList<SelectListItem> AvailableIPBlockNotificationTypes { get; set; }

        #region Order scoring

        /// <summary>
        /// Gets or sets a value indicating whether to the Order scoring service is enabled
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.OrderScoringEnabled")]
        public bool OrderScoringEnabled { get; set; }
        public bool OrderScoringEnabled_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the fraud score for blocking
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.RiskScoreForBlocking")]
        public double RiskScoreForBlocking { get; set; }
        public bool RiskScoreForBlocking_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the transaction strictness. Adjusts the weights for penalties applied due to irregularities and fraudulent patterns detected on order and transaction details that can be optionally provided on each API request. This feature is only beneficial if you are passing order and transaction details. A table is available further down the page with supported transaction variables.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.TransactionStrictness")]
        public int TransactionStrictness { get; set; }
        public bool TransactionStrictness_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the order status ID when cusomer has been approved by IPQualityScore
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.ApproveStatusId")]
        public int ApproveStatusId { get; set; }
        public bool ApproveStatusId_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the order status ID when cusomer has been approved by IPQualityScore
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.RejectStatusId")]
        public int RejectStatusId { get; set; }
        public bool RejectStatusId_OverrideForStore { get; set; }

        public List<SelectListItem> AvailableOrderStatuses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to inform the user about potential fraud
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.InformCustomerAboutFraud")]
        public bool InformCustomerAboutFraud { get; set; }
        public bool InformCustomerAboutFraud_OverrideForStore { get; set; }

        #endregion

        #endregion

        #region Email validation

        /// <summary>
        /// Gets or sets a value indicating whether to enable the email validation
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.EmailValidationEnabled")]
        public bool EmailValidationEnabled { get; set; }
        public bool EmailValidationEnabled_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the email reputation fraud score for blocking
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.EmailReputationFraudScoreForBlocking")]
        public double EmailReputationFraudScoreForBlocking { get; set; }
        public bool EmailReputationFraudScoreForBlocking_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the strictness level for machine learning pattern recognition of abusive email addresses with the "recent_abuse" data point. Default level of 0 provides good coverage, however if you are filtering account applications and facing advanced fraudsters then we recommend increasing this value to level 1 or 2.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.AbuseStrictness")]
        public int AbuseStrictness { get; set; }
        public bool AbuseStrictness_OverrideForStore { get; set; }

        #endregion

        #region Device fingerprint

        /// <summary>
        /// Gets or sets a value indicating whether to enable the device fingerprint
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.DeviceFingerprintEnabled")]
        public bool DeviceFingerprintEnabled { get; set; }
        public bool DeviceFingerprintEnabled_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the JavaScript snippet code
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.DeviceFingerprintTrackingCode")]
        public string DeviceFingerprintTrackingCode { get; set; }
        public bool DeviceFingerprintTrackingCode_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the value that this device is to commit fraud or engage in abusive behavior. 0 = not likely, 100 = very likely. 25 is the median result. Fraud Scores >= 75 are suspicious, but not necessarily fraudulent. We recommend flagging or blocking traffic with Fraud Scores >= 85, but you may find it beneficial to use a higher or lower threshold.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.DeviceFingerprintFraudChance")]
        public int DeviceFingerprintFraudChance { get; set; }
        public bool DeviceFingerprintFraudChance_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the user if script is blocked by that user
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.BlockUserIfScriptIsBlocked")]
        public bool BlockUserIfScriptIsBlocked { get; set; }
        public bool BlockUserIfScriptIsBlocked_OverrideForStore { get; set; }

        /// <summary>
        /// Gets or sets the user ID variable name to track fingerprint on server side
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.UserIdVariableName")]
        public string UserIdVariableName { get; set; }
        public bool UserIdVariableName_OverrideForStore { get; set; }

        #endregion

        #endregion

        #region Ctor

        public ConfigurationModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            AvailableIPQualityGroupIds = new List<SelectListItem>();
            AvailableIPBlockNotificationTypes = new List<SelectListItem>();
        }

        #endregion
    }
}
