using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.IPQualityScore.Models;
using Nop.Plugin.Misc.IPQualityScore.Services;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.IPQualityScore.Components
{
    /// <summary>
    /// Represents a view component to display the IPQualityScore Device Fingerprint JavaScript snippet in public store
    /// </summary>
    [ViewComponent(Name = Defaults.DEVICE_FINGERPRINT_VIEW_COMPONENT_NAME)]
    public class DeviceFingerprintViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IPQualityScoreSettings _iPQualityScoreSettings;
        private readonly IPQualityScoreService _iPQualityScoreService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public DeviceFingerprintViewComponent(IPQualityScoreSettings iPQualityScoreSettings,
            IPQualityScoreService iPQualityScoreService,
            IWorkContext workContext
        )
        {
            _iPQualityScoreSettings = iPQualityScoreSettings;
            _iPQualityScoreService = iPQualityScoreService;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (!await _iPQualityScoreService.CanDisplayDeviceFingerprintAsync(HttpContext))
                return Content(string.Empty);

            var customer = await _workContext.GetCurrentCustomerAsync();
            var model = new DeviceFingerprintModel
            {
                TrackingCode = _iPQualityScoreSettings.DeviceFingerprintTrackingCode,
                FraudChance = _iPQualityScoreSettings.DeviceFingerprintFraudChance,
                BlockUserIfScriptIsBlocked = _iPQualityScoreSettings.BlockUserIfScriptIsBlocked,
                UserIdVariableName = _iPQualityScoreSettings.UserIdVariableName,
                UserId = customer.Id,
                IPBlockNotificationType = _iPQualityScoreSettings.IPBlockNotificationType,
            };

            return View("~/Plugins/Misc.IPQualityScore/Views/DeviceFingerprint.cshtml", model);
        }

        #endregion
    }
}