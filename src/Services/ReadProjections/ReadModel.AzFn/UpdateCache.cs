using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReadModel.AzFn.DB;
using ReadModel.AzFn.Events;
using System.Text;

namespace ReadModel.AzFn
{
    public static class UpdateCache
    {
        [FunctionName("UpdateCache")]
        public static async void Run([ServiceBusTrigger("microcouriers-topic", "readprojection", Connection = "ServiceBus")]Message serviceBusMessage, ILogger log)
        {
            //Redis is our read model.Every Event Triggered will be appended against the booking.
            //We are building the read Model in redis since we are using CQRS where read 
            //model is seperate from Write

            EventStore eS = new EventStore();
            await eS.UpdateBookingModelInCache(serviceBusMessage);
          
            // log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
