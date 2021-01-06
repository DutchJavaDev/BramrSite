using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BramrSite.Pages
{
    public partial class PageNotFound : ComponentBase
    {
        [Inject] IJSRuntime IJSRuntime { get; set; }

        [Parameter] public string Url { get; set; }

        protected override async void OnInitialized()
        {
            var module = await IJSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/PageNotFoundScript.js");

            await module.InvokeVoidAsync("startCountDown", "/home");
        }
    }
}
