using BramrSite.Classes;
using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace BramrSite.Pages
{
    public partial class Editor : ComponentBase
    {
        [Inject] ApiService Api { get; set; }

        public List<TextModel> AllTextElements { get; private set; } = new List<TextModel>();
        public List<ImageModel> AllImageElements { get; private set; } = new List<ImageModel>();
        public List<object> AllDesignElements { get; private set; } = new List<object>();

        public TextModel Naam { get; private set; } = new TextModel() { Location = 0 };
        public TextModel Adres { get; private set; } = new TextModel() { Location = 1 };
        public TextModel Postcode { get; private set; } = new TextModel() { Location = 2 };
        public TextModel Woonplaats { get; private set; } = new TextModel() { Location = 3 };
        public TextModel Telefoon { get; private set; } = new TextModel() { Location = 4 };
        public TextModel Email { get; private set; } = new TextModel() { Location = 5 };
        public TextModel Geboortedatum { get; private set; } = new TextModel() { Location = 6 };
        public TextModel Nationaliteit { get; private set; } = new TextModel() { Location = 7 };
        public TextModel Rijbewijs { get; private set; } = new TextModel() { Location = 8 };
        public TextModel LinkedIn { get; private set; } = new TextModel() { Location = 9 };
        public TextModel Werkervaring { get; private set; } = new TextModel() { Location = 10 };
        public TextModel Schoolervaring { get; private set; } = new TextModel() { Location = 11 };
        public TextModel Skillset { get; private set; } = new TextModel() { Location = 12 };
        public TextModel Interesses { get; private set; } = new TextModel() { Location = 13 };
        public TextModel Motivatie { get; private set; } = new TextModel() { Location = 14 };
        //Art Aanpassing 
        public ImageModel ProfielFoto { get; private set; } = new ImageModel() { Location = 15, Alt = "ProfielFoto", FileType = ImageModel.FileTypes.ProfielFoto };
        //Art Aanpassing einde

        private TextModel CurrentTextElement { get; set; } = new TextModel();

        private List<int> AllFontSizes { get; set; } = new List<int>()
        {
            8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72
        };

        public int EditAmount { get; set; }
        public int HistoryLocation { get; set; }

        private bool UndoButton { get; set; } = true;
        private bool RedoButton { get; set; } = true;

        public delegate void Del(string uri, string src);

        Del CallBackMethod;

        protected override async void OnInitialized()
        {
            CallBackMethod = ApplySource;
            await Api.DeleteAllFromHistory();
            
            AllTextElements.Add(Naam);
            AllTextElements.Add(Adres);
            AllTextElements.Add(Postcode);
            AllTextElements.Add(Woonplaats);
            AllTextElements.Add(Telefoon);
            AllTextElements.Add(Email);
            AllTextElements.Add(Geboortedatum);
            AllTextElements.Add(Nationaliteit);
            AllTextElements.Add(Rijbewijs);
            AllTextElements.Add(LinkedIn);
            AllTextElements.Add(Werkervaring);
            AllTextElements.Add(Schoolervaring);
            AllTextElements.Add(Skillset);
            AllTextElements.Add(Interesses);
            AllTextElements.Add(Motivatie);
            //Art Aanpassing 
            AllImageElements.Add(ProfielFoto);
            //Art Aanpassing einde

            foreach (var item in AllTextElements)
            {
                AllDesignElements.Add(item);
            }
            foreach (var item in AllImageElements)
            {
                AllDesignElements.Add(item);
            }

            LoadSite();
        }

        private void Selection(TextModel NewTextElement)
        {
            CurrentTextElement.Selected = false;
            CurrentTextElement = NewTextElement;
            CurrentTextElement.Selected = true;
        }

        private async void Save()
        {
            string json = JsonConvert.SerializeObject(AllDesignElements, Formatting.Indented);

            await Api.UploadCV(json);
            await Api.DeleteAllFromHistory();
        }

        private async void LoadSite()
        {
            List<object> AllDesignElements = await Api.GetDesignElements();

            if(AllDesignElements.Count != 0)
            {
                Naam = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[0].ToString());
                Adres = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[1].ToString());
                Postcode = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[2].ToString());
                Woonplaats = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[3].ToString());
                Telefoon = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[4].ToString());
                Email = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[5].ToString());
                Geboortedatum = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[6].ToString());
                Nationaliteit = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[7].ToString());
                Rijbewijs = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[8].ToString());
                LinkedIn = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[9].ToString());
                Werkervaring = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[10].ToString());
                Schoolervaring = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[11].ToString());
                Skillset = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[12].ToString());
                Interesses = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[13].ToString());
                Motivatie = JsonConvert.DeserializeObject<TextModel>(AllDesignElements[14].ToString());
                ProfielFoto = JsonConvert.DeserializeObject<ImageModel>(AllDesignElements[15].ToString());
#if DEBUG
                ProfielFoto.Src = $"https://localhost:44372/api/image/download/{ProfielFoto.FileUri}";
#else
                ProfielFoto.Src = $"https://bramr.tech/api/image/download/{ProfielFoto.FileUri}";
#endif
            }

            StateHasChanged();
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
                case ChangeModel.Type.Margin:
                   //CurrentImageElement.Margin = int.Parse(result.ToString());
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
                case ChangeModel.Type.Margin:
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
            ProfielFoto.FileUri = Uri;
            ProfielFoto.Src = Src;
            StateHasChanged();
        }
    }
}
