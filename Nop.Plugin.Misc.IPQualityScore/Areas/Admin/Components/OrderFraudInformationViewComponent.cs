using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Models;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Orders;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.IPQualityScore.Areas.Admin.Components
{
    /// <summary>
    /// Represents a view component to display the IPQualityScore order fraud information in admin store
    /// </summary>
    public class OrderFraudInformationViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderService _orderService;
        private readonly IWidgetPluginManager _widgetPluginManager;

        #endregion

        #region Ctor

        public OrderFraudInformationViewComponent(IGenericAttributeService genericAttributeService,
            IOrderService orderService,
            IWidgetPluginManager widgetPluginManager
        )
        {
            _genericAttributeService = genericAttributeService;
            _orderService = orderService;
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

            if (additionalData is OrderModel orderModel)
            {
                var order = await _orderService.GetOrderByIdAsync(orderModel.Id);
                if (order != null)
                {
                    var payload = await _genericAttributeService.GetAttributeAsync<string>(order, Defaults.OrderFraudInformationAttributeName);
                    if (payload != null)
                    {
                        var model = JsonConvert.DeserializeObject<OrderFraudInformationModel>(payload);
                        return View("~/Plugins/Misc.IPQualityScore/Areas/Admin/Views/OrderFraudInformation.cshtml", model);
                    }
                }
            }

            return Content(string.Empty);
        }

        #endregion
    }
}