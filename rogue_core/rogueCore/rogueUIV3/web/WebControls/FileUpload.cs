using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class FileUpload : WebBaseControl
    {
        protected override String uiText { get { return "input id=\"files\" name=\"files\" type=\"file\" size=\"1\" multiple onchange=\"uploadFiles('files')\" "; } }
        public override string endTag { get { return "/>"; } }
        public override string elementNM { get { return Elements.fileupload; } }
    }
}
