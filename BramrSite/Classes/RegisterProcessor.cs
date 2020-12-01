using BramrSite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BramrSite.Classes
{
    public class RegisterProcessor
    {
        public async Task<ApiResponseModel> CreateUser(string someJson)
        {

            var content = new StringContent(someJson, Encoding.UTF8, "application/json");
#if DEBUG
            string url = "http://localhost:54540/api/signup/account";
#else
            string url = "https://bramr.tech/api/signup/account";
#endif
            using (HttpResponseMessage response = await ApiHelper.client.PostAsync(url, content))
            {
                if (response.IsSuccessStatusCode)
                {

                    ApiResponseModel apiresponse = JsonConvert.DeserializeObject<ApiResponseModel>(await response.Content.ReadAsStringAsync());
                    return apiresponse;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
