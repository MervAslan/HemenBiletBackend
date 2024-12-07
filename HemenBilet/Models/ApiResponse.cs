namespace HemenBilet.Models
{
    // API'den gelen yanıtın uçuş listesini temsil eder
    public class ApiResponse
    {
        public List<Flight>? Flights { get; set; }
    }

    public class Flight
    {
        public string AirlineName { get; set; } // Firma adı
        public string FlightId { get; set; } // Uçuş ID'si
        public DateTime DepartureTime { get; set; } // Kalkış zamanı
        public DateTime ArrivalTime { get; set; } // Varış zamanı
        public string FromAirport { get; set; } // Kalkış noktası
        public string ToAirport { get; set; } // Varış noktası
        public double Price { get; set; } // Bilet ücreti
    }
}
