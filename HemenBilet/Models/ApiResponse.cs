using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HemenBilet.Models
{

        public class ApiResponse
        {
            public List<FlightData> Flights { get; set; }
        }

        public class Flight
        {
            public string FlightId { get; set; }
            public string DepartureTime { get; set; }
            public string ArrivalTime { get; set; }
            public decimal Price { get; set; }
        }

    
}