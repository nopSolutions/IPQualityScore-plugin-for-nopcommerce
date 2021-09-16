using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Models
{
    /// <summary>
    /// Represents a order fraud information model
    /// </summary>
    public class OrderFraudInformationModel : BaseNopModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets a a value for the likelihood that this device is to commit fraud or engage in abusive behavior. Fraud Scores >= 75 are suspicious, but not necessarily fraudulent. We recommend flagging or blocking traffic with Fraud Scores >= 85, but you may find it beneficial to use a higher or lower threshold.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.DeviceFingerprintRiskScore")]
        public double? DeviceFingerprintRiskScore { get; set; }

        /// <summary>
        /// Gets or sets the confidence that this user or transaction is exhibiting malicious behavior.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.PaymentRiskScore")]
        public double PaymentRiskScore { get; set; }

        /// <summary>
        /// Gets or sets the abusive check and reputation analysis for the email address.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.ValidBillingEmail")]
        public bool? ValidBillingEmail { get; set; }

        /// <summary>
        /// Gets or sets the valid & active phone number with the phone carrier (not disconnected).
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.ValidBillingPhone")]
        public bool? ValidBillingPhone { get; set; }

        /// <summary>
        /// Gets or sets the physical address validation and reputation analysis.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.ValidBillingAddress")]
        public bool? ValidBillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the abusive check and reputation analysis for the email address.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.ValidShippingEmail")]
        public bool? ValidShippingEmail { get; set; }

        /// <summary>
        /// Gets or sets the valid & active phone number with the phone carrier (not disconnected).
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.ValidShippingPhone")]
        public bool? ValidShippingPhone { get; set; }

        /// <summary>
        /// Gets or sets the physical address validation and reputation analysis.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.OrderFraudInformation.ValidShippingAddress")]
        public bool? ValidShippingAddress { get; set; }

        #endregion
    }
}