using FilesAndFolders;
using rogue_core.rogueCore.hqlSyntaxV2.segments;
using rogue_core.rogueCore.install;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.row.encoded.manual;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.select;
using rogueCore.hqlSyntaxV2.segments.table;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace rogueCore.hqlSyntaxV2.segments.levelConversion
{
    class LevelConversion
    {
        internal const string splitter = "CONVERT";
        internal Action<List<MultiRogueRow>> ConversionFunc;
        internal Action<List<MultiRogueRow>> PreFillMod;
        ConversionTypes conversionType;
        HQLMetaData metaData;
        SelectRow selectRow;
        string baseLevelName { get; }
        int baseLevelNum { get; }
        internal LevelConversion(string hqlTxt, string levelName, int levelNum, HQLMetaData metaData) 
        {
            this.metaData = metaData;
            //this.selectRow = selectRow;
            this.baseLevelName = levelName;
            this.baseLevelNum = levelNum;
            conversionType = ConversionTypes.none;
            ConversionFunc = NoConverstion;
            PreFillMod = PreFillNone;
            if (hqlTxt != "")
            {
                selectRow = new SelectRow(stringHelper.get_string_between_2(hqlTxt, "(", ")"), metaData);
                conversionType = ConversionTypes.GetByString(hqlTxt.BeforeFirstChar('(').Trim().ToLower());
                if(conversionType == ConversionTypes.rowToColumn)
                {
                    InitalizeRowToColumn();
                }
                else if (conversionType == ConversionTypes.hierarchyTable)
                {
                    InitalizeHierarchyTable();
                    PreFillMod = PreFillHierarchyTable;
                }
                //lvlPair = new CodeFilledLevel(levelName + "_PAIR", levelNum +1, metaData);
                //metaData.AddLevelStatement(lvlPair);
                //metaData.tableRefIDs.Add(lvlPair.levelName, -1011);
                //lvlKey = new CodeFilledLevel(levelName + "_KEY", levelNum + 2, metaData); 
                //metaData.AddLevelStatement(lvlKey);
                ////metaData.tableRefIDs.Add(lvlPair.levelName, metaData.levelStatements[levelName].allTableStatements.Where(tbl => tbl.tableRefName == levelName).First().fromInfo.tableID);
                //lvlValue = new CodeFilledLevel(levelName + "_VALUE", levelNum + 2, metaData);
                //metaData.AddLevelStatement(lvlValue);
                // metaData.tableRefIDs.Add(lvlPair.levelName, metaData.levelStatements[levelName].allTableStatements.Where(tbl => tbl.tableRefName == levelName).First().fromInfo.tableID);
            }
        }
        internal static void NoConverstion(List<MultiRogueRow> thsRows) { }
        void InitalizeRowToColumn()
        {
            ConversionFunc = RowToColumn;
            //lvlPair = new CodeFilledLevel(baseLevelName + "_PAIR", baseLevelNum + 1, metaData);
            //lvlPair.allTableNames.Add(lvlPair.levelName + "_KEY");
            //lvlPair.allTableNames.Add(lvlPair.levelName + "_VALUE");
            //metaData.AddLevelStatement(lvlPair);
            //metaData.tableRefIDs.Add(lvlPair.levelName, -1011);
            //lvlKey = new CodeFilledLevel(baseLevelName + "_KEY", baseLevelNum + 2, metaData);
            //metaData.AddLevelStatement(lvlKey);
            ////metaData.tableRefIDs.Add(lvlPair.levelName, metaData.levelStatements[levelName].allTableStatements.Where(tbl => tbl.tableRefName == levelName).First().fromInfo.tableID);
            //lvlValue = new CodeFilledLevel(baseLevelName + "_VALUE", baseLevelNum + 2, metaData);
            //metaData.AddLevelStatement(lvlValue);
        }
        void InitalizeHierarchyTable()
        {
            ConversionFunc = HierarchyTable;
            //headerLvlRow = new CodeFilledLevel(baseLevelName + "_HEADER_ROW", baseLevelNum, metaData);
            //metaData.AddLevelStatement(headerLvlRow);
            //metaData.tableRefIDs.Add(headerLvlRow.levelName, -1011);
            //headerLvlRow.combinedTables.Add(new FilledTable("Root.Stock.UIDatabase.UIControlTable AS ddlItem  JOIN ON * = RESULTSTABLE_PAIR.RogueColumnID WHERE ControlName = \"textbox\"", metaData));

            //headerPair = new CodeFilledLevel(baseLevelName + "_HEADER_PAIR", baseLevelNum + 1, metaData);
            //headerPair.allTableNames.Add(headerPair.levelName + "_VALUE");
            //metaData.AddLevelStatement(headerPair);
            //metaData.tableRefIDs.Add(headerPair.levelName, -1011);

            //headerValue = new CodeFilledLevel(baseLevelName + "_HEADER_VALUE", baseLevelNum + 2, metaData);
            //metaData.AddLevelStatement(headerValue);

            //lvlPair = new CodeFilledLevel(baseLevelName + "_PAIR", baseLevelNum + 1, metaData);
            //lvlPair.allTableNames.Add(lvlPair.levelName + "_VALUE");
            //metaData.AddLevelStatement(lvlPair);
            //metaData.tableRefIDs.Add(lvlPair.levelName, -1011);

            //lvlValue = new CodeFilledLevel(baseLevelName + "_VALUE", baseLevelNum + 2, metaData);
            //metaData.AddLevelStatement(lvlValue);
        }
        void PreFillHierarchyTable(List<MultiRogueRow> parentRows)
        {
            //headerLvlRow.SetIndexes();
            //headerPair.SetIndexes();
            ////headerValue.SetIndexes();
            //foreach (MultiRogueRow parentRow in parentRows)
            //{
            //    IRogueRow topHeaderRogueRow = new ManualRogueRow(0);
            //    MultiRogueRow topHeaderRow = new MultiRogueRow(headerLvlRow.levelName, headerLvlRow.levelNum, topHeaderRogueRow, parentRow, headerLvlRow.selectRow, metaData);
            //    headerLvlRow.AddRow(topHeaderRow);
            //    foreach (var col in selectRow.SelectColumns.Values)
            //    {
            //        IRogueRow headerPairRogueRow = new ManualRogueRow(0);
            //        MultiRogueRow headerPairRow = new MultiRogueRow(headerPair.levelName, headerPair.levelNum, headerPairRogueRow, topHeaderRow, headerPair.selectRow, metaData);
            //        IRogueRow valueRogueRow = new ManualRogueRow(0);
            //        valueRogueRow.NewWritePair(0, col.columnName);
            //        headerPairRow.MergeRow(headerPair.levelName + "_VALUE", valueRogueRow, null);
            //        //MultiRogueRow headerRow = new MultiRogueRow(headerValue.levelName, headerValue.levelNum, keyRogueRow, headerPairRow, SelectRow.ValueSelectRow(metaData, headerValue.levelName), metaData);
            //        //headerValue.AddRow(headerRow);
            //        headerPair.AddRow(headerPairRow);
            //    }
            //}
            //headerLvlRow.FillCombinedTables();
            //headerPair.FillCombinedTables();
        }
        void PreFillNone(List<MultiRogueRow> parentRows)
        {
            
        }
        void RowToColumn(List<MultiRogueRow> rows)
        {
            //lvlPair.SetIndexes();
            ////lvlKey.SetIndexes();
            ////lvlValue.SetIndexes();
            //foreach(var thsRow in rows)
            //{
            //    //foreach (KeyValuePair<SelectColumn, string> kvp in thsRow.values.Values)
            //    foreach (SelectColumn col in selectRow.SelectColumns.Values)
            //    {
            //        IRogueRow pairRogueRow = new FilledHQLQuery("FROM COLUMN WHERE ROGUECOLUMNID = \"" + col.BaseColumnID + "\"").TopRows().First().baseRow;
            //        MultiRogueRow pairRow = new MultiRogueRow(lvlPair.levelName, lvlPair.levelNum, pairRogueRow, thsRow, lvlPair.selectRow, metaData);

            //        IRogueRow keyRogueRow = new ManualRogueRow(0);
            //        keyRogueRow.NewWritePair(0, col.columnName);

            //        IRogueRow valueRogueRow = new ManualRogueRow(0);
            //        valueRogueRow.NewWritePair(0, col.GetValue(thsRow.tableRefRows));

            //        pairRow.MergeRow(lvlPair.levelName + "_KEY", keyRogueRow, rows);
            //        pairRow.MergeRow(lvlPair.levelName + "_VALUE", valueRogueRow, rows);
            //        lvlPair.AddRow(pairRow);
                    
            //        //IRogueRow keyRogueRow = new ManualRogueRow(0);
            //        ////keyRogueRow.NewWritePair(kvp.Key.BaseColumnID, kvp.Key.columnName);
            //        //keyRogueRow.NewWritePair(0, kvp.Key.columnName);
            //        //MultiRogueRow columnRow = new MultiRogueRow(lvlKey.levelName, lvlKey.levelNum, keyRogueRow, pairRow, SelectRow.ValueSelectRow(metaData, lvlKey.levelName), metaData);
            //        //lvlKey.AddRow(columnRow);
                    
            //        //MultiRogueRow valueRow = new MultiRogueRow(lvlValue.levelName, lvlValue.levelNum, valueRogueRow, pairRow, SelectRow.ValueSelectRow(metaData, lvlValue.levelName), metaData);
            //        //lvlValue.AddRow(valueRow);
            //    }
            //    //thsRow.values.Clear();
            //}
            //lvlPair.FillCombinedTables();
        }
        void HierarchyTable(List<MultiRogueRow> rows)
        {
            //headerPair.SetIndexes();
            /////headerValue.SetIndexes();
            //lvlPair.SetIndexes();
            ////lvlValue.SetIndexes();
            //foreach (var thsRow in rows)
            //{
            //    //foreach (KeyValuePair<SelectColumn, string> kvp in thsRow.values.Values)
            //    foreach (SelectColumn col in selectRow.SelectColumns.Values)
            //    {
            //        IRogueRow pairRogueRow = new FilledHQLQuery("FROM COLUMN WHERE ROGUECOLUMNID = \"" + col.BaseColumnID + "\"").TopRows().First().baseRow;
            //        MultiRogueRow pairRow = new MultiRogueRow(lvlPair.levelName, lvlPair.levelNum, pairRogueRow, thsRow, lvlPair.selectRow, metaData);
            //       // lvlPair.AddRow(pairRow);

            //        //IRogueRow keyRogueRow = new ManualRogueRow(0);
            //        ////keyRogueRow.NewWritePair(kvp.Key.BaseColumnID, kvp.Key.columnName);
            //        //keyRogueRow.NewWritePair(0, kvp.Key.columnName);
            //        //MultiRogueRow columnRow = new MultiRogueRow(lvlKey.levelName, lvlKey.levelNum, keyRogueRow, pairRow, SelectRow.ValueSelectRow(metaData, lvlKey.levelName), metaData);
            //        //lvlKey.AddRow(columnRow);

            //        IRogueRow valueRogueRow = new ManualRogueRow(0);
            //        //valueRogueRow.NewWritePair(kvp.Key.BaseColumnID, kvp.Value);
            //        valueRogueRow.NewWritePair(0, col.GetValue(thsRow.tableRefRows));
            //        // valueRogueRow.NewWritePair(0, kvp.Value);
            //        pairRow.MergeRow(lvlPair.levelName + "_VALUE", valueRogueRow, rows);
            //        lvlPair.AddRow(pairRow);
            //        //MultiRogueRow valueRow = new MultiRogueRow(lvlValue.levelName, lvlValue.levelNum, valueRogueRow, pairRow, SelectRow.ValueSelectRow(metaData, lvlValue.levelName), metaData);
            //        //lvlValue.AddRow(valueRow);
            //    }
            //    //thsRow.values.Clear();
            //}
            //lvlPair.FillCombinedTables();
        }
    }
    public class ConversionTypes : EnumBaseType<ConversionTypes>
    {
        public static readonly ConversionTypes rowToColumn = new ConversionTypes(1, "rowtocolumn");
        public static readonly ConversionTypes hierarchyTable = new ConversionTypes(3, "hierarchytable");
        public static readonly ConversionTypes none = new ConversionTypes(2, "none");
        //spublic static readonly ColumnType childColumnRef = new ColumnType(3, "childColumnRef");
       
        public ConversionTypes(int key, string value) : base(key, value)
        {

        }
        public static ReadOnlyCollection<ConversionTypes> GetValues()
        {
            return GetBaseValues();
        }

        public static ConversionTypes GetByKey(int key)
        {
            return GetBaseByKey(key);
        }
    }
}
