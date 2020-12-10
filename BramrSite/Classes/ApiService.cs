using BramrSite.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace BramrSite.Classes
{
    public class ApiService
    {
#if DEBUG
            private readonly string BaseUrl = "https://localhost:44372/api/";
#else
            private readonly string BaseUrl = "https://bramr.tech/api/";
#endif
        private string JWT { get; set; }

        public void SetToken(string token)
        {
            JWT = token;
        }

        public async Task<ApiResponse> SignUp(SignupModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("signup", content);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
                else
                    return new ApiResponse { Message = response.ReasonPhrase };
            }
            catch (Exception e)
            {
                return new ApiResponse { Success = false, Message = e.Message };
            }
        }

        public async Task<ApiResponse> SignIn(SigninModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("signin", content);

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
                else
                    return new ApiResponse { Message = response.ReasonPhrase };
            }
            catch (Exception e)
            {
                return new ApiResponse { Success = false, Message = e.Message };
            }
        }

        public async Task<ApiResponse> UploadImage(Stream FileStream, string FileName)
        {
            var content = new MultipartFormDataContent();
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(FileStream, (int)FileStream.Length), "image", FileName);

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("image/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
                }   
                else
                {
                    return new ApiResponse { Message = response.ReasonPhrase };
                }   
            }
            catch (Exception e)
            {
                return new ApiResponse { Success = false, Message = e.Message };
            }
        }

        public async Task<string> DonwloadImage()
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync("image/download");

                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
            catch
            {
                return null;
            }
        }


        private HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/png"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jpg"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jpeg"));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

            if (!string.IsNullOrEmpty(JWT))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWT);
            }

            return client;
        }
    }
}
