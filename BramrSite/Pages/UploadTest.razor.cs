using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class UploadTest : ComponentBase
    {
        [Inject] ApiService Api { get; set; }

        public List<TextModel> List { get; set; } = new List<TextModel>();

        public TextModel Test { get; set; } = new TextModel() { ID = 0, Text = "ewa boris", TextColor = "black", Bold = true, Italic = true, Underlined = false, FontSize = 20 };

        protected override void OnInitialized()
        {
            List.Add(Test);
        }

        public async void Save()
        {
            string list = JsonConvert.SerializeObject(List, Formatting.Indented);

            await Api.UploadJson(list);
        }
    }
}
