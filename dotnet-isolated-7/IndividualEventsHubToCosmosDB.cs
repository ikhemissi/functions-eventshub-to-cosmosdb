using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace functions_eventshub_to_cosmosdb
{
    public class IndividualEventsHubToCosmosDB
    {
        private readonly ILogger<IndividualEventsHubToCosmosDB> _logger;

        public IndividualEventsHubToCosmosDB(ILogger<IndividualEventsHubToCosmosDB> logger)
        {
            _logger = logger;
        }

        [Function(nameof(IndividualEventsHubToCosmosDB))]
        [CosmosDBOutput("%COSMOSDB_DATABASE%", "%COSMOSDB_INDIVIDUAL_EVENTS_CONTAINER%", Connection = "COSMOSDB_CONNECTION_STRING", CreateIfNotExists = true)]
        public object? Run([EventHubTrigger("%EVENT_HUB_NAME%", Connection = "EVENT_HUB_CONNECTION_STRING", ConsumerGroup = "%EVENT_HUB_INDIVIDUAL_CONSUMER_GROUP%", IsBatched = false)] EventData eventData)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            string jsonMessageBody = Encoding.UTF8.GetString(eventData.Body.ToArray());

            YelloTaxiEvent? yelloTaxiEvent = JsonSerializer.Deserialize<YelloTaxiEvent>(jsonMessageBody, serializeOptions);

            if (yelloTaxiEvent != null) {
                yelloTaxiEvent.Id = yelloTaxiEvent.Id ?? Guid.NewGuid().ToString();

                _logger.LogInformation("Event MessageId: {messageId}", eventData.MessageId);
                _logger.LogInformation("Event CorrelationId: {correlationId}", eventData.CorrelationId);
                _logger.LogInformation("Event Id: {id}", yelloTaxiEvent.Id);
                _logger.LogInformation("Event Body: {body}", yelloTaxiEvent);
                _logger.LogInformation("Vendor ID: {vendorId}", yelloTaxiEvent?.VendorID);

                return yelloTaxiEvent;
            }

            return null;
        }
    }
}
