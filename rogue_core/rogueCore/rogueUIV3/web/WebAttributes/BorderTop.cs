using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class BorderTop : UIWebAttribute, IStyleAttribute
    {
        public BorderTop(String border) { Value = border; }
        public override String uiText { get { return "border-top"; } }
        public override string elementNM { get { return Attributes.bordertop; } }
    }
}
