using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BramrSite.Models;
using System.Collections.Generic;
using BramrSite.Classes;

namespace BramrSite.Pages
{
    public partial class Account : ComponentBase
    {
        [Inject] IJSRuntime IJSRuntime { get; set; }
        [Inject] ApiService Api { get; set; }
        private IJSObjectReference Module;

        private List<NoteModel> Notes { get; set; } = new List<NoteModel>();
        private List<string> UserInfo { get; set; } = new List<string>() 
        {
            "UserName",
            "Joined Bramr on:",
            "CV",
            "Portfolio",
            "Email"
        };
        

        protected override async void OnInitialized()
        {
            UserInfo = await Api.GetUserInfo();

            Module = await IJSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/AccountScript.js");

            await Module.InvokeVoidAsync("Init");
            await Module.InvokeVoidAsync("tabs",0);

            StateHasChanged();
        }


        public async void Tab(int tabIndex)
        {
            await Module.InvokeVoidAsync("tabs", tabIndex);
        }

        public void AddNote()
        {
            Notes.Add(new NoteModel {
                EditMode = EditMode.Edit,
                Text = ""
            });
        }

        public void RemoveNote(NoteModel model)
        {
            Notes.Remove(model);
        }
    }
}
