using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class FontFamily : UIWebAttribute, IStyleAttribute
    {
        public FontFamily(String fontSize) { Value = fontSize; }
        public override String uiText { get { return "font-family"; } }
        public override string elementNM { get { return Attributes.fontFamily; } }
    }
}

