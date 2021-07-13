using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV2.segments
{
    public interface ILocationColumn
    {
        String CalcStringValue(IRogueRow thsRow);
        String CalcStringValue(Dictionary<String,IRogueRow> rows);
        ColumnRowID columnRowID { get; set; }
        String colTableRefName { get;  }
        bool isConstant { get; }
        //public List<UIDecoratedTextItem> txtItems();
    }
}
