using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class HeightPercent : UIWebAttribute, IStyleAttribute
    {
        public HeightPercent(String heightPercent) { Value = heightPercent + "%"; }
        public override string uiText { get { return "height"; } }
        public override string elementNM { get { return Attributes.heightpercent; } }
    }
}
