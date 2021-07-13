using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RTableRow : WebBaseControl
    {
        public void HeightPercent(String HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
        public void RowSpan(String rowSpan) { this.attributes.Add(new RowSpan(rowSpan)); }
        protected override string uiText { get { return "tr "; } }
        public override string endTag { get { return "</tr>"; } }
        public override string elementNM { get { return Elements.tablerow; } }
    }
}
