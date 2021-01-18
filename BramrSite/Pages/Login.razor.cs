using BramrSite.Classes;
using BramrSite.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using BramrSite.Auth;
using Microsoft.JSInterop;

namespace BramrSite.Pages
{
    public partial class Login : ComponentBase
    {
        //[Inject] IJSRuntime IJSRuntime { get; set; }
        
        public readonly User Model = new User()
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

        public string SignInMessage { get; set; }
        public string SignUpMessage { get; set; }

        private bool ReturnToIndex { get; set; }

        public bool Disabled { get; set; }

        protected async override void OnInitialized()
        {
            if (await Auth.HasToken())
            {
                Navigation.NavigateTo("/account", true);
            }

            if (ReturnUrl == "index" || ReturnUrl == "/")
            {
                ReturnToIndex = true;
                ReturnUrl = string.Empty;
            }
            // eventuele delay
            //await Task.Delay(5000);
            //var module = await IJSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/Imageclassification.js");

            //await module.InvokeVoidAsync("setup");
            //await module.InvokeVoidAsync("preload");
        }
        
        public async Task SignInSubmit()
        {
            Disabled = true;

            if (Model.IsValidSignIn())
            {
                var apiResult = await Api.SignIn(Model);

                SignInMessage = apiResult.Message;

                if (apiResult.Success)
                {
                    //Set JWt token
                    await Auth.UpdateAutenticationState(apiResult.GetData<string>("jwt_token"));

                    if (ReturnToIndex)
                    {
                        Navigation.NavigateTo("/");
                    }
                    else if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        Navigation.NavigateTo(ReturnUrl, true);
                    }
                    else
                    {
                        Navigation.NavigateTo("/");
                    }
                }
            }

            Disabled = false;
        }
    }
}