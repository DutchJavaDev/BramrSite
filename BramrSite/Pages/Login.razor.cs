using BramrSite.Classes;
using BramrSite.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using BramrSite.Auth;

namespace BramrSite.Pages
{
    public partial class Login : ComponentBase
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

        public bool Disabled { get; set; }

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

                    if (!string.IsNullOrEmpty(ReturnUrl))
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
