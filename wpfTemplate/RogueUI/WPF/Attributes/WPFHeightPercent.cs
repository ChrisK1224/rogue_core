using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using wpfTemplate.RogueUI.WPF.Controls;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFHeightPercent : WPFUIAttribute
    {
        public WPFHeightPercent(String height) { Value = height; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
           // thsElement.Height = double.Parse(Value);
           if(thsElement is Grid)
            {
                Grid thsGrid = (Grid)thsElement;
                thsGrid.RowDefinitions[thsGrid.RowDefinitions.Count - 1].Height = new GridLength(double.Parse(Value), GridUnitType.Star);
            }
        }
        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
