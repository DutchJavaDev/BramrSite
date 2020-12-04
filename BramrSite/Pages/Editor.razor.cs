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

        public List<DesignModel> AllDesignElements { get; private set; } = new List<DesignModel>();

        public DesignModel Naam { get; private set; } = new DesignModel() { ID = 0 };
        public DesignModel Adres { get; private set; } = new DesignModel() { ID = 1 };
        public DesignModel Postcode { get; private set; } = new DesignModel() { ID = 2 };
        public DesignModel Woonplaats { get; private set; } = new DesignModel() { ID = 3 };
        public DesignModel Telefoon { get; private set; } = new DesignModel() { ID = 4 };
        public DesignModel Email { get; private set; } = new DesignModel() { ID = 5 };
        public DesignModel Geboortedatum { get; private set; } = new DesignModel() { ID = 6 };
        public DesignModel Nationaliteit { get; private set; } = new DesignModel() { ID = 7 };
        public DesignModel Rijbewijs { get; private set; } = new DesignModel() { ID = 8 };
        public DesignModel LinkedIn { get; private set; } = new DesignModel() { ID = 9 };
        public DesignModel Werkervaring { get; private set; } = new DesignModel() { ID = 10 };
        public DesignModel Schoolervaring { get; private set; } = new DesignModel() { ID = 11 };
        public DesignModel Skillset { get; private set; } = new DesignModel() { ID = 12 };
        public DesignModel Interesses { get; private set; } = new DesignModel() { ID = 13 };
        public DesignModel Motivatie { get; private set; } = new DesignModel() { ID = 14 };


        private DesignModel CurrentTextElement { get; set; } = new DesignModel();

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
            await API.DeleteAllFromDB();

            AllDesignElements.Add(Naam);
            AllDesignElements.Add(Adres);
            AllDesignElements.Add(Postcode);
            AllDesignElements.Add(Woonplaats);
            AllDesignElements.Add(Telefoon);
            AllDesignElements.Add(Email);
            AllDesignElements.Add(Geboortedatum);
            AllDesignElements.Add(Nationaliteit);
            AllDesignElements.Add(Rijbewijs);
            AllDesignElements.Add(LinkedIn);
            AllDesignElements.Add(Werkervaring);
            AllDesignElements.Add(Schoolervaring);
            AllDesignElements.Add(Skillset);
            AllDesignElements.Add(Interesses);
            AllDesignElements.Add(Motivatie);
        }

        private void Selection(DesignModel NewTextElement)
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
            DesignModel CurrentElement = new DesignModel();
            object result;

            foreach (var Element in AllDesignElements)
            {
                if (Element.ID == CurrentChange.DesignElement)
                {
                    CurrentElement = Element;
                    break;
                }
            }

            switch (CurrentChange.EditType)
            {
                case ChangeModel.Type.Text:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.Text = result.ToString();
                    break;
                case ChangeModel.Type.TextColor:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.TextColor = result.ToString();
                    break;
                case ChangeModel.Type.BackgroundColor:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.BackgroundColor = result.ToString();
                    break;
                case ChangeModel.Type.Bold:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.Bold = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Italic:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.Italic = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Underlined:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.Underlined = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.Strikedthrough:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.StrikedThrough = bool.Parse(result.ToString());
                    break;
                case ChangeModel.Type.TextAllignment:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.TextAllignment = (DesignModel.Allignment)Enum.Parse(typeof(DesignModel.Allignment), result.ToString());
                    break;
                case ChangeModel.Type.FontSize:
                    result = await DetermineChange(CurrentChange.EditType, CurrentChange, GoingBack);
                    CurrentElement.FontSize = int.Parse(result.ToString());
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
                    return DesignModel.Allignment.Left;
            }
        }
    }
}
