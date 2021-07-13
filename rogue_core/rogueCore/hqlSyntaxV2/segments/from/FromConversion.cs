using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.install;
using rogue_core.rogueCore.pair;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.row.encoded.manual;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.select;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.from
{
    public class FromConversion : IFrom
    {
        public string tableRefName { get; private set; }
        public IORecordID tableID { get; private set; }
        internal const string convertSymbol = "CONVERT";
        const string convertSep = " TO ";
        Func<MultiRogueRow, IEnumerable<IRogueRow>> thsStreamIRows;
        string levelName;
        HQLMetaData metaData;
        SelectRow invertSelectRow;
        internal ConversionTypes conversionType;
        Dictionary<ColumnRowID, IRogueRow> columns = new Dictionary<ColumnRowID, IRogueRow>();
        //CONVERT ColumnEnumerations TO ROWTOCOLUMN(ColumnEnumerations.ENUMERATION_VALUE)
        internal FromConversion(string hqlTxt, HQLMetaData metaData)
        {
            this.metaData = metaData;
            var nmTxt = hqlTxt.Split(new string[1] { "AS" }, StringSplitOptions.None);
            tableRefName = nmTxt[nmTxt.Length - 1].ToUpper().Trim();
            hqlTxt = stringHelper.get_string_between_2(hqlTxt, "(", ")");
            var items = new MultiSymbolString<DictionaryValues<string>>(SymbolOrder.symbolbefore, hqlTxt, new string[2] { convertSep, From.splitTableName }, metaData).segmentItems;
            
            //commandTxt = 
            string commandTxt = items[convertSep].BeforeFirstChar('(').Trim();
            levelName = items[MutliSegmentEnum.firstEntrySymbol].Trim().ToUpper();
            invertSelectRow = new SelectRow(stringHelper.get_string_between_2(items[convertSep], "(", ")").Trim(), metaData, metaData.levelStatements[levelName].allTableStatements.Select(x => x.fromInfo).ToList());
            
            tableID = -1011;
            LoadColumns();
            //var conversionType = ConversionTypes.GetByString(commandTxt);
            conversionType = (ConversionTypes)Enum.Parse(typeof(ConversionTypes), commandTxt.ToLower());
            switch (conversionType)
            {
                case ConversionTypes.hierarchyheader:
                    thsStreamIRows = TableHeaderStreamIRows;
                    break;
                case ConversionTypes.hierarchytable:
                    thsStreamIRows = TableStreamIRows;
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
            foreach(IRogueRow row in new IORecordID(-1011).ToTable().StreamIRows())
            {
                columns.Add(row.rowID as ColumnRowID, row);
            }
        }
        IEnumerable<IRogueRow> TableStreamIRows(MultiRogueRow parentRow)
        {
            throw new Exception();
        }
        IEnumerable<IRogueRow> RowToColumnStreamIRows(MultiRogueRow parentRow)
        {
            //foreach (MultiRogueRow parentRow in metaData.levelStatements[levelName.ToUpper()].rows)
            //{
            foreach (SelectColumn col in invertSelectRow.SelectColumns.Values)
            {

                IRogueRow pairRogueRow = new ManualRogueRow(0);
                foreach(IRoguePair pair in columns[col.BaseColumnID].pairs())
                {
                    pairRogueRow.SetValue(pair.KeyColumnID, pair.DisplayValue());
                }
                pairRogueRow.SetValue(8676, col.columnName);
                pairRogueRow.SetValue(8637, parentRow.tableRefRows[col.columnTableRefName].rowID.ToString());
                //pairRogueRow.SetValue(-2, parentRow.);
                //new FilledHQLQuery("FROM COLUMN WHERE ROGUECOLUMNID = \"" + col.BaseColumnID + "\"").TopRows().First().baseRow;
                //pairRogueRow.NewWritePair(-1, col.columnName);
                pairRogueRow.SetValue(8619, col.GetValue(parentRow.tableRefRows));
                yield return pairRogueRow;
            }
            //}
        }
        IEnumerable<IRogueRow> TableHeaderStreamIRows(MultiRogueRow parentRow)
        {
            foreach (SelectColumn col in invertSelectRow.SelectColumns.Values)
            {
                IRogueRow headerRogueRow = new ManualRogueRow(0);
                headerRogueRow.NewWritePair(8619, col.columnName);
                yield return headerRogueRow;
            }
        }
        public IEnumerable<IRogueRow> StreamIRows(MultiRogueRow parentRow)
        {
            //Stopwatch stopwatch = Stopwatch.StartNew();
            return thsStreamIRows(parentRow);
            //stopw
        }
        public enum ConversionTypes { rowtocolumn, hierarchytable, hierarchyheader}
    }
    //public class ConversionTypes : EnumBaseType<ConversionTypes>
    //{
    //    public static readonly ConversionTypes rowToColumn = new ConversionTypes(1, "rowtocolumn");
    //    public static readonly ConversionTypes hierarchyTable = new ConversionTypes(3, "hierarchytable");
    //    public static readonly ConversionTypes hierarchyTableHeader = new ConversionTypes(2, "hierarchyheader");
    //    //spublic static readonly ColumnType childColumnRef = new ColumnType(3, "childColumnRef");
    //    public ConversionTypes(int key, string value) : base(key, value){ }
    //    public static ReadOnlyCollection<ConversionTypes> GetValues()
    //    {
    //        return GetBaseValues();
    //    }
    //    public static ConversionTypes GetByKey(int key)
    //    {
    //        return GetBaseByKey(key);
    //    }
    //}
}
