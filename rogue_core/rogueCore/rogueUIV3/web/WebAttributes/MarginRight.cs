using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class MarginRight : UIWebAttribute, IStyleAttribute
    {
        public MarginRight(String margin) { Value = margin + "px"; }
        public override String uiText { get { return "margin-right"; } }
        public override string elementNM { get { return Attributes.marginright; } }
    }
}
