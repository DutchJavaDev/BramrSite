using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BramrSite.Classes.Interfaces;
using BramrSite.Models;
using BramrSite.Classes;

namespace BramrSite.Pages
{
    public partial class Editor : ComponentBase
    {
        [Inject] IApiDesignConnection API { get; set; }

        public List<TextModel> AllTextElements { get; private set; } = new List<TextModel>();

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


        private TextModel CurrentTextElement { get; set; } = new TextModel();

        private List<int> AllFontSizes { get; set; } = new List<int>()
        {
            8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72
        };

        public int EditAmount { get; set; }
        public int HistoryLocation { get; set; }

        private bool UndoButton { get; set; }
        private bool RedoButton { get; set; }

        protected override async void OnInitialized()
        {
            //////////////await API.DeleteAllFromDB();

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

            RedoButton = true;
            CurrentChange = await API.GetOneFromDB(HistoryLocation);
            Console.WriteLine(HistoryLocation);
            HistoryLocation--;
            if (HistoryLocation == 0)
            {
                UndoButton = false;
            }

            await UseChange(CurrentChange, true);
            StateHasChanged();
        }
        private async Task Redo()
        {
            ChangeModel CurrentChange;

            UndoButton = true;
            HistoryLocation++;
            CurrentChange = await API.GetOneFromDB(HistoryLocation);
            Console.WriteLine(HistoryLocation);
            if (HistoryLocation == EditAmount)
            {
                RedoButton = false;

            }

            await UseChange(CurrentChange, false);
            StateHasChanged();
        }

        private async Task AddToDB(ChangeModel.Type EditType, string Edit)
        {
            ChangeModel CurrentChange = new ChangeModel(CurrentTextElement.ID, EditType, Edit);

            UndoButton = true;
            RedoButton = false;
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

            object result;

            foreach (var Element in AllTextElements)
            {
                if (Element.ID == CurrentChange.DesignElement)
                {
                    CurrentTextElement = Element;
                    break;
                }
            }

            switch (CurrentChange.EditType)
            {
                case ChangeModel.Type.Text:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.Text = result.ToString();
                    break;
                case ChangeModel.Type.TextColor:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.TextColor = result.ToString();
                    break;
                case ChangeModel.Type.BackgroundColor:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.BackgroundColor = result.ToString();
                    break;
                case ChangeModel.Type.Bold:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.Bold = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Italic:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.Italic = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Underlined:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.Underlined = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Strikedthrough:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.StrikedThrough = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.TextAllignment:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.TextAllignment = (TextModel.Allignment)Enum.Parse(typeof(TextModel.Allignment), result.ToString());
                    break;
                case ChangeModel.Type.FontSize:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentTextElement.FontSize = int.Parse(result.ToString());
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
                case ChangeModel.Type.Text:
                case ChangeModel.Type.TextColor:
                case ChangeModel.Type.BackgroundColor:
                    return string.Empty;
                case ChangeModel.Type.FontSize:
                    return 10;
                default:
                    return TextModel.Allignment.Left;
            }
        }
    }
}
