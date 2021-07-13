using rogueWebInterpreter.WebUI.WebAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueWebInterpreter.WebUI.WebControls
{
    class RGroupBox : BaseControl
    {
        public void WidthPercent(String widthPercent) { this.attributes.Add(new WidthPercent(widthPercent)); }
        public void HeightPercent(String HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
    }
}
