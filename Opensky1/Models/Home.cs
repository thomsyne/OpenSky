using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opensky1.Models
{
    public class Home
    {
        public string Icao24 { get; set; }

        public int? FirstSeen { get; set; }

        public string EstDeparture { get; set; }

        public string LastSeen { get; set; }

        public string EstArrivalAirport { get; set; }

        public string CallSign { get; set; }

        public int? EstDepartureAirportHorizDistance { get; set; }

        public int? EstDepartureAirportVertDistance { get; set; }

        public int? EstArrivalAirportHorizDistance { get; set; }

        public int? EstArrivalAirportVertDistance { get; set; }

        public int? DepartureAirportCandidateCount { get; set; }

        public int? ArrivalAirportCandidateCount { get; set; }

    }
}