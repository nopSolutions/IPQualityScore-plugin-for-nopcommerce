using System.Threading.Tasks;
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

        public EventConsumer(IActionContextAccessor actionContextAccessor,
            IPQualityScoreService iPQualityScoreService)
        {
            _actionContextAccessor = actionContextAccessor;
            _iPQualityScoreService = iPQualityScoreService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle order placed event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task HandleEventAsync(OrderPlacedEvent eventMessage)
        {
            var order = eventMessage.Order;
            var actionContext = _actionContextAccessor.ActionContext;

            if (await _iPQualityScoreService.CanValidateOrderAsync(order, actionContext))
            {
                if (await _iPQualityScoreService.ValidateOrderAsync(order, actionContext))
                    await _iPQualityScoreService.ApproveOrderAsync(order);
                else
                    await _iPQualityScoreService.RejectOrderAsync(order);
            }
        }

        #endregion
    }
}