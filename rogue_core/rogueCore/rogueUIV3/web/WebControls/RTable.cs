using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RTable : WebBaseControl
    {
        protected override string uiText {  get{ return "table "; } }
        public override string endTag { get{ return "</table>"; } }
        public override string elementNM { get { return Elements.table; } }
    }
}
