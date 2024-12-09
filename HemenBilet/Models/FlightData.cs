using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HemenBilet.Models
{
    public class FlightData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("flight_id")]
        public string FlightId { get; set; } // Uçuş ID'si

        [BsonElement("airline")]
        public string Airline { get; set; } // Firma adı

        [BsonElement("from_airport")]
        public string FromAirport { get; set; } // Kalkış noktası

        [BsonElement("to_airport")]
        public string ToAirport { get; set; } // Varış noktası

        [BsonElement("departure_time")]
        public DateTime DepartureTime { get; set; } // Kalkış zamanı

        [BsonElement("arrival_time")]
        public DateTime ArrivalTime { get; set; } // Varış zamanı

        [BsonElement("price")]
        public double Price { get; set; } // Bilet ücreti
    }
}
