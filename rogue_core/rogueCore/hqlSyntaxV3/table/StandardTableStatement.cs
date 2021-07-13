using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using rogue_core.rogueCore.syntaxCommand;
using rogue_core.rogueCore.binary;

namespace rogueCore.hqlSyntaxV3.table
{
    class StandardTableStatement : CoreTableStatement, ITableStatement
    {
        StandardTableLocation standardTableLocation;
        public bool isEncoded { get { return false; } }
        public override string tableRefName { get { return standardTableLocation.tableRefName; } }
        public override IORecordID tableID { get { return standardTableLocation.tableID; } }
        public override string displayTableRefName => throw new NotImplementedException();
        public StandardTableStatement(string tableTxt) : base(tableTxt){ }
        protected override void InitializeFrom(string txt)
        {
            standardTableLocation =  new StandardTableLocation(txt);
        }
        public override IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            //if (tableRefName == "DDLITEMTXT")
            //{
            //    string lbha = "SFM";
            //}
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            //**Ok need to think of way to hanlde this same in encoded tye which needs parent row but in this case it needs testRow before parentROW JOIN
            foreach (IRogueRow testRow in tableID.ToTable().StreamDataRows().TakeWhile(x => rowCount != limit.limitRows))
            {
                //Stopwatch joinWatch = new Stopwatch();
                //joinWatch.Start();
                foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, testRow, snapshotRowAmount))
                {
                    //Stopwatch whereWatch = new Stopwatch();
                    //whereWatch.Start();
                    if (WhereClauseCheck(tableRefName, testRow, parentRow))
                    {
                        //Stopwatch rowWatch = new Stopwatch();
                        //rowWatch.Start();
                        yield return NewRow(tableRefName, testRow, parentRow);
                        //rowWatch.Stop();
                        //rowLoader += rowWatch.ElapsedMilliseconds;
                    }
                    //whereWatch.Stop();
                    //whereLoader += whereWatch.ElapsedMilliseconds;
                }
                //joinWatch.Stop();
                //joinLoader += joinWatch.ElapsedMilliseconds;
                rowCount++;
            }
            //encodeTimer.Stop();
            //Console.WriteLine("Stadnard Table Load time (" + this.tableRefName + "): " + encodeTimer.ElapsedMilliseconds + " AND In RowLoad: " + rowLoader.ToString() + " AND WHERE TIME: " + whereLoader.ToString() + " AND JOIN TIME" + joinLoader.ToString());
        }
        protected override void LoadFromSyntax(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            standardTableLocation.LoadSyntaxParts(parentRow, syntaxCommands);
        }
        public override void PreFill(QueryMetaData metaData)
        {
            standardTableLocation.PreFill(metaData);
            base.PreFill(metaData);
        }
        public override List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            unsets.AddRange(standardTableLocation.UnsetParams());
            unsets.AddRange(base.UnsetParams());
            return unsets;
        }
    }
}
