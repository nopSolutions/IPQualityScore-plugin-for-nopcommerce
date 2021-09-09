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
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.FraudChance")]
        public double? FraudChance { get; set; }

        /// <summary>
        /// Gets or sets the confidence that this user or transaction is exhibiting malicious behavior.
        /// </summary>
        [NopResourceDisplayName("Plugins.Misc.IPQualityScore.Fields.RiskScore")]
        public double RiskScore { get; set; }

        #endregion
    }
}
