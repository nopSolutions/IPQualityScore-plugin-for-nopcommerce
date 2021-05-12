using Microsoft.AspNetCore.Mvc.Infrastructure;
using Nop.Core.Domain.Orders;
using Nop.Services.Events;

namespace Nop.Plugin.Misc.IPQualityScore.Services
{
    public class EventConsumer : IConsumer<OrderPlacedEvent>
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IPQualityScoreService _iPQualityScoreService;

        #endregion

        #region Ctor

        public EventConsumer(
            IActionContextAccessor actionContextAccessor,
            IPQualityScoreService iPQualityScoreService)
        {
            _actionContextAccessor = actionContextAccessor;
            _iPQualityScoreService = iPQualityScoreService;
        }

        #endregion

        #region Method

        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            var order = eventMessage.Order;
            var actionContext = _actionContextAccessor.ActionContext;

            if (_iPQualityScoreService.CanValidateOrder(order, actionContext))
            {
                if (_iPQualityScoreService.ValidateOrderAsync(order, actionContext).Result)
                    _iPQualityScoreService.ApproveOrder(order);
                else
                    _iPQualityScoreService.RejectOrder(order);
            }
        }

        #endregion
    }
}
