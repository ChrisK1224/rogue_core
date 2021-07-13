using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;
using System.Linq;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;
using rogueCore.hqlSyntaxV3.segments.table;
using rogueCore.hqlSyntaxV3.segments.namedLocation;
using rogue_core.rogueCore.syntaxCommand;
using rogue_core.rogueCore.binary;

namespace rogueCore.hqlSyntaxV3.table
{
    class ConversionTableStatement : CoreTableStatement, ITableStatement,IFrom
    {
        public bool isEncoded { get { return false; } }
        public override string tableRefName { get { return localTableRefName; } }
        public override string displayTableRefName { get { return localTableRefName; } }
        string localTableRefName;
        public override IORecordID tableID { get { return -1011; } }
        public string upperTableRefName { get; private set; }
        internal const string convertSymbol = "CONVERT";
        const string convertSep = " TO ";
        Func<IMultiRogueRow, IEnumerable<IReadOnlyRogueRow>> thsStreamIRows;
        string invertLevelName;
        //SelectHQLStatement queryStatement;
        SelectRow invertSelectRow;
        internal ConversionTypes conversionType;
        Dictionary<ColumnRowID, IReadOnlyRogueRow> columns = new Dictionary<ColumnRowID, IReadOnlyRogueRow>();
        //*Going to be issue with wrong tableID maybee?? For complexTable
        internal ConversionTableStatement(string hqlTxt) : base(hqlTxt)
        {
            //this.queryStatement = queryStatement;
        }
        public override void PreFill(QueryMetaData metaData)
        {
            try
            {
                invertSelectRow.PreFill(metaData, invertLevelName);
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
        protected override void InitializeFrom(string txt)
        {
            localTableRefName = new NamedLocation(txt).Name.ToUpper();
            upperTableRefName = localTableRefName.ToUpper();
            txt = stringHelper.get_string_between_2(txt, "(", ")");
            var items = new MultiSymbolString<DictionaryValues<string>>(SymbolOrder.symbolbefore, txt, new string[2] { convertSep, From.splitTableName }).segmentItems;
            string commandTxt = items[convertSep].BeforeFirstChar('(');
            invertLevelName = items[MutliSegmentEnum.firstEntrySymbol].Trim().ToUpper();
            invertSelectRow = new SelectRow(stringHelper.get_string_between_2(items[convertSep], "(", ")"));
            //invertSelectRow = new SelectRow(stringHelper.get_string_between_2(items[convertSep], "(", ")"), queryStatement.ParentLevelByChildName(invertLevelName).allTableStatements.Select(x => x.fromInfo).ToList());           
            LoadColumns();
            conversionType = (ConversionTypes)Enum.Parse(typeof(ConversionTypes), commandTxt.ToLower());
            switch (conversionType)
            {
                case ConversionTypes.hierarchyheader:
                    thsStreamIRows = TableHeaderStreamIRows;
                    break;
                case ConversionTypes.rowtocolumn:
                    thsStreamIRows = RowToColumnStreamIRows;
                    break;
                default:
                    throw new Exception("Unknown Conversion Type");
            }
        }
        void LoadColumns()
        {
            foreach (IReadOnlyRogueRow row in new IORecordID(-1011).ToTable().StreamDataRows())
            {
                columns.Add(row.rowID as ColumnRowID, row);
            }
            columns.Add(0, new ManualBinaryRow());
        }
        IEnumerable<IRogueRow> RowToColumnStreamIRows(IMultiRogueRow parentRow)
        {
            foreach(IRogueRow thsRow in parentRow.InvertRow(invertSelectRow, columns, complexWordTable))
            {
                yield return thsRow;
            }
        }
        IEnumerable<IReadOnlyRogueRow> TableHeaderStreamIRows(IMultiRogueRow parentRow)
        {
            foreach (ISelectColumn col in invertSelectRow.selectColumns.Values)
            {
                var headerRogueRow = new ManualBinaryRow();
                headerRogueRow.AddPair(8619, col.columnName);
                yield return headerRogueRow;
            }
        }
        public new IEnumerable<IReadOnlyRogueRow> StreamIRows(IMultiRogueRow parentRow)
        {
            return thsStreamIRows(parentRow);
        }
        public override  IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILevelStatement parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            foreach (IMultiRogueRow parentRow in parentLvl.rows)
            {
                foreach (IRogueRow testRow in StreamIRows(parentRow).TakeWhile(x => rowCount != limit.limitRows))
                {
                    if (whereClauses.CheckRow(upperTableRefName, testRow, parentRow))
                    {
                        yield return NewRow(upperTableRefName, testRow, parentRow);
                    }
                }
                rowCount++;
            }
        }
        protected override void LoadFromSyntax(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, convertSymbol + "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            syntaxCommands.GetLabel(parentRow, invertLevelName, IntellsenseDecor.MyColors.black);
            syntaxCommands.GetLabel(parentRow, convertSep + conversionType.ToString().ToUpper() + "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            invertSelectRow.LoadSyntaxParts(parentRow, syntaxCommands);
            syntaxCommands.GetLabel(parentRow, ")", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
        }
        public override List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            unsets.AddRange(invertSelectRow.UnsetParams());
            unsets.AddRange(base.UnsetParams());
            return unsets;
        }
        public enum ConversionTypes { rowtocolumn, hierarchytable, hierarchyheader }
    }
}
