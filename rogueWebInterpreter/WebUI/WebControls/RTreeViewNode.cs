using rogueWebInterpreter.WebUI.WebAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueWebInterpreter.WebUI.WebControls
{
    public class RTreeViewNode : BaseControl
    {
        public void Text(String text)
        {
            this.attributes.Add(new Text(text));
        }
        public void IDName(String idName)
        {
            this.attributes.Add(new NameID(idName));
        }
        public void MouseClick(String mouseClickEvent) { this.attributes.Add(new MouseClick(mouseClickEvent)); }
        public void MouseDoubleClick(String mouseDoubleEvent) { this.attributes.Add(new MouseDoubleClick(mouseDoubleEvent)); }
    }
}
