using System;
using System.Collections.Generic;
using System.Text;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogueCore.hqlSyntax.segments.from.human;

namespace rogueCore.hqlSyntax.segments.select.human
{
    //public class HumanSelectRow : SelectRow
    //{
    //    public const String splitKey = "SELECT";
    //    const string colSeparater = ",";
    //    protected string[] keys { get { return new string[1] { colSeparater }; } }
    //    public HumanSelectRow(String selectSegment) : base(selectSegment)
    //    {
    //        LoadColumns();
    //    }
    //    public HumanSelectRow(From fromInfo) : base(GenerateAllColumnsSelect(fromInfo)) { LoadColumns(); }
    //    void LoadColumns()
    //    {
    //        //*Should probably find better way to have this directly start as a dictionary instead of having to convert
    //        foreach (SelectColumn thsCol in segmentItems)
    //        {
    //            SelectColumns.Add(thsCol.columnName, thsCol);
    //        }
    //    }
    //    static string GenerateAllColumnsSelect(From fromInfo)
    //    {
    //        String columnTxt = "";
    //        foreach (ColumnRowID thsColID in HQLEncoder.GetAllColumnsPerTable(fromInfo.tableID))
    //        {
    //            columnTxt += thsColID.ToColumnName() + colSeparater;
    //        }
    //        columnTxt = columnTxt.Substring(0, columnTxt.Length - 1);
    //        return columnTxt;
    //    }
    //    //protected override SelectColumn ItemParse(string txt)
    //    //{
    //    //    return new HumanSelectColumn(txt);
    //    //}
    //}
}
