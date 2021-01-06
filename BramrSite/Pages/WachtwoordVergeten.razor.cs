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
        string email;
        public async void klik() {

            Disabled = true;
            var response =  await Api.ForgotPassword(email);
            if (response.Success)
            {
                Message = "Als dit email bij ons bekend is zal u een email van ons ontvangen.";
                Console.WriteLine("Als dit email bij ons bekend is zal u een email van ons ontvangen.");
                StateHasChanged();
            }
            else
            {
                Console.WriteLine("Er is iets misgegaan probeer het opnieuw");
                Message = "Er is iets misgegaan probeer het opnieuw";
                Disabled = false;
                StateHasChanged();
            }
        }
    }
}
