using BramrSite.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BramrSite.Classes
{
    public class SignInProcessor
    {
        public async Task<ApiResponse> SignIn(SignInModel model)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
#if DEBUG
                string url = "http://localhost:54540/api/signin/account";
#else
            string url = "https://bramr.tech/api/signin/account";
#endif
                using (HttpResponseMessage response = await ApiHelper.client.PostAsync(url, content))
                {

                    ApiResponse apiresponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
                    return apiresponse;
                }
            }
            catch (Exception e)
            {
                return ApiResponse.Error(e.ToString());
            }
        }
    }
}
