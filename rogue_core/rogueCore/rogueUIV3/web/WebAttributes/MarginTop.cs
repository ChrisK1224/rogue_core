using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class MarginTop : UIWebAttribute, IStyleAttribute
    {
        public MarginTop(String margin) { Value = margin + "px"; }
        public override String uiText { get { return "margin-top"; } }
        public override string elementNM { get { return Attributes.margintop; } }
    }
}
