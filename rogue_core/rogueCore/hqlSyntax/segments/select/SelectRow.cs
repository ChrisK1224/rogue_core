using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogueCore.hqlSyntax.segments.from;
using static rogueCore.hqlSyntax.MutliSegmentEnum;

namespace rogueCore.hqlSyntax.segments.select
{
    //public abstract class SelectRow : MultiSymbolSegment<PlainList<SelectColumn>, SelectColumn>
    public class SelectRow
    {
        public const String splitKey = "SELECT";
        const string colSeparater = ",";
        protected string[] keys { get { return new string[1] { colSeparater }; } }
        public Dictionary<string, SelectColumn> SelectColumns { get; protected set; } = new Dictionary<string, SelectColumn>();
        internal SelectRow(String selectSegment, HQLMetaData metaData)
        {
            var items = new MultiSymbolSegmentNew<PlainList<SelectColumn>, SelectColumn>(SymbolOrder.symbolafter, selectSegment, keys, metaData).segmentItems;
            LoadColumns(items);
        }
        /// <summary>
        /// This constructor is for select *
        /// </summary>
        /// <param name="fromInfo"></param>
        internal SelectRow(From fromInfo, HQLMetaData metaData)
        {
            string allCols = GenerateAllColumnsSelect(fromInfo);
            var items = new MultiSymbolSegmentNew<PlainList<SelectColumn>, SelectColumn>(SymbolOrder.symbolafter, allCols, keys, metaData).segmentItems;
            LoadColumns(items);
        }
        static string GenerateAllColumnsSelect(From fromInfo)
        {
                String columnTxt = "";
                foreach (ColumnRowID thsColID in HQLEncoder.GetAllColumnsPerTable(fromInfo.tableID))
                {
                    columnTxt += thsColID.ToColumnName() + colSeparater;
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
            }
        }
    }
 }
