using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.query.insert;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.join;
using rogueCore.hqlSyntaxV3.segments.limit;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.idableLocation.encoded.table;
using rogueCore.hqlSyntaxV3.segments.select;
using rogueCore.hqlSyntaxV3.segments.table;
using rogueCore.hqlSyntaxV3.segments.where;
using rogueCore.hqlSyntaxV3.table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogue_core.rogueCore.hqlSyntaxV3.query
{
    class InsertHQLStatement : IHQLStatement,  ITableStatement
    {
        public const string splitKey = "INSERT";
        public const string intoKey = "INTO";
        public string tableRefName { get; }
        IInsertCommand insertCommand { get; }
        public string displayTableRefName => throw new NotImplementedException();
        public String parentTableRefName { get { return joinClause.parentTableRef; } }
        public IJoinClause joinClause { get; }
        WhereClauses whereClauses { get; }
        IInsertFrom iFrom { get; }
        Limit limit { get; }
        public List<ILocationColumn> IndexedWhereColumns { get { return whereClauses.evalColumns.Where(iCol => iCol.isConstant == false).ToList(); } }
        public IORecordID tableID { get; }
        protected Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts { private get; set; }
        public InsertHQLStatement(string qry)
        {
            qry = qry.Substring(intoKey.Length).Trim();
            string insertPortion = qry.BeforeFirstChar('(');
            string fromTxt = insertPortion.BeforeLastSpace();
            if (fromTxt.Contains("{"))
            {                
                var encodedTableLocation = new EncodedTableLocation(fromTxt);
                tableRefName = encodedTableLocation.tableRefName;
                iFrom = encodedTableLocation;
                //encodedTableLocation.CalcEncodedTableID(row);
                tableID = -1012;
            }
            else
            {
                var standardTableLocation = new StandardTableLocation(fromTxt);
                tableRefName = standardTableLocation.tableRefName;
                tableID = standardTableLocation.tableID;
                iFrom = standardTableLocation;
            }
            switch (insertPortion.AfterLastSpace().ToUpper())
            {
                case "JSON_VALUE":
                    insertCommand =new JsonInsert(qry);
                    break;
            }
            var segmentItems = new MultiSymbolString<StringMyList>(SymbolOrder.symbolbefore, qry, this.SplitKeys()).segmentItems;
            joinClause = this.ParseJoinClause(segmentItems.GetValue(JoinClause.splitKey));
            whereClauses = new WhereClauses(segmentItems.GetValue(WhereClauses.splitKey));
            limit = new Limit(segmentItems.GetValue(Limit.splitKey));
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            var fakeRow = new ManualBinaryRow();
            foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, fakeRow, snapshotRowAmount))
            {
                if (whereClauses.CheckRow(tableRefName, fakeRow, parentRow))
                {                                    
                    IReadOnlyRogueRow recordRow = insertCommand.Execute(parentRow, iFrom.CalcTableID(parentRow));
                    var newRow = NewRow(tableRefName, recordRow, parentRow);
                    yield return newRow;
                }
            }
            rowCount++;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            throw new NotImplementedException();
        }
        void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            throw new NotImplementedException();
        }
        void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            throw new NotImplementedException();
        }
        public void PreFill(QueryMetaData metaData)
        {
            try
            {
                joinClause.PreFill(metaData, tableRefName);
                whereClauses.PreFill(metaData, tableRefName);
                LocalSyntaxParts = StandardSyntaxParts;
                insertCommand.PreFill(metaData, tableRefName);
                iFrom.PreFill(metaData);
            }
            catch (Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            unsets.AddRange(joinClause.UnsetParams());
            unsets.AddRange(whereClauses.UnsetParams());
            unsets.AddRange(insertCommand.UnsetParams());
            return unsets;
        }
    }
}
