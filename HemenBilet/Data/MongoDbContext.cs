using HemenBilet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HemenBilet.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var settings = mongoDbSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoDatabase Database => _database;

        public IMongoCollection<FlightData> Flights => _database.GetCollection<FlightData>("Flights");
    }
}
