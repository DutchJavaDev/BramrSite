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
            Console.WriteLine(Imagebuilder);
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
                Imagebuilder.AddAttribute(ElementIndex, "style", $"float:{CurrentDesignElement.FloatSet}; opacity:{CurrentDesignElement.Opacity.ToString().Replace(",", ".")}; width:{CurrentDesignElement.Width}%; height:{CurrentDesignElement.Height}px; padding:{CurrentDesignElement.Padding}px; border:{CurrentDesignElement.Border}px solid black; object-fit:{CurrentDesignElement.ObjectFitSet};");
                Imagebuilder.CloseElement();
            }
        }
    }
}
