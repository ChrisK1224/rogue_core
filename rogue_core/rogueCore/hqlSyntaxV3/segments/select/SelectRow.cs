using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using files_and_folders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    public class SelectRow 
    {
        public const String splitKey = "SELECT";
        const string colSeparater = ",";
        internal string origStatement;
        protected string[] keys { get { return new string[1] { colSeparater }; } }
        public Dictionary<string, ISelectColumn> selectColumns { get; protected set; } = new Dictionary<string, ISelectColumn>();
        public List<ISelectColumn> columnList { get; protected set; } = new List<ISelectColumn>();
        List<ISelectColOrStar> origColumnList { get; set; } = new List<ISelectColOrStar>();
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        //*This list is to indicate to multirogueRow which Seelct Columns by tables (by tableRefName) to load inially that are higher level than currLevel being loaded
        internal SelectRow(string selectSegment)
        {
            origStatement = selectSegment;
            try
            {
                if (selectSegment != "")
                {
                    origColumnList = new MultiSymbolSegment<PlainList<ISelectColOrStar>, ISelectColOrStar>(SymbolOrder.symbolafter, selectSegment, keys, GetSelectColType, "", GetOutsideQuotesAndParenthesisPattern).segmentItems;
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        internal static String GetOutsideQuotesAndParenthesisPattern(String sep)
        {
            String keyStr = "";
            if (sep.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                keyStr += "\\" + sep;
            }
            else
            {
                keyStr += sep;
            }
            //**BUG here not sure why working. shaves last char off keyStr
            keyStr = keyStr.Substring(0, keyStr.Length);
            //return "(" + keyStr + ")(?=(?:[^\"]|\"[^\"]*?\")*?$)";
            //works**(\,)(?=(?:[^\)]|\([^\)]*\))*$)(?=(?:[^\"]|\"[^\"]*\")*$)
            //***ACTUALLY AMYBE NOTTHIS MIGHT WORK BETTER( + keyStr + ) ((?![^\(]*\))|(?![^\"]*\"))
            return "(" + keyStr + @")(?= (?:[^\)]|\([^\)]*\))*$)(?= (?:[^\""]|\""[^\""]*\"")*$)";
        }
        public void PreFill(QueryMetaData metaData, string levelName)
        {
            try
            {
                int repeatColNum = 1;
                List<string> checkDupCols = new List<string>();
                foreach (var col in origColumnList)
                {
                    col.PreFill(metaData, levelName);
                    foreach (var finalCol in col.generatedColumns)
                    {
                        if (checkDupCols.Contains(finalCol.columnName))
                        {
                            finalCol.ResetColName(finalCol.columnName + "_" + repeatColNum);
                            repeatColNum++;
                        }
                        columnList.Add(finalCol);
                        selectColumns.Add(finalCol.columnName, finalCol);
                        checkDupCols.Add(finalCol.columnName);
                    }
                }
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        ISelectColOrStar GetSelectColType(string colTxt)
        {
            bool isStar = Regex.IsMatch(colTxt, MutliSegmentEnum.GetOutsideQuotesPattern( new string[1] { "*" }));
            if (isStar)
            {
                return new StarSelectColumn(colTxt);
            }
            else
            {
                return new SelectColumn(colTxt);
            }
        }
        static string GenerateAllColumnsSelect(List<IFrom> fromInfo)
        {
            String columnTxt = "";
            int repeatColNum = 1;
            List<string> checkDupCols = new List<string>();
            foreach(IFrom from in fromInfo)
            {
                foreach (ColumnRowID thsColID in BinaryDataTable.columnTable.AllColumnsPerTable(from.tableID).Select(x => x.rowID))
                {
                    string finalColName = thsColID.ToColumnName();
                    if (checkDupCols.Contains(finalColName))
                    {
                        finalColName = finalColName + IDableLocation<ColumnRowID>.columnAliasSep + from.tableRefName + "_" + finalColName;
                    }
                    columnTxt += from.tableRefName + IDableLocation<ColumnRowID>.fullColumnSplitter + finalColName + colSeparater;
                    checkDupCols.Add(thsColID.ToColumnName());
                }
            }
            columnTxt = columnTxt.Substring(0, columnTxt.Length - 1);
            return columnTxt;
        }
        public List<string> UnsetParams()
        {
            List<string> unsets = new List<string>();
            foreach(ISelectColOrStar col in origColumnList)
            {
                unsets.AddRange(col.UnsetParams());
            }
            return unsets;
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //syntaxCommands.GetLabel(parentRow, splitKey + " ", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //for (int i = 0; i < columnList.Count - 1; i++)
            //{
            //    columnList[i].LoadSyntaxParts(parentRow);
            //    syntaxCommands.GetLabel(parentRow, colSeparater + " ", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //}
            //if(columnList.Count > 0)
            //{
            //    columnList[columnList.Count - 1].LoadSyntaxParts(parentRow);
            //}
            //if()
        }
        void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, splitKey + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            for (int i = 0; i < origColumnList.Count - 1; i++)
            {
                origColumnList[i].LoadSyntaxParts(parentRow, syntaxCommands);
                syntaxCommands.GetLabel(parentRow, colSeparater + "&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            }
            if (origColumnList.Count > 0)
            {
                origColumnList[origColumnList.Count - 1].LoadSyntaxParts(parentRow, syntaxCommands);
            }
        }
        void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp; " + splitKey + " " + origStatement + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //syntaxCommands.GetLabel(parentRow, splitKey + " ", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //for (int i = 0; i < columnList.Count - 1; i++)
            //{
            //    columnList[i].LoadSyntaxParts(parentRow);
            //    syntaxCommands.GetLabel(parentRow, colSeparater + " ", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //}
            //if (columnList.Count > 0)
            //{
            //    columnList[columnList.Count - 1].LoadSyntaxParts(parentRow);
            //}
        }
    }
 }
