using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Classes
{
    public class ImageComponent : ComponentBase
    {
        [Parameter] public ImageModel CurrentDesignElement { get; set; } = new ImageModel();

        private int ElementIndex { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder Imagebuilder)
        {
            ElementIndex = 0;
            base.BuildRenderTree(Imagebuilder);

            if (string.IsNullOrWhiteSpace(CurrentDesignElement.Src))
            {
                Imagebuilder.OpenElement(ElementIndex, "img"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "alt", $"{CurrentDesignElement.Alt}");
                Imagebuilder.CloseElement();
                
            }
            else
            {
                Imagebuilder.OpenElement(ElementIndex, "img"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "src", $"{CurrentDesignElement.Src}");  ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "style", $"float:{CurrentDesignElement.FloatSet}; opacity:{CurrentDesignElement.Opacity.ToString().Replace(",",".")}; width:{CurrentDesignElement.Width}%; height:{CurrentDesignElement.Height}px; padding:{CurrentDesignElement.Padding}px; border:{CurrentDesignElement.Border}px solid black; object-fit:{CurrentDesignElement.ObjectFitSet};"); ElementIndex++;

                Imagebuilder.AddAttribute(ElementIndex, "width", $"{CurrentDesignElement.Width}%"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "height", $"{CurrentDesignElement.Height}px"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "border", $"{CurrentDesignElement.Border}px solid black"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "float", $"{CurrentDesignElement.FloatSet}"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "opacity", $"{CurrentDesignElement.Opacity}"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "object-fit", $"{CurrentDesignElement.ObjectFitSet}"); ElementIndex++;
                //Imagebuilder.AddAttribute(ElementIndex, "margin", $"{CurrentDesignElement.Margin}px {CurrentDesignElement.Margin}px {CurrentDesignElement.Margin}px {CurrentDesignElement.Margin}px"); ElementIndex++;
                Imagebuilder.AddAttribute(ElementIndex, "padding", $"{CurrentDesignElement.Padding}px");

                Imagebuilder.CloseElement();
            }
        }
    }
}
