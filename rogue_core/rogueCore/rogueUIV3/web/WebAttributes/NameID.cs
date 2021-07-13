using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class NameID : UIWebAttribute
    {
        public NameID(String nameID) { Value = nameID; }
        public override string uiText { get{ return " id=\"" + Value + "\" Name=\"" + Value + "\""; } }
        public override string elementNM { get { return Attributes.idname; } }
    }
}
