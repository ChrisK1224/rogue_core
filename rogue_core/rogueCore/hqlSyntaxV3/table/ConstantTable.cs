using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogueCore.hqlSyntaxV3.table
{
    class ConstantTable : CoreTableStatement, ITableStatement, IFrom
    {        
        public override string tableRefName { get { return constCol.constValue; } }
        ConstantColumn constCol;
        public override IORecordID tableID{ get { return 0; } }
        public override string displayTableRefName { get { return constCol.constValue; } }
        public ConstantTable(string hql) : base(hql){ }
        public override IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int snapshotRowAmount = parentLvl.rows.Count;
            IReadOnlyRogueRow testRow = new ManualBinaryRow();
            //foreach (IRogueRow testRow in tableID.ToTable().StreamIRows().TakeWhile(x => rowCount != limit.limitRows))
            //{
                foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
                {
                    if (WhereClauseCheck(tableRefName, testRow, parentRow))
                    {
                        yield return NewRow(tableRefName, testRow, parentRow);
                    }
                }
            //}
        }
        protected override void InitializeFrom(string txt)
        {
            constCol = new ConstantColumn(txt);
        }
        protected override void LoadFromSyntax(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            constCol.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public override List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            unsets.AddRange(constCol.UnsetParams());
            unsets.AddRange(base.UnsetParams());
            return unsets;
        }
    }
}
