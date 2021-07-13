using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class Margin : UIWebAttribute, IStyleAttribute
    {
        public Margin(String margin) { Value = margin + "px"; }
        public override String uiText { get { return "margin"; } }
        public override string elementNM { get { return Attributes.margin; } }
    }
}
