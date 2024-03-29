﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.IPQualityScore.Domain;
using Nop.Services.Cms;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.IPQualityScore.Components
{
    /// <summary>
    /// Represents a view component to display the IPQualityScore IP result information in public store
    /// </summary>
    public class IPResultInformationViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IPQualityScoreSettings _iPQualityScoreSettings;
        private readonly IWidgetPluginManager _widgetPluginManager;

        #endregion

        #region Ctor

        public IPResultInformationViewComponent(IPQualityScoreSettings iPQualityScoreSettings,
            IWidgetPluginManager widgetPluginManager
        )
        {
            _iPQualityScoreSettings = iPQualityScoreSettings;
            _widgetPluginManager = widgetPluginManager;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (await _widgetPluginManager.LoadPluginBySystemNameAsync(Defaults.SystemName) is not IPQualityScorePlugin plugin)
                return Content(string.Empty);

            if (!_widgetPluginManager.IsPluginActive(plugin))
                return Content(string.Empty);

            if (_iPQualityScoreSettings.IPReputationEnabled &&
                    _iPQualityScoreSettings.IPBlockNotificationType == IPBlockNotificationType.DisplayNotification &&
                        HttpContext.Items?.ContainsKey(Defaults.IPQualityResultId) == true)
            {
                return View("~/Plugins/Misc.IPQualityScore/Views/IPResultInformation.cshtml");
            }

            return Content(string.Empty);
        }

        #endregion
    }
}