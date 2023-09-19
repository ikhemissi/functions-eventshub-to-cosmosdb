using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace functions_eventshub_to_cosmosdb
{
    public class BatchEventsHubToCosmosDB
    {
        private readonly ILogger<BatchEventsHubToCosmosDB> _logger;

        public BatchEventsHubToCosmosDB(ILogger<BatchEventsHubToCosmosDB> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BatchEventsHubToCosmosDB))]
        [CosmosDBOutput("%COSMOSDB_DATABASE%", "%COSMOSDB_BATCH_EVENTS_CONTAINER%", Connection = "COSMOSDB_CONNECTION_STRING", CreateIfNotExists = true)]
        public IEnumerable<YelloTaxiEvent> Run([EventHubTrigger("%EVENT_HUB_NAME%", Connection = "EVENT_HUB_CONNECTION_STRING", ConsumerGroup = "%EVENT_HUB_BATCH_CONSUMER_GROUP%")] EventData[] events)
        {
            var entries = new List<YelloTaxiEvent>();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            foreach (EventData @event in events)
            {
                string jsonMessageBody = Encoding.UTF8.GetString(@event.Body.ToArray());

                YelloTaxiEvent? yelloTaxiEvent = JsonSerializer.Deserialize<YelloTaxiEvent>(jsonMessageBody, serializeOptions);

                if (yelloTaxiEvent != null) {
                    yelloTaxiEvent.Id = yelloTaxiEvent.Id ?? Guid.NewGuid().ToString();

                    _logger.LogInformation("Event Id: {id}", yelloTaxiEvent.Id);
                    _logger.LogInformation("Vendor ID: {vendorId}", yelloTaxiEvent?.VendorID);

                    entries.Add(yelloTaxiEvent);
                }
            }

            _logger.LogInformation("Saving {count} items in the DB", entries.Count);

            return entries;
        }
    }
}
