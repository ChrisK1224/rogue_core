using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RHeaderCell : WebBaseControl
    {
        public void WidthPercent(String widthPercent) { this.attributes.Add(new WidthPercent(widthPercent)); }
        public void ColumnSpan(String columnSpan) { this.attributes.Add(new ColumnSpan(columnSpan)); }
        protected override string uiText { get { return "th "; } }
        public override string endTag { get { return "</th>"; } }
        public override string elementNM { get { return Elements.headertablecell; } }
    }
}
