using HemenBilet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HemenBilet.Services
{
    public class FlightService
    {
        private readonly IMongoCollection<Flight> _flights;

        public FlightService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _flights = database.GetCollection<Flight>("Flights");
        }

        public async Task<List<Flight>> GetFlightsAsync()
        {
            return await _flights.Find(f => true).ToListAsync();
        }

        public async Task CreateFlightAsync(Flight flight)
        {
            await _flights.InsertOneAsync(flight);
        }
    }
}
