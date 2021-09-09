using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core;
using Nop.Plugin.Misc.IPQualityScore.Domain;
using Nop.Plugin.Misc.IPQualityScore.Services;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.IPQualityScore.Infrastructure
{
    /// <summary>
    /// Represents a filter that verify the user usnig IPQualityScore service
    /// </summary>
    public class IPQualityScoreAsyncActionFilter : IAsyncActionFilter
    {
        #region Fields

        private readonly IPQualityScoreSettings _iPQualityScoreSettings;
        private readonly IPQualityScoreService _iPQualityScoreService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public IPQualityScoreAsyncActionFilter(
            IPQualityScoreSettings iPQualityScoreSettings,
            IPQualityScoreService iPQualityScoreService,
            ILocalizationService localizationService,
            IWebHelper webHelper)
        {
            _iPQualityScoreSettings = iPQualityScoreSettings;
            _iPQualityScoreService = iPQualityScoreService;
            _localizationService = localizationService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">The action context</param>
        /// <param name="next">The delegate that asynchronously returns an <see cref="ActionExecutedContext"/> indicating the action or the next action filter has executed.</param>
        /// <returns>The <see cref="Task"/> that on completion indicates the filter has executed.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_iPQualityScoreService.CanValidateIPReputation(context))
            {
                var isValid = await _iPQualityScoreService.ValidateRequestAsync(context);
                if (!isValid)
                {
                    if (_iPQualityScoreSettings.IPBlockNotificationType == IPBlockNotificationType.DisplayNotification)
                        context.HttpContext.Items.Add(Defaults.IPQualityResultId, string.Empty);
                    else
                    {
                        context.Result = GeneratePreventFraudResult(context);
                        return;
                    }
                }
            }

            if (_iPQualityScoreService.CanValidateEmailForRequest(context))
            {
                var isValid = await _iPQualityScoreService.ValidateEmailForRequestAsync(context);
                if (!isValid)
                {
                    if (_iPQualityScoreSettings.IPBlockNotificationType == IPBlockNotificationType.DisplayNotification)
                        context.HttpContext.Items.Add(Defaults.IPQualityResultId, string.Empty);
                    else
                    {
                        context.Result = GeneratePreventFraudResult(context);
                        return;
                    }
                }
            }

            await next();
        }

        #endregion

        #region Utilities

        private IActionResult GeneratePreventFraudResult(ActionExecutingContext context)
        {
            if (!_webHelper.IsAjaxRequest(context.HttpContext.Request))
                return new RedirectToActionResult("PreventFraud", "IPQualityScore", null);
            else
            {
                var fraudMessage = _localizationService
                    .GetResource("Plugins.Misc.IPQualityScore.PreventFraudPage.Content");
                return new BadRequestObjectResult(fraudMessage);
            }
        }

        #endregion
    }
}
