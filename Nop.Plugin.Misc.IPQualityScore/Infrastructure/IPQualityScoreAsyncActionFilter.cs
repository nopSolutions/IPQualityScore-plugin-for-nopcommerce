using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core;
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

        private readonly IPQualityScoreService _iPQualityScoreService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public IPQualityScoreAsyncActionFilter(
            IPQualityScoreService iPQualityScoreService,
            ILocalizationService localizationService,
            IWebHelper webHelper)
        {
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
            if (_iPQualityScoreService.CanValidateRequest(context))
            {
                var isValid = await _iPQualityScoreService.ValidateRequestAsync(context);
                if (!isValid)
                {
                    context.Result = GeneratePreventFraudResult(context);
                    return;
                }
            }

            if (_iPQualityScoreService.CanValidateEmailForRequest(context))
            {
                var isValid = await _iPQualityScoreService.ValidateEmailForRequestAsync(context);
                if (!isValid)
                {
                    context.Result = GeneratePreventFraudResult(context);
                    return;
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
