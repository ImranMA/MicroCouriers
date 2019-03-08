using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadModel.AzFn.DB;
using ReadModel.AzFn.Events;

namespace ReadModel.AzFn
{
    public static class UpdateCache
    {
        [FunctionName("UpdateCache")]
        public static async void Run([ServiceBusTrigger("microcourier", "readprojection", Connection = "ServiceBus")]string mySbMsg, ILogger log)
        {
            JObject obj = JsonConvert.DeserializeObject<JObject>(mySbMsg);
            var de = obj.ToObject(typeof(OrderStatusChangedIntegrationEvent)) as OrderStatusChangedIntegrationEvent;
            EventStore eS = new EventStore();
            await eS.UpdateBookingInCache(de.BookingId);
           // log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
