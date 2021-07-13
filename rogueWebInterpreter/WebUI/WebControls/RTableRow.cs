using rogueWebInterpreter.WebUI.WebAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueWebInterpreter.WebUI.WebControls
{
    class RTableRow : BaseControl
    {
        public void HeightPercent(String HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
        public void RowSpan(String rowSpan) { this.attributes.Add(new RowSpan(rowSpan)); }
    }
}
