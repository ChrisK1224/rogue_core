using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class CssClass : UIWebAttribute
    {
        public CssClass(String className) { Value = className; }
        public override String uiText { get { return Value; } }
        public override string elementNM { get { return Attributes.cssclass; } }
    }
}
