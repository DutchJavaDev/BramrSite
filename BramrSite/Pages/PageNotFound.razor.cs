using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class PageNotFound : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public string Url { get; set; }
        int count = 10;

        protected override async void OnInitialized()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                count--;
                StateHasChanged();
            }
            
            NavigationManager.NavigateTo("/home");

        }
    }
}
