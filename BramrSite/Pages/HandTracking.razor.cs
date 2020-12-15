using BramrSite.Auth;
using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class HandTracking : ComponentBase
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
            [Parameter] public string ReturnUrl { get; set; }

            public string SignInMessage { get; set; }
            public string SignUpMessage { get; set; }

            private bool ReturnToIndex { get; set; }

            public bool Disabled { get; set; }

            protected override void OnInitialized()
            {
                if (ReturnUrl == "index" || ReturnUrl == "/")
                {
                    ReturnToIndex = true;
                    ReturnUrl = string.Empty;
                }
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
