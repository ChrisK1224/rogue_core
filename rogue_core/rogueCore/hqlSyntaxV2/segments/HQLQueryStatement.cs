using rogueCore.hqlSyntaxV2;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.hqlSyntaxV2.segments.fullExecutable;
using rogueCore.hqlSyntaxV2.segments.snippet;
using rogueCore.hqlSyntaxV2.segments.table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogue_core.rogueCore.hqlSyntaxV2.segments
{
    public abstract class HQLQueryStatement
    {
        protected HQLMetaData metaData = new HQLMetaData();
        const string startComment = "#--";
        const string endComment = "--#";
        string splitter { get { return "|"; } }
        protected string[] keys { get { return new string[1] { splitter }; } }
        public List<TableGroupStatement> groups { get; protected set; }
        public string finalQuery { get; private set; }
        protected bool isExecutable = false;
        protected string executableName;
        internal string nonModifiedQry { get; private set; }
        protected HQLQueryStatement(string hqlTxt) 
        {
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            nonModifiedQry = hqlTxt;
            if (hqlTxt.Trim().ToUpper().StartsWith("EXECUTE("))
            {
                FullExecutable exec = new FullExecutable(hqlTxt.Trim());
                isExecutable = true;
                executableName = exec.executableName;
                hqlTxt = exec.paramQry;
            }
            hqlTxt = hqlTxt.Replace(Environment.NewLine, " ");
            hqlTxt = new SnippetFormatter(hqlTxt).hqlBuilder.ToString();
            hqlTxt = RemoveComments(hqlTxt);
            //hqlTxt = hqlTxt.Replace(Environment.NewLine, " ");
            //hqlTxt = RemoveComments(hqlTxt, startComment,endComment);
            hqlTxt = FixSingleGroupText(hqlTxt);
            finalQuery = hqlTxt;
            
            //groups = new MultiSymbolSegment<PlainList<TableGroupStatement>, TableGroupStatement>(SymbolOrder.symbolbefore, hqlTxt, keys, (x, y) => new FilledGroup(x, y), metaData).segmentItems;
            stopwatch.Stop();
            Console.WriteLine("Query Syntax Load: " + stopwatch.ElapsedMilliseconds);

            //Stopwatch stopwatch2 = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            //Fill();
            //stopwatch2.Stop();
            //Console.WriteLine("Query Data Load: " + stopwatch2.ElapsedMilliseconds);
            //foreach(string line in FormattedText(hqlTxt))
            //{
            //    Console.WriteLine(line);
            //}
            
        }
        
        public abstract FilledHQLQuery Fill();
        public static IEnumerable<string> FormattedText(String humanHQL)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            //tableRefIDs = new Dictionary<string, int>();
            //humanHQL = File.ReadAllText("/home/chris/Development/rogue9.5/queries/ExampleOne.txt");
            String finalHQL = FixSingleGroupText(humanHQL);
            HQLMetaData metaData = new HQLMetaData();
            var tableGroups = new TableGroups(finalHQL, metaData);
            List<string> lines = new List<string>();
            foreach (TableGroupStatement thsGroup in tableGroups.groups)
            {
                lines.AddRange(thsGroup.FormatQueryTxt());
            }
            return lines;
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
        //static string RemoveComments(string hqlTxt)
        //{
            
        //    //string errString = "This {match here} uses 3 other {match here} to {match here} the {match here}ation";
        //    hqlTxt = hqlTxt.Replace(Environment.NewLine, " ");
        //    string answer = RemoveBetween(hqlTxt, "#--", "--#");
        //    string answer2 = Remove(hqlTxt, "#--", "--#");
        //    //var toReplace = Regex.Match(hqlTxt, @"(?<=#--)(.*)(?=--#)");
        //    //var toReplace = Regex.Match(hqlTxt, @"(?<=#--)(.*)(?=--#)");
        //    //var toReplace = Regex.Match(hqlTxt, @"#--([^\}]+)--#");
        //    Regex regex = new Regex(@"(?<=#--)(.*)(?=--#)");

        //    // Step 2: call Match on Regex instance.
        //    Match match = regex.Match(hqlTxt);
        //    foreach(var grp in match.Groups)
        //    {
        //        hqlTxt = hqlTxt.Replace(grp.ToString(), "");
        //    }
        //    //string correctString = hqlTxt.Replace("#--" +toReplace + "--#", "");
        //    return "";
        //}
        //public static string RemoveBetween(string sourceString, string startTag, string endTag)
        //{
        //    Regex regex = new Regex(string.Format("{0}(.*?){1}", Regex.Escape(startTag), Regex.Escape(endTag)), RegexOptions.RightToLeft);
        //    return regex.Replace(sourceString, "#--" + startTag + endTag + "--#");
        //}
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
        public IEnumerable<MultiRogueRow> TopRows() { return metaData.TopRows(); }
        public void SetQueryParam(string paramKey, string paramValue)
        {
            finalQuery = finalQuery.Replace(paramKey, paramValue);
        }
        public string GetFormatedQuery()
        {
            foreach(TableGroupStatement grp in groups)
            {
                foreach(var lvl in grp.levelStatements)
                {
                    foreach(var tbl in lvl.allTableStatements)
                    {
                        //foreach(var iSeg in tbl.segments)
                        //{

                        //}
                    }
                }
            }
            return "";
        }
        
    }
}
