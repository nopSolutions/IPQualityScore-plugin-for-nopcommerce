namespace Nop.Plugin.Misc.IPQualityScore.Domain
{
    /// <summary>
    /// Represents an IP block notification type
    /// </summary>
    public enum IPBlockNotificationType
    {
        /// <summary>
        /// Display the notification to customer
        /// </summary>
        DisplayNotification,

        /// <summary>
        /// Redirect customer to block page
        /// </summary>
        RedirectToBlockPage
    }
}
