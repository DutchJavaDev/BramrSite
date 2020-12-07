using Newtonsoft.Json;
using System;

namespace BramrSite.Models
{
    public class ChangeModel
    {
        public int DesignElement { get; set; }
        public Type EditType { get; set; }
        public string Edit { get; set; }

        [JsonConstructor]
        public ChangeModel(int id, string designElement, string editType, string edit)
        {
            DesignElement = int.Parse(designElement);
            EditType = (Type)Enum.Parse(typeof(Type), editType);
            Edit = edit;
        }

        public ChangeModel(int DesignElement, Type EditType, string Edit)
        {
            this.DesignElement = DesignElement;
            this.EditType = EditType;
            this.Edit = Edit;
        }

        public enum Type
        {
            Text,
            TextColor,
            BackgroundColor,
            Bold,
            Italic,
            Underlined,
            Strikedthrough,
            TextAllignment,
            FontSize,
            // Art Aanpassing
            Width,
            Height,
            Src,
            Alt,
            Border,
            FloatSet,
            Opacity,
            ObjectFitSet,
            Margin,
            Padding
                
        }
    }
}
