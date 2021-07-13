using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class BorderBottom : UIWebAttribute, IStyleAttribute
    {
        public BorderBottom(String border) { Value = border; }
        public override String uiText { get { return "border-bottom"; } }
        public override string elementNM { get { return Attributes.borderbottom; } }
    }
}
