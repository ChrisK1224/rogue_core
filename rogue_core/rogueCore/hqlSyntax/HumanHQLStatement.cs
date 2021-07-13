using System.Diagnostics;
using FilesAndFolders;
using rogueCore.hqlSyntax.segments.table;
using rogueCore.hqlSyntax.segments.table.human;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace rogueCore.hqlSyntax
{
    public class HumanHQLStatement : HQLQueryTwo
    {
        //internal static Dictionary<String, int> tableRefIDs;
        //internal static String currTableRefName;
       public HumanHQLStatement(String humanHQL)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            
            //tableRefIDs = new Dictionary<string, int>();
            //humanHQL = File.ReadAllText("/home/chris/Development/rogue9.5/queries/ExampleOne.txt");
            String finalHQL = FixSingleGroupText(humanHQL);
            tableGroups = new TableGroups(finalHQL, metaData);
            Trace.WriteLine("Segments:" + watch.ElapsedMilliseconds);
            LoadData();
            Trace.WriteLine("AfterLoad:" + watch.ElapsedMilliseconds);
        }
       public static string StoredProcByID(int queryID)
        {
            return new HumanHQLStatement("FROM HQL_QUERIES SELECT * WHERE ROGUECOLUMNID = \"" + queryID.ToString() + "\"").TopRows().First().values["QUERY_TXT"].Value;
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
            foreach(TableGroup thsGroup in tableGroups.groups)
            {
                // List<string> newLines = thsGroup.FormatQueryTxt();
                lines.AddRange(thsGroup.FormatQueryTxt());
            }
            return lines;
       }
    }
}
