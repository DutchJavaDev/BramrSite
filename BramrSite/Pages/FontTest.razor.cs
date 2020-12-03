using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class FontTest : ComponentBase
    {
        public string FontType { get; set; } = "Montserrat";
        public string FontName { get; set; } = "sans-serif";

        FontModel Monstserrat = new FontModel("Montserrat", "sans-serif");
        FontModel OpenSansCondensed = new FontModel("Open Sans Condensed", "sans-serif");
        FontModel Rubik = new FontModel("Rubik", "sans-serif");
        FontModel Quicksand = new FontModel("Quicksand", "sans-serif");
        FontModel Teko = new FontModel("Teko", "sans-serif");
        FontModel Exo2 = new FontModel("Exo 2", "sans-serif");
        FontModel MavenPro = new FontModel("Maven Pro", "sans-serif");
        FontModel Sarabun = new FontModel("Sarabun", "sans-serif");
        FontModel OldStandardTT = new FontModel("Old Standard TT", "sans-serif");
        FontModel Marvel = new FontModel("Marvel", "sans-serif");

        protected override void OnInitialized()
        {
            StateHasChanged();
        }

        void SelectGet(ChangeEventArgs e)
        {
            var value = e.Value.ToString();

            if (value == "Monstserrat")
            {
                FontType = Monstserrat.FontType;
                FontName = Monstserrat.FontName;
                StateHasChanged();
            }
            if (value == "Open Sans Condensed")
            {
                FontType = OpenSansCondensed.FontType;
                FontName = OpenSansCondensed.FontName;
                StateHasChanged();
            }
            if (value == "Rubik")
            {
                FontType = Rubik.FontType;
                FontName = Rubik.FontName;
                StateHasChanged();
            }
            if (value == "Quicksand")
            {
                FontType = Quicksand.FontType;
                FontName = Quicksand.FontName;
                StateHasChanged();
            }
            if (value == "Teko")
            {
                FontType = Teko.FontType;
                FontName = Teko.FontName;
                StateHasChanged();
            }
            if (value == "Exo 2")
            {
                FontType = Exo2.FontType;
                FontName = Exo2.FontName;
                StateHasChanged();
            }
            if (value == "Maven Pro")
            {
                FontType = MavenPro.FontType;
                FontName = MavenPro.FontName;
                StateHasChanged();
            }
            if (value == "Sarabun")
            {
                FontType = Sarabun.FontType;
                FontName = Sarabun.FontName;
                StateHasChanged();
            }
            if (value == "Old Standard TT")
            {
                FontType = OldStandardTT.FontType;
                FontName = OldStandardTT.FontName;
                StateHasChanged();
            }
            if (value == "Marvel")
            {
                FontType = Marvel.FontType;
                FontName = Marvel.FontName;
                StateHasChanged();
            }
            //if else (value == "Roboto")
            //{

            //}
        }
    }

}
