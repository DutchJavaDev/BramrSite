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
    public partial class PortfolioEditor : ComponentBase
    {
        [Inject] ApiService Api { get; set; }

        public List<TextModel> AllTextElements { get; private set; } = new List<TextModel>();
        public List<ImageModel> AllImageElements { get; private set; } = new List<ImageModel>();
        public List<object> AllDesignElements { get; private set; } = new List<object>();

        private TextModel CurrentTextElement { get; set; } = new TextModel();
        private ImageModel CurrentImageElement { get; set; } = new ImageModel();

        private bool IsText { get; set; }

        private int HistoryAmount { get; set; }
        private int HistoryLocation { get; set; }

        private bool UndoButton { get; set; } = true;
        private bool RedoButton { get; set; } = true;

        protected override async void OnInitialized()
        {
            await Api.DeleteAllFromHistory();

            for(int x = 0; x < 15; x++)
            {
                AllTextElements.Add(new TextModel { Location = x });
            }
            for(int y = 15; y < 20; y++)
            {
                AllImageElements.Add(new ImageModel { Location = y });
            }

            foreach (var item in AllTextElements)
            {
                AllDesignElements.Add(item);
            }
            foreach (var item in AllImageElements)
            {
                AllDesignElements.Add(item);
            }
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
        }

        private async void Save()
        {
            string Json = JsonConvert.SerializeObject(AllDesignElements, Formatting.Indented);

            await Api.UploadPortfolio(Json);
            await Api.DeleteAllFromHistory();
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
            if (HistoryLocation == HistoryAmount)
            {
                RedoButton = true;

            }

            await UseChange(CurrentChange, false);
            StateHasChanged();
        }

        private async Task AddToDB(ChangeModel.Type EditType, string Edit)
        {
            ChangeModel CurrentChange = new ChangeModel();

            if (IsText)
            {
                CurrentChange.DesignElement = CurrentTextElement.Location; 
                CurrentChange.EditType = EditType; 
                CurrentChange.Edit = Edit;
            }
            else
            {
                CurrentChange.DesignElement = CurrentImageElement.Location;
                CurrentChange.EditType = EditType;
                CurrentChange.Edit = Edit;
            }

            UndoButton = false;
            RedoButton = true;
            HistoryAmount++;
            HistoryLocation++;
            if (HistoryLocation != HistoryAmount)
            {
                await Api.DeleteAmountFromHistory(HistoryLocation - 1);
                HistoryAmount = HistoryLocation;
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
            foreach (var Element in AllImageElements)
            {
                if (Element.Location == CurrentChange.DesignElement)
                {
                    CurrentImageElement = Element;

                    break;
                }
            }

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
                while (Value < HistoryAmount)
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
            }
        }
    }
}
