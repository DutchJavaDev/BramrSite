using BramrSite.Classes;
using BramrSite.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BramrSite.Auth;
using Microsoft.JSInterop;
using System;

namespace BramrSite.Pages
{
    public partial class Home : ComponentBase
    {
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
        [Inject] public IJSRuntime IJSRuntime { get; set; }
        [Parameter] public string ReturnUrl { get; set; }

        public string SignInMessage { get; set; }
        public string SignUpMessage { get; set; }

        private bool ReturnToIndex { get; set; }

        public bool ShowModal { get; set; }
        public bool Disabled { get; set; }

        private IJSObjectReference Module { get; set; }

        protected async override void OnInitialized()
        {
            Module = await IJSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/HomeScript.js");

            await Module.InvokeVoidAsync("updateContainer");

            if (ReturnUrl == "index" || ReturnUrl == "/")
            {
                ReturnToIndex = true;
                ReturnUrl = string.Empty;
            }
        }


        /// <summary>
        /// Beter fix: https://chrissainty.com/fragment-routing-with-blazor/
        /// </summary>
        public async void MeetTheTeam()
        {
            await Module.InvokeVoidAsync("scrollToId", "#meetTheTeam");
        }

        public async void About()
        {
            await Module.InvokeVoidAsync("scrollToId", "#about");
        }

        public async void _Home()
        {
            await Module.InvokeVoidAsync("scrollToId", "#Home");
        }

        public async Task SignUpSubmit()
        {
            Disabled = true;
            StateHasChanged();

            if (Model.IsValidSignUp())
            {
                var apiResult = await Api.UserNameExist(Model.UserName);

                if (!apiResult.Success)
                {
                    SignUpMessage = apiResult.Message;
                    Disabled = false;
                    StateHasChanged();
                    return;
                }

                _ = bool.TryParse(apiResult.GetData<string>("user_exists"), out var nameExists);
                
                if (nameExists)
                {
                    SignUpMessage = "This username is not available";
                    Disabled = false;
                    StateHasChanged();
                    return;
                }
                else
                {
                    SignUpMessage = "User created";
                    Console.WriteLine(apiResult.Message);
                }

                apiResult = await Api.SignUp(Model);

                if (apiResult.Success)
                {
                    Navigation.NavigateTo("/");
                }
                else
                {
                    SignUpMessage = apiResult.Message;
                }
            }

            Disabled = false;
            StateHasChanged();
        }

        public async Task SignInSubmit()
        {
            Disabled = true;
            StateHasChanged();

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
                        Navigation.NavigateTo("/account");
                    }
                    else if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        Navigation.NavigateTo(ReturnUrl, true);
                    }
                    else
                    {
                        Navigation.NavigateTo("/account");
                    }
                }
            }

            Disabled = false;
            StateHasChanged();
        }
    }
}
