using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Misc.IPQualityScore.Controllers
{
    public class IPQualityScoreController : BasePluginController
    {
        #region Methods

        public IActionResult PreventFraud()
        {
            return View("~/Plugins/Misc.IPQualityScore/Views/PreventFraud.cshtml");
        }

        #endregion
    }
}
