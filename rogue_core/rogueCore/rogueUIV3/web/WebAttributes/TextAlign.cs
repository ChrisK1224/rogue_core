using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class TextAlign : UIWebAttribute, IStyleAttribute
    {
        public TextAlign(String border) { Value = border; }
        public override String uiText { get { return "text-align"; } }
        public override string elementNM { get { return Attributes.textalign; } }
    }
}
