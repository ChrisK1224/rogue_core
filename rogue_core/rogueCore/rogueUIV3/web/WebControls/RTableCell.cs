using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RTableCell : WebBaseControl
    {
        public void WidthPercent(String widthPercent) { this.attributes.Add(new WidthPercent(widthPercent)); }
        public void ColumnSpan(String columnSpan) { this.attributes.Add(new ColumnSpan(columnSpan)); }
        protected override string uiText{ get{ return "td "; } }
        public override string endTag{ get{ return "</td>"; }  }
        public override string elementNM { get { return Elements.tablecell; } }
    }
}
