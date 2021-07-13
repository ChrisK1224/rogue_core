using rogue_core.rogueCore.UI.UIControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFBackgroundColor : WPFUIAttribute
    {
        public WPFBackgroundColor(String backColor) { Value = backColor; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            var bc = new BrushConverter();
            if(thsElement is Panel){
                ((Panel)thsElement).Background = (Brush)bc.ConvertFrom(ColorTranslation(Value));
            }
            else if(thsElement is Control)
            {
                ((Control)thsElement).Background = (Brush)bc.ConvertFrom(ColorTranslation(Value));
            }
            
            //myTextBox.Background = (Brush)bc.ConvertFrom(Value);

        }

        public override void SetAttribute(TextElement thsElement)
        {
            var bc = new BrushConverter();
            ((TextElement)thsElement).Background = (Brush)bc.ConvertFrom(Value);
            
        }
    }
}
