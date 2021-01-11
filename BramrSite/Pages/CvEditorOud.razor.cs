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
    public partial class CvEditorOud : ComponentBase
    {
        [Inject] IJSRuntime IJSRuntime { get; set; }
        [Inject] ApiService Api { get; set; }

        public List<TextModel> AllTextElements { get; private set; } = new List<TextModel>() 
        {
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel(),
            new TextModel()
        };
        public List<ImageModel> AllImageElements { get; private set; } = new List<ImageModel>() 
        {
            new ImageModel()
        };
        public List<object> AllDesignElements { get; private set; } = new List<object>();

        //Art Aanpassing 
        public ImageModel ProfielFoto { get; private set; } = new ImageModel() { Location = 15, Alt = "ProfielFoto", TemplateType = "Cv" };
        //Art Aanpassing einde

        private TextModel CurrentTextElement { get; set; } = new TextModel();
        private ImageModel CurrentImageElement { get; set; } = new ImageModel();

        public int EditAmount { get; set; }
        public int HistoryLocation { get; set; }

        private bool UndoButton { get; set; } = true;
        private bool RedoButton { get; set; } = true;

        private bool IsText { get; set; }

        public delegate void Del(string uri, string src);
        public delegate void CvDel(bool IsText, int Index);

        Del CallBackMethod;
        CvDel SelectionCallback;

        protected override async void OnInitialized()
        {
            var module = await IJSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/CvScript.js");

            await module.InvokeVoidAsync("Init");

            CallBackMethod = ApplySource;
            SelectionCallback = Selection;
            await Api.DeleteAllFromHistory();

            for (int x = 0; x < 15; x++)
            {
                AllTextElements[x].Location = x; AllTextElements[x].TemplateType = "Cv";
            }
            AllImageElements[0].Location = 15; AllImageElements[0].TemplateType = "Cv"; AllImageElements[0].Alt = "Profielfoto";

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

            string json = JsonConvert.SerializeObject(AllDesignElements, Formatting.Indented);

            await Api.UploadCV(json);
            await Api.DeleteAllFromHistory();
        }

        private async void LoadSite()
        {
            List<object> AllDesignElements = await Api.GetDesignElements();

            if (AllDesignElements.Count != 0)
            {
                for (int x = 0; x < 15; x++)
                {
                    AllTextElements[x] = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[x].ToString());
                }
                for(int y = 0; y < 1; y++)
                {
                    AllImageElements[y] = JsonConvert.DeserializeObject<ImageModel>(AllDesignElements[y + 15].ToString());
#if DEBUG
                    AllImageElements[y].Src = $"https://localhost:44372/api/image/download/{AllImageElements[y].FileUri}";
#else
                    AllImageElements[y].Src = $"https://bramr.tech/api/image/download/{AllImageElements[y].FileUri}";
#endif
                }
            }

            StateHasChanged();
        }

        private bool SelectionCheck(bool IsTextEdit)
        {
            if(IsText == IsTextEdit)
            {
                return true;
            }
            return false;
        }

        private async Task Undo()
        {
            ChangeModel CurrentChange;

            RedoButton = false;
            CurrentChange = await Api.GetOneFromHistory(HistoryLocation);
            Console.WriteLine(HistoryLocation);
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
            Console.WriteLine(HistoryLocation);
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
                case ChangeModel.Type.TextAllignment:
                    CurrentTextElement.TextAllignment = (TextModel.Allignment)Enum.Parse(typeof(TextModel.Allignment), result.ToString());
                    break;
                case ChangeModel.Type.FontSize:
                    CurrentTextElement.FontSize = int.Parse(result.ToString());
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
                    return false;
                case ChangeModel.Type.FontSize:
                    return 10;
                case ChangeModel.Type.Text:
                case ChangeModel.Type.TextColor:
                case ChangeModel.Type.BackgroundColor:
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

        public void ApplySource(string Uri, string Src)
        {
            AllImageElements[0].FileUri = Uri;
            AllImageElements[0].Src = Src;
            StateHasChanged();
        }
    }
}
