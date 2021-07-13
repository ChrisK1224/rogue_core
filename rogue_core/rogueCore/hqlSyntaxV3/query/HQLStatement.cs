using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using files_and_folders;
using FilesAndFolders;
using rogueCore.apiV3.formats.json;
using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.select;
using rogueCore.hqlSyntaxV3.segments.update;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogue_core.rogueCore.hqlSyntaxV3.query
{
    public class HQLStatement
    {
        string[] splitters = new string[3] { "INSERT", "UPDATE", "WITH" };
        const string insert = "INSERT";
        const string update = "UPDATE";
        const string select = "FROM";
        const string with = "WITH";
        Dictionary<string, WithHQLStatement> withs = new Dictionary<string, WithHQLStatement>();
        List<IHQLStatement> statements = new List<IHQLStatement>();
        public HQLStatement()
        {
            //string qry = @"INSERT INTO StockHistory.CryptoCompare (
            //            FROM ""APIINFO"" SELECT CURRENTDATE() AS ToDate, ""CRYPTOCOMPARE"" AS API_SOURCE_NM, ""SEG"" AS API_SEGMENT_NM, ""day"" as RUNTYPE, ""ETH"" AS CRYPTOID
            //                 FROM RUN_API() as APIResult JOIN TO APIINFO SELECT FILE_CONTENT(DataFilePath) as jsonData
            //            ) APIResult.jsonData";
            string qry = @"WITH APIResult (
            FROM ""APIINFO"" SELECT CURRENTDATE() AS ToDate, ""CRYPTOCOMPARE"" AS API_SOURCE_NM, ""SEG"" AS API_SEGMENT_NM, ""day"" as RUNTYPE, ""ETH"" AS CRYPTOID
            FROM RUN_API() as APIResult JOIN TO APIINFO SELECT DataFilePath
            )  INSERT JS(APIResult.DatabaseId, APIResult.DataFilePath , APIResult.Default_Table_NM)";
            
            var segmentItems = new MultiSymbolString<ListKeyPairs<string>>(SymbolOrder.symbolbefore, qry, splitters).segmentItems;
            foreach(var pair in segmentItems)
            {
                statements.Add(GetHQLStatementType(pair.Key, pair.Value));
            }
            //var segs = new MultiSymbolString<ListKeyValPairs<string>, string>(SymbolOrder.symbolbefore, qry, splitters, "", SelectRow.GetOutsideQuotesAndParenthesisPattern).segmentItems;
            //var segs = new MultiSymbolSegment<ListKeyPairs<IHQLStatement>, IHQLStatement>(SymbolOrder.symbolafter, qry, splitters, GetHQLStatementType, "", SelectRow.GetOutsideQuotesAndParenthesisPattern).segmentItems;
            qry = qry.ToUpper().Trim();
            if (qry.StartsWith(insert))
            {
                string beforeParenthsis = qry.BeforeFirstChar('(');
                SelectHQLStatement selectHQL = new SelectHQLStatement(qry.get_string_between_2("(", ")"));
                selectHQL.Fill();
                string columnByName = qry.AfterLastChar(')');
                foreach (string jsonVal in selectHQL.GetValuesByLevelAndColumnName(columnByName))
                {
                    //new jsonTester()
                }
            }
            else if (qry.StartsWith(update))
            {

            }
            else
            {

            }
        }
        IHQLStatement GetHQLStatementType(string typ, string content)
        {
            typ = typ.ToUpper();
            switch (typ)
            {
                case select:
                    var sel = new SelectHQLStatement(content);
                    //sel.Fill();
                    return sel;
                case with:
                    var withState = new WithHQLStatement(content);
                    withs.Add(withState.withName, withState);
                    return withState;
                case insert:
                    return new InsertHQLStatement(content);
                case update:
                    return new UpdateHQLStatement(content);
                default:
                    throw new Exception("Unknown High Level statement type needs to start with update, insert, with, delete, from");
            }
        }
        internal static String GetOutsideQuotesAndParenthesisPattern(String[] seps)
        {
            String keyStr = "";
            foreach (string sep in seps)
            {
                if (sep.Any(ch => !Char.IsLetterOrDigit(ch)))
                {
                    keyStr += "\\" + sep;
                }
                else
                {
                    keyStr += sep;
                }
                keyStr += "|";
            }
            //**BUG here not sure why working. shaves last char off keyStr
            keyStr = keyStr.Substring(0, keyStr.Length-1);
            //works**(\,)(?=(?:[^\)]|\([^\)]*\))*$)(?=(?:[^\"]|\"[^\"]*\")*$)
            return "(" + keyStr + @")(?= (?:[^\)]|\([^\)]*\))*$)(?= (?:[^\""]|\""[^\""]*\"")*$)";
        }
    }
}
