using Dapper;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ReadModel.AzFn.Events;
using ReadModel.AzFn.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Tracking.Domain.AggregatesModel.TrackingAggregate;
using Tracking.Domain.Events;
using Tracking.Domain.Interfaces;
using Tracking.Domain.Model;

namespace ReadModel.AzFn.DB
{

    public class EventStore
    {
        public EventStore()
        {

        }

        public async Task<IEnumerable<string>> UpdateBookingModelInCache(Message msg)
        {
            RedisCacheService rCache = new RedisCacheService();
            List<EventBase> events = new List<EventBase>();
            Track tracking = new Track();
            var bookingId = string.Empty;

            List<OrderHistory> trackingHistory = new List<OrderHistory>();


            IEnumerable<string> listId = new List<string>();
            try
            {
                if(msg.Label == "BookingAdd")
                {
                    BookingAddIntegrationEvent eventMsg = JsonConvert.DeserializeObject<BookingAddIntegrationEvent>(Encoding.UTF8.GetString(msg.Body));

                    string messageType = "BookingCreated";

                    BookingCreated bookingCreated = new
                       BookingCreated(eventMsg.BookingId, string.Empty, eventMsg.Id
                       , messageType, eventMsg.CreationDate, eventMsg.Origin, eventMsg.Destination);

                    bookingId = eventMsg.BookingId;
                  
                    
                }
                else if (msg.Label == "OrderPicked")
                {
                    OrderPickedIntegrationEvent eventMsg = JsonConvert.DeserializeObject<OrderPickedIntegrationEvent>(Encoding.UTF8.GetString(msg.Body));

                    string messageType = "OrderPicked";

                    OrderPicked orderPicked = new
                        OrderPicked(eventMsg.BookingId, eventMsg.Description, eventMsg.Id
                        , messageType, eventMsg.CreationDate);

                    bookingId = eventMsg.BookingId;
                    tracking.OrderPicked(orderPicked);
                   
                }

                if(!string.IsNullOrEmpty(rCache.Get(bookingId)))
                trackingHistory = JsonConvert.DeserializeObject<List<OrderHistory>>(rCache.Get(bookingId));

                trackingHistory.AddRange(tracking.orderHistory);

                var result = JsonConvert.SerializeObject(trackingHistory);
                await rCache.Remove(bookingId);
                await rCache.Save(bookingId, result);

            }
            catch (Exception ex)
            {

            }
            return listId;
        }
    }
}
