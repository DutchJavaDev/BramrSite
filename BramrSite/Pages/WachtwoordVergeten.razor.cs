using BramrSite.Classes;
using BramrSite.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using BramrSite.Auth;
using Microsoft.JSInterop;

namespace BramrSite.Pages
{
    public partial class WachtwoordVergeten : ComponentBase
    {
        [Inject] ApiService Api { get; set; }
        bool Disabled { get; set; }
        string Message { get; set; }
        string email = "";
        public async void klik() {

            if(email != string.Empty)
            {
                Message = "";
                StateHasChanged();
                Disabled = true;
                var response = await Api.ForgotPassword(email);
                if (response.Success)
                {
                    Message = ("If this email is linked to a Bramr Account you will receive an email from us with further instructions");
                    StateHasChanged();
                }
                else
                {
                    Message = "Something went wrong please try again.";
                    Disabled = false;
                    StateHasChanged();
                }
            }
            else
            {
                Message = "Please fill in your email";
                Disabled = false;
                StateHasChanged();
            }
           
        }
    }
}
