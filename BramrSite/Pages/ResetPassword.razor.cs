using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace BramrSite.Pages
{
    public partial class ResetPassword : ComponentBase
    {
        [Inject] ApiService Api { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ErrorMessage { get; set; }
        public bool Disabled { get; set; }
                private char[] LowerCases { get; set; } = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private char[] UpperCases { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private char[] Numbers { get; set; } = "1234567890".ToCharArray();
        private char[] StrangeCharacters { get; set; } = "!@#$%^&*()-=_+<>/?,.:;[]{}|".ToCharArray();

        private List<char[]> AllCharacters { get; set; } = new List<char[]>();


        private void CheckInput()
        {
            if(string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "Please fill in all fields";
                Disabled = true;
            }
            else if (NewPassword.Length < 8)
            {
                ErrorMessage = "Your password is not long enough";
                Disabled = true;
            }
            else if(NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match";
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
                        ErrorMessage = "Your password has to include atleast one number and one special character";
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
        public async void clickPost_btn()
        {
            Disabled = true;
                if(NewPassword == ConfirmPassword)
                {
                ResetPasswordModel model = new ResetPasswordModel() { Email = Email, NewPassword = NewPassword, Token = Token };
                var respondse = await Api.ResetPassword(model);
                    if (respondse.Success)
                    {
                    ErrorMessage = "You have changed your password successfully.";
                    StateHasChanged();
                    }
                    else 
                    {
                    ErrorMessage = "Something went wrong, please try again.";
                    StateHasChanged();
                    Disabled = false;
                    }
                }
                else
                {
                ErrorMessage = "Passwords do not match";
                StateHasChanged();
                Disabled = false;
                }  
        }

        protected override void OnInitialized()
        {
            AllCharacters.Add(LowerCases);
            AllCharacters.Add(UpperCases);
            AllCharacters.Add(Numbers);
            AllCharacters.Add(StrangeCharacters);
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out var _Token))
            {
                Token = _Token;
            }

        }
    }
}
