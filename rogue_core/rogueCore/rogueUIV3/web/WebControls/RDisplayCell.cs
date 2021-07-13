using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RDisplayCell : WebBaseControl
    {
        public void WidthPercent(String widthPercent) { this.attributes.Add(new WidthPercent(widthPercent)); }
        public void HeightPercent(String HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
        protected override string uiText { get { return "div "; } }
        public override string endTag { get { return "</div>"; } }
        public RDisplayCell()
        {
            styleAttributes.Add(new CssDisplayType("table-cell"));
            //styleAttributes.Add(new PaddingLeft("15"));
            //styleAttributes.Add(new Border("solid"));
        }
        public override string elementNM { get { return Elements.displaycell; } }
    }
}
