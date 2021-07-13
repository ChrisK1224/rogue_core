using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class RNavBar : WebBaseControl
    {
        protected override string uiText { get { return "nav class=\"navbar navbar-expand-sm bg-dark navbar-dark\"> <a class=\"navbar-brand\" href=\"#\">HQL</a>  <ul class=\"navbar-nav\""; } }
        public override string endTag { get { return "</ul></nav>"; } }
        public override string elementNM { get { return Elements.navbar; } }
    }
}
