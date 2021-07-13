using rogue_core.RogueCode.hql.hqlSegments.table;
using rogue_core.rogueCore.hql.hqlSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hql
{
    public static class ConvertHumanHQL
    {
        public static String Convert(String humanHQL)
        {
            humanHQL = humanHQL.Trim();
            String[] tableSegments = humanHQL.Split(new string[] { "FROM " }, StringSplitOptions.RemoveEmptyEntries);
            String decodedHQL = "";
            Dictionary<String, int> tableRefIDs = new Dictionary<String, int>();
            tableRefIDs.Add("ROOT", -1004);
            foreach (String segment in tableSegments)
            {
                //SplitKeepSeparators("FROM " + segment);
                decodedHQL += "^" + TableSegment.HumanToEncodedHQL("FROM " + segment, tableRefIDs);
            }
            return decodedHQL;
        }
        public static String ConvertTwo(String humanHQL)
        {
            humanHQL = humanHQL.Trim();
            String[] tableSegments = humanHQL.Split(new string[] { "FROM " }, StringSplitOptions.RemoveEmptyEntries);
            String decodedHQL = "";
            Dictionary<String, int> tableRefIDs = new Dictionary<String, int>();
            tableRefIDs.Add("ROOT", -1004);
            foreach (String segment in tableSegments)
            {
                 //SplitKeepSeparators("FROM " + segment);
                 decodedHQL += "^" + TableSegment.HumanToEncodedHQL("FROM " + segment, tableRefIDs);
            }
            return decodedHQL;
        }
        public static String DecodeHQLToHumanReadable(String hql)
        {
            MatrixTableSegments matrixTableSegments = new MatrixTableSegments(hql);
            String decodedHQL = "";
            foreach (TableSegment tableSegment in matrixTableSegments.tableSegments.Values)
            {
                //*SF
                decodedHQL += tableSegment.tableInfo.GetFullHQLText() + tableSegment.joinClauses[0].GetFullHQLText() + tableSegment.selectedCols.GetFullHQLText(tableSegment.tableInfo.tableRefName) + tableSegment.whereClauses.GetHQLFullText();
                //*Remove later
                //Trace.WriteLine(tableSegment.tableInfo.GetFullHQLText() + tableSegment.joinClauses[0].GetFullHQLText() + tableSegment.selectedCols.GetFullHQLText(tableSegment.tableInfo.tableRefName) + tableSegment.whereClauses.GetHQLFullText());
            }
            return decodedHQL;
        }
    }
}
