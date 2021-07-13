using rogue_core.rogueCore.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueWebInterpreter.WebUI.WebAttributes
{
    public class FontSize : IUIAttribute
    {
        public FontSize(String fontSize) { Value = fontSize; }
    }
}
