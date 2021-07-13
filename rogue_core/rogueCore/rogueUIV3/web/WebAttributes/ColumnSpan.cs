using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web.element
{
    public class ColumnSpan : UIWebAttribute
    {
        public ColumnSpan(String columnSpan) { Value = columnSpan; }

        public override string uiText { get { return " colspan=\"" + Value + "\""; } }
        public override string elementNM { get { return Attributes.columnspan; } }
    }
}
