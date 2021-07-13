using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class BackgroundColor : UIWebAttribute, IStyleAttribute
    {
        public BackgroundColor(String backColor) { Value = ColorTranslation(backColor);  }
        public override String uiText { get { return "background-color"; } }
        public override string elementNM { get { return Attributes.backgroundcolor; } }
    }
}
