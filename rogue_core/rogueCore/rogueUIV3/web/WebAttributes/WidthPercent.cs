using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class WidthPixels : UIWebAttribute, IStyleAttribute
    {
        public WidthPixels(String widthPercent) { Value = widthPercent + "px"; }
        public override string uiText { get{ return "width"; } }
        public override string elementNM { get { return Attributes.widthpixels; } }
    }
}
