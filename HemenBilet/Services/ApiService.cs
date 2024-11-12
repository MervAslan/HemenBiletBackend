// using System;
// using System.Collections.Generic;
// using System.Net.Http;
// using System.Threading.Tasks;
// using MongoDB.Driver;
// using Microsoft.AspNetCore.WebUtilities;
// using Newtonsoft.Json;
// using HemenBilet.Data;
// using HemenBilet.Models;

// namespace HemenBilet.Services
// {
//     public class ApiService
//     {
//         private static readonly string apiKey = "75124b138bmshe3abacbc5483bbcp1b1212jsnd3d2b6f8f117";
//         private static readonly string apiHost = "booking-com15.p.rapidapi.com";
//         private static readonly string url = "https://booking-com15.p.rapidapi.com/api/v1/flights/searchFlights";

//         private readonly MongoDbContext _dbContext;

//         public ApiService(MongoDbContext dbContext)
//         {
//             _dbContext = dbContext;
//         }

//         public async Task FetchFlightData()
//         {
//             using (HttpClient client = new HttpClient())
//             {
//                 client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
//                 client.DefaultRequestHeaders.Add("X-RapidAPI-Host", apiHost);

//                 // Konum çiftlerini tanımla
//                 var locations = new List<(string from, string to)>
//                 {
//                     ("IST.AIRPORT", "JFK.AIRPORT"),
//                     ("LHR.AIRPORT", "DXB.AIRPORT"),
//                     ("BOM.AIRPORT", "DEL.AIRPORT")
//                     // Diğer kalkış-varış çiftlerini buraya ekleyin
//                 };

//                 string nextDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

//                 foreach (var loc in locations)
//                 {
//                     var queryParams = new Dictionary<string, string>
//                     {
//                         { "fromId", loc.from },
//                         { "toId", loc.to },
//                         { "pageNo", "1" },
//                         { "adults", "1" },
//                         { "children", "0,17" },
//                         { "sort", "BEST" },
//                         { "cabinClass", "ECONOMY" },
//                         { "currency_code", "AED" },
//                         { "departDate", nextDay }
//                     };

//                     var uri = new Uri(QueryHelpers.AddQueryString(url, queryParams));

//                     HttpResponseMessage response = await client.GetAsync(uri);

//                     if (response.IsSuccessStatusCode)
//                     {
//                         string jsonResponse = await response.Content.ReadAsStringAsync();

//                         // API'den gelen JSON'u FlightData'ya dönüştürme
//                         var flightData = ParseFlightData(jsonResponse, loc);

//                         // MongoDB'ye kaydet
//                         await _dbContext.Flights.InsertOneAsync(flightData);

//                         Console.WriteLine($"Veri MongoDB'ye kaydedildi: {loc.from} - {loc.to}");
//                     }
//                     else
//                     {
//                         Console.WriteLine($"{loc.from} ile {loc.to} arasında veri çekme hatası: {response.StatusCode}");
//                     }
//                 }
//             }
//         }

//         private FlightData ParseFlightData(string jsonResponse, (string from, string to) loc)
//         {
//             // API yanıtını deserialize et
//             var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);  // Use fully qualified name

//             // Flight verilerini al
//             var flight = apiResponse.Flights.FirstOrDefault(); // İlk uçuşu alıyoruz

//             if (flight == null)
//             {
//                 throw new Exception("Uçuş verisi bulunamadı.");
//             }

//             // FlightData modelini doldur
//             var flightData = new FlightData
//             {
//                 FlightId = flight.FlightId, // API'den gelen FlightId
//                                             // API'den gelen DepartureTime ve ArrivalTime string türünde, ISO 8601 formatında
//                 DepartureTime = DateTime.ParseExact(flight.DepartureTime.ToString(), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), // DepartureTime'ı ISO formatında dönüştürme
//                 ArrivalTime = DateTime.ParseExact(flight.ArrivalTime.ToString(), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), // ArrivalTime'ı ISO formatında dönüştürme
//                 FromAirport = loc.from, // Giriş parametresinden gelen veriler
//                 ToAirport = loc.to, // Giriş parametresinden gelen veriler
//                 Price = flight.Price // API'den gelen Price
//             };



