using System.Text.Json.Serialization;

namespace functions_eventshub_to_cosmosdb
{
    public class YelloTaxiEvent {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        public string? VendorID { get; set; }

        public int PassengerCount { get; set; }

        public float FareAmount { get; set; }

        public float TripDistance { get; set; }

        public long TpepPickupDateTime { get; set; }

        public long TpepDropoffDateTime { get; set; }
    }
}
