using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.order
{
    interface IOrderBy : ITempBase
    {
        void OrderRows(List<IMultiRogueRow> rows);
    }
}