//             return flightData;
//         }
//         public async Task FetchAndSaveAllFlightData()
//         {
//             // Tüm uçuş verilerini API’den çek
//             var allFlightData = await apiClient.GetAllFlightDataAsync(); // GetAllFlightDataAsync, API’den tüm verileri çeken metodunuz olsun

//             // Tüm verileri veritabanına kaydet
//             foreach (var flightData in allFlightData)
//             {
//                 await SaveFlightDataToMongoDb(flightData);
//             }
//         }


//         public async Task SaveFlightDataToMongoDb(FlightData flightData)
//         {
//             Console.WriteLine("SaveFlightDataToMongoDb fonksiyonu çağrıldı.");

//             var collection = _dbContext.Flights; // Flights koleksiyonuna doğrudan erişim

//             if (collection == null)
//             {
//                 Console.WriteLine("Koleksiyon bulunamadı.");
//             }

//             await collection.InsertOneAsync(flightData); // Veriyi MongoDB'ye ekler

//             Console.WriteLine("Veri MongoDB'ye kaydedildi");
//         }

//     }
// }
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
        private static readonly string apiKey = "75124b138bmshe3abacbc5483bbcp1b1212jsnd3d2b6f8f117";
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

                // Konum çiftlerini tanımla
                var locations = new List<(string from, string to)>
                {
                    ("IST.AIRPORT", "JFK.AIRPORT"),
                    ("LHR.AIRPORT", "DXB.AIRPORT"),
                    ("BOM.AIRPORT", "DEL.AIRPORT")
                    // Diğer kalkış-varış çiftlerini buraya ekleyin
                };

                string nextDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                foreach (var loc in locations)
                {
                    await Task.Delay(2000);
                    var queryParams = new Dictionary<string, string>
                    {
                        { "fromId", loc.from },
                        { "toId", loc.to },
                        { "pageNo", "1" },
                        { "adults", "1" },
                        { "children", "0,17" },
                        { "sort", "BEST" },
                        { "cabinClass", "ECONOMY" },
                        { "currency_code", "AED" },
                        { "departDate", nextDay }
                    };

                    var uri = new Uri(QueryHelpers.AddQueryString(url, queryParams));

                    HttpResponseMessage response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        try
                        {
                            // API'den gelen JSON'u FlightData'ya dönüştürme
                            var flightData = ParseFlightData(jsonResponse, loc);

                            // MongoDB'ye kaydet
                            await SaveFlightDataToMongoDb(flightData);

                            Console.WriteLine($"Veri MongoDB'ye kaydedildi: {loc.from} - {loc.to}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Veri işleme hatası: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{loc.from} ile {loc.to} arasında veri çekme hatası: {response.StatusCode}");
                    }
                }
            }
        }

        private FlightData ParseFlightData(string jsonResponse, (string from, string to) loc)
        {
            // API yanıtını deserialize et
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            // İlk uçuş verisini alıyoruz
            var flight = apiResponse?.Flights?.FirstOrDefault();

            if (flight == null)
            {
                throw new Exception("Uçuş verisi bulunamadı.");
            }

            // FlightData modelini doldur
            var flightData = new FlightData
            {
                FlightId = flight.FlightId,
                DepartureTime = DateTime.ParseExact(flight.DepartureTime.ToString(), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), // DepartureTime'ı ISO formatında dönüştürme
                ArrivalTime = DateTime.ParseExact(flight.ArrivalTime.ToString(), "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), // ArrivalTime'ı ISO formatında dönüştürme
                FromAirport = loc.from,
                ToAirport = loc.to,
                Price = flight.Price
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
