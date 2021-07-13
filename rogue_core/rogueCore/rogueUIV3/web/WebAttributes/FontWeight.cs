using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class FontWeight : UIWebAttribute, IStyleAttribute
    {
        public FontWeight(String margin) { Value = margin; }
        public override String uiText { get { return "font-weight"; } }
        public override string elementNM { get { return Attributes.fontstyle; } }
    }
}

