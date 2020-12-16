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

        public List<object> List { get; set; } = new List<object>();

        public TextModel Test { get; set; } = new TextModel() { Index = 0, Text = "ewa boris", TextColor = "black", Bold = true, Italic = true, Underlined = false, FontSize = 20 };
        public TextModel Test2 { get; set; } = new TextModel() { Index = 1, Text = "ewa mathijs", TextColor = "red", BackgroundColor = "blue", Bold = false, Italic = true, Underlined = true, FontSize = 50 };
        public ImageModel Test3 { get; set; } = new ImageModel() { Index = 2, Src = @"C:\Users\ruben\OneDrive\Bureaublad\temp\websites\Ruben\html\images\ProfielFoto.png", FileUri = "ca0c40c0c305473e9c4b90d8d530a2fc" };

        protected override void OnInitialized()
        {
            List.Add(Test);
            List.Add(Test2);
            List.Add(Test3);
        }

        public async void Save()
        {
            string json = JsonConvert.SerializeObject(List, Formatting.Indented);

            await Api.UploadJson(json);
        }
    }
}
