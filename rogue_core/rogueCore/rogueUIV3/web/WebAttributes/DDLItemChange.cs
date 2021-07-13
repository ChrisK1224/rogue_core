using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class DDLItemChange : UIWebAttribute
    {
        public DDLItemChange(String mouseClickEvent) { Value = mouseClickEvent; }
        public override string uiText { get { return "onchange=\"GenericClick(this.value)\""; } }
        public override string elementNM { get { return Attributes.onchange; } }
    }
}
