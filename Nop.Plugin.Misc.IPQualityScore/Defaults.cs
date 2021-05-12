using Nop.Core;

namespace Nop.Plugin.Misc.IPQualityScore
{
    /// <summary>
    /// Represents an plugin defaults.
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// Gets the plugin system name.
        /// </summary>
        public static string SystemName => "Misc.IPQualityScore";

        /// <summary>
        /// Gets the plugin configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Misc.IPQualityScore.Configure";

        /// <summary>
        /// Gets the plugin prevent fraud route name
        /// </summary>
        public static string PreventFraudRouteName => "Plugin.Misc.IPQualityScore.PreventFraud";

        /// <summary>
        /// Gets the route names to validate customer email
        /// </summary>
        public static string[] EmailValidationRouteNames => new[]
        {
            "CustomerInfo",
            "Register"
        };

        /// <summary>
        /// Gets a name of the view component to display the IPQualityScore device fingerprint in public store
        /// </summary>
        public const string DEVICE_FINGERPRINT_VIEW_COMPONENT_NAME = "IPQualityScoreDeviceFingerprint";

        /// <summary>
        /// Gets the route names to display the IPQualityScore device fingerprint in public store
        /// </summary>
        public static string[] DeviceFingerprintRouteNames => new[]
        {
            "CustomerInfo",
            "CustomerChangePassword",
            "Register",
            "Login",
            "Checkout",
            "CheckoutOnePage"
        };

        /// <summary>
        /// Represents a OpenPay defaults
        /// </summary>
        public static class IPQualityScore
        {
            /// <summary>
            /// Represents a API defaults
            /// </summary>
            public static class Api
            {
                /// <summary>
                /// Gets the user agent
                /// </summary>
                public static string BaseUrl => "https://ipqualityscore.com";

                /// <summary>
                /// Gets the user agent
                /// </summary>
                public static string UserAgent => $"nopCommerce-{NopVersion.CurrentVersion}";

                /// <summary>
                /// Gets the default timeout
                /// </summary>
                public static int DefaultTimeout => 20;

                /// <summary>
                /// Represents endpoints defaults
                /// </summary>
                public static class Endpoints
                {
                    /// <summary>
                    /// Gets the IP Reputation endpoint path
                    /// </summary>
                    public static string IPReputationPath => "/api/json/ip";

                    /// <summary>
                    /// Gets the email reputation endpoint path
                    /// </summary>
                    public static string EmailReputationPath => "/api/json/email";
                }
            }
        }
    }
}
