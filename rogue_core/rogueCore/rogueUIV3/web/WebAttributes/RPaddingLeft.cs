using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class PaddingLeft : UIWebAttribute, IStyleAttribute
    {
        public PaddingLeft(String paddingLeft) { Value = paddingLeft + "px"; }
        public override String uiText { get { return "padding-left"; } }
        public override string elementNM { get { return Attributes.paddingleft; } }
    }
}
