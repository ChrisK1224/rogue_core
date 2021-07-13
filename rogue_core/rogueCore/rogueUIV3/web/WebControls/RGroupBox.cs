using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    class RGroupBox : WebBaseControl
    {
        public void WidthPercent(String widthPercent) { this.attributes.Add(new WidthPercent(widthPercent)); }
        public void HeightPercent(String HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
        protected override string uiText { get { return "div "; } }
        public override string endTag { get { return "</div>"; } }
        public override string elementNM { get { return Elements.groupbox; } }
    }
}
