using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class OnTextChange : UIWebAttribute
    {
        public OnTextChange(String onTextChange) { Value = onTextChange; }
        public override string uiText { get { return "onclick=\"GenericClick('" + Value + "')\""; } }
        public override string elementNM { get { return Attributes.onTextChange; } }
    }
}
