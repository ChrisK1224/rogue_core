using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class MarginBottom : UIWebAttribute, IStyleAttribute
    {
        public MarginBottom(String margin) { Value = margin + "px"; }
        public override String uiText { get { return "margin-bottom"; } }
        public override string elementNM { get { return Attributes.marginbottom; } }
    }
}
