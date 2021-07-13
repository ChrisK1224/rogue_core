using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.from;
using rogueCore.hqlSyntaxV3.segments.fullExecutable;
using rogueCore.hqlSyntaxV3.segments.snippet;
using rogueCore.hqlSyntaxV3.segments.table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;
using files_and_folders;
using rogueCore.hqlSyntaxV3.segments.level;
using rogueCore.hqlSyntaxV3.group;
using System.Text;
using rogueCore.rogueUIV3;
using LevelGroup = rogueCore.hqlSyntaxV3.segments.table.LevelGroup;
using rogue_core.rogueCore.hqlSyntaxV3;
using System.Windows.Automation.Peers;
using FilesAndFolders;
using static rogueCore.hqlSyntaxV3.IntellsenseDecor;
using rogue_core.rogueCore.syntaxCommand;
using System.Data;
using rogue_core.rogueCore.hqlSyntaxV3.query;

namespace rogueCore.hqlSyntaxV3.segments
{
    public class SelectHQLStatement : IHQLStatement
    {
        internal ILevelStatement currLevel;
        const string startComment = "#--";
        const string endComment = "--#";
        string splitter { get { return "|"; } }
        protected string[] keys { get { return new string[1] { splitter }; } }
        protected List<ILevelGroup> groups { get; set; }
        public string finalQuery { get; private set; }
        protected bool isExecutable = false;
        protected string executableName;
        int iterateRowCount = 0;
        Dictionary<string, ILevelStatement> levelStatements { get; set; } = new Dictionary<string, ILevelStatement>();
        Dictionary<string, ITableStatement> tableStatements = new Dictionary<string, ITableStatement>();
        internal ILevelStatement rootLevel;
        internal string nonModifiedQry { get; private set; }
        public Dictionary<string, string> parameters;
        public List<string> UnsetParams { get; private set; } = new List<string>();
        string origQry { get; }
        public SelectHQLStatement(string hqlTxt, Dictionary<string,string> parameters = null) 
        {
            origQry = hqlTxt;
            try
            {
                hqlTxt = hqlTxt.Trim();
                if (hqlTxt.StartsWith("\"") && hqlTxt.EndsWith("\""))
                {
                    hqlTxt = hqlTxt.Trim().Substring(1, hqlTxt.Length - 2);
                }
                int queryId = 0;
                bool result = int.TryParse(hqlTxt, out queryId);
                if (result)
                {
                    hqlTxt = SelectHQLStatement.GetQueryByID(queryId);
                }
                origQry = hqlTxt;
                this.parameters = parameters;
                rootLevel = LevelStatement.MasterLevel();
                nonModifiedQry = hqlTxt;
                if (hqlTxt.Trim().ToUpper().StartsWith("EXECUTE("))
                {
                    FullExecutable exec = new FullExecutable(hqlTxt.Trim());
                    isExecutable = true;
                    executableName = exec.executableName;
                    hqlTxt = exec.paramQry;
                }
                hqlTxt = hqlTxt.Replace(Environment.NewLine, " ");
                hqlTxt = RemoveComments(hqlTxt);
                hqlTxt = FixSingleGroupText(hqlTxt);
                finalQuery = SetQueryParam(hqlTxt, parameters);
                groups = new MultiSymbolSegment<PlainList<ILevelGroup>, ILevelGroup>(SymbolOrder.symbolbefore, finalQuery, keys, (x) => new LevelGroup(x, parameters)).segmentItems;
                SetUnsetParams();
                PreLoad();
                }
                catch(Exception ex)
                {
                    string predLoad = ex.ToString();
                }
        }
        internal SelectHQLStatement()
        {
            rootLevel = LevelStatement.MasterLevel();
        }
        internal static string GetQueryByID(int storedProcID)
        {
            var filledQry = new SelectHQLStatement("FROM HQL_QUERIES WHERE ROGUECOLUMNID = \"" + storedProcID.ToString() + "\" SELECT * ");
            filledQry.Fill();
            return filledQry.TopRows().First().GetValue("QUERY_TXT");
        }        
        static String FixSingleGroupText(String humanHQL)
        {
            if (!humanHQL.Trim().StartsWith("|"))
            {
                humanHQL = "|" + humanHQL;
                humanHQL += " FORMAT STANDARD AS ROGUEDEFAULT";
            }
            return humanHQL;
        }
        protected static string RemoveComments(string original)
        {
            string pattern = startComment + "(.*?)" + endComment;
            Regex regex = new Regex(pattern, RegexOptions.RightToLeft);
            foreach (Match match in regex.Matches(original))
            {
                original = original.Replace(startComment + match.Groups[1].Value + endComment, string.Empty);
            }
            return original;
        }
        string SetQueryParam(string hqlTxt, Dictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                foreach (var pair in parameters)
                {
                    hqlTxt = hqlTxt.Replace(pair.Key, pair.Value);
                }
            }
            return hqlTxt;
        }
        //*Used for select row *
        //internal List<IFrom> CurrLevelFroms()
        //{
        //    return currLevel.allTableStatements.Select(x => x.fromInfo).ToList();
        //}
        internal void AddLevelStatement(ILevelStatement newStatement)
        {
            currLevel = newStatement;
            levelStatements.Add(newStatement.levelName.ToUpper(), newStatement);
        }
        internal void AddTableStatement(ITableStatement newTable)
        {
            //currTable = newTable;
            tableStatements.FindChangeIfNotFound(newTable.tableRefName.ToUpper(), newTable);
        }
        //** This method gets a list of all columnRowIDs that are referenced anywhere in the join or where clause of a query to tell the FilledTabe to index these values
        internal List<ILocationColumn> RefIndexesByTable(string tableRefName)
        {
            List<ILocationColumn> indexes = new List<ILocationColumn>();
            tableStatements.Values.Where(tbl => !tbl.joinClause.joinAll && tbl.joinClause.parentTableRef == tableRefName).ToList().ForEach(tbl => indexes.Add(tbl.joinClause.parentColumn));
            foreach (ITableStatement thsTbl in tableStatements.Values)
            {
                thsTbl.IndexedWhereColumns.Where(col => col.colTableRefName == tableRefName).ToList().ForEach(col => indexes.Add(col));
            }
            indexes = indexes.Distinct().ToList();
            return indexes;
        }
        //**Might have to split diction to include table name and use table name in elvel fill
        //internal Dictionary<string,List<ILocationColumn>> LevelIndexedRows(List<ITableStatement> tables)
        //{
        //    var indexesPerTbl = new Dictionary<string, List<ILocationColumn>>();            
        //    foreach (var thsTable in tables)
        //    {
        //        List<ILocationColumn> indexes = new List<ILocationColumn>();
        //        //*All table statements that have tableRefName as parent Get their parentColumn
        //        tableStatements.Values.Where(tbl => !tbl.joinClause.joinAll && tbl.joinClause.parentTableRef == thsTable.tableRefName).ToList().ForEach(tbl => indexes.Add(tbl.joinClause.parentColumn));
        //        foreach (ITableStatement thsTbl in tableStatements.Values)
        //        {
        //            thsTbl.IndexedWhereColumns.Where(col => col.colTableRefName == thsTable.tableRefName).ToList().ForEach(col => indexes.Add(col));
        //        }
        //        indexes = indexes.Distinct().ToList();
        //        indexesPerTbl.Add(thsTable.tableRefName, indexes);
        //    }
        //    return indexesPerTbl;
        //}
        public IEnumerable<IMultiRogueRow> TopRows()
        {
            return rootLevel.rows.First().childRows;
        }
        /// <summary>
        /// Needed for select * (*)
        /// </summary>
        /// <param name="parentName"></param>
        /// <returns></returns>
        internal ILevelStatement ParentLevelByChildName(string parentName)
        {
            //ILevelStatement foundLvl = null;
            //groups.ForEach(grp => foundLvl = grp.levelStatements.Where(lvl => lvl.levelName == parentName).First());
            //return foundLvl;
            return levelStatements[parentName.ToUpper()];
        }
        public SelectHQLStatement Fill()
        {
            //Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            //groups = new MultiSymbolSegment<PlainList<ILevelGroup>, ILevelGroup>(SymbolOrder.symbolbefore, finalQuery, keys, (x, y) => new LevelGroup(x, y), this).segmentItems;
            //Console.WriteLine("Segment Split: " + stopwatch2.ElapsedMilliseconds);
            try
            {
                //if(UnsetParams.Count > 0)
                //{
                    foreach (ILevelGroup thsGroup in groups)
                    {
                        thsGroup.Fill();
                    }
                    if (isExecutable)
                    {
                        CodeCaller.RunProcedure(executableName, TopRows().ToArray());
                    }
                //}                
            }
            catch(Exception ex)
            {

            }
            return this;
            //stopwatch2.Stop();
            //Console.WriteLine("Query Data Load: " + stopwatch2.ElapsedMilliseconds);
            //return this;
        }
        public DataTable AsDataTable()
        {
            DataTable results = new DataTable();
            foreach(var col in groups[0].levelStatements[0].selectRow.columnList)
            {
                results.Columns.Add(col.columnName);
            }
            foreach(IMultiRogueRow row in TopRows())
            {
                 DataRow newRow = results.NewRow();
                 foreach(var pair in row.GetValueList())
                 {
                     newRow[pair.Key] = pair.Value;
                 }
                 results.Rows.Add(newRow);
            }
            return results;
        }
        public List<string> SetUnsetParams()
        {
            List<string> unsets = new List<string>();
            foreach(var grp in groups)
            {
                unsets.AddRange(grp.UnsetParams());
            }
            unsets.ForEach(x => x =stringHelper.BeforeFirstChar(x, '_'));
            UnsetParams = unsets.Distinct().ToList();
            return UnsetParams;
        }
        public void PreLoad()
        {
            try
            {
            var tables = new List<ITableStatement>();
            var levels = new List<ILevelStatement>();
            foreach (ILevelGroup thsGroup in groups)
            {
                foreach (ILevelStatement thsLvl in thsGroup.levelStatements)
                {
                        try
                        {
                            levels.Add(thsLvl);
                        }
                        catch(Exception ex)
                        {
                            string g = ex.ToString();
                        }
                    
                    foreach (ITableStatement thsTbl in thsLvl.allTableStatements)
                    {
                            try
                            {
                                tables.Add(thsTbl);
                            }
                            catch(Exception ex)
                            {
                                string g = ex.ToString();
                            }
                    }
                }
            }
            QueryMetaData queryMetaData = new QueryMetaData(rootLevel, levels, tables);
            foreach (ILevelGroup thsGroup in groups)
            {
                thsGroup.PreFill(queryMetaData);
            }
                //UnsetParams = queryMetaData.allParams;
            }
            catch (Exception ex)
            {
                string blah = ex.ToString();
            }
        }
        public void IterateRows(Action<IMultiRogueRow> newRowOutput = null, Action< IMultiRogueRow> endRowOutput = null)
        {
            if (newRowOutput == null)
            {
                newRowOutput = (IMultiRogueRow row) => { };
            }
            if (endRowOutput == null)
            {
                endRowOutput = (IMultiRogueRow row) => { };
            }
            foreach (var topRow in TopRows())
            {
                LoopHierachy(topRow, 0, newRowOutput, endRowOutput);
            }
            //Console.WriteLine("Iterate Row Count:" + iterateRowCount);
            //return hierarchyGrid;
        }
        internal void LoopHierachy(IMultiRogueRow topRow, int currLvl, Action<IMultiRogueRow> newRowOutput, Action<IMultiRogueRow> endRowOutput)
        {
            //Stopwatch stopwatch2 = new Stopwatch();
            //stopwatch2.Start();
            iterateRowCount++;
            newRowOutput(topRow);
            //stopwatch2.Stop();
            //Console.WriteLine("Fill Level:" + stopwatch2.ElapsedMilliseconds);
            //if (stopwatch2.ElapsedMilliseconds > 50)
            //{
            //    bullshit = true;
            //    //newRowOutput(topRow);
            //    endRowOutput(topRow);
            //    Stopwatch stopwatch4 = new Stopwatch();
            //    stopwatch4.Start();
            //    newRowOutput(topRow);
            //    stopwatch4.Stop();
            //    Console.WriteLine("Fill Level:" + stopwatch4.ElapsedMilliseconds);
            //    string blah = topRow.levelName;

            //}
            //hierarchyGrid.Add(new KeyValuePair<int, IMultiRogueRow>(currLvl, topRow));
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                //Stopwatch stopwatch3 = new Stopwatch();
                //stopwatch3.Start();
                LoopHierachy(childRow, currLvl, newRowOutput, endRowOutput);
                //stopwatch3.Stop();
                //Console.WriteLine("Fill Level:" + stopwatch3.ElapsedMilliseconds);
            }
            //finalOutput(rowstatus.close, topRow);
            endRowOutput(topRow);
            //if (!bullshit)
            //{
            //    endRowOutput(topRow);
            //}
            //else
            //{
            //    bullshit = false;
            //}
           
        }
        public StringBuilder PrintQuery()
        {
            //*print from top down
            StringBuilder strBuild = new StringBuilder();
            foreach (IMultiRogueRow topRow in TopRows())
            {
                strBuild = LoopPrintHierachy(topRow, 0, strBuild);
            }
            return strBuild;
        }
        StringBuilder LoopPrintHierachy(IMultiRogueRow topRow, int currLvl, StringBuilder stringBuild)
        {
            stringBuild.Append(topRow.PrintRow(false));
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopPrintHierachy(childRow, currLvl, stringBuild);
            }
            return stringBuild;
        }
        public IMultiRogueRow GenerateIntellisenseParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            try
            {
                foreach (ILevelGroup thsGroup in groups)
                {
                    thsGroup.LoadSyntaxParts(parentRow, syntaxCommands);
                }
            }
            catch(Exception ex)
            {
                syntaxCommands.GetLabel(parentRow, "&nbsp;" + origQry + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            }
            return parentRow;
        }
        public IMultiRogueRow GenerateParameterParts(IMultiRogueRow parentRow, Action<IMultiRogueRow,string> CreateParam)
        {
            try
            {
                var tblRow = IntellsenseDecor.MyTable(parentRow);
                var rowRow = IntellsenseDecor.MyTableRow(tblRow);
                for (int i = 0; i <UnsetParams.Count; i++)
                {
                    if (i%3 == 0)
                    {
                        rowRow = IntellsenseDecor.MyTableRow(tblRow);
                    }
                    CreateParam(rowRow, UnsetParams[i]);                    
                    //var cellRowLbl = IntellsenseDecor.MyTableCell(rowRow);
                    //syntaxCommands.GetLabel(cellRowLbl, param , IntellsenseDecor.MyColors.black, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular);
                    //var cellRowTextBox = IntellsenseDecor.MyTableCell(rowRow);
                    //IntellsenseDecor.MyTextbox(cellRowTextBox, "", "PARAM_" + param);
                }
            }
            catch (Exception ex)
            {
                IntellsenseDecor.MyLabel(parentRow, "&nbsp;" + origQry + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            }
            return parentRow;
        }
        public void ParamPart(IMultiRogueRow rowRow, string param)
        {
            var cellRowLbl = IntellsenseDecor.MyTableCell(rowRow);
            IntellsenseDecor.MyLabel(cellRowLbl, param, IntellsenseDecor.MyColors.black, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular);
            var cellRowTextBox = IntellsenseDecor.MyTableCell(rowRow);
            IntellsenseDecor.MyTextbox(cellRowTextBox, "", "PARAM:" + param);
        }
        public string AsJsonResult()
        {
            Stopwatch TMR = new Stopwatch();
            TMR.Start();
            Fill();
            TMR.Stop();
            string fillTim = TMR.ElapsedMilliseconds.ToString();
            TMR.Restart();
            string json = "{";
            int currLevel = 0;
            int itCount = 0;
            TimeSpan totalGetValTime = new TimeSpan(0);
            TimeSpan totalEndTime = new TimeSpan(0);
            TimeSpan totalIfEndTime = new TimeSpan(0);
            Action<IMultiRogueRow> openJson = (row) => {
                itCount++;
                if (row.levelNum > currLevel)
                {
                    json += "\"" + row.levelName + "\" : [";
                }
                currLevel = row.levelNum;
                json += "{";
                Stopwatch valTmr = new Stopwatch();
                valTmr.Start();
                foreach (var pair in row.GetValueList())
                {
                    json += "\"" + pair.Key + "\"" + ":" + "\"" + pair.Value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\",";
                    //json += "\"" + pair.Key + "\"" + ":" + "\"" + pair.Value.Replace("\"", "\\\"").Replace("\\", "\\\\") + "\",";
                    //json += "\"" + pair.Key + "\"" + ":" + "\"" + pair.Value.Replace("\"", "\\\"") + "\",";
                }
                valTmr.Stop();
                totalGetValTime += valTmr.Elapsed;
                string vl = valTmr.ElapsedMilliseconds.ToString();
            };
            Action<IMultiRogueRow> closeJson = (row) =>
            {
                Stopwatch endTmr = new Stopwatch();
                endTmr.Start();
                json = json.Substring(0, json.Length - 1);
                //for (int i = row.levelNum; i < currLevel; i++)
                Stopwatch endifTmr = new Stopwatch();
                endifTmr.Start();
                if (row.levelNum < currLevel)
                {
                    json += "]";
                }
                endifTmr.Stop();
                totalIfEndTime += endifTmr.Elapsed;
                json += "},";
                endTmr.Stop();
                totalEndTime += endTmr.Elapsed;
            };
            IterateRows(openJson, closeJson);
            json = json.Substring(0, json.Length - 1);
            //for (int i = 0; i < currLevel; i++)
            //{
            //    json += "]";
            //}
            json += "]}";
            TMR.Stop();
            string ff = TMR.ElapsedMilliseconds.ToString();
            return json;
        }
        public List<string> GetValuesByLevelAndColumnName(string fullId)
        {
            string[] splits = fullId.Split('.');
            var result = TopRows().ToList().Where(x => x.levelName.ToUpper() == splits[0].ToUpper()).Select(val => val.GetValue(splits[1]));
            return result.ToList();
        }
    }
    //class SyntaxPartsCommands
    //{
    //    public delegate IMultiRogueRow GetLabel(IMultiRogueRow parentRow,string txt, MyColors myColor = MyColors.black, Boldness boldness = Boldness.none, FontSize fontSize = FontSize.regular, Underline isUnderlined = Underline.none);
    //    public delegate IMultiRogueRow IndentedGroupBox();
    //    public delegate IMultiRogueRow BreakLine();
    //    public GetLabel thsLabel;
    //    public static SyntaxPartsCommands StandardParts()
    //    {
    //        thsLabel = syntaxCommands.GetLabel;

    //    }
    //}

}
