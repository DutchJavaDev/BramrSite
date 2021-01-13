using BramrSite.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BramrSite.Pages.PortfolioEditor;
using static BramrSite.Pages.CvEditor;

namespace BramrSite.Classes
{
    public class ImageComponent : ComponentBase
    {
        [Parameter] public ImageModel CurrentDesignElement { get; set; } = new ImageModel();
        [Parameter] public PortfolioDel PortfolioCallBack { get; set; }
        [Parameter] public CvDel CvCallBack { get; set; }
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
                if (CurrentDesignElement.CssCode != string.Empty) { builder.AddAttribute(ElementIndex, CurrentDesignElement.CssCode); }
                if (CvCallBack != null)
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { CvCallBack(false, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                else
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { PortfolioCallBack(false, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                builder.CloseElement();
                
            }
            else
            {
                builder.OpenElement(ElementIndex, "img"); ElementIndex++;
                builder.AddAttribute(ElementIndex, "src", $"{CurrentDesignElement.Src}"); ElementIndex++;
                builder.AddAttribute(ElementIndex, "style", $"float:{CurrentDesignElement.FloatSet}; opacity:{CurrentDesignElement.Opacity.ToString().Replace(",", ".")}; width:{CurrentDesignElement.Width}%; height:{CurrentDesignElement.Height}%; padding:{CurrentDesignElement.Padding}px; border:{CurrentDesignElement.Border}px solid black; object-fit:{CurrentDesignElement.ObjectFitSet};"); ElementIndex++;
                if (CurrentDesignElement.CssCode != string.Empty) { builder.AddAttribute(ElementIndex, CurrentDesignElement.CssCode); }
                if (CvCallBack != null)
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { CvCallBack(false, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                else
                {
                    var method = new KeyValuePair<string, object>("onclick", EventCallback.Factory.Create<MouseEventArgs>(this, x => { PortfolioCallBack(false, Index); }));
                    builder.AddAttribute(ElementIndex, method.Key, method.Value); ElementIndex++;
                }
                builder.CloseElement();
            }
        }
    }
}
