using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Opensky1.Models;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System;

namespace Opensky1.Controllers

{
    public class TrustAllCertificatePolicy : ICertificatePolicy
    {

        public TrustAllCertificatePolicy() { }

        public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest req, int problem)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return true;
        }
    }




    [Authorize]
    public class HomeController : Controller
    {

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<object> GetInfo(string airport, string begin, string end)
        {
            string arrivalResponse = string.Empty;
            string departureResponse = string.Empty;
            HttpClient client = new HttpClient
            {
                BaseAddress = new System.Uri("https://thomsyne:ayokunle@opensky-network.org/api/flights/")
            };

           
            Task departureTask = Task.Run(async () =>
            {
                try
                {
                    var departure = await client.GetAsync($"departure?airport={airport}&begin={begin}&end={end}");

                    if (departure.IsSuccessStatusCode)
                    {
                        departureResponse = await departure.Content.ReadAsStringAsync();
                    }
                }
                catch(Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Departure Exception >>>>>>>> {ex.Message}");
#endif
                }
            });

            Task arrivalTask = Task.Run(async () =>
            {
                HttpResponseMessage arrival = null;
                try
                {
                    arrival = await client.GetAsync($"arrival?airport={airport}&begin={begin}&end={end}");
                }
                catch(Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Arrival Exception >>>>>>>> {ex.Message}");
#endif
                }
                
                if (arrival.IsSuccessStatusCode)
                {
                    arrivalResponse = await arrival.Content.ReadAsStringAsync();
                }
            });

            await Task.WhenAll(arrivalTask, departureTask);
            var data = Serialize(new { arrivalResponse, departureResponse });
            return data;
        }

        public string Serialize<T>(T data)
        {
            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });
            return result;
        }
        public static T Deserialize<T>(string jsonData)
        {

            var result = JsonConvert.DeserializeObject<T>(jsonData, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });
            return result;
        }


    }
}