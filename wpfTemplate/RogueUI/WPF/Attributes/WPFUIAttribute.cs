using rogue_core.rogueCore.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    public abstract class WPFUIAttribute : IUIAttribute
    {
        public abstract void SetAttribute(FrameworkElement thsElement);
        public abstract void SetAttribute(TextElement thsElement);
    }
}
