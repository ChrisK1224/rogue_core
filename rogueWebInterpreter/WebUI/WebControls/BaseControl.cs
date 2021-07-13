using rogue_core.rogueCore.UI.UIControls;
using rogue_ui.UIBuilder.Controls;
using rogueWebInterpreter.WebUI.WebAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueWebInterpreter.WebUI.WebControls
{
    public abstract class BaseControl : IUIControl
    {
        public List<IUIAttribute> attributes { get; set; } = new List<IUIAttribute>();

        ///List<IUIAttribute> IUIControl.attributes => throw new NotImplementedException();

        public void SetAttribute(string attName, string attVal)
        {
            throw new NotImplementedException();
        }
    }
}
