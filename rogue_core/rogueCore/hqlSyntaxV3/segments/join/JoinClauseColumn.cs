using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.segments.join
{
    //class JoinClauseColumn : StandardColumn
    //{
    //    public JoinClauseColumn(string colTxt) : base(colTxt)
    //    {
    //        colTableRefName = colTxt.Split(".")[0].ToUpper();
    //    }
    //    public override void PreFill(QueryMetaData metaData, string assumedTblName)
    //    {
    //        if (isDirectID)
    //        {
    //            ID = new ColumnRowID(items[0]);
    //            name = aliasName.isNameSet ? aliasName.Name : ID.ToColumnName();
    //        }
    //        else
    //        {
    //            int ownerTableID = metaData.GetTableIDByRefName(colTableRefName);
    //            ID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(items[items.Length - 1], ownerTableID));
    //        }
    //        name = name.ToUpper();
    //    }
    //}
}
