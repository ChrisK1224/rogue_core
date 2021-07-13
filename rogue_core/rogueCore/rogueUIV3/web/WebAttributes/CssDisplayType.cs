using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class CssDisplayType : UIWebAttribute, IStyleAttribute
    {
        public CssDisplayType(String displayType) { Value = displayType; }
        public override String uiText { get { return "display"; } }
        public override string elementNM { get { return Attributes.displayType; } }
    }
}
