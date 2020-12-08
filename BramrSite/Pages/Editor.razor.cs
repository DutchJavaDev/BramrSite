using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BramrSite.Classes.Interfaces;
using BramrSite.Models;
using BramrSite.Classes;
using Tewr.Blazor.FileReader;

namespace BramrSite.Pages
{
    public partial class Editor : ComponentBase
    {
        [Inject] IApiDesignConnection API { get; set; }
        [Inject] IFileReaderService FileReader { get; set; }

        public List<TextModel> AllTextElements { get; private set; } = new List<TextModel>();
        public List<ImageModel> AllImageElements { get; private set; } = new List<ImageModel>();

        public TextModel Naam { get; private set; } = new TextModel() { ID = 0 };
        public TextModel Adres { get; private set; } = new TextModel() { ID = 1 };
        public TextModel Postcode { get; private set; } = new TextModel() { ID = 2 };
        public TextModel Woonplaats { get; private set; } = new TextModel() { ID = 3 };
        public TextModel Telefoon { get; private set; } = new TextModel() { ID = 4 };
        public TextModel Email { get; private set; } = new TextModel() { ID = 5 };
        public TextModel Geboortedatum { get; private set; } = new TextModel() { ID = 6 };
        public TextModel Nationaliteit { get; private set; } = new TextModel() { ID = 7 };
        public TextModel Rijbewijs { get; private set; } = new TextModel() { ID = 8 };
        public TextModel LinkedIn { get; private set; } = new TextModel() { ID = 9 };
        public TextModel Werkervaring { get; private set; } = new TextModel() { ID = 10 };
        public TextModel Schoolervaring { get; private set; } = new TextModel() { ID = 11 };
        public TextModel Skillset { get; private set; } = new TextModel() { ID = 12 };
        public TextModel Interesses { get; private set; } = new TextModel() { ID = 13 };
        public TextModel Motivatie { get; private set; } = new TextModel() { ID = 14 };
        //Art Aanpassing 
        public ImageModel ProfielFoto { get; private set; } = new ImageModel() { ID = 15, Alt = "ProfielFoto", Src = "https://localhost:44372/api/image/download"};
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

        ElementReference FileReference { get; set; }

        public string ImageSrc { get; set; }

        protected override /*async*/ void OnInitialized()
        {
            //await API.DeleteAllFromDB();

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

        }

        private void Selection(TextModel NewTextElement)
        {
            CurrentTextElement.Selected = false;
            CurrentTextElement = NewTextElement;
            CurrentTextElement.Selected = true;
        }

        private async Task Undo()
        {
            ChangeModel CurrentChange;

            RedoButton = false;
            CurrentChange = await API.GetOneFromDB(HistoryLocation);
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
            CurrentChange = await API.GetOneFromDB(HistoryLocation);
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
            ChangeModel CurrentChange = new ChangeModel(CurrentTextElement.ID, EditType, Edit);

            UndoButton = false;
            RedoButton = true;
            EditAmount++;
            HistoryLocation++;
            if (HistoryLocation != EditAmount)
            {
                await API.DeleteAmountFromDB(HistoryLocation - 1);
                EditAmount = HistoryLocation;
            }

            await API.AddToDB(HistoryLocation, CurrentChange);
        }

        private async Task UseChange(ChangeModel CurrentChange, bool GoingBack)
        {
            TextModel CurrentTextElement = new TextModel();
            ImageModel CurrentImageElement = new ImageModel();

            object result;

            foreach (var Element in AllTextElements)
            {
                if (Element.ID == CurrentChange.DesignElement)
                {
                    CurrentTextElement = Element;

                    break;
                }
            }
            // Art aanpassing
            foreach (var Element in AllImageElements)
            {
                if (Element.ID == CurrentChange.DesignElement)
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
                    CurrentImageElement.Margin = int.Parse(result.ToString());
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
                    CurrentChange = await API.GetOneFromDB(Value);
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
                    CurrentChange = await API.GetOneFromDB(Value);
                    Value++;
                    if (CurrentChange.EditType == Type && Current.DesignElement == CurrentChange.DesignElement)
                    {
                        Edit = CurrentChange.Edit;
                        return Edit;
                    }
                }
                CurrentChange = await API.GetOneFromDB(HistoryLocation);
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
                default:
                    return TextModel.Allignment.Left;
                    //Art Aanpassing einde
            }
        }

        private async Task UploadFile()
        {
            var file = (await FileReader.CreateReference(FileReference).EnumerateFilesAsync()).FirstOrDefault();
        }
    }
}
