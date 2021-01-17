using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using BramrSite.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using static BramrSite.Pages.PortfolioEditor;
using static BramrSite.Pages.CvEditor;

namespace BramrSite.Classes
{
    public class TextComponent : ComponentBase
    {
        [Parameter] public TextModel CurrentDesignElement { get; set; } = new TextModel();
        [Parameter] public string TagType { get; set; }
        [Parameter] public CvDel CvCallback { get; set; }
        [Parameter] public PortfolioDel PortfolioCallback { get; set; }
        [Parameter] public int Index { get; set; }

        private int ElementIndex { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ElementIndex = 0;

            base.BuildRenderTree(builder);

            if (string.IsNullOrWhiteSpace(CurrentDesignElement.Text))
            {
                builder.OpenElement(ElementIndex, TagType); ElementIndex++;
                if (CurrentDesignElement.CssCode != string.Empty) { builder.AddAttribute(ElementIndex, CurrentDesignElement.CssCode); ElementIndex++; }
                if(CvCallback != null)
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { CvCallback(true, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                else
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { PortfolioCallback(true, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                builder.OpenElement(ElementIndex, "i"); ElementIndex++;
                builder.AddContent(ElementIndex, "Type some text..."); ElementIndex++;
            }
            else
            {
                builder.OpenElement(ElementIndex, TagType); ElementIndex++;
                builder.AddAttribute(ElementIndex, "style", $"{(CurrentDesignElement.TextColor != string.Empty ? $"color:{CurrentDesignElement.TextColor};" : string.Empty)} {(CurrentDesignElement.BackgroundColor != string.Empty ? $"background-color:{CurrentDesignElement.BackgroundColor};" : string.Empty)} {(CurrentDesignElement.Shadow ? $"text-shadow: 1px 1px 5px {(CurrentDesignElement.TextColor != string.Empty ? CurrentDesignElement.TextColor : "#000000")};" : string.Empty)} font-size:{CurrentDesignElement.FontSize.ToString().Replace(",", ".")}rem; {(CurrentDesignElement.FontWeight != 0 ? $"font-weight:{CurrentDesignElement.FontWeight};" : string.Empty)} text-align:{CurrentDesignElement.TextAllignment}; {(CurrentDesignElement.Font != string.Empty ? $"font-family:{CurrentDesignElement.Font}, sans-serif;" : string.Empty)}"); ElementIndex++;
                if (CurrentDesignElement.CssCode != string.Empty) { builder.AddAttribute(ElementIndex, CurrentDesignElement.CssCode); ElementIndex++; }
                if (CvCallback != null)
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { CvCallback(true, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                else
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { PortfolioCallback(true, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                if (CurrentDesignElement.Bold) { builder.OpenElement(ElementIndex, "b"); ElementIndex++; }
                if (CurrentDesignElement.Italic) { builder.OpenElement(ElementIndex, "i"); ElementIndex++; }
                if (CurrentDesignElement.Underlined) { builder.OpenElement(ElementIndex, "u"); ElementIndex++; }
                if (CurrentDesignElement.StrikedThrough) { builder.OpenElement(ElementIndex, "s"); ElementIndex++; }
                builder.AddContent(ElementIndex, CurrentDesignElement.Text);
            }

            for (int i = 0; i < ElementIndex - (CurrentDesignElement.CssCode != string.Empty ? 3 : 2); i++)
            {
                builder.CloseElement();
            }
        }

       
    }
}
