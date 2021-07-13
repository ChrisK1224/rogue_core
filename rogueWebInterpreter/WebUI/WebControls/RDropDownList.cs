using rogueWebInterpreter.WebUI.WebAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueWebInterpreter.WebUI.WebControls
{
    class RDropDownList : BaseControl
    {
        public void IDName(String idName)
        {
            this.attributes.Add(new NameID(idName));
        }
    }
}
