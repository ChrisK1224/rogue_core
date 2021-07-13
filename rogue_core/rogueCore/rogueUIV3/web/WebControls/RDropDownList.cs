using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RDropDownList : WebBaseControl
    {
        protected override string uiText { get { return "select "; } }
        public override string endTag { get { return "</select>"; } }
        public void IDName(String idName) { this.attributes.Add(new NameID(idName)); }
        public override string elementNM { get { return Elements.dropdownlist; } }
    }
}
