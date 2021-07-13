using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class FontSize : UIWebAttribute, IStyleAttribute
    {
        public FontSize(String fontSize) { Value = fontSize + "px"; }
        public override String uiText {   get{ return "font-size" ; } }
        public override string elementNM { get { return Attributes.fontsize; } }
    }
}
