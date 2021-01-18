using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace BramrSite.Pages
{
    public partial class CvEditor : ComponentBase
    {
        [Inject] IJSRuntime IJSRuntime { get; set; }
        [Inject] ApiService Api { get; set; }
        [Inject] BramrSite.Auth.ITokenHandler TokenHandler { get; set; }
        [Inject] NavigationManager Navigation { get; set; }

        public List<TextModel> AllTextElements { get; private set; } = new List<TextModel>()
        {
            new TextModel() { Text = "Insert Name", FontSize = 2 },
            new TextModel() { Text = "Insert Job", FontSize = 1 },
            new TextModel() { Text = "City, Country", FontSize = 1 },
            new TextModel() { Text = "Phone Number", FontSize = 1 },
            new TextModel() { Text = "Mail Address", FontSize = 1 },
            new TextModel() { Text = "Personal Website", FontSize = 1 },
            new TextModel() { Text = "Skill 1", FontSize = 1 },
            new TextModel() { Text = "Skill 2", FontSize = 1 },
            new TextModel() { Text = "Skill 3", FontSize = 1 },
            new TextModel() { Text = "Skill 4", FontSize = 1 },
            new TextModel() { Text = "Insert Accountname", FontSize = 1 },
            new TextModel() { Text = "Insert Accountname", FontSize = 1 },
            new TextModel() { Text = "Insert Accountname", FontSize = 1 },
            new TextModel() { Text = "Insert Accountname", FontSize = 1 },
            new TextModel() { Text = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Nobis suscipit, vitae debitis quod, quibusdam odio eum possimus eveniet pariatur cumque totam nisi, tempora atque temporibus? Lorem ipsum dolor sit amet, consectetur adipisicing elit. Rerum facilis cumque a nisi nobis at ut et, iure officia dolore, hic, quae deserunt doloribus repellendus.", FontSize = 1 },
            new TextModel() { Text = "0000-0000", FontSize = 1 },
            new TextModel() { Text = "Insert Job Title", FontSize = 1 },
            new TextModel() { Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ratione maiores qui porro illum alias magni?", FontSize = 1 },
            new TextModel() { Text = "0000-0000", FontSize = 1 },
            new TextModel() { Text = "Insert Job Title", FontSize = 1 },
            new TextModel() { Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ratione maiores qui porro illum alias magni?", FontSize = 1 },
            new TextModel() { Text = "0000-present", FontSize = 1 },
            new TextModel() { Text = "Insert Job Title", FontSize = 1 },
            new TextModel() { Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ratione maiores qui porro illum alias magni?", FontSize = 1 },
            new TextModel() { Text = "0000-0000", FontSize = 1 },
            new TextModel() { Text = "Insert School", FontSize = 1 },
            new TextModel() { Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ratione maiores qui porro illum alias magni?", FontSize = 1 },
            new TextModel() { Text = "0000-0000", FontSize = 1 },
            new TextModel() { Text = "Insert School", FontSize = 1 },
            new TextModel() { Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ratione maiores qui porro illum alias magni?", FontSize = 1 },
            new TextModel() { Text = "0000-present", FontSize = 1 },
            new TextModel() { Text = "Insert School", FontSize = 1 },
            new TextModel() { Text = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ratione maiores qui porro illum alias magni?", FontSize = 1 },
            new TextModel() { Text = "Insert Hobby 1", FontSize = 1 },
            new TextModel() { Text = "Insert Hobby 2", FontSize = 1 },
            new TextModel() { Text = "Insert Hobby 3", FontSize = 1 },
            new TextModel() { Text = "Insert Hobby 4", FontSize = 1 }
        };
        public List<ImageModel> AllImageElements { get; private set; } = new List<ImageModel>()
        {
            new ImageModel() { CssCode = "b-cvsqx07klj" }
        };
        public List<int> Skills { get; private set; } = new List<int>()
        {
            50,
            50,
            50,
            50
        };
        public List<object> AllDesignElements { get; private set; } = new List<object>();

        private TextModel CurrentTextElement { get; set; } = new TextModel();
        private ImageModel CurrentImageElement { get; set; } = new ImageModel();

        public int EditAmount { get; set; }
        public int HistoryLocation { get; set; }

        private bool UndoButton { get; set; } = true;
        private bool RedoButton { get; set; } = true;

        private bool IsText { get; set; }

        public delegate void CvImageDel(string uri, string src);
        public delegate void CvDel(bool IsText, int Index);

        CvImageDel ImageCallback;
        CvDel SelectionCallback;

        protected override async void OnInitialized()
        {
            ImageCallback = ApplySource;
            SelectionCallback = Selection;

            for (int x = 0; x < 37; x++)
            {
                AllTextElements[x].Location = x; AllTextElements[x].TemplateType = "Cv"; AllTextElements[x].CssCode = "b-cvsqx07klj";
            }
            AllImageElements[0].Location = 37; AllImageElements[0].TemplateType = "Cv"; AllImageElements[0].Alt = "Profielfoto";


            var module = await IJSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/CvScript.js");

            await module.InvokeVoidAsync("Init");

            await Api.DeleteAllFromHistory();
            
            LoadSite();
            StateHasChanged();
        }

        private void Selection(bool IsText, int Index)
        {
            this.IsText = IsText;
            CurrentTextElement.Selected = false;
            CurrentImageElement.Selected = false;

            if (IsText)
            {
                CurrentTextElement = AllTextElements[Index];
                CurrentTextElement.Selected = true;
            }
            else
            {
                CurrentImageElement = AllImageElements[Index];
                CurrentImageElement.Selected = true;
            }

            StateHasChanged();
        }

        private async void Save()
        {
            foreach (var item in AllTextElements)
            {
                AllDesignElements.Add(item);
            }
            foreach (var item in AllImageElements)
            {
                AllDesignElements.Add(item);
            }
            foreach (var item in Skills)
            {
                AllDesignElements.Add(item);
            }

            string json = JsonConvert.SerializeObject(AllDesignElements, Formatting.Indented);

            await Api.UploadCV(json);
            await Api.DeleteAllFromHistory();
        }

        private async void LoadSite()
        {
            List<object> AllDesignElements = await Api.GetDesignElements(true);

            if (AllDesignElements.Count != 0)
            {
                for (int x = 0; x < 37; x++)
                {
                    AllTextElements[x] = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[x].ToString());
                }
                for (int y = 0; y < 1; y++)
                {
                    AllImageElements[y] = JsonConvert.DeserializeObject<ImageModel>(AllDesignElements[y + 37].ToString());
#if DEBUG
                    AllImageElements[y].Src = $"https://localhost:44372/api/image/download/{AllImageElements[y].FileUri}";
#else
                    AllImageElements[y].Src = $"https://bramr.tech/api/image/download/{AllImageElements[y].FileUri}";
#endif
                }
            }

            StateHasChanged();
        }

        private async Task Undo()
        {
            ChangeModel CurrentChange;

            RedoButton = false;
            CurrentChange = await Api.GetOneFromHistory(HistoryLocation);
            HistoryLocation--;
            if (HistoryLocation == 0)
            {
                UndoButton = true;
            }

            await UseChange(CurrentChange, true);
            StateHasChanged();
        }
        private async Task Redo()
        {
            ChangeModel CurrentChange;

            UndoButton = false;
            HistoryLocation++;
            CurrentChange = await Api.GetOneFromHistory(HistoryLocation);
            if (HistoryLocation == EditAmount)
            {
                RedoButton = true;
            }

            await UseChange(CurrentChange, false);
            StateHasChanged();
        }

        private async Task AddToDB(ChangeModel.Type EditType, string Edit)
        {
            ChangeModel CurrentChange = new ChangeModel() { DesignElement = CurrentTextElement.Location, EditType = EditType, Edit = Edit };

            UndoButton = false;
            RedoButton = true;
            EditAmount++;
            HistoryLocation++;
            if (HistoryLocation != EditAmount)
            {
                await Api.DeleteAmountFromHistory(HistoryLocation - 1);
                EditAmount = HistoryLocation;
            }

            await Api.AddToHistory(HistoryLocation, CurrentChange);
            StateHasChanged();
        }

        private async Task UseChange(ChangeModel CurrentChange, bool GoingBack)
        {
            TextModel CurrentTextElement = new TextModel();
            ImageModel CurrentImageElement = new ImageModel();

            object result;

            foreach (var Element in AllTextElements)
            {
                if (Element.Location == CurrentChange.DesignElement)
                {
                    CurrentTextElement = Element;

                    break;
                }
            }
            // Art aanpassing
            foreach (var Element in AllImageElements)
            {
                if (Element.Location == CurrentChange.DesignElement)
                {
                    CurrentImageElement = Element;

                    break;
                }
            }
            //Art Aanpassing einde

            result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
            switch (CurrentChange.EditType)
            {

                case ChangeModel.Type.Text:
                    CurrentTextElement.Text = result.ToString();
                    break;
                case ChangeModel.Type.TextColor:
                    CurrentTextElement.TextColor = result.ToString();
                    break;
                case ChangeModel.Type.BackgroundColor:
                    CurrentTextElement.BackgroundColor = result.ToString();
                    break;
                case ChangeModel.Type.Bold:
                    CurrentTextElement.Bold = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Italic:
                    CurrentTextElement.Italic = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Underlined:
                    CurrentTextElement.Underlined = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Strikedthrough:
                    CurrentTextElement.StrikedThrough = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Shadow:
                    CurrentTextElement.Shadow = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.TextAllignment:
                    CurrentTextElement.TextAllignment = (TextModel.Allignment)Enum.Parse(typeof(TextModel.Allignment), result.ToString());
                    break;
                case ChangeModel.Type.Font:
                    CurrentTextElement.Font = result.ToString();
                    break;
                case ChangeModel.Type.FontSize:
                    CurrentTextElement.FontSize = double.Parse(result.ToString());
                    break;
                case ChangeModel.Type.FontWeight:
                    CurrentTextElement.FontWeight = int.Parse(result.ToString());
                    break;
                //Art aanpassing
                case ChangeModel.Type.Width:
                    CurrentImageElement.Width = int.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Height:
                    CurrentImageElement.Height = int.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Src:
                    CurrentImageElement.Src = result.ToString();
                    break;
                case ChangeModel.Type.Alt:
                    CurrentImageElement.Alt = result.ToString();
                    break;
                case ChangeModel.Type.Border:
                    CurrentImageElement.Border = int.Parse(result.ToString());
                    break;
                case ChangeModel.Type.FloatSet:
                    CurrentImageElement.FloatSet = (ImageModel.Float)Enum.Parse(typeof(ImageModel.Float), result.ToString());
                    break;
                case ChangeModel.Type.Opacity:
                    CurrentImageElement.Opacity = int.Parse(result.ToString());
                    break;
                case ChangeModel.Type.ObjectFitSet:
                    CurrentImageElement.ObjectFitSet = (ImageModel.ObjectFit)Enum.Parse(typeof(ImageModel.ObjectFit), result.ToString());
                    break;
                case ChangeModel.Type.Padding:
                    CurrentImageElement.Padding = int.Parse(result.ToString());
                    break;
                    //Art Aanpassing einde
            }
        }

        private async Task<object> DetermineChange(ChangeModel.Type Type, ChangeModel Current, bool GoingBack)
        {
            string Edit;
            var Value = HistoryLocation;
            ChangeModel CurrentChange;

            if (GoingBack)
            {
                while (Value > 0)
                {
                    CurrentChange = await Api.GetOneFromHistory(Value);
                    Value--;
                    if (CurrentChange.EditType == Type && Current.DesignElement == CurrentChange.DesignElement)
                    {
                        Edit = CurrentChange.Edit;
                        return Edit;
                    }
                }
            }
            else if (!GoingBack)
            {
                while (Value < EditAmount)
                {
                    CurrentChange = await Api.GetOneFromHistory(Value);
                    Value++;
                    if (CurrentChange.EditType == Type && Current.DesignElement == CurrentChange.DesignElement)
                    {
                        Edit = CurrentChange.Edit;
                        return Edit;
                    }
                }
                CurrentChange = await Api.GetOneFromHistory(HistoryLocation);
                return CurrentChange.Edit;
            }

            switch (Type)
            {
                case ChangeModel.Type.Bold:
                case ChangeModel.Type.Italic:
                case ChangeModel.Type.Underlined:
                case ChangeModel.Type.Strikedthrough:
                case ChangeModel.Type.Shadow:
                    return false;
                case ChangeModel.Type.FontSize:
                    return 1;
                case ChangeModel.Type.FontWeight:
                    return 0;
                case ChangeModel.Type.Text:
                case ChangeModel.Type.TextColor:
                case ChangeModel.Type.BackgroundColor:
                case ChangeModel.Type.Font:
                //Art Aanpassing
                case ChangeModel.Type.Src:
                case ChangeModel.Type.Alt:
                    return string.Empty;
                case ChangeModel.Type.Height:
                case ChangeModel.Type.Width:
                    return 100;
                case ChangeModel.Type.Padding:
                    return 0;
                case ChangeModel.Type.Opacity:
                    return 1;
                case ChangeModel.Type.FloatSet:
                    return ImageModel.Float.none;
                case ChangeModel.Type.ObjectFitSet:
                    return ImageModel.ObjectFit.cover;
                //Art Aanpassing einde
                default:
                    return TextModel.Allignment.Left;
            }
        }
        private async void Logout()
        {
            await TokenHandler.UpdateAutenticationState(string.Empty);
            Navigation.NavigateTo("/", false);
        }
        public void ApplySource(string Uri, string Src)
        {
            AllImageElements[0].FileUri = Uri;
            AllImageElements[0].Src = Src;
            StateHasChanged();
        }
    }
}
