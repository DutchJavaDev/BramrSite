using BramrSite.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

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

        public async Task<ApiResponse> UserNameExist(string name)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync($"signup/username/exists/{name}");

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
                else
                    return new ApiResponse { Message = response.ReasonPhrase };
            }
            catch (Exception e)
            {
                return ApiResponse.Error(message: e.Message);
            }
        }

        public async Task<ApiResponse> SignUp(User model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { model.Email, model.UserName }), Encoding.UTF8, "application/json");

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

        public async Task<bool> IsValidToken()
        {
            try
            {
                using var client = CreateClient();
                var result = await client.GetAsync("signin/verify/jwt");

                return bool.Parse(await result.Content.ReadAsStringAsync());
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApiResponse> SignIn(User model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { model.Email, model.Password}), Encoding.UTF8, "application/json");

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

        public async Task<ApiResponse> UploadImage(Stream FileStream, string FileName, bool IsCV)
        {
            var content = new MultipartFormDataContent();
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(FileStream, (int)FileStream.Length), "image", FileName);

            try
            {
                using var client = CreateClient();
                HttpResponseMessage response;
                if (IsCV)
                {
                    response = await client.PostAsync("image/upload/cv", content);
                }
                else
                {
                    response = await client.PostAsync("image/upload/portfolio", content);
                }


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

        public async Task<string> GetFileInfo(string FileName)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync($"image/info/{FileName}");
                var result =  JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<ChangeModel> GetOneFromHistory(int Location)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync($"edithistory/get/{Location}");
                var result = JsonConvert.DeserializeObject<ChangeModel>(await response.Content.ReadAsStringAsync());
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new ChangeModel();
            }
        }

        public async Task<ApiResponse> AddToHistory(int Location, ChangeModel CurrentChange)
        {
            HistoryModel edit = new HistoryModel() { Location = Location, DesignElement = CurrentChange.DesignElement, EditType = CurrentChange.EditType.ToString(), Edit = CurrentChange.Edit };

            var content = new StringContent(JsonConvert.SerializeObject(edit), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("edithistory/post", content);

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

        public async Task<ApiResponse> DeleteAllFromHistory()
        {
            try
            {
                using var client = CreateClient();
                var response = await client.DeleteAsync("edithistory/delete");

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

        public async Task<ApiResponse> DeleteAmountFromHistory(int Location)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.DeleteAsync($"edithistory/deletefrom/{Location}");

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

        public async Task<ApiResponse> UploadCV(string List)
        {
            var content = new StringContent(JsonConvert.SerializeObject(List), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("website/uploadcv", content);

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

        public async Task<ApiResponse> UploadPortfolio(string List)
        {
            var content = new StringContent(JsonConvert.SerializeObject(List), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("website/uploadportfolio", content);

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

        public async Task<List<object>> GetDesignElements()
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync($"website/get");
                var result = JsonConvert.DeserializeObject<List<object>>(await response.Content.ReadAsStringAsync());
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<object>();
            }
        }

        public async Task<ApiResponse> ChangePassword(string List)
        {
            var content = new StringContent(JsonConvert.SerializeObject(List), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("password/change", content);

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
        public async Task<ApiResponse> ForgotPassword(string email)
        {
            var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("password/forgot", content);

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
        public async Task<ApiResponse> ResetPassword(ResetPasswordModel model)
        {
            var i = JsonConvert.SerializeObject(model);
            Console.WriteLine(i);
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            try
            {
                using var client = CreateClient();
                var response = await client.PostAsync("password/reset", content);

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

            if (!string.IsNullOrEmpty(JWT))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWT);
            }

            return client;
        }
    }
}
