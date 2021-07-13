using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    public class EditableContent : UIWebAttribute
    {
        public EditableContent(String editable) { Value = editable; }
        public override string uiText { get { return " contenteditable=\"" + Value + "\""; } }
        public override string elementNM { get { return Attributes.contenteditable; } }
    }
}
