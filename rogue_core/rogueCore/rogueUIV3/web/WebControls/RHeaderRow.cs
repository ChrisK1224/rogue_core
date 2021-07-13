using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RHeaderGroup : WebBaseControl
    {
        public void HeightPercent(String HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
        public void RowSpan(String rowSpan) { this.attributes.Add(new RowSpan(rowSpan)); }
        protected override string uiText { get { return "thead "; } }
        public override string endTag { get { return "</thead>"; } }
        public override string elementNM { get { return Elements.headertablegroup; } }
    }
}
