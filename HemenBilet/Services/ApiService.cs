using HemenBilet.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace HemenBilet.Services
{
    public class ApiService
    {
        private readonly FlightService _flightService;

        public ApiService(FlightService flightService)
        {
            _flightService = flightService;
        }

        public async Task FetchAndSaveAllFlightData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "51339b2856msh90f7c2da6a88527p18a60djsnb702ca5efe9b");
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "booking-com15.p.rapidapi.com");

                var locations = new List<(string from, string to)>
                {
                    ("BOM.AIRPORT", "DEL.AIRPORT")
                };

                foreach (var loc in locations)
                {
                    var queryParams = new Dictionary<string, string?>
                    {
                        { "fromId", loc.from },
                        { "toId", loc.to },
                        { "departDate", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") }
                    };

                    var uri = new Uri(QueryHelpers.AddQueryString("https://booking-com15.p.rapidapi.com/api/v1/flights/searchFlights", queryParams));

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(uri);

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("API Yanıtı: " + jsonResponse);

                            var flightData = ParseFlightData(jsonResponse);

                            if (flightData != null)
                            {
                                await _flightService.CreateFlightAsync(flightData);
                                Console.WriteLine("Veri MongoDB'ye kaydedildi.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"API çağrısı başarısız: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Hata: " + ex.Message);
                    }
                }
            }
        }

        private FlightData ParseFlightData(string jsonResponse)
        {
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

            if (apiResponse?.Flights == null || !apiResponse.Flights.Any())
            {
                Console.WriteLine("Uçuş verisi bulunamadı.");
                return null;
            }

            var flight = apiResponse.Flights.First(); // İlk uçuşu al

            return new FlightData
            {
                FlightId = flight.FlightId,
                Airline = flight.AirlineName,
                FromAirport = flight.FromAirport,
                ToAirport = flight.ToAirport,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Price = (double)flight.Price // Decimal -> Double dönüşümü
            };
        }

    }
}
