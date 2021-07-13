using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax.segments.human
{
    //public class HumanLocationColumn : LocationColumn
    //{
    //    public HumanLocationColumn(String humanHQL)
    //    {
    //        //*This can be simplified once i fix bug to get rid of ROgueCOlumnID not showing up in regular column query
    //        if (humanHQL == "*")
    //        {
    //            isStar = true;
    //            columnRowID = -1012;
    //            colTableRefName = HumanHQLStatement.currTableRefName;
    //        }
    //        else
    //        {
    //            String[] parts = humanHQL.Split('.');
    //            String colNm;
    //            if (parts.Length == 1)
    //            {
    //                colTableRefName = HumanHQLStatement.currTableRefName;
    //                colNm = parts[0];
    //            }
    //            else
    //            {
    //                colTableRefName = parts[0];
    //                colNm = parts[1];
    //            }
    //            //* TODO terrible code TryGetValue is only here to handle hierarchytable format when this coltablerefanem doesn't exist yet for roguevalue columnID. need to inclu this column in generic rouge columns that all get queried
    //            int parentID = 0;
    //            HumanHQLStatement.tableRefIDs.TryGetValue(colTableRefName.ToUpper(), out parentID);
    //            columnRowID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(colNm, parentID));
    //        }
    //    }
    //    public HumanLocationColumn(int colID, String tableName)
    //    {
    //        //HumanHQLStatement.tableRefIDs.TryGetValue(colTableRefName.ToUpper(), out parentID);
    //        if(tableName == ""){
    //            colTableRefName = HumanHQLStatement.currTableRefName;
    //        }else{
    //            colTableRefName = tableName;
    //        }
    //        columnRowID = new ColumnRowID(colID);
    //        //colNm = HQLEncoder.GetColumnNameByID(columnRowID);
    //    }
   // }
}
