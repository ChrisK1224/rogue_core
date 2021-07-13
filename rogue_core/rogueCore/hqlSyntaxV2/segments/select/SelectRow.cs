using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.segments.from;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.select
{
    //public abstract class SelectRow : MultiSymbolSegment<PlainList<SelectColumn>, SelectColumn>
    public class SelectRow 
    {
        public const String splitKey = "SELECT";
        const string colSeparater = ",";
        internal bool isEncoded;
        internal string origStatement;
        protected string[] keys { get { return new string[1] { colSeparater }; } }
        internal Dictionary<string, List<SelectColumn>> encodedCols = new Dictionary<string, List<SelectColumn>>();
        public Dictionary<string, SelectColumn> SelectColumns { get; protected set; } = new Dictionary<string, SelectColumn>();
        public Dictionary<string, List<SelectColumn>> TableRefIndexedColumns { get; protected set; } = new Dictionary<string, List<SelectColumn>>();

        //public List<UIDecoratedTextItem> txtItems()
        //{
        //    List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
        //    items.Add(new UIDecoratedTextItem(splitKey, "red", "bold"));
        //    List<SelectColumn> cols = SelectColumns.Values.ToList();
        //    for(int i = 0; i < cols.Count; i++)
        //    {
        //        foreach(UIDecoratedTextItem subItem in cols[i].txtItems())
        //        {
        //            items.Add(subItem);
        //        }
        //        if(i != cols.Count-1)
        //        {
        //            items.Add(new UIDecoratedTextItem(colSeparater, "red", "bold"));
        //        }
        //    }
        //    return items;
        //}

        //*This list is to indicate to multirogueRow which Seelct Columns by tables (by tableRefName) to load inially that are higher level than currLevel being loaded
        internal List<string> HigherLevelColTables = new List<string>();
        HQLMetaData metaData;
        //int levelNum;
        //internal SelectRow(String selectSegment, HQLMetaData metaData)
        //{
        //    var items = new MultiSymbolSegment<PlainList<SelectColumn>, SelectColumn>(SymbolOrder.symbolafter, selectSegment, keys, (x,y) => new SelectColumn(x,y), metaData).segmentItems;
        //    LoadColumns(items);
        //}
        /// <summary>
        /// This constructor is for select *
        /// </summary>
        /// <param name="fromInfo"></param>
        //internal SelectRow(List<From> fromInfo, HQLMetaData metaData)
        //{
        //    string allCols = GenerateAllColumnsSelect(fromInfo);
        //    var items = new MultiSymbolSegment<PlainList<SelectColumn>, SelectColumn>(SymbolOrder.symbolafter, allCols, keys,(x,y) => new SelectColumn(x,y), metaData).segmentItems;
        //    LoadColumns(items);
        //}
        internal SelectRow(string selectSegment, HQLMetaData metaData, List<IFrom> manualColumnFroms = null)
        {
            this.metaData = metaData;
            origStatement = selectSegment;
            if(selectSegment != "")
            {
                if (selectSegment == "*")
                {
                    if(manualColumnFroms == null)
                    {
                        selectSegment = GenerateAllColumnsSelect(metaData.CurrLevelFroms());
                    }
                    else
                    {
                        selectSegment = GenerateAllColumnsSelect(manualColumnFroms);
                    }
                    
                }
                if(selectSegment.Contains("{") && selectSegment.Contains("}")){
                    isEncoded = true;
                    var items = new MultiSymbolSegment<PlainList<SelectColumn>, SelectColumn>(SymbolOrder.symbolafter, selectSegment, keys, (x, y) => new SelectColumn(x, y), metaData).segmentItems;
                    LoadColumns(items);
                }
                else
                {
                    var items = new MultiSymbolSegment<PlainList<SelectColumn>, SelectColumn>(SymbolOrder.symbolafter, selectSegment, keys, (x, y) => new SelectColumn(x, y), metaData).segmentItems;
                    LoadColumns(items);
                }
            }
            HigherLevelColTables = HigherLevels(TableRefIndexedColumns.Keys.ToList());
        }
        internal List<string> HigherLevels(List<string> cols)
        {
            List<string> higherLevelNames = new List<string>();
            metaData.levelStatements.Values.ToList().Where(lvl => lvl.levelNum < metaData.currLevel.levelNum).ToList().ForEach(lvl => higherLevelNames.AddRange(lvl.allTableNames.Where(tbl => cols.Contains(tbl))));
            return cols.Where(col => higherLevelNames.Contains(col)).ToList();
        }
        static string GenerateAllColumnsSelect(List<IFrom> fromInfo)
        {
            String columnTxt = "";
            List<string> checkDupCols = new List<string>();
            foreach(IFrom from in fromInfo)
            {
                foreach (ColumnRowID thsColID in HQLEncoder.GetAllColumnsPerTable(from.tableID))
                {
                    string finalColName = thsColID.ToColumnName();
                    if (checkDupCols.Contains(finalColName))
                    {
                        finalColName = finalColName + SelectColumn.columnAliasSep + from.tableRefName + "_" + finalColName;
                    }
                    columnTxt += from.tableRefName + LocationColumn.fullColumnSplitter + finalColName + colSeparater;
                    checkDupCols.Add(thsColID.ToColumnName());
                }
            } 
            columnTxt = columnTxt.Substring(0, columnTxt.Length - 1);
            return columnTxt;
        }
        void LoadColumns(List<SelectColumn> segmentItems)
        {
            //*Should probably find better way to have this directly start as a dictionary instead of having to convert
            foreach (SelectColumn thsCol in segmentItems)
            {
                SelectColumns.Add(thsCol.columnName, thsCol);
                TableRefIndexedColumns.FindAddIfNotFound(thsCol.columnTableRefName).Add(thsCol);
                if (thsCol.isEncoded)
                {
                    encodedCols.FindAddIfNotFound(thsCol.columnTableRefName).Add(thsCol);
                }
            }
        }
        //*Row to column SELCTROWS
        internal static SelectRow PairSelectRow(HQLMetaData metaData)
        {
            return new SelectRow("", metaData);
        }
        internal static SelectRow ValueSelectRow(HQLMetaData metaData, string tableRefName)
        {
            return new SelectRow(tableRefName + ".[0] AS ROGUEVALUE", metaData);
        }
        //internal Dictionary<string, List<SelectColumn>> ModifiedEncodedColsPerTable(string tableRefName, Dictionary<string, IRogueRow> tableRefRows)
        //{
        //    var copyCols = new Dictionary<string, List<SelectColumn>>(this.TableRefIndexedColumns);
        //    List<SelectColumn> newModifiedCols = new List<SelectColumn>();
        //    foreach (var col in copyCols.TryFindReturn(tableRefName))
        //    {
        //        if (col.isEncoded)
        //        {
        //            newModifiedCols.Add(col.ModifyEncodedValues(col.GetValue(tableRefRows), tableRefName));
        //        }
        //        else
        //        {
        //            newModifiedCols.Add(col);
        //        }
        //    }
        //    copyCols[tableRefName] = newModifiedCols;
        //    return copyCols;
        //}
    }
 }
