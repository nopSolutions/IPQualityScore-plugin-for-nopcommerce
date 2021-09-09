using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core.Domain.Orders;
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
    [ViewComponent(Name = Defaults.ORDER_FRAUD_INFORMATION_VIEW_COMPONENT_NAME)]
    public class OrderFraudInfrormationViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IOrderService _orderService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWidgetPluginManager _widgetPluginManager;

        #endregion

        #region Ctor

        public OrderFraudInfrormationViewComponent(
            IOrderService orderService,
            IGenericAttributeService genericAttributeService,
            IWidgetPluginManager widgetPluginManager
        )
        {
            _orderService = orderService;
            _genericAttributeService = genericAttributeService;
            _widgetPluginManager = widgetPluginManager;
        }

        #endregion

        #region Methods

        public virtual IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            if (!(_widgetPluginManager.LoadPluginBySystemName(Defaults.SystemName) is IPQualityScorePlugin plugin) || !_widgetPluginManager.IsPluginActive(plugin))
                return Content(string.Empty);

            if (additionalData is OrderModel orderModel)
            {
                var order = _orderService.GetOrderById(orderModel.Id);
                if (order != null)
                {
                    var payload = _genericAttributeService.GetAttribute<string>(order, Defaults.OrderFraudInformationAttributeName);
                    if (payload != null)
                    {
                        var model = JsonConvert.DeserializeObject<OrderFraudInformationModel>(payload);
                        return View("~/Plugins/Misc.IPQualityScore/Areas/Admin/Views/OrderFraudInfrormation.cshtml", model);
                    }
                }
            }

            return Content(string.Empty);
        }

        #endregion
    }
}
