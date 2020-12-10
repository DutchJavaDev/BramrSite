using BramrSite.Classes;
using BramrSite.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using BramrSite.Auth;

namespace BramrSite.Pages
{
    public partial class Home : ComponentBase
    {
        public readonly SigninModel Model = new SigninModel()
#if DEBUG
        {
            Email = "admin@bramr.tech",
            Password = "XtS8tT~w"
        }
#endif
        ;
        [Inject] public JWTAuthentication Auth { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public ApiService Api { get; set; }
        [Parameter] public string ReturnUrl { get; set; }

        public string Message { get; set; }

        private bool ReturnToIndex { get; set; }

        public bool Disabled { get; set; }

        protected override void OnInitialized()
        {
            if (ReturnUrl == "index" || ReturnUrl == "/")
                ReturnToIndex = true;
        }

        public async Task OnSubmit()
        {
            Disabled = true;

            if (Model.IsValid())
            {
                var response = await Api.SignIn(Model);

                // Debug
                Message = response.ToString();

                if (response.Success)
                {
                    // Set JWt token
                    await Auth.UpdateAutenticationState(response.RequestedData.ToString());

                    if (ReturnToIndex)
                    {
                        Navigation.NavigateTo("/", true);
                    }
                    else if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        Navigation.NavigateTo(ReturnUrl, true);
                    }
                    else
                    {
                        Navigation.NavigateTo("/", true);
                    }
                }
            }

            Disabled = false;
        }
    }
}
