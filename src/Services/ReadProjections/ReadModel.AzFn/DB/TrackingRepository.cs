using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Domain.AggregatesModel.TrackingAggregate;
using Tracking.Domain.Events;
using Tracking.Domain.Interfaces;
using Newtonsoft.Json;
using Dapper;
using Newtonsoft.Json.Converters;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Linq;
using ReadModel.AzFn.Model;
using ReadModel.AzFn.Events;
using Tracking.Common;

namespace ReadModel.AzFn.DB
{
    public class TrackingRepository : ITrackingRepository
    {
        private static readonly JsonSerializerSettings _serializerSettings;
        private static readonly Dictionary<DateTime, string> _store = new Dictionary<DateTime, string>();
        private string _connectionString;
        private static List<Type> _assemblyTypes;

        static TrackingRepository()
        {
            _serializerSettings = new JsonSerializerSettings();
            _serializerSettings.Formatting = Formatting.Indented;
            _serializerSettings.Converters.Add(new StringEnumConverter
            {
               // NamingStrategy = new CamelCaseNamingStrategy()
            });

            _assemblyTypes = TypeResolver.AssemblyTypes;
        }

        public TrackingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> GetOrderHistory(string id)
        {
            return string.Empty;
        }

        public async Task<Track> GetEventVersion(string id)
        {
            Track tracking = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // get aggregate
                Aggregate aggregate = await conn
                       .QuerySingleOrDefaultAsync<Aggregate>(
                           "select * from Tracking where Id = @Id",
                           new { Id = id });

                if (aggregate == null)
                {
                    return null;
                }

                // get events
                /*IEnumerable<AggregateEvent> aggregateEvents = await conn
                    .QueryAsync<AggregateEvent>(
                        "select * from TrackingEvent where Id = @Id order by [Version];",
                        new { Id = id });

                List<EventBase> events = new List<EventBase>();
                foreach (var aggregateEvent in aggregateEvents)
                {
                    events.Add(DeserializeEventData(aggregateEvent.MessageType, aggregateEvent.EventData));
                }*/
                tracking = new Track();
                tracking.OriginalVersion = aggregate.CurrentVersion;

            }
            return tracking;
        }

        public async Task<Track> GetTrackingAsync(string id)
        {
            Track tracking = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // get aggregate
                var aggregate = await conn
                       .QuerySingleOrDefaultAsync<Aggregate>(
                           "select * from Tracking where Id = @Id",
                           new { Id = id });

                if (aggregate == null)
                {
                    return null;
                }

                // get events
                IEnumerable<AggregateEvent> aggregateEvents = await conn
                    .QueryAsync<AggregateEvent>(
                        "select * from TrackingEvent where Id = @Id order by [Version];",
                        new { Id = id });

                List<EventBase> events = new List<EventBase>();
                foreach (var aggregateEvent in aggregateEvents)
                {
                    events.Add(DeserializeEventData(aggregateEvent.MessageType, aggregateEvent.EventData));
                }
                tracking = new Track(events);
            }
            return tracking;
        }

        public async Task SaveTrackingAsync(string bookingId, int originalVersion, int newVersion,
            IEnumerable<EventBase> newEvents)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // update eventstore
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    // store aggregate
                    int affectedRows = 0;
                    var aggregate = await conn
                        .QuerySingleOrDefaultAsync<Aggregate>(
                            "select * from Tracking where Id = @Id",
                            new { Id = bookingId },
                            transaction);

                    if (aggregate != null)
                    {
                        // update existing aggregate
                        affectedRows = await conn.ExecuteAsync(
                            @"update Tracking
                              set [CurrentVersion] = @NewVersion
                              where [Id] = @Id
                              and [CurrentVersion] = @CurrentVersion;",
                            new
                            {
                                Id = bookingId,
                                NewVersion = newVersion,
                                CurrentVersion = originalVersion
                            },
                            transaction);
                    }
                    else
                    {
                        // insert new aggregate
                        affectedRows = await conn.ExecuteAsync(
                            "insert Tracking ([Id], [CurrentVersion]) values (@Id, @CurrentVersion)",
                            new { Id = bookingId, CurrentVersion = newVersion },
                            transaction);
                    }

                    // check concurrency
                    if (affectedRows == 0)
                    {
                        transaction.Rollback();
                        //throw new ConcurrencyException();
                    }

                    // store events
                    int eventVersion = originalVersion;
                    foreach (var e in newEvents)
                    {
                        eventVersion++;
                        await conn.ExecuteAsync(
                            @"insert TrackingEvent ([Id], [Version], [Timestamp], [MessageType], [EventData])
                              values (@Id, @NewVersion, @Timestamp, @MessageType,@EventData);",
                            new
                            {
                                Id = bookingId,
                                NewVersion = eventVersion,
                                Timestamp = DateTime.Now,
                                MessageType = e.MessageType,
                                EventData = SerializeEventData(e)
                            }, transaction);
                    }

                    // commit
                    transaction.Commit();
                }
            }
        }
       

        /// <summary>
        /// Serialize event-data to JSON.
        /// </summary>
        /// <param name="eventData">The event-data to serialize.</param>
        private string SerializeEventData(EventBase eventData)
        {
            return JsonConvert.SerializeObject(eventData, _serializerSettings);
        }

        /// <summary>
        /// Deserialize event-data from JSON.
        /// </summary>
        /// <param name="messageType">The message-type of the event.</param>
        /// <param name="eventData">The event-data JSON to deserialize.</param>
        private EventBase DeserializeEventData(string messageType, string eventData)
        {
            Type eventType = _assemblyTypes
                     .Where(t => t.Name.Contains(messageType)).FirstOrDefault();

            //Type eventType1 = Type.GetType($"Tracking.Domain.Events.{messageType}");
            JObject obj = JsonConvert.DeserializeObject<JObject>(eventData, _serializerSettings);
            return obj.ToObject(eventType) as EventBase;
        }

        public void EnsureDatabase()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetOrderHistroy(string bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
