using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Domain.AggregatesModel.TrackingAggregate;
using Tracking.Domain.Events;
using Tracking.Domain.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Dapper;
using Newtonsoft.Json.Converters;
using System.Data.SqlClient;
using Tracking.Persistence.Model;
using Polly;
using Newtonsoft.Json.Linq;
using System.Linq;
using Tracking.Common;

namespace Tracking.Persistence.Repositories
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
                NamingStrategy = new CamelCaseNamingStrategy()
            });

            _assemblyTypes = TypeResolver.AssemblyTypes;
        }

        public TrackingRepository(string connectionString )
        {
            _connectionString = connectionString;          
        }

        //Get the Event Version against booking ID
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

                tracking = new Track();
                tracking.OriginalVersion = aggregate.CurrentVersion;
            }

            return tracking;
        }

        //Get the Complete events histroy and append
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

        //Append the new event and save it in the table.
        //We need to make sure there is no version conflict
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
                        throw new Exception("Concurency Exception");
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

        //Create the database if doesn't exists
        public void EnsureDatabase()
        {

            try
            {
                // init db
                using (SqlConnection conn = new SqlConnection(_connectionString.Replace("TrackingEventsStore", "master")))
                {
                    conn.Open();

                    // create database
                    string sql = "if DB_ID('TrackingEventsStore') IS NULL CREATE DATABASE TrackingEventsStore;";

                    conn.Execute(sql);

                    conn.ChangeDatabase("TrackingEventsStore");
                    sql = @" 
                    if OBJECT_ID('Tracking') IS NULL 
                    CREATE TABLE Tracking (
                        [Id] varchar(50) NOT NULL,
                        [CurrentVersion] int NOT NULL,
                    PRIMARY KEY([Id]));
                   
                    if OBJECT_ID('TrackingEvent') IS NULL
                    CREATE TABLE TrackingEvent (
                        [Id] varchar(50) NOT NULL REFERENCES Tracking([Id]),
                        [Version] int NOT NULL,
                        [Timestamp] datetime2(7) NOT NULL,
                        [MessageType] varchar(75) NOT NULL,
                        [EventData] text,
                    PRIMARY KEY([Id], [Version]));";

                    conn.Execute(sql);
                }
            }
            catch(Exception)
            {

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
                           
            JObject eventObj = JsonConvert.DeserializeObject<JObject>(eventData, _serializerSettings);
            return eventObj.ToObject(eventType) as EventBase;
        }
    }
}
