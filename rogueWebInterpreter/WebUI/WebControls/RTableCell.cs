using rogueWebInterpreter.WebUI.WebAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueWebInterpreter.WebUI.WebControls
{
    class RTableCell : BaseControl
    {
        public void WidthPercent(String widthPercent) { this.attributes.Add(new WidthPercent(widthPercent)); }
        public void ColumnSpan(String columnSpan) { this.attributes.Add(new ColumnSpan(columnSpan)); }
    }
}
