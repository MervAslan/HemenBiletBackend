using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using HemenBilet.Data;
using HemenBilet.Models;

namespace HemenBilet.Services
{
    public class ApiService
    {
        private static readonly string apiKey = "51339b2856msh90f7c2da6a88527p18a60djsnb702ca5efe9b";
        private static readonly string apiHost = "booking-com15.p.rapidapi.com";
        private static readonly string url = "https://booking-com15.p.rapidapi.com/api/v1/flights/searchFlights";

        private readonly MongoDbContext _dbContext;

        public ApiService(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task FetchAndSaveAllFlightData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", apiHost);

                // Konum çiftlerini BOM ve DEL olarak değiştir
                var locations = new List<(string from, string to)>
                {
                    ("BOM.AIRPORT", "DEL.AIRPORT")
                };

                string nextDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                foreach (var loc in locations)
                {

                    var queryParams = new Dictionary<string, string?>
                    {
                        { "fromId", "BOM.AIRPORT" },
                        { "toId", "DEL.AIRPORT" },
                        { "departDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") },
                        { "pageNo", "1" },
                        { "adults", "1" },
                        { "children", "0,17" },
                        { "sort", "BEST" },
                        { "cabinClass", "ECONOMY" },
                        { "currency_code", "AED" }
                    };


                    var uri = new Uri(QueryHelpers.AddQueryString(url, queryParams));

                    HttpResponseMessage response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(jsonResponse);  // Yanıtı konsola yazdır

                        try
                        {
                            var flightData = ParseFlightData(jsonResponse, loc);

                            if (flightData != null)
                            {
                                await SaveFlightDataToMongoDb(flightData);
                                Console.WriteLine($"Veri MongoDB'ye kaydedildi: {loc.from} - {loc.to}");
                            }
                            else
                            {
                                Console.WriteLine("Geçerli uçuş verisi bulunamadı, MongoDB'ye kaydedilmedi.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Veri işleme hatası: {ex.Message}");
                        }
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Hata mesajı: {errorResponse}");
                        Console.WriteLine($"{loc.from} ile {loc.to} arasında veri çekme hatası: {response.StatusCode}");
                    }
                }
            }
        }

        private FlightData ParseFlightData(string jsonResponse, (string from, string to) loc)
        {
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            if (apiResponse?.Flights == null || apiResponse.Flights.Count == 0)
            {
                Console.WriteLine("Yanıtta uçuş verisi bulunamadı. Yanıt formatını veya parametreleri kontrol edin.");
                return null;  // Uçuş verisi bulunmadığı durumda null döndür
            }

            var flight = apiResponse.Flights.FirstOrDefault();

            if (flight == null)
            {
                throw new Exception("Uçuş verisi bulunamadı.");
            }

            // API'den gelen DepartureTime ve ArrivalTime zaten DateTime olduğundan direkt kullanıyoruz
            var flightData = new FlightData
            {
                FlightId = flight.FlightId,
                DepartureTime = flight.DepartureTime,  // Direkt kullanıyoruz
                ArrivalTime = flight.ArrivalTime,      // Direkt kullanıyoruz
                FromAirport = loc.from,
                ToAirport = loc.to,
                Price = (double)flight.Price  // Fiyatı double'a dönüştürüyoruz
            };

            return flightData;
        }


        private async Task SaveFlightDataToMongoDb(FlightData flightData)
        {
            Console.WriteLine("SaveFlightDataToMongoDb fonksiyonu çağrıldı.");

            var collection = _dbContext.Flights;

            if (collection == null)
            {
                Console.WriteLine("Koleksiyon bulunamadı.");
                return;
            }

            await collection.InsertOneAsync(flightData); // Veriyi MongoDB'ye ekler

            Console.WriteLine("Veri MongoDB'ye kaydedildi");
        }
    }
}
