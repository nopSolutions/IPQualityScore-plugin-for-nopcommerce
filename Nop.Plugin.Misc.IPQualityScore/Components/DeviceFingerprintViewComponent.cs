using Microsoft.AspNetCore.Mvc;
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

        #endregion

        #region Ctor

        public DeviceFingerprintViewComponent(
            IPQualityScoreSettings iPQualityScoreSettings,
            IPQualityScoreService iPQualityScoreService)
        {
            _iPQualityScoreSettings = iPQualityScoreSettings;
            _iPQualityScoreService = iPQualityScoreService;
        }

        #endregion

        #region Methods

        public virtual IViewComponentResult Invoke()
        {
            if (!_iPQualityScoreService.CanDisplayDeviceFingerprint(HttpContext))
                return Content(string.Empty);

            var model = new DeviceFingerprintModel
            {
                TrackingCode = _iPQualityScoreSettings.DeviceFingerprintTrackingCode,
                FraudChance = _iPQualityScoreSettings.DeviceFingerprintFraudChance,
                BlockUserIfScriptIsBlocked = _iPQualityScoreSettings.BlockUserIfScriptIsBlocked,
            };

            return View("~/Plugins/Misc.IPQualityScore/Views/DeviceFingerprint.cshtml", model);
        }

        #endregion
    }
}
