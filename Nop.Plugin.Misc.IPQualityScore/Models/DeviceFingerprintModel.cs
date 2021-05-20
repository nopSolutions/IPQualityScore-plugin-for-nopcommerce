using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.IPQualityScore.Models
{
    /// <summary>
    /// Represents a device fingerprint model
    /// </summary>
    public class DeviceFingerprintModel : BaseNopModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the JavaScript snippet code
        /// </summary>
        public string TrackingCode { get; set; }

        /// <summary>
        /// Gets or sets the value that this device is to commit fraud or engage in abusive behavior. 0 = not likely, 100 = very likely. 25 is the median result. Fraud Scores >= 75 are suspicious, but not necessarily fraudulent. We recommend flagging or blocking traffic with Fraud Scores >= 85, but you may find it beneficial to use a higher or lower threshold.
        /// </summary>
        public int FraudChance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to block the user if script is blocked by that user
        /// </summary>
        public bool BlockUserIfScriptIsBlocked { get; set; }

        #endregion
    }
}
