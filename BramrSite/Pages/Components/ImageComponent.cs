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
                Imagebuilder.AddAttribute(ElementIndex, "src", $"{CurrentDesignElement.Src}");                
                Imagebuilder.CloseElement();
            }
        }
    }
}
