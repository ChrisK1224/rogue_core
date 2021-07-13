using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntax.segments.human;
using rogueCore.hqlSyntax.segments.select.human;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntax.MutliSegmentEnum;

namespace rogueCore.hqlSyntax.segments.select
{
    //public abstract class SelectColumn : MultiSymbolSegment<DictionaryListValues<String>, String>, ISplitSegment
    public class SelectColumn
    {
        public String columnName;
        protected string columnAliasSeparator { get; } = " AS ";
        protected string columnConcatSymbol { get; } = "&";
        protected string[] keys { get { return new String[2] { columnAliasSeparator, columnConcatSymbol }; } }
        protected ILocationColumn column { get; set; }
        HQLMetaData metaData;
        internal ColumnRowID BaseColumnID { get{return column.columnRowID;}}
        //internal string tableRefName {get{return column.colTableRefName;}}
        protected List<ILocationColumn> concatColumns = new List<ILocationColumn>();
        internal bool isConstant = false;
        public SelectColumn(String colTxt, HQLMetaData metaData) {
            this.metaData = metaData;
            var segmentItems = new MultiSymbolSegmentNew<DictionaryListValues<string>, string>(SymbolOrder.symbolbefore, colTxt, keys, metaData).segmentItems;
           // Boolean isConstant = false;
            ////*DLEETE
            //if (segmentItems["START"][0].StartsWith("EXECUTE"))
            //{
            //    string lbha = segmentItems["START"][0];
            //    string hey = "SDF";
            //}
            if (segmentItems["START"][0].StartsWith("\"{"))
            {
                metaData.encodedTableStatements.Add(metaData.currTableRefName);
                string tableRefName = "";
                ColumnRowID colId = 0;
                column = new LocationColumn(colId, tableRefName, metaData);
                isConstant = true;
            }
            else if (segmentItems["START"][0].StartsWith("\""))
            {
                column = new ConstLocationColumn(segmentItems["START"][0]);
                isConstant = true;
            }
            //else if (segmentItems["START"][0].Contains("[{") || segmentItems["START"][0].StartsWith("\"{"))
            else if (segmentItems["START"][0].Contains("[{") || segmentItems["START"][0].StartsWith("\"{"))
            {
                //string tableRefName = segmentItems["START"][0].Substring(2, segmentItems["START"][0].IndexOf("[{") - 4);
                metaData.encodedTableStatements.Add(metaData.currTableRefName);
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
                        concatColumns.Add(new ConstLocationColumn(thsLocationColumn));
                    }
                    else
                    {
                        concatColumns.Add(new LocationColumn(thsLocationColumn, metaData));
                    }
                }
            }
        } 
        internal SelectColumn(ILocationColumn thsCol) : base() {
            column = thsCol;
         }
        //* TODO fix the redudant code for GetValue its almost the same. Dictionary is used to include parent rows got to be a better way to get the single Irow to just send as sdictionary maybe
        public String GetValue(Dictionary<string,IRogueRow> rows)
        {
            String finalValue = column.CalcStringValue(rows);
            foreach (ILocationColumn concatCol in concatColumns)
            {
                finalValue += concatCol.CalcStringValue(rows);
            }
            return finalValue;
        }
        public String GetValue(IRogueRow row)
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
