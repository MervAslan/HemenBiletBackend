using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace HemenBilet.Models
{
    public class FlightData
    {
         [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("flight_id")]
        public string FlightId { get; set; }

        [BsonElement("departure")]
        public DateTime DepartureTime { get; set; }

        [BsonElement("arrival")]
        public DateTime ArrivalTime { get; set; }

        [BsonElement("from_airport")]
        public string FromAirport { get; set; }

        [BsonElement("to_airport")]
        public string ToAirport { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
    
    }
}
