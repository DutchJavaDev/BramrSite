using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BramrSite.Pages.PortfolioEditor;

namespace BramrSite.Classes
{
    public class ImageComponent : ComponentBase
    {
        [Parameter] public ImageModel CurrentDesignElement { get; set; } = new ImageModel();
        [Parameter] public PortfolioDel CallBack { get; set; }
        [Parameter] public int Index { get; set; }

        private int ElementIndex { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ElementIndex = 0;
            base.BuildRenderTree(builder);

            if (string.IsNullOrWhiteSpace(CurrentDesignElement.Src))
            {
                builder.OpenElement(ElementIndex, "img"); ElementIndex++;
                builder.AddAttribute(ElementIndex, "alt", $"{CurrentDesignElement.Alt}"); ElementIndex++;
                if (CurrentDesignElement.CssCode != string.Empty) { builder.AddAttribute(ElementIndex, "b-qmx0h9ieg0"); }
                var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { CallBack(false, Index); }));
                builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                builder.CloseElement();
                
            }
            else
            {
                builder.OpenElement(ElementIndex, "img"); ElementIndex++;
                builder.AddAttribute(ElementIndex, "src", $"{CurrentDesignElement.Src}"); ElementIndex++;
                builder.AddAttribute(ElementIndex, "style", $"float:{CurrentDesignElement.FloatSet}; opacity:{CurrentDesignElement.Opacity.ToString().Replace(",", ".")}; width:{CurrentDesignElement.Width}%; height:{CurrentDesignElement.Height}%; padding:{CurrentDesignElement.Padding}px; border:{CurrentDesignElement.Border}px solid black; object-fit:{CurrentDesignElement.ObjectFitSet};"); ElementIndex++;
                if (CurrentDesignElement.CssCode != string.Empty) { builder.AddAttribute(ElementIndex, "b-qmx0h9ieg0"); }
                var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { CallBack(false, Index); }));
                builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                builder.CloseElement();
            }
        }
    }
}
