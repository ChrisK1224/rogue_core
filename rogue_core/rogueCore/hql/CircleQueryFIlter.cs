using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FilesAndFolders;
using rogue_core.RogueCode.hql;
using rogue_core.rogueCore.hql;
using rogue_core.rogueCore.hql.hqlSegments.join;
using rogue_core.rogueCore.hql.hqlSegments.where;
using rogue_core.rogueCore.hql.segments.tableInfo;
using rogue_core.rogueCore.hqlFilter;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;

namespace rogue_core.rogueCore.hql.hqlSegments
{
    //public class CircleQueryFilter
    //{
    //    public int repeatTxtLvlCount;
    //    //public int currLevelNum;
    //    String EncodedSeparatorType;
    //    public String repeatTableText;
    //    public RelativeColumnLocation parentColumnToMatch;
    //    public WhereClauses firstWhereClauses;
    //    public JoinClauses firstJoinClauses;
    //    public TableInfo tableInfo;
    //    //public String repeatHqlTxt;
    //    //public IORecordID circleTableID;
    //    public RelativeColumnLocation localColumnTableRef;
    //    public LevelNum StartLvlNum;
    //    public List<int> startParentValues = new List<int>();
    //    List<String> repeatTableSegments = new List<String>();
    //    String encodedSelectCols;
    //    // public CircleQueryFilter(String qryTxt, FullColumnLocations selectCols)
    //    // {
    //    //     // if (qryTxt.Trim() == "")
    //    //     // {
    //    //     //     parentColumnToMatch = null;
    //    //     // }
    //    //     // else
    //    //     // {
    //    //         this.selectCols = selectCols;
    //    //         String colTxt = stringHelper.get_string_between_first_occurs(qryTxt, "{", "}");
    //    //         String[] colsTxt = colTxt.Split('=');
    //    //         //int lastPeriodInd = colTxt.LastIndexOf(".");
    //    //         localColumnToMatchOn = new RelativeColumnLocation(colsTxt[0]);
    //    //         parentColumnToMatch = new RelativeColumnLocation(colsTxt[1]);
    //    //         //String startParentTxt = stringHelper.GetStringBetween(qryTxt,"{","}");
    //    //         // foreach(String thsStart in startParentTxt.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //    //         // {
    //    //         startParentValues.Add(-1004);
    //    //         //}
    //    //         repeatHqlTxt = stringHelper.get_string_between_2(qryTxt, "[", "]");
    //    //         repeatTxtLvlCount = Regex.Matches(repeatHqlTxt, "#").Count;
    //    //     //}
    //    // }
    //    public CircleQueryFilter(String fulltxt)
    //    {
    //        //@#|1|-1026|{*=0.0.-1012}|[-1016=0]|0.-1012|0.-1016| # {*=0.0.-1012} / 0.-1026[-2006=treeviewnode] & 1.-1026[-2006=textbox] / @
    //        string[] vals = fulltxt.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
    //        StartLvlNum = int.Parse(vals[0]);
    //        tableInfo = new TableInfo(int.Parse(vals[1]), "");
    //        firstJoinClauses = new JoinClauses(vals[2]);
    //        firstWhereClauses = new WhereClauses(vals[3]);
    //        parentColumnToMatch = new RelativeColumnLocation(vals[4]);
    //        localColumnTableRef= new RelativeColumnLocation(vals[5]);
    //        encodedSelectCols = vals[6];
    //        repeatTableText = vals[7].Trim();
    //        foreach(String thsTableSegment in repeatTableText.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries))
    //        {
    //            repeatTableSegments.Add(" ^ " + thsTableSegment);
    //        }
    //        //repeatHqlTxt = vals[8];
    //        repeatTxtLvlCount = Regex.Matches(repeatTableText, "#").Count + 1;
    //    }
    //    public FullColumnLocations DecodedSelectCols(int matrixTableNum)
    //    {
    //        return new FullColumnLocations(DecodeQry(encodedSelectCols, matrixTableNum));
    //    }
    //    public static String DecodeQry(String decodedStr, int matrixTableNum)
    //    {
    //        decodedStr = decodedStr.Replace("%", matrixTableNum.ToString());
    //        decodedStr = decodedStr.Replace("<-1>", (matrixTableNum - 1).ToString());
    //        decodedStr = decodedStr.Replace("<-2>", (matrixTableNum - 2).ToString());
    //        decodedStr = decodedStr.Replace("<-3>", (matrixTableNum - 3).ToString());
    //        decodedStr = decodedStr.Replace("<-4>", (matrixTableNum - 4).ToString());
    //        decodedStr = decodedStr.Replace("<+1>", (matrixTableNum + 1).ToString());
    //        decodedStr = decodedStr.Replace("<+2>", (matrixTableNum + 2).ToString());
    //        decodedStr = decodedStr.Replace("<+3>", (matrixTableNum + 3).ToString());
    //        decodedStr = decodedStr.Replace("<+4>", (matrixTableNum + 4).ToString());
    //        //decodedStr = decodedStr.Replace("#", "^");
    //        return decodedStr;
    //    }
    //    public FullColumnLocations DecodedJoinClause(int matrixTableNum)
    //    {
    //        String decodedCols = encodedSelectCols.Replace("%", matrixTableNum.ToString());
    //        decodedCols = decodedCols.Replace("<-1>", (matrixTableNum - 1).ToString());
    //        decodedCols = decodedCols.Replace("<-2>", (matrixTableNum - 2).ToString());
    //        decodedCols = decodedCols.Replace("<-3>", (matrixTableNum - 3).ToString());
    //        decodedCols = decodedCols.Replace("<-4>", (matrixTableNum - 4).ToString());
    //        return new FullColumnLocations(decodedCols);
    //    }
    //    public List<String> DecodedRepeatTableText(int matrixTableNum)
    //    {
    //        List<String> decodedSegments = new List<String>();
    //        foreach(String thsSeg in repeatTableSegments)
    //        {
    //            decodedSegments.Add(DecodeQry(thsSeg, matrixTableNum));
    //        }
    //        return decodedSegments;
    //    }
    //}
}