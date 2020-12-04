using System;
using System.Net.Http.Headers;
using System.Net.Http;

namespace BramrSite.Classes
{
    public class ApiDesign
    {
        public static HttpClient Client { get; set; } = new HttpClient();

        public static void IntitClient()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44381/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
