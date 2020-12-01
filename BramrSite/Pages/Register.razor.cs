using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class Register : ComponentBase
    {
        string response;
        string Email { get; set; }
        string Password { get; set; }
        bool Disabled { get; set; }
        private async Task btnPost_Click()
        {
            Disabled = true;
            RegisterModel register = new RegisterModel() { Email = Email, Password = Password };
            string someJson = JsonConvert.SerializeObject(register);
            RegisterProcessor p = new RegisterProcessor();
            var ruben = await p.CreateUser(someJson);
            response = ruben.Message;
            Disabled = false;

        }

    }
}
