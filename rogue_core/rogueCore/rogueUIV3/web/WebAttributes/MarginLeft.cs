using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class MarginLeft : UIWebAttribute, IStyleAttribute
    {
        public MarginLeft(String margin) { Value = margin + "px"; }
        public override String uiText { get { return "margin-left"; } }
        public override string elementNM { get { return Attributes.marginleft; } }
    }
}
