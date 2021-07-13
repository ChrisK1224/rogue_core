using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class RImage : WebBaseControl
    {
        protected override String uiText { get { return "img "; } }
        public override string endTag { get { return "</img>"; } }
        public override string elementNM { get { return Elements.image; } }
    }
}
