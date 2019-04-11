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

        public async Task UpdateBookingModelInCache(Message msg)
        {
            RedisCacheService rCache = new RedisCacheService();
            List<EventBase> events = new List<EventBase>();
            Track tracking = new Track();
            var bookingId = string.Empty;
            List<OrderHistory> trackingHistory = new List<OrderHistory>();

            try
            {
                if (msg.Label == "BookingAdd")
                {
                    BookingAddIntegrationEvent eventMsg = JsonConvert.DeserializeObject<BookingAddIntegrationEvent>(Encoding.UTF8.GetString(msg.Body));

                    string messageType = "BookingCreated";

                    BookingCreated bookingCreated = new
                       BookingCreated(eventMsg.BookingId, string.Empty, eventMsg.Id
                       , messageType, eventMsg.CreationDate, eventMsg.Origin, eventMsg.Destination);

                    bookingId = eventMsg.BookingId;
                    tracking.BookingAdd(bookingCreated);

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

                else if (msg.Label == "OrderTransit")
                {
                    OrderTransitIntegrationEvent eventMsg = JsonConvert.DeserializeObject<OrderTransitIntegrationEvent>(Encoding.UTF8.GetString(msg.Body));

                    string messageType = "OrderInTransit";

                    OrderInTransit orderInTransit = new
                          OrderInTransit(eventMsg.BookingId, eventMsg.Description, eventMsg.Id
                          , messageType, eventMsg.CreationDate);


                    bookingId = eventMsg.BookingId;
                    tracking.OrderInTransit(orderInTransit);

                }
                else if (msg.Label == "OrderDelivered")
                {
                    OrderDeliveredIntegrationEvent eventMsg = JsonConvert.DeserializeObject<OrderDeliveredIntegrationEvent>(Encoding.UTF8.GetString(msg.Body));

                    string messageType = "OrderDelivered";

                    OrderDelivered orderDelivered = new
                       OrderDelivered(eventMsg.BookingId, eventMsg.Description, eventMsg.Id
                       , messageType, eventMsg.CreationDate, eventMsg.SignedBy);


                    bookingId = eventMsg.BookingId;
                    tracking.OrderDelivered(orderDelivered);

                }
                else if (msg.Label == "PaymentProcessed")
                {
                    PaymentProcessedIntegrationEvent eventMsg = JsonConvert.DeserializeObject<PaymentProcessedIntegrationEvent>(Encoding.UTF8.GetString(msg.Body));

                    string messageType = "PaymentProcessed";

                    string description = string.Empty;
                    if (eventMsg.PaymentStatus == PaymetStatus.Completed)
                    {
                        description = "Payment Done";
                    }
                    else if (eventMsg.PaymentStatus == PaymetStatus.Canceled)
                    {
                        description = "Payment Failed";
                    }
                    else if (eventMsg.PaymentStatus == PaymetStatus.Pending)
                    {
                        description = "Payment Pending";
                    }

                    PaymentProcessed eventPaymentProcessed = new
                        PaymentProcessed(eventMsg.BookingOrderId, description, eventMsg.Id, messageType, eventMsg.CreationDate);


                    bookingId = eventMsg.BookingOrderId;
                    tracking.PaymentProcessed(eventPaymentProcessed);
                }

                //If  Booking ID Exists
                if (!string.IsNullOrEmpty(bookingId))
                {

                    if (!string.IsNullOrEmpty(rCache.Get(bookingId)))
                        trackingHistory = JsonConvert.DeserializeObject<List<OrderHistory>>(rCache.Get(bookingId));

                    //Append new event to old events
                    trackingHistory.AddRange(tracking.orderHistory);

                    //Serialze the result
                    var result = JsonConvert.SerializeObject(trackingHistory);
                    await rCache.Remove(bookingId);

                    //Update the Cache
                    if (!string.IsNullOrEmpty(result))
                    {
                        await rCache.Save(bookingId, result);
                    }
                }

            }
            catch (Exception)
            {
            }

        }
    }
}
