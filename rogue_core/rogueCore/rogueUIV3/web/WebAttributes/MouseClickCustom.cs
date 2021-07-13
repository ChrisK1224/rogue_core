using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class MouseClickCustom : UIWebAttribute
    {
        public MouseClickCustom(String mouseClickCustomEvent) { Value = mouseClickCustomEvent; }
        public override string uiText { get { return "onclick=\"" + Value + "\""; } } //GenericDivContents(id)
        public override string elementNM { get { return Attributes.mouseclickCustom; } }
    }
}
