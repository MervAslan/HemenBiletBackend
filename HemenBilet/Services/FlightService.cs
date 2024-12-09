using HemenBilet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HemenBilet.Services
{
    public class FlightService
    {
        private readonly IMongoCollection<FlightData> _flights;

        public FlightService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _flights = database.GetCollection<FlightData>(settings.Value.CollectionName);  // CollectionName parametresi kullanılıyor
        }


        public async Task<List<FlightData>> GetFlightsAsync()
        {
            return await _flights.Find(f => true).ToListAsync();
        }

        public async Task CreateFlightAsync(FlightData flight)
        {
            try
            {
                await _flights.InsertOneAsync(flight);
                Console.WriteLine("Uçuş başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }


        public async Task CreateTestFlightAsync()
        {
            var flight = new FlightData
            {
                FlightId = "TestFlight123",
                DepartureTime = DateTime.Now.AddHours(1),
                ArrivalTime = DateTime.Now.AddHours(3),
                FromAirport = "IST",
                ToAirport = "ANK",
                Price = 350
            };

            await CreateFlightAsync(flight);
            Console.WriteLine("Test uçuşu başarıyla kaydedildi.");
        }
    }

}
