using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class Anchor : WebBaseControl
    {
        protected override String uiText { get { return "a "; } }
        public override string endTag { get { return "</a>"; } }
        public override string elementNM { get { return Elements.anchor; } }
    }
}
