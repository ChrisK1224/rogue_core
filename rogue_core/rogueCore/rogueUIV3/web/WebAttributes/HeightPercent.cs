using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class HeightPixels : UIWebAttribute, IStyleAttribute
    {
        public HeightPixels(String heightPercent) { Value = heightPercent + "px"; }
        public override string uiText { get { return "height"; } }
        public override string elementNM { get { return Attributes.heightpixels; } }
    }
}
