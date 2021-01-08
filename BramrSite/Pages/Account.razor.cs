using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BramrSite.Models;
using System.Collections.Generic;

namespace BramrSite.Pages
{
    public partial class Account : ComponentBase
    {
        [Inject] IJSRuntime IJSRuntime { get; set; }
        private IJSObjectReference Module;

        private List<NoteModel> Notes { get; set; }
        

        protected override async void OnInitialized()
        {
            Notes = new List<NoteModel>();

            Module = await IJSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/AccountScript.js");

            await Module.InvokeVoidAsync("Init");
            await Module.InvokeVoidAsync("tabs",0);
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
