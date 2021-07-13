using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntax.segments.human;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax.segments.select.human
{
    //public class HumanSelectColumn : SelectColumn
    //{
    //    protected override string columnAliasSeparator {get;} = " AS ";
    //    protected override string columnConcatSymbol {get;} = "&";
    //    protected override string[] keys { get { return new String[2] { columnAliasSeparator, columnConcatSymbol }; } }
    //    public HumanSelectColumn(String colTxt) : base(colTxt) 
    //    {
    //        Boolean isConstant = false;
    //        if (segmentItems["START"][0].StartsWith("\""))
    //        {
    //            column = new HumanConstLocationColumn(segmentItems["START"][0]);
    //            isConstant = true;
    //        }
    //        else if(segmentItems["START"][0].Contains("["))
    //        {
    //            string tableRefName = segmentItems["START"][0].Substring(0, segmentItems["START"][0].IndexOf("[")-1);
    //            ColumnRowID colId = new ColumnRowID(stringHelper.GetStringBetween(segmentItems["START"][0], "[","]"));
    //            column = new HumanLocationColumn(colId, tableRefName);
    //        }
    //        else
    //        {
    //            column = new HumanLocationColumn(segmentItems["START"][0]);
    //        }
    //        if(segmentItems.ContainsKey(columnAliasSeparator))
    //        {
    //            columnName = segmentItems[columnAliasSeparator][0];
    //        }
    //        else if(isConstant == false)
    //        {
    //            columnName = column.columnRowID.ToColumnName();
    //        }
    //        if (segmentItems.ContainsKey(columnConcatSymbol))
    //        {
    //            foreach(String thsLocationColumn in segmentItems[columnConcatSymbol])
    //            {
    //                if (thsLocationColumn.StartsWith("\""))
    //                {
    //                    concatColumns.Add(new HumanConstLocationColumn(thsLocationColumn));
    //                }
    //                else
    //                {
    //                    concatColumns.Add(new HumanLocationColumn(thsLocationColumn));
    //                }
    //            }
    //        }
    //        //string[] additionalParts = colTxt.Split(new char[] { '&', }, StringSplitOptions.RemoveEmptyEntries);
    //        //String constValue = "";
    //        //String columnAliasName = "";
    //        //String[] columnParts = additionalParts[0].Trim().Split(new char[0]);
    //        //columnParts[0] = columnParts[0].Trim();
    //        //ILocationColumn thsCol;
    //        //if (columnParts[0].StartsWith("\""))
    //        //{
    //        //    constValue = columnParts[0].Substring(1, columnParts[0].Length - 2);
    //        //    thsCol = new ConstantValue(constValue, tableRefName);
    //        //}
    //        //else
    //        //{
    //        //    if (!columnParts[0].Contains("."))
    //        //    {
    //        //        thsCol = new LocationColumn(tableRefName, HQLEncoder.GuessColumnIDByName(columnParts[0], tableRefNameIDs[tableRefName]));
    //        //    }
    //        //    else
    //        //    {
    //        //        thsCol = LocationColumn.HumanToEncodedHQL(columnParts[0], tableRefNameIDs);
    //        //    }
    //        //}
    //        //if (columnParts.Length > 1)
    //        //{
    //        //    columnAliasName = columnParts[2];
    //        //}
    //        //List<ILocationColumn> comboColumns = new List<ILocationColumn>();
    //        //for (int i = 1; i < additionalParts.Length; i++)
    //        //{
    //        //    additionalParts[i] = additionalParts[i].Trim();
    //        //    if (additionalParts[i].Trim().StartsWith("\""))
    //        //    {
    //        //        comboColumns.Add(new ConstantValue(additionalParts[i].TrimFirstAndLastChar(), tableRefName));
    //        //    }
    //        //    else
    //        //    {
    //        //        comboColumns.Add(LocationColumn.HumanToEncodedHQL(additionalParts[i], tableRefNameIDs));
    //        //    }
    //        //}
    //    }
    //    protected override string ItemParse(string txt) { return txt;}
    //}
}
