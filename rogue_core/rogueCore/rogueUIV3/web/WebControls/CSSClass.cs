using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class CSSClass : WebBaseControl
    {
        
        protected override string uiText { get { return "style "; } }
        public override string endTag { get { return "</style>"; } }
        public override string elementNM { get { return Attributes.cssclass; } }
    }
}
