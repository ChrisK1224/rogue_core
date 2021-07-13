using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3
{
    class UIQueryResultsBuilder
    {
        SelectHQLStatement qry;
        List<IMultiRogueRow> currRows = new List<IMultiRogueRow>();
        public UIQueryResultsBuilder(string query)
        {
            qry = new SelectHQLStatement(query);
            
            //foreach (var topRow in qry.TopRows())
            //{
            //    var rowNameLabel = syntaxCommands.GetLabel(parentRow, topRow.levelName + "|", IntellsenseDecor.MyColors.blue);
            //    foreach(var valPair in topRow.GetValueList())
            //    {
            //        syntaxCommands.GetLabel(parentRow, valPair.Key + ":" + valPair.Value, IntellsenseDecor.MyColors.black);
            //    }
            //}
            //foreach (var tbl in combinedTables)
            //{
            //    syntaxCommands.BreakLine(divRow);
            //    syntaxCommands.GetLabel(divRow, lvlSplitKey, IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //    tbl.LoadSyntaxParts(divRow);
            //}
            //syntaxCommands.BreakLine(divRow);
            //selectRow.LoadSyntaxParts(divRow);
            
        }
        public void RowLoop(IMultiRogueRow dataRow)
        {
            var divRow = IntellsenseDecor.IndentedGroupbox(currRows[currRows.Count - 1], 0);
            currRows.Add(divRow);
            var rowNameRow = IntellsenseDecor.MyLabel(divRow, dataRow.levelName + "&nbsp;|", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            foreach (var valPair in dataRow.GetValueList())
            {
                IntellsenseDecor.MyLabel(divRow, "&nbsp;" + valPair.Key + ":", IntellsenseDecor.MyColors.blue);
                IntellsenseDecor.MyLabel(divRow, "&nbsp;" + valPair.Value + " ,", IntellsenseDecor.MyColors.black);
            }
            //syntaxCommands.BreakLine(currRows[currRows.Count - 2]);
            //foreach (var childRow in dataRow.childRows)
            //{                
            //    RowLoop(childRow);
            //}            
        }
        public void EndRowLoop(IMultiRogueRow dataRow)
        {
            currRows.RemoveAt(currRows.Count - 1);
        }
        public IMultiRogueRow Build(IMultiRogueRow parentRow)
        {
            qry.Fill();
            currRows.Add(parentRow);
            qry.IterateRows(RowLoop, EndRowLoop);
            return parentRow;
        }
    }
}
