using BramrSite.Classes;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Pages
{
    public partial class ChangePassword : ComponentBase
    {
        [Inject] ApiService Api { get; set; }

        private string CurrentPassword { get; set; }
        private string NewPassword { get; set; }
        private string RepeatNewPassword { get; set; }
        private string ErrorMessage { get; set; }
        private bool Disabled { get; set; }

        private char[] LowerCases { get; set; } = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private char[] UpperCases { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private char[] Numbers { get; set; } = "1234567890".ToCharArray();
        private char[] StrangeCharacters { get; set; } = "!@#$%^&*()-=_+<>/?,.:;[]{}|".ToCharArray();

        private List<char[]> AllCharacters { get; set; } = new List<char[]>();

        protected override void OnInitialized()
        {
            AllCharacters.Add(LowerCases);
            AllCharacters.Add(UpperCases);
            AllCharacters.Add(Numbers);
            AllCharacters.Add(StrangeCharacters);
        }

        private void CheckInput()
        {
            if(string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(RepeatNewPassword))
            {
                ErrorMessage = "Not all field are filled in.";
                Disabled = true;
            }
            else if (NewPassword.Length < 8)
            {
                ErrorMessage = "Password isn't long enough.";
                Disabled = true;
            }
            else if(NewPassword != RepeatNewPassword)
            {
                ErrorMessage = "Passwords don't match.";
                Disabled = true;
            }
            else
            {
                foreach (var item in AllCharacters)
                {
                    bool contains = false;
                    foreach (var character in item)
                    {
                        if (NewPassword.Contains(character))
                        {
                            contains = true;
                            break;
                        }
                        else
                        {
                            contains = false;
                        }
                    }
                    if (!contains)
                    {
                        ErrorMessage = "New password does not contain all characters.";
                        Disabled = true;
                        break;
                    }
                    else
                    {
                        ErrorMessage = string.Empty;
                        Disabled = false;
                    }
                }
            }
            StateHasChanged();
        }

        private async void Save()
        {
            Disabled = true;
            List<string> passwords = new List<string>() { CurrentPassword, NewPassword };
            string list = JsonConvert.SerializeObject(passwords);
            var result = await Api.ChangePassword(list);
            ErrorMessage = result.Message;
            StateHasChanged();
        }
    }
}
