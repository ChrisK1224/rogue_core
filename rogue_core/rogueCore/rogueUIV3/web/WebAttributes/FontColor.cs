using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class FontColor : UIWebAttribute, IStyleAttribute
    {
        public FontColor(String margin) { Value = margin; }
        public override String uiText { get { return "color"; } }
        public override string elementNM { get { return Attributes.fontcolor; } }
    }
}

