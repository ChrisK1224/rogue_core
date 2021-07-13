using FilesAndFolders;
using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using System;
using System.Collections.Generic;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.select
{
    //public abstract class SelectColumn : MultiSymbolSegment<DictionaryListValues<String>, String>, ISplitSegment
    public class SelectColumn 
    {
        public String columnName;
        internal const string columnAliasSep = " AS ";
        internal string columnTableRefName { get { return (isEncoded) ? encodedTableNm : column.colTableRefName; } }
        string columnAliasSeparator { get; } = columnAliasSep;
        protected string columnConcatSymbol { get; } = "&";
        protected string[] keys { get { return new String[2] { columnAliasSeparator, columnConcatSymbol }; } }
        internal ILocationColumn column { get; set; }
        HQLMetaData metaData;
        internal bool isEncoded = false;
        bool isAlias = false;
        internal ColumnRowID BaseColumnID { get { return column.columnRowID; } }
        public string origText { get; }
        protected List<ILocationColumn> concatColumns = new List<ILocationColumn>();
        internal Func<Dictionary<string, IRogueRow>, string> GetValue;
        string encodedTableNm;
        //public List<UIDecoratedTextItem> txtItems()
        //{
        //    List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
        //    items.AddRange(column.txtItems());
        //    //*Handle column with an alias name
        //    if (isAlias)
        //    {
        //        items.Add(new UIDecoratedTextItem(columnAliasSep, "red", "bold"));
        //        items.Add(new UIDecoratedTextItem(columnName, "black", "normal"));
        //    }
        //    //*Handle concat columns
        //    for (int i =0; i < concatColumns.Count;i++)
        //    {
        //        items.Add(new UIDecoratedTextItem(columnConcatSymbol, "red", "bold"));
        //        items.AddRange(concatColumns[i].txtItems());
        //    }
        //    return items;
        //}
        public SelectColumn(String colTxt, HQLMetaData metaData)
        {
            this.origText = colTxt;
            this.metaData = metaData;
            var segmentItems = new MultiSymbolString<DictionaryListValues<string>>(SymbolOrder.symbolbefore, colTxt, keys, metaData).segmentItems;
            Boolean isConstant = false;
            //** This is no good just for QueryDecor to ignore parameters
            if (colTxt.Contains("@") && !colTxt.StartsWith("\"")) {
                string tableRefName = "";
                ColumnRowID colId = 0;
                return;
            }
            if (colTxt.Contains("{"))
            {
                isEncoded = true;
                encodedTableNm = colTxt.Substring(0, colTxt.IndexOf("."));
                GetValue = GetValueEncoded;
            }
            else
            {
                GetValue = GetValueStandard;
            }
            ////*DLEETE
            //if (segmentItems["START"][0].StartsWith("EXECUTE"))
            //{
            //    string lbha = segmentItems["START"][0];
            //    string hey = "SDF";
            //}
            if (segmentItems["START"][0].StartsWith("\"{"))
            {
                metaData.AddEncodedTableRef(metaData.currTableRefName);
                string tableRefName = "";
                ColumnRowID colId = 0;
                column = new LocationColumn(colId, tableRefName, metaData);
                isConstant = true;
            }
            else if (segmentItems["START"][0].StartsWith("\""))
            {
                column = new ConstLocationColumn(segmentItems["START"][0], metaData);
                isConstant = true;
            }
            //else if (segmentItems["START"][0].Contains("[{") || segmentItems["START"][0].StartsWith("\"{"))
            else if (segmentItems["START"][0].Contains("[{") || segmentItems["START"][0].StartsWith("\"{"))
            {
                //string tableRefName = segmentItems["START"][0].Substring(2, segmentItems["START"][0].IndexOf("[{") - 4);
                metaData.AddEncodedTableRef(metaData.currTableRefName);
                string tableRefName = "";
                ColumnRowID colId = 0;
                column = new LocationColumn(colId, tableRefName, metaData);
            }//* TODO Should probably move a lot of this logic with { and [ and quotes to regular location column
            else if (segmentItems["START"][0].StartsWith("["))
            {
                string tableRefName = metaData.currTableRefName;
                ColumnRowID colId = new ColumnRowID(stringHelper.GetStringBetween(segmentItems["START"][0], "[", "]"));
                column = new LocationColumn(colId, tableRefName, metaData);
            }
            else
            {
                column = new LocationColumn(segmentItems["START"][0], metaData);
            }
            if (segmentItems.ContainsKey(columnAliasSeparator))
            {
                columnName = segmentItems[columnAliasSeparator][0];
                isAlias = true;
            }
            else if (isConstant == false)
            {
                columnName = column.columnRowID.ToColumnName();
            }

            if (segmentItems.ContainsKey(columnConcatSymbol))
            {
                foreach (String thsLocationColumn in segmentItems[columnConcatSymbol])
                {
                    if (thsLocationColumn.StartsWith("\""))
                    {
                        concatColumns.Add(new ConstLocationColumn(thsLocationColumn, metaData));
                    }
                    else
                    {
                        concatColumns.Add(new LocationColumn(thsLocationColumn, metaData));
                    }
                }
            }
            if (isEncoded)
            {
                var encodedCol = new LocationColumn(stringHelper.get_string_between_2(colTxt, "{", "}"), metaData);
                if (colTxt.Contains("["))
                {
                    encodedCol.isDirectID = true;
                }
                column = encodedCol;
            }
        }
        internal SelectColumn(ILocationColumn thsCol) : base()
        {
            column = thsCol;
        }
        //internal SelectColumn ModifyEncodedValues(string calcValue, string tableRefName)
        //{
        //    var col = (LocationColumn)column;
        //    //col.isDirectID = this.isDirectID;
        //    var selectCol = new SelectColumn(col.ModifyEncodedColumn(calcValue, tableRefName));
        //    selectCol.columnName = this.columnName;
        //    return selectCol;
        //}
        //* TODO fix the redudant code for GetValue its almost the same. Dictionary is used to include parent rows got to be a better way to get the single Irow to just send as sdictionary maybe
        String GetValueEncoded(Dictionary<string, IRogueRow> rows)
        {
            string metaColName = column.CalcStringValue(rows);
            var col = (LocationColumn)column;
                //col.isDirectID = this.isDirectID;
            var selectCol = col.ModifyEncodedColumn(metaColName, encodedTableNm);
                //selectCol.columnName = this.columnName;
            //string finalValue = selectCol.CalcStringValue(rows);
            return GetBaseFinalValue(selectCol, rows);
        }
        String GetValueStandard(Dictionary<string, IRogueRow> rows)
        {
            return GetBaseFinalValue(column, rows);
            
            //foreach (ILocationColumn concatCol in concatColumns)
            //{
            //    finalValue += concatCol.CalcStringValue(rows);
            //}
            //return finalValue;
        }
        string GetBaseFinalValue(ILocationColumn calcCol, Dictionary<string, IRogueRow> rows)
        {
            string finalValue = calcCol.CalcStringValue(rows);
            foreach (ILocationColumn concatCol in concatColumns)
            {
                finalValue += concatCol.CalcStringValue(rows);
            }
            return finalValue;
        }
        public String GetValueSingleRow(IRogueRow row)
        {
            String finalValue = column.CalcStringValue(row);
            foreach (ILocationColumn concatCol in concatColumns)
            {
                finalValue += concatCol.CalcStringValue(row);
            }
            return finalValue;
        }
    }
}
