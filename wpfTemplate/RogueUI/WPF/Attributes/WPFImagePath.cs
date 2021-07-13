using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace wpfTemplate.RogueUI.WPF.Attributes
{
    class WPFImagePath : WPFUIAttribute
    {
        public WPFImagePath(String path) { Value = path; }

        public override void SetAttribute(FrameworkElement thsElement)
        {
            if(thsElement is Image)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("H:\\Development\\Visual Studio\\Rogue\\Library\\rogue_core\\wpfTemplate\\pics\\" + Value + ".png");
                logo.EndInit();
                ((Image)thsElement).Source = logo;
            }
        }

        public override void SetAttribute(TextElement thsElement)
        {
            
        }
    }
}
