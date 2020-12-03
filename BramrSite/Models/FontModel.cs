using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BramrSite.Models
{
    public class FontModel
    {
       
        public string FontType { get; private set; }
        public string FontName { get; private set; }
        

        public FontModel(string fontStyle, string fontName)
        {
            FontType = fontStyle;
            FontName = fontName;
        }


    }
}
