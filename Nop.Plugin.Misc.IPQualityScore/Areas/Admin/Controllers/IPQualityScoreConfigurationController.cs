using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Models;
using Nop.Plugin.Misc.IPQualityScore.Domain;
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
    [AuthorizeAdmin]
    public class IPQualityScoreConfigurationController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public IPQualityScoreConfigurationController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var iPQualityScoreSettings = await _settingService.LoadSettingAsync<IPQualityScoreSettings>(storeScope);

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
                AbuseStrictness = iPQualityScoreSettings.AbuseStrictness,
                DeviceFingerprintEnabled = iPQualityScoreSettings.DeviceFingerprintEnabled,
                DeviceFingerprintTrackingCode = iPQualityScoreSettings.DeviceFingerprintTrackingCode,
                DeviceFingerprintFraudChance = iPQualityScoreSettings.DeviceFingerprintFraudChance,
                AllowCrawlers = iPQualityScoreSettings.AllowCrawlers,
                BlockUserIfScriptIsBlocked = iPQualityScoreSettings.BlockUserIfScriptIsBlocked,
                InformCustomerAboutFraud = iPQualityScoreSettings.InformCustomerAboutFraud,
                UserIdVariableName = iPQualityScoreSettings.UserIdVariableName,
                IPQualityGroupIds = iPQualityScoreSettings.IPQualityGroupIds,
                IPBlockNotificationTypeId = (int)iPQualityScoreSettings.IPBlockNotificationType,
            };

            var orderStatusItems = await OrderStatus.Pending.ToSelectListAsync(false);
            foreach (var statusItem in orderStatusItems)
                model.AvailableOrderStatuses.Add(statusItem);

            model.AvailableIPQualityGroupIds.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.IPQualityScore.IPQualityGroups.Customer"),
                Value = Defaults.IPQualityGroups.Customer.Id.ToString()
            });
            model.AvailableIPQualityGroupIds.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.IPQualityScore.IPQualityGroups.Catalog"),
                Value = Defaults.IPQualityGroups.Catalog.Id.ToString()
            });
            model.AvailableIPQualityGroupIds.Add(new SelectListItem
            {
                Text = await _localizationService.GetResourceAsync("Plugins.Misc.IPQualityScore.IPQualityGroups.Checkout"),
                Value = Defaults.IPQualityGroups.Checkout.Id.ToString()
            });

            var iPBlockNotificationTypeItems = await IPBlockNotificationType.DisplayNotification.ToSelectListAsync(false);
            foreach (var iPBlockNotificationTypeItem in iPBlockNotificationTypeItems)
                model.AvailableIPBlockNotificationTypes.Add(iPBlockNotificationTypeItem);

            if (storeScope > 0)
            {
                model.ApiKey_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.ApiKey, storeScope);
                model.IPReputationEnabled_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.IPReputationEnabled, storeScope);
                model.IPReputationFraudScoreForBlocking_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.IPReputationFraudScoreForBlocking, storeScope);
                model.ProxyBlockingEnabled_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.ProxyBlockingEnabled, storeScope);
                model.VpnBlockingEnabled_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.VpnBlockingEnabled, storeScope);
                model.TorBlockingEnabled_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.TorBlockingEnabled, storeScope);
                model.IPReputationStrictness_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.IPReputationStrictness, storeScope);
                model.AllowPublicAccessPoints_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.AllowPublicAccessPoints, storeScope);
                model.LighterPenalties_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.LighterPenalties, storeScope);
                model.OrderScoringEnabled_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.OrderScoringEnabled, storeScope);
                model.RiskScoreForBlocking_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.RiskScoreForBlocking, storeScope);
                model.TransactionStrictness_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.TransactionStrictness, storeScope);
                model.ApproveStatusId_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.ApproveStatusId, storeScope);
                model.RejectStatusId_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.RejectStatusId, storeScope);
                model.EmailValidationEnabled_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.EmailValidationEnabled, storeScope);
                model.EmailReputationFraudScoreForBlocking_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.EmailReputationFraudScoreForBlocking, storeScope);
                model.AbuseStrictness_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.AbuseStrictness, storeScope);
                model.DeviceFingerprintEnabled_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.DeviceFingerprintEnabled, storeScope);
                model.DeviceFingerprintTrackingCode_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.DeviceFingerprintTrackingCode, storeScope);
                model.DeviceFingerprintFraudChance_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.DeviceFingerprintFraudChance, storeScope);
                model.AllowCrawlers_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.AllowCrawlers, storeScope);
                model.BlockUserIfScriptIsBlocked_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.BlockUserIfScriptIsBlocked, storeScope);
                model.InformCustomerAboutFraud_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.InformCustomerAboutFraud, storeScope);
                model.UserIdVariableName_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.UserIdVariableName, storeScope);
                model.IPQualityGroupIds_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.IPQualityGroupIds, storeScope);
                model.IPBlockNotificationTypeId_OverrideForStore = await _settingService.SettingExistsAsync(iPQualityScoreSettings, x => x.IPBlockNotificationType, storeScope);
            }

            return View("~/Plugins/Misc.IPQualityScore/Areas/Admin/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var iPQualityScoreSettings = await _settingService.LoadSettingAsync<IPQualityScoreSettings>(storeScope);

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
            iPQualityScoreSettings.AbuseStrictness = model.AbuseStrictness;
            iPQualityScoreSettings.DeviceFingerprintEnabled = model.DeviceFingerprintEnabled;
            iPQualityScoreSettings.DeviceFingerprintTrackingCode = model.DeviceFingerprintTrackingCode;
            iPQualityScoreSettings.DeviceFingerprintFraudChance = model.DeviceFingerprintFraudChance;
            iPQualityScoreSettings.AllowCrawlers = model.AllowCrawlers;
            iPQualityScoreSettings.BlockUserIfScriptIsBlocked = model.BlockUserIfScriptIsBlocked;
            iPQualityScoreSettings.InformCustomerAboutFraud = model.InformCustomerAboutFraud;
            iPQualityScoreSettings.UserIdVariableName = model.UserIdVariableName;
            iPQualityScoreSettings.IPQualityGroupIds = model.IPQualityGroupIds?.ToList();
            iPQualityScoreSettings.IPBlockNotificationType = (IPBlockNotificationType)model.IPBlockNotificationTypeId;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.ApiKey, model.ApiKey_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.IPReputationEnabled, model.IPReputationEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.IPReputationFraudScoreForBlocking, model.IPReputationFraudScoreForBlocking_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.ProxyBlockingEnabled, model.ProxyBlockingEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.VpnBlockingEnabled, model.VpnBlockingEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.TorBlockingEnabled, model.TorBlockingEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.IPReputationStrictness, model.IPReputationStrictness_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.AllowPublicAccessPoints, model.AllowPublicAccessPoints_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.LighterPenalties, model.LighterPenalties_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.OrderScoringEnabled, model.OrderScoringEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.RiskScoreForBlocking, model.RiskScoreForBlocking_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.TransactionStrictness, model.TransactionStrictness_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.ApproveStatusId, model.ApproveStatusId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.RejectStatusId, model.RejectStatusId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.EmailValidationEnabled, model.EmailValidationEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.EmailReputationFraudScoreForBlocking, model.EmailReputationFraudScoreForBlocking_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.AbuseStrictness, model.AbuseStrictness_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.DeviceFingerprintEnabled, model.DeviceFingerprintEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.DeviceFingerprintTrackingCode, model.DeviceFingerprintTrackingCode_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.DeviceFingerprintFraudChance, model.DeviceFingerprintFraudChance_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.AllowCrawlers, model.AllowCrawlers_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.BlockUserIfScriptIsBlocked, model.BlockUserIfScriptIsBlocked_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.InformCustomerAboutFraud, model.InformCustomerAboutFraud_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.UserIdVariableName, model.UserIdVariableName_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.IPQualityGroupIds, model.IPQualityGroupIds_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.IPBlockNotificationType, model.IPBlockNotificationTypeId_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return RedirectToAction("Configure");
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save-token")]
        public async Task<IActionResult> SaveToken(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (string.IsNullOrWhiteSpace(model.ApiKey))
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Plugins.Misc.IPQualityScore.Fields.ApiKey.Invalid"));
                return RedirectToAction("Configure");
            }

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var iPQualityScoreSettings = await _settingService.LoadSettingAsync<IPQualityScoreSettings>(storeScope);

            //save token
            iPQualityScoreSettings.ApiKey = model.ApiKey;

            await _settingService.SaveSettingOverridablePerStoreAsync(iPQualityScoreSettings, x => x.ApiKey, model.ApiKey_OverrideForStore, storeScope);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return RedirectToAction("Configure");
        }

        #endregion
    }
}