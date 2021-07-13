using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RDisplayRow: WebBaseControl
    {
        public void WidthPercent(String widthPercent) { this.attributes.Add(new WidthPercent(widthPercent)); }
        public void HeightPercent(String HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
        protected override string uiText { get { return "div "; } }
        public override string endTag { get { return "</div>"; } }
        public RDisplayRow()
        {
            styleAttributes.Add(new CssDisplayType("table-row"));
        }
        public override string elementNM { get { return Elements.displayrow; } }
    }
}
