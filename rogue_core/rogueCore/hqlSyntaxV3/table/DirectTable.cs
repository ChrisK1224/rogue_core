using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.join;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogueCore.hqlSyntaxV3.table
{
    class DirectTable : ITableStatement, IFrom
    {
        public IFrom fromInfo { get {return this; } }

        public string tableRefName { get; private set; }

        public string displayTableRefName { get; private set; }

        public string parentTableRefName { get; private set; }

        public IJoinClause joinClause { get; private set; }

        public List<ILocationColumn> IndexedWhereColumns { get; set; }

        public IORecordID tableID { get; private set; }
        public DirectTable(IORecordID tableID)
        {
            this.tableID = tableID;
            this.joinClause = new EmptyJoinClause();
            tableRefName = tableID.TableName();
            displayTableRefName = tableRefName;
            parentTableRefName = "";
        }

        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            //return tableID.ToTable().StreamIRows();
            //int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            //**Ok need to think of way to hanlde this same in encoded tye which needs parent row but in this case it needs testRow before parentROW JOIN
            foreach (IRogueRow testRow in tableID.ToTable().StreamDataRows())
            {
                foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
                //{
                //    if (WhereClauseCheck(tableRefName, testRow, parentRow))
                //    {
                        yield return NewRow(tableRefName, testRow, parentRow);
                    //}
                //}
            }
        }
        public List<string> UnsetParams()
        {
            return new List<string>();
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
           
        }
        public void PreFill(QueryMetaData metaData)
        {
            
        }
    }
}
