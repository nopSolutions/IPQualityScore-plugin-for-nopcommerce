using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.IPQualityScore.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(Defaults.PreventFraudRouteName, "/IPQualityScore/PreventFraud",
                new { controller = "IPQualityScore", action = "PreventFraud" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => -1;
    }
}
