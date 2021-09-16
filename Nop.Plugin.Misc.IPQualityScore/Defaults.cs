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
        /// Gets the plugin prevent fraud route name
        /// </summary>
        public static string OrderFraudInformationAttributeName => "Plugin.Misc.IPQualityScore.OrderFraudInformationAttributeName";

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
        /// Gets a name of the view component to display the IPQualityScore IP result information in public store
        /// </summary>
        public const string IP_RESULT_INFORMATION_VIEW_COMPONENT_NAME = "IPQualityScoreResultInformation";

        /// <summary>
        /// Gets a name of the view component to display the IPQualityScore order fraud information in admin store
        /// </summary>
        public const string ORDER_FRAUD_INFORMATION_VIEW_COMPONENT_NAME = "IPQualityScoreOrderFraudInformation";

        /// <summary>
        /// Gets the route names to display the IPQualityScore device fingerprint in public store
        /// </summary>
        public static string[] DeviceFingerprintRouteNames => new[]
        {
            "CustomerInfo",
            "CustomerChangePassword",
            "Register",
            "Login",
            "CheckoutConfirm",
            "CheckoutOnePage"
        };

        /// <summary>
        /// Gets the IP quality result id
        /// </summary>
        public static string IPQualityResultId => "IPQualityScore.IPQualityResult";

        /// <summary>
        /// Represents a default groups to check IP quality
        /// </summary>
        public static class IPQualityGroups
        {
            /// <summary>
            /// Gets the all available groups
            /// </summary>
            public static (int Id, string[] RouteNames)[] All => new[]
            {
                Customer, Catalog, Checkout
            };

            /// <summary>
            /// Gets the customer group
            /// </summary>
            public static (int Id, string[] RouteNames) Customer =>
            (
                1,
                new[]
                {
                    "Login",
                    "Register",
                    "Logout",
                    "CustomerInfo",
                    "CustomerAddresses",
                    "CustomerOrders",
                    "CustomerProductReviews",
                    "PasswordRecovery",
                    "PasswordRecoveryConfirm",
                    "CustomerReturnRequests",
                    "CustomerDownloadableProducts",
                    "CustomerBackInStockSubscriptions",
                    "CustomerRewardPoints",
                    "CustomerChangePassword",
                    "AccountActivation",
                    "EmailRevalidation",
                    "CustomerProfile",
                    "OrderDetails",
                    "ShipmentDetails",
                    "ReturnRequest",
                    "ReOrder",
                }
            );

            /// <summary>
            /// Gets the catalog group
            /// </summary>
            public static (int Id, string[] RouteNames) Catalog =>
            (
                2,
                new[]
                {
                    "ProductDetails",
                    "Category",
                    "Manufacturer",
                    "Vendor",
                    "NewsItem",
                    "BlogPost",
                    "ProductsByTag",
                    "EstimateShipping",
                    "ContactUs",
                    "Sitemap",
                    "Blog",
                    "ProductSearch",
                    "NewProducts",
                    "NewsArchive",
                    "Boards",
                    "CompareProducts",
                    "ProductTagsAll",
                    "ManufacturerList",
                    "VendorList",
                    "ProductReviews",
                    "BlogByTag",
                    "BlogByMonth",
                    "ContactVendor",
                    "ApplyVendorAccount",
                    "NewsletterActivation",
                }
            );

            /// <summary>
            /// Gets the checkout group
            /// </summary>
            public static (int Id, string[] RouteNames) Checkout =>
            (
                3,
                new[]
                {
                    "ShoppingCart",
                    "Wishlist",
                    "Checkout",
                    "CheckoutOnePage",
                    "CheckoutShippingAddress",
                    "CheckoutBillingAddress",
                    "CheckoutShippingMethod",
                    "CheckoutPaymentMethod",
                    "CheckoutPaymentInfo",
                    "CheckoutConfirm",
                    "CheckoutCompleted",
                }
            );
        }

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

                    /// <summary>
                    /// Gets the lookup request endpoint path
                    /// </summary>
                    public static string LookupRequestPath => "/api/json/postback";
                }
            }
        }
    }
}
