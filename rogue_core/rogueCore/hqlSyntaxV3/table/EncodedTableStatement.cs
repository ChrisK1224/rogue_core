using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Linq;
using rogueCore.hqlSyntaxV3.segments.from;
using hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.idableLocation.encoded.table;
using System.Threading.Tasks;
using System.Diagnostics;
using rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.syntaxCommand;
using rogue_core.rogueCore.binary;

namespace rogueCore.hqlSyntaxV3.table
{
    class EncodedTableStatement : CoreTableStatement, ITableStatement, IFrom
    {
        EncodedTableLocation encodedTableLocation;
        public bool isEncoded { get { return true; } }
        Dictionary<IORecordID, List<IReadOnlyRogueRow>> indexedEncodedRows = new Dictionary<IORecordID, List<IReadOnlyRogueRow>>();
        public override string tableRefName { get { return encodedTableLocation.tableRefName; } }
        public override IORecordID tableID { get { return -1012; } }
        public override string displayTableRefName => throw new NotImplementedException();
        public EncodedTableStatement(string tableTxt) : base(tableTxt)
        {
            //encodedTableLocation = new EncodedTableLocation(tableTxt, queryStatement);
            //EncodedToTableStream = (encodedColumn.isDirectID) ? EncodedToTableStream = EncodedDirectToTableID : EncodedNonDirectToTableID;
        }
        public override IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            //*Bring back for parallel use. IT WAS A LITTLE SLOWER for my example when using.*
            //List<List<IMultiRogueRow>> newRows = new List<List<IMultiRogueRow>>();
            //Parallel.ForEach(parentLvl.rows, row => newRows.Add(TempTableSelect(row, rowCount, NewRow)));
            //foreach(List<IMultiRogueRow> rowLst in newRows)
            //{
            //    foreach(IMultiRogueRow singleRow in rowLst)
            //    {
            //        yield return singleRow;
            //    }
            //}
            //Stopwatch encodeTimer = new Stopwatch();
            //encodeTimer.Start();
            List<IMultiRogueRow> encodedParentRows = new List<IMultiRogueRow>(parentLvl.rows);
            foreach (IMultiRogueRow parentRow in encodedParentRows)
            {
                foreach (IRogueRow testRow in StreamIRows(parentRow).TakeWhile(x => rowCount != limit.limitRows))
                {
                    if (WhereClauseCheck(tableRefName, testRow, parentRow))
                    {
                        yield return NewRow(tableRefName, testRow, parentRow);
                    }
                    rowCount++;
                }
            }
            //encodeTimer.Stop();
            //Console.WriteLine("Encoded row stream time: " + encodeTimer.ElapsedMilliseconds);
        }
        public override void PreFill(QueryMetaData metaData)
        {
            try
            {
                //if(tableRefName == "ddlItemtxt")
                //{
                //    string lbha = "SFM";
                //}
                encodedTableLocation.PreFill(metaData);
                base.PreFill(metaData);
                //joinClause.PreFill(metaData, tableRefName);
                //whereClauses.PreFill(metaData, tableRefName);
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch (Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        protected override IEnumerable<IReadOnlyRogueRow> StreamIRows(IMultiRogueRow parentRow)
        {
            IORecordID encodedTblID = encodedTableLocation.CalcTableID(parentRow);
            if (indexedEncodedRows.ContainsKey(encodedTblID))
            {
                foreach (var row in indexedEncodedRows[encodedTblID])
                {
                    yield return row;
                }
            }
            else
            {
                indexedEncodedRows.Add(encodedTblID, new List<IReadOnlyRogueRow>());
                foreach (var row in encodedTblID.ToTable().StreamDataRows())
                {
                    yield return row;
                    indexedEncodedRows[encodedTblID].Add(row);
                }
            }            
        }
        protected override void InitializeFrom(string txt)
        {
            encodedTableLocation = new EncodedTableLocation(txt);
            //tableRefName = encodedTableLocation.tableRefName.ToUpper();
        }
        public override List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            unsets.AddRange(encodedTableLocation.UnsetParams());
            unsets.AddRange(base.UnsetParams());
            return unsets;
        }
        protected override void LoadFromSyntax(IMultiRogueRow parentRow,ISyntaxPartCommands syntaxCommands)
        {
            encodedTableLocation.LoadSyntaxParts(parentRow, syntaxCommands);
        }
    }
}
