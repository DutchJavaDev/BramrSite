using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using BramrSite.Models;

namespace BramrSite.Classes
{
    public class DesignComponent : ComponentBase
    {
        [Parameter] public TextModel CurrentDesignElement { get; set; } = new TextModel();

        private int ElementIndex { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ElementIndex = 0;

            base.BuildRenderTree(builder);

            if (string.IsNullOrWhiteSpace(CurrentDesignElement.Text))
            {
                builder.OpenElement(ElementIndex, "p"); ElementIndex++;
                builder.OpenElement(ElementIndex, "i"); ElementIndex++;
                builder.AddContent(ElementIndex, "Type some text..."); ElementIndex++;
            }
            else
            {
                builder.OpenElement(ElementIndex, "p"); ElementIndex++;
                builder.AddAttribute(ElementIndex, "style", $"color:{CurrentDesignElement.TextColor}; background-color:{CurrentDesignElement.BackgroundColor}; font-size:{CurrentDesignElement.FontSize / 5}vh; text-align:{CurrentDesignElement.TextAllignment}"); ElementIndex++;
                if (CurrentDesignElement.Bold) { builder.OpenElement(ElementIndex, "b"); ElementIndex++; }
                if (CurrentDesignElement.Italic) { builder.OpenElement(ElementIndex, "i"); ElementIndex++; }
                if (CurrentDesignElement.Underlined) { builder.OpenElement(ElementIndex, "u"); ElementIndex++; }
                if (CurrentDesignElement.StrikedThrough) { builder.OpenElement(ElementIndex, "s"); ElementIndex++; }
                builder.AddContent(ElementIndex, CurrentDesignElement.Text);
            }

            for (int i = 0; i < ElementIndex - 1; i++)
            {
                builder.CloseElement();
            }
        }
    }
}
