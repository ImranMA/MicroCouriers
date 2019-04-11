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
        public static async void Run([ServiceBusTrigger("microcouriers-topic", "readprojection", Connection = "ServiceBus")]Message mySbMsg, ILogger log)
        {         
            EventStore eS = new EventStore();
            await eS.UpdateBookingModelInCache(mySbMsg);
           // log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
