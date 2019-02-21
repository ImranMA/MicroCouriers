using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.IntegrationEvents
{
    public class BookingAddIntegrationEventHandler : IIntegrationEventHandler<BookingAddIntegrationEvent>
    {
        public async Task Handle(BookingAddIntegrationEvent eventMsg)
        {
            var result = false;

            if (eventMsg.Id != Guid.Empty)
            {
                string a = "";
            }
        }
    }
}
