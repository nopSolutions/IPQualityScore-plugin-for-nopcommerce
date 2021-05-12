using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Models;
using Nop.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    [ValidateVendor]
    public class IPQualityScoreConfigurationController : BasePluginController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public IPQualityScoreConfigurationController(
            IPermissionService permissionService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the plugin in admin area.
        /// </summary>
        /// <returns>The view to configure.</returns>
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var iPQualityScoreSettings = _settingService.LoadSetting<IPQualityScoreSettings>(storeScope);

            var model = new ConfigurationModel
            {
                ActiveStoreScopeConfiguration = storeScope,
                ApiKey = iPQualityScoreSettings.ApiKey,
                IPReputationEnabled = iPQualityScoreSettings.IPReputationEnabled,
                IPReputationFraudScoreForBlocking = iPQualityScoreSettings.IPReputationFraudScoreForBlocking,
                ProxyBlockingEnabled = iPQualityScoreSettings.ProxyBlockingEnabled,
                VpnBlockingEnabled = iPQualityScoreSettings.VpnBlockingEnabled,
                TorBlockingEnabled = iPQualityScoreSettings.TorBlockingEnabled,
                IPReputationStrictness = iPQualityScoreSettings.IPReputationStrictness,
                AllowPublicAccessPoints = iPQualityScoreSettings.AllowPublicAccessPoints,
                LighterPenalties = iPQualityScoreSettings.LighterPenalties,
                OrderScoringEnabled = iPQualityScoreSettings.OrderScoringEnabled,
                RiskScoreForBlocking = iPQualityScoreSettings.RiskScoreForBlocking,
                TransactionStrictness = iPQualityScoreSettings.TransactionStrictness,
                ApproveStatusId = iPQualityScoreSettings.ApproveStatusId,
                RejectStatusId = iPQualityScoreSettings.RejectStatusId,
                EmailValidationEnabled = iPQualityScoreSettings.EmailValidationEnabled,
                EmailReputationFraudScoreForBlocking = iPQualityScoreSettings.EmailReputationFraudScoreForBlocking,
                EmailReputationStrictness = iPQualityScoreSettings.EmailReputationStrictness,
                AbuseStrictness = iPQualityScoreSettings.AbuseStrictness,
                DeviceFingerprintEnabled = iPQualityScoreSettings.DeviceFingerprintEnabled,
                DeviceFingerprintTrackingCode = iPQualityScoreSettings.DeviceFingerprintTrackingCode,
                DeviceFingerprintFraudChance = iPQualityScoreSettings.DeviceFingerprintFraudChance,
                AllowCrawlers = iPQualityScoreSettings.AllowCrawlers
            };

            var orderStatusItems = OrderStatus.Pending.ToSelectList(false);
            foreach (var statusItem in orderStatusItems)
                model.AvailableOrderStatuses.Add(statusItem);

            if (storeScope > 0)
            {
                model.ApiKey_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.ApiKey, storeScope);
                model.IPReputationEnabled_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.IPReputationEnabled, storeScope);
                model.IPReputationFraudScoreForBlocking_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.IPReputationFraudScoreForBlocking, storeScope);
                model.ProxyBlockingEnabled_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.ProxyBlockingEnabled, storeScope);
                model.VpnBlockingEnabled_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.VpnBlockingEnabled, storeScope);
                model.TorBlockingEnabled_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.TorBlockingEnabled, storeScope);
                model.IPReputationStrictness_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.IPReputationStrictness, storeScope);
                model.AllowPublicAccessPoints_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.AllowPublicAccessPoints, storeScope);
                model.LighterPenalties_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.LighterPenalties, storeScope);
                model.OrderScoringEnabled_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.OrderScoringEnabled, storeScope);
                model.RiskScoreForBlocking_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.RiskScoreForBlocking, storeScope);
                model.TransactionStrictness_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.TransactionStrictness, storeScope);
                model.ApproveStatusId_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.ApproveStatusId, storeScope);
                model.RejectStatusId_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.RejectStatusId, storeScope);
                model.EmailValidationEnabled_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.EmailValidationEnabled, storeScope);
                model.EmailReputationFraudScoreForBlocking_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.EmailReputationFraudScoreForBlocking, storeScope);
                model.EmailReputationStrictness_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.EmailReputationStrictness, storeScope);
                model.AbuseStrictness_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.AbuseStrictness, storeScope);
                model.DeviceFingerprintEnabled_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.DeviceFingerprintEnabled, storeScope);
                model.DeviceFingerprintTrackingCode_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.DeviceFingerprintTrackingCode, storeScope);
                model.DeviceFingerprintFraudChance_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.DeviceFingerprintFraudChance, storeScope);
                model.AllowCrawlers_OverrideForStore = _settingService.SettingExists(iPQualityScoreSettings, x => x.AllowCrawlers, storeScope);
            }

            return View("~/Plugins/Misc.IPQualityScore/Areas/Admin/Views/Configure.cshtml", model);
        }

        /// <summary>
        /// Configures the plugin in admin area.
        /// </summary>
        /// <param name="model">The configuration model.</param>
        /// <returns>The view to configure.</returns>
        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var iPQualityScoreSettings = _settingService.LoadSetting<IPQualityScoreSettings>(storeScope);

            //save settings
            iPQualityScoreSettings.ApiKey = model.ApiKey;
            iPQualityScoreSettings.IPReputationEnabled = model.IPReputationEnabled;
            iPQualityScoreSettings.IPReputationFraudScoreForBlocking = model.IPReputationFraudScoreForBlocking;
            iPQualityScoreSettings.ProxyBlockingEnabled = model.ProxyBlockingEnabled;
            iPQualityScoreSettings.VpnBlockingEnabled = model.VpnBlockingEnabled;
            iPQualityScoreSettings.TorBlockingEnabled = model.TorBlockingEnabled;
            iPQualityScoreSettings.IPReputationStrictness = model.IPReputationStrictness;
            iPQualityScoreSettings.AllowPublicAccessPoints = model.AllowPublicAccessPoints;
            iPQualityScoreSettings.LighterPenalties = model.LighterPenalties;
            iPQualityScoreSettings.OrderScoringEnabled = model.OrderScoringEnabled;
            iPQualityScoreSettings.RiskScoreForBlocking = model.RiskScoreForBlocking;
            iPQualityScoreSettings.TransactionStrictness = model.TransactionStrictness;
            iPQualityScoreSettings.ApproveStatusId = model.ApproveStatusId;
            iPQualityScoreSettings.RejectStatusId = model.RejectStatusId;
            iPQualityScoreSettings.EmailValidationEnabled = model.EmailValidationEnabled;
            iPQualityScoreSettings.EmailReputationFraudScoreForBlocking = model.EmailReputationFraudScoreForBlocking;
            iPQualityScoreSettings.EmailReputationStrictness = model.EmailReputationStrictness;
            iPQualityScoreSettings.AbuseStrictness = model.AbuseStrictness;
            iPQualityScoreSettings.DeviceFingerprintEnabled = model.DeviceFingerprintEnabled;
            iPQualityScoreSettings.DeviceFingerprintTrackingCode = model.DeviceFingerprintTrackingCode;
            iPQualityScoreSettings.DeviceFingerprintFraudChance = model.DeviceFingerprintFraudChance;
            iPQualityScoreSettings.AllowCrawlers = model.AllowCrawlers;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.ApiKey, model.ApiKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.IPReputationEnabled, model.IPReputationEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.IPReputationFraudScoreForBlocking, model.IPReputationFraudScoreForBlocking_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.ProxyBlockingEnabled, model.ProxyBlockingEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.VpnBlockingEnabled, model.VpnBlockingEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.TorBlockingEnabled, model.TorBlockingEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.IPReputationStrictness, model.IPReputationStrictness_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.AllowPublicAccessPoints, model.AllowPublicAccessPoints_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.LighterPenalties, model.LighterPenalties_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.OrderScoringEnabled, model.OrderScoringEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.RiskScoreForBlocking, model.RiskScoreForBlocking_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.TransactionStrictness, model.TransactionStrictness_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.ApproveStatusId, model.ApproveStatusId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.RejectStatusId, model.RejectStatusId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.EmailValidationEnabled, model.EmailValidationEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.EmailReputationFraudScoreForBlocking, model.EmailReputationFraudScoreForBlocking_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.EmailReputationStrictness, model.EmailReputationStrictness_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.AbuseStrictness, model.AbuseStrictness_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.DeviceFingerprintEnabled, model.DeviceFingerprintEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.DeviceFingerprintTrackingCode, model.DeviceFingerprintTrackingCode_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.DeviceFingerprintFraudChance, model.DeviceFingerprintFraudChance_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.AllowCrawlers, model.AllowCrawlers_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return RedirectToAction("Configure");
        }

        /// <summary>
        /// Saves the token.
        /// </summary>
        /// <param name="model">The configuration model.</param>
        /// <returns>The view to configure.</returns>
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save-token")]
        public IActionResult SaveToken(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (string.IsNullOrWhiteSpace(model.ApiKey))
            {
                _notificationService.ErrorNotification(_localizationService.GetResource("Plugins.Misc.IPQualityScore.Fields.ApiKey.Invalid"));
                return RedirectToAction("Configure");
            }

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var iPQualityScoreSettings = _settingService.LoadSetting<IPQualityScoreSettings>(storeScope);

            //save token
            iPQualityScoreSettings.ApiKey = model.ApiKey;

            _settingService.SaveSettingOverridablePerStore(iPQualityScoreSettings, x => x.ApiKey, model.ApiKey_OverrideForStore, storeScope);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return RedirectToAction("Configure");
        }

        #endregion
    }
}
