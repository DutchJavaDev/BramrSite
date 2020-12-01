using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BramrSite.Classes
{
    public class ApiHelper
    {
        public static HttpClient client { get; set; } = new HttpClient();

        public static void IntitClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44362");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}
