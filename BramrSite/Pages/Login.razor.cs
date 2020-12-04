using BramrSite.Classes;
using BramrSite.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BramrSite.Pages
{
    public partial class Login : ComponentBase
    {
        public readonly SignInModel Model = new SignInModel() {
            Email = "admin@bramr.tech",
            Password = "XtS8tT~w"
        };

        public string Message { get; set; }

        public bool Disabled { get; set; }

        public async Task OnSubmit()
        {
            Disabled = true;

            var processor = new SignInProcessor();

            if (Model.IsValid())
            {
                var response = await processor.SignIn(Model);

                // Debug
                Message = response.ToString();
            }

            Disabled = false;
        }
    }
}
