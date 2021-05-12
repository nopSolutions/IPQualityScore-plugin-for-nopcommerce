using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Plugin.Misc.IPQualityScore.Services;

namespace Nop.Plugin.Misc.IPQualityScore.Infrastructure
{
    /// <summary>
    /// Represents a filter that verify the user usnig IPQualityScore service
    /// </summary>
    public class IPQualityScoreAsyncActionFilter : IAsyncActionFilter
    {
        #region Fields

        private readonly IPQualityScoreService _iPQualityScoreService;

        #endregion

        #region Ctor

        public IPQualityScoreAsyncActionFilter(
            IPQualityScoreService iPQualityScoreService)
        {
            _iPQualityScoreService = iPQualityScoreService;
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
                    context.Result = new RedirectToActionResult("PreventFraud", "IPQualityScore", null);
                    return;
                }
            }

            if (_iPQualityScoreService.CanValidateEmailForRequest(context))
            {
                var isValid = await _iPQualityScoreService.ValidateEmailForRequestAsync(context);
                if (!isValid)
                {
                    context.Result = new RedirectToActionResult("PreventFraud", "IPQualityScore", null);
                    return;
                }
            }

            await next();
        }

        #endregion
    }
}
