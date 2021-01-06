using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class PageNotFound : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public string Url { get; set; }

        protected override async void OnInitialized()
        {
            await Task.Delay(10000);
            NavigationManager.NavigateTo("/home");

        }
    }
}
