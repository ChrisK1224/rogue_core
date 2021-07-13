using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class FontStyle : UIWebAttribute, IStyleAttribute
    {
        public FontStyle(String fontSize) { Value = fontSize; }
        public override String uiText { get { return "font-style"; } }
        public override string elementNM { get { return Attributes.fontstyle; } }
    }
}
