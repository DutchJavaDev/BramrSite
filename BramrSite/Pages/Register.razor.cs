using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class Register : ComponentBase
    {
        public readonly SignupModel Model = new SignupModel();

        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public ApiService Api { get; set; }

        public string Message { get; set; }

        public bool Disabled { get; set; }

        public async Task OnSubmit()
        {
            Disabled = true;

            if (Model.IsValid())
            {
                var response = await Api.SignUp(Model);

                // Debug
                Message = response.ToString();

                if (response.Success)
                    Navigation.NavigateTo("login", true);
            }
            Disabled = false;
        }
    }
}
