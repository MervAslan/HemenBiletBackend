namespace HemenBilet.Models
{
    // ApiResponse sınıfı uçuşları FlightData olarak döndürüyor
    public class ApiResponse
    {
        public List<FlightData>? Flights { get; set; }
    }

    // API'den gelen verilerin türünü DateTime olarak güncelliyoruz
    public class Flight
    {
        public string FlightId { get; set; }

        // DepartureTime ve ArrivalTime artık DateTime türünde
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public decimal Price { get; set; }
    }
}