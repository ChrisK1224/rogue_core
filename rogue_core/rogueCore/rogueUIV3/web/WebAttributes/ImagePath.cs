using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class ImagePath : UIWebAttribute
    {
        public ImagePath(String imgPath) { Value = imgPath; }
        public override String uiText { get { return " src='images/" + Value + ".png'"; } }
        public override string elementNM { get { return Attributes.imagepath; } }
    }
}
