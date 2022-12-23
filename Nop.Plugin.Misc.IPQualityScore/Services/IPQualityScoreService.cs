using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Models;
using Nop.Plugin.Misc.IPQualityScore.Models;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.IPQualityScore.Services
{
    public class IPQualityScoreService
    {
        #region Fields

        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IPQualityScoreApi _iPQualityScoreApi;
        private readonly IPQualityScoreSettings _iPQualityScoreSettings;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly IWidgetPluginManager _widgetPluginManager;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public IPQualityScoreService(IAddressService addressService,
            ICountryService countryService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            ILogger logger,
            IOrderService orderService,
            IOrderProcessingService orderProcessingService,
            IPQualityScoreApi iPQualityScoreApi,
            IPQualityScoreSettings iPQualityScoreSettings,
            IStateProvinceService stateProvinceService,
            IUserAgentHelper userAgentHelper,
            IWidgetPluginManager widgetPluginManager,
            IWorkContext workContext,
            IWebHelper webHelper)
        {
            _addressService = addressService;
            _countryService = countryService;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _languageService = languageService;
            _localizationService = localizationService;
            _logger = logger;
            _orderService = orderService;
            _orderProcessingService = orderProcessingService;
            _iPQualityScoreApi = iPQualityScoreApi;
            _iPQualityScoreSettings = iPQualityScoreSettings;
            _stateProvinceService = stateProvinceService;
            _userAgentHelper = userAgentHelper;
            _widgetPluginManager = widgetPluginManager;
            _workContext = workContext;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the value indicating whether to the request can be validated using IPQS.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the check result
        /// </returns>
        public async Task<bool> CanValidateIPReputationAsync(ActionContext actionContext)
        {
            if (actionContext is null)
                throw new ArgumentNullException(nameof(actionContext));

            if (!await CanValidateRequestIPAsync(actionContext.HttpContext))
                return false;

            if (!_iPQualityScoreSettings.IPReputationEnabled)
                return false;

            if (_iPQualityScoreSettings.IPQualityGroupIds?.Any() == true)
            {
                var routeName = GetCurrentRouteName(actionContext.HttpContext);
                var availableRouteNames = Defaults.IPQualityGroups.All
                    .Where(g => _iPQualityScoreSettings.IPQualityGroupIds.Contains(g.Id))
                    .SelectMany(g => g.RouteNames);

                return availableRouteNames.Contains(routeName);
            }

            return true;
        }

        /// <summary>
        /// Returns the value indicating whether to the request is valid.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>The <see cref="Task"/> containing the value indicating whether to the request is valid.</returns>
        public async Task<bool> ValidateRequestAsync(ActionContext actionContext)
        {
            if (actionContext is null)
                throw new ArgumentNullException(nameof(actionContext));

            var language = await _workContext.GetWorkingLanguageAsync();
            var query = GetIpReputationQuery(actionContext.HttpContext, language.LanguageCulture);
            var response = await GetIPReputationAsync(query);

            return IsValidResponse(response);
        }

        /// <summary>
        /// Returns the value indicating whether to the order can be validated using IPQS.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="actionContext">The action context.</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the check result
        /// </returns>
        public async Task<bool> CanValidateOrderAsync(Order order, ActionContext actionContext)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (actionContext is null)
                throw new ArgumentNullException(nameof(actionContext));

            if (!await CanValidateRequestIPAsync(actionContext.HttpContext))
                return false;

            if (!_iPQualityScoreSettings.OrderScoringEnabled)
                return false;

            return true;
        }

        /// <summary>
        /// Returns the value indicating whether to the order is valid.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="actionContext">The action context.</param>
        /// <returns>The <see cref="Task"/> containing the value indicating whether to the order is valid.</returns>
        public async Task<bool> ValidateOrderAsync(Order order, ActionContext actionContext)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            if (actionContext is null)
                throw new ArgumentNullException(nameof(actionContext));

            var query = await GetIpReputationTransactionQueryAsync(order, actionContext.HttpContext);
            var response = await GetIPReputationAsync(query);

            var orderFraudInformationModel = new OrderFraudInformationModel
            {
                PaymentRiskScore = response?.TransactionDetails?.RiskScore ?? 0,
                ValidBillingAddress = response?.TransactionDetails?.ValidBillingAddress,
                ValidBillingPhone = response?.TransactionDetails?.ValidBillingPhone,
                ValidBillingEmail = response?.TransactionDetails?.ValidBillingEmail,
                ValidShippingAddress = response?.TransactionDetails?.ValidShippingAddress,
                ValidShippingPhone = response?.TransactionDetails?.ValidShippingPhone,
                ValidShippingEmail = response?.TransactionDetails?.ValidShippingEmail
            };

            if (!string.IsNullOrEmpty(_iPQualityScoreSettings.UserIdVariableName))
            {
                var deviceFingerprintResponse = await _iPQualityScoreApi.LookupRequestAsync(new Dictionary<string, string>()
                {
                    [_iPQualityScoreSettings.UserIdVariableName] = (await _workContext.GetCurrentCustomerAsync()).Id.ToString(),
                    ["type"] = "devicetracker",
                });
                if (deviceFingerprintResponse?.Success == true)
                    orderFraudInformationModel.DeviceFingerprintRiskScore = deviceFingerprintResponse.FraudChance;
            }

            var payload = JsonConvert.SerializeObject(orderFraudInformationModel);
            await _genericAttributeService.SaveAttributeAsync(order, Defaults.OrderFraudInformationAttributeName, payload);

            return IsValidTransactionalResponse(response);
        }

        /// <summary>
        /// Approves the order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>
        /// A task that represents the asynchronous operation</returns>
        public async Task ApproveOrderAsync(Order order)
        {
            await SetOrderStatusAsync(order, _iPQualityScoreSettings.ApproveStatusId);
        }

        /// <summary>
        /// Rejectes the order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>
        /// A task that represents the asynchronous operation</returns>
        public async Task RejectOrderAsync(Order order)
        {
            await SetOrderStatusAsync(order, _iPQualityScoreSettings.RejectStatusId);

            await _orderService.InsertOrderNoteAsync(new OrderNote
            {
                OrderId = order.Id,
                DisplayToCustomer = _iPQualityScoreSettings.InformCustomerAboutFraud,
                CreatedOnUtc = DateTime.UtcNow,
                Note = string.Format(
                    await _localizationService.GetResourceAsync("Plugins.Misc.IPQualityScore.Order.MessageToCustomerWhenFraudIsDetected"),
                    await _localizationService.GetLocalizedEnumAsync(order.OrderStatus))
            });
        }

        /// <summary>
        /// Returns the value indicating whether to the email can be validated using IPQS.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the check result
        /// </returns>
        public async Task<bool> CanValidateEmailForRequestAsync(ActionContext actionContext)
        {
            if (actionContext is null)
                throw new ArgumentNullException(nameof(actionContext));

            if (actionContext.HttpContext.Request.Method != HttpMethods.Post)
                return false;

            if (!actionContext.ModelState.IsValid)
            {
                // don't validate email for invalid POST requests
                return false;
            }

            if (!await CanValidateRequestAsync(actionContext.HttpContext))
                return false;

            var routeName = GetCurrentRouteName(actionContext.HttpContext);
            if (!Defaults.EmailValidationRouteNames.Contains(routeName))
                return false;

            if (!actionContext.HttpContext.Request.Form.TryGetValue("Email", out var email))
                return false;

            if (!_iPQualityScoreSettings.EmailValidationEnabled)
                return false;

            var customer = await _workContext.GetCurrentCustomerAsync();
            if (customer.Email == email)
                return false;

            return await IsConfiguredAsync();
        }

        /// <summary>
        /// Returns the value indicating whether to the email is valid.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>The <see cref="Task"/> containing the value indicating whether to the email is valid.</returns>
        public async Task<bool> ValidateEmailForRequestAsync(ActionContext actionContext)
        {
            if (actionContext is null)
                throw new ArgumentNullException(nameof(actionContext));

            if (!actionContext.HttpContext.Request.Form.TryGetValue("Email", out var email))
                return false;

            var query = new Dictionary<string, string>()
            {
                ["timeout"] = _iPQualityScoreSettings.EmailReputationTimeout.ToString(),
                ["abuse_strictness"] = _iPQualityScoreSettings.AbuseStrictness.ToString(),
            };

            EmailReputationResponse response = null;
            try
            {
                response = await _iPQualityScoreApi.GetEmailReputationAsync(email, query);
            }
            catch (ApiException ex)
            {
                if (_iPQualityScoreSettings.LogRequestErrors)
                {
                    var customer = await _workContext.GetCurrentCustomerAsync();
                    var error = $"{Defaults.SystemName}: Error when calling the Email Reputation endpoint for email '{email}'.";
                    await _logger.ErrorAsync(error, ex, customer);
                }
            }

            if (response is null)
            {
                // don't validate request if the HTTP error occurs
                return true;
            }

            var isFraud = response.FraudScore >= Convert.ToDouble(_iPQualityScoreSettings.EmailReputationFraudScoreForBlocking);

            return response.Success && response.Valid && !response.Disposable && !isFraud;
        }

        /// <summary>
        /// Returns the value indicating whether to the IPQualityScore device fingerprint can be displayed in public store.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the value indicating whether to the IPQualityScore device fingerprint can be displayed in public store
        /// </returns>
        public async Task<bool> CanDisplayDeviceFingerprintAsync(HttpContext httpContext)
        {
            if (httpContext is null)
                throw new ArgumentNullException(nameof(httpContext));

            if (!await CanValidateRequestAsync(httpContext))
                return false;

            if (!_iPQualityScoreSettings.DeviceFingerprintEnabled)
                return false;

            var routeName = GetCurrentRouteName(httpContext);
            if (!Defaults.DeviceFingerprintRouteNames.Contains(routeName))
                return false;

            return true;
        }

        #endregion

        #region Utilities

        private async Task<IPReputationResponse> GetIPReputationAsync(IDictionary<string, string> query)
        {
            var ipAddress = _webHelper.GetCurrentIpAddress();

            try
            {
                return await _iPQualityScoreApi.GetIPReputationAsync(ipAddress, query);
            }
            catch (ApiException ex)
            {
                if (_iPQualityScoreSettings.LogRequestErrors)
                {
                    var customer = await _workContext.GetCurrentCustomerAsync();
                    var error = $"{Defaults.SystemName}: Error when calling the IP Reputation endpoint for IP '{ipAddress}'.";
                    await _logger.ErrorAsync(error, ex, customer);
                }
            }

            return null;
        }

        private IDictionary<string, string> GetIpReputationQuery(HttpContext httpContext, string culture)
        {
            var query = new Dictionary<string, string>()
            {
                ["mobile"] = _userAgentHelper.IsMobileDevice().ToString(),
                ["strictness"] = _iPQualityScoreSettings.IPReputationStrictness.ToString(),
                ["allow_public_access_points"] = _iPQualityScoreSettings.AllowPublicAccessPoints.ToString(),
                ["lighter_penalties"] = _iPQualityScoreSettings.LighterPenalties.ToString(),
                ["user_language"] = culture
            };

            if (httpContext.Request.Headers.TryGetValue("User-Agent", out var userAgentRaw))
                query["user_agent"] = userAgentRaw.ToString();

            return query;
        }

        private async Task<IDictionary<string, string>> GetIpReputationTransactionQueryAsync(Order order, HttpContext httpContext)
        {
            var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);
            var language = await _languageService.GetLanguageByIdAsync(customer.LanguageId ?? 0);
            var orderItems = await _orderService.GetOrderItemsAsync(order.Id);
            var query = GetIpReputationQuery(httpContext, language?.LanguageCulture);

            var billingAddress = await _addressService.GetAddressByIdAsync(order.BillingAddressId);
            if (billingAddress != null)
            {
                query["billing_first_name"] = billingAddress.FirstName ?? string.Empty;
                query["billing_last_name"] = billingAddress.LastName ?? string.Empty;
                query["billing_company"] = billingAddress.Company ?? string.Empty;
                query["billing_country"] = (await _countryService.GetCountryByIdAsync(billingAddress.CountryId ?? 0))?.TwoLetterIsoCode;
                query["billing_address_1"] = billingAddress.Address1 ?? string.Empty;
                query["billing_address_2"] = billingAddress.Address2 ?? string.Empty;
                query["billing_city"] = billingAddress.City ?? string.Empty;
                query["billing_region"] = (await _stateProvinceService.GetStateProvinceByIdAsync(billingAddress.StateProvinceId ?? 0))?.Name;
                query["billing_postcode"] = billingAddress.ZipPostalCode ?? string.Empty;
                query["billing_email"] = billingAddress.Email ?? string.Empty;
                query["billing_phone"] = billingAddress.PhoneNumber ?? string.Empty;
            }

            if (order.ShippingAddressId.HasValue && !order.PickupInStore)
            {
                var shippingAddress = await _addressService.GetAddressByIdAsync(order.ShippingAddressId.Value);
                if (shippingAddress != null)
                {
                    query["shipping_first_name"] = shippingAddress.FirstName ?? string.Empty;
                    query["shipping_last_name"] = shippingAddress.LastName ?? string.Empty;
                    query["shipping_company"] = shippingAddress.Company ?? string.Empty;
                    query["shipping_country"] = (await _countryService.GetCountryByIdAsync(shippingAddress.CountryId ?? 0))?.TwoLetterIsoCode;
                    query["shipping_address_1"] = shippingAddress.Address1 ?? string.Empty;
                    query["shipping_address_2"] = shippingAddress.Address2 ?? string.Empty;
                    query["shipping_city"] = shippingAddress.City ?? string.Empty;
                    query["shipping_region"] = (await _stateProvinceService.GetStateProvinceByIdAsync(shippingAddress.StateProvinceId ?? 0))?.Name;
                    query["shipping_postcode"] = shippingAddress.ZipPostalCode ?? string.Empty;
                    query["shipping_email"] = shippingAddress.Email ?? string.Empty;
                    query["shipping_phone"] = shippingAddress.PhoneNumber ?? string.Empty;
                }
            }

            query["username"] = customer.Username ?? string.Empty;
            query["order_amount"] = order.OrderTotal.ToString(CultureInfo.InvariantCulture);
            query["order_quantity"] = orderItems.Sum(item => item.Quantity).ToString(CultureInfo.InvariantCulture);

            return query;
        }

        private bool IsValidResponse(IPReputationResponse response)
        {
            if (response is null)
            {
                // don't validate request if the HTTP error occurs
                return true;
            }

            var isProxy = _iPQualityScoreSettings.ProxyBlockingEnabled && response.IsProxy;
            var isVpn = _iPQualityScoreSettings.VpnBlockingEnabled && response.IsVpn;
            var isTor = _iPQualityScoreSettings.TorBlockingEnabled && response.IsTor;
            var isFraud = response.FraudScore >= Convert.ToDouble(_iPQualityScoreSettings.IPReputationFraudScoreForBlocking);

            if (_iPQualityScoreSettings.AllowCrawlers && response.IsCrawler)
                return response.Success;

            return response.Success && !isFraud && !isProxy && !isVpn && !isTor;
        }

        private bool IsValidTransactionalResponse(IPReputationResponse response)
        {
            var isValidRequest = IsValidResponse(response);
            if (isValidRequest && response?.TransactionDetails != null)
                return response.TransactionDetails.RiskScore < Convert.ToDouble(_iPQualityScoreSettings.RiskScoreForBlocking);

            return isValidRequest;
        }

        private async Task SetOrderStatusAsync(Order order, int statusId)
        {
            if (order is null)
                throw new ArgumentNullException(nameof(order));

            order.OrderStatusId = statusId;
            await _orderService.UpdateOrderAsync(order);
            await _orderProcessingService.CheckOrderStatusAsync(order);
        }

        private string GetCurrentRouteName(HttpContext httpContext)
        {
            //first, try to get a common route name
            var routeName = httpContext
                .GetEndpoint()?.Metadata
                .GetMetadata<RouteNameMetadata>()?.RouteName;

            //then try to get a generic one (actually it's an action name, not the route)
            if (string.IsNullOrEmpty(routeName) && httpContext.GetRouteValue(NopRoutingDefaults.RouteValue.SeName) != null)
                routeName = httpContext.GetRouteValue(NopRoutingDefaults.RouteValue.Action)?.ToString();

            return routeName;
        }

        private async Task<bool> CanValidateRequestIPAsync(HttpContext httpContext)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (customer.IsSystemAccount)
            {
                if (!_userAgentHelper.IsSearchEngine())
                    return false;
                else
                {
                    if (_iPQualityScoreSettings.AllowCrawlers)
                        return false;
                }
            }

            return await CanValidateRequestAsync(httpContext);
        }

        private async Task<bool> CanValidateRequestAsync(HttpContext httpContext)
        {
            if (httpContext.GetRouteValue("area") is string area && area == AreaNames.Admin)
                return false;

            var routeName = GetCurrentRouteName(httpContext);
            if (routeName == Defaults.PreventFraudRouteName)
                return false;

            var ipAddress = _webHelper.GetCurrentIpAddress();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress.Equals("127.0.0.1"))
                return false;

            if (await _widgetPluginManager.LoadPluginBySystemNameAsync(Defaults.SystemName) is not IPQualityScorePlugin plugin)
                return false;

            if (!_widgetPluginManager.IsPluginActive(plugin))
                return false;

            if (await _customerService.IsAdminAsync(await _workContext.GetCurrentCustomerAsync()))
                return false;

            return await IsConfiguredAsync();
        }

        private async Task<bool> IsConfiguredAsync()
        {
            var validator = EngineContext.Current.Resolve<IValidator<ConfigurationModel>>();
            var validationResult = await validator.ValidateAsync(new ConfigurationModel
            {
                ApiKey = _iPQualityScoreSettings.ApiKey,
                IPReputationFraudScoreForBlocking = _iPQualityScoreSettings.IPReputationFraudScoreForBlocking,
                IPReputationStrictness = _iPQualityScoreSettings.IPReputationStrictness,
                TransactionStrictness = _iPQualityScoreSettings.TransactionStrictness,
                RiskScoreForBlocking = _iPQualityScoreSettings.RiskScoreForBlocking,
                IPReputationEnabled = _iPQualityScoreSettings.IPReputationEnabled,
                OrderScoringEnabled = _iPQualityScoreSettings.OrderScoringEnabled,
                EmailValidationEnabled = _iPQualityScoreSettings.EmailValidationEnabled,
                EmailReputationFraudScoreForBlocking = _iPQualityScoreSettings.EmailReputationFraudScoreForBlocking,
                AbuseStrictness = _iPQualityScoreSettings.AbuseStrictness,
                DeviceFingerprintEnabled = _iPQualityScoreSettings.DeviceFingerprintEnabled,
                DeviceFingerprintFraudChance = _iPQualityScoreSettings.DeviceFingerprintFraudChance,
                DeviceFingerprintTrackingCode = _iPQualityScoreSettings.DeviceFingerprintTrackingCode,
            });

            return validationResult.IsValid;
        }

        #endregion
    }
}