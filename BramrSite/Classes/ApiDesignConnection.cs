using BramrSite.Classes.Interfaces;
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
    public class ApiDesignConnection : IApiDesignConnection
    {
        string url = string.Empty;
#if DEBUG
        const string baseUrl = "https://localhost:44381/api/design";
#else
        const string baseUrl = "https://localhost:44381/api/design";
#endif

        public async Task<ChangeModel> GetOneFromDB(int ID)
        {
            ChangeModel APIResponse;

            url = $"{baseUrl}/get/{ID}";

            using (HttpResponseMessage response = await ApiDesign.Client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {

                    APIResponse = JsonConvert.DeserializeObject<ChangeModel>(await response.Content.ReadAsStringAsync());
                    if (APIResponse.EditType == ChangeModel.Type.TextColor || APIResponse.EditType == ChangeModel.Type.BackgroundColor)
                    {
                        APIResponse.Edit = "#" + APIResponse.Edit;
                    }
                    return APIResponse;
                }
                else
                {
                    Console.WriteLine(response.ReasonPhrase);
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public async Task AddToDB(int ID, ChangeModel CurrentChange)
        {
            HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            CurrentChange.Edit = CurrentChange.Edit.Replace("#", string.Empty);

            url = $"{baseUrl}/post/{ID}/{CurrentChange.DesignElement}/{CurrentChange.EditType}/{CurrentChange.Edit}";

            using (HttpResponseMessage response = await ApiDesign.Client.PostAsync(url, content))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }
        public async Task DeleteAllFromDB()
        {
            url = $"{baseUrl}/delete";

            using (HttpResponseMessage response = await ApiDesign.Client.DeleteAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }
        public async Task DeleteAmountFromDB(int ID)
        {
            url = $"{baseUrl}/deletefrom/{ID}";

            using (HttpResponseMessage response = await ApiDesign.Client.DeleteAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }
    }
}
