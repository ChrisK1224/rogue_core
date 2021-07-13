using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV3.segments.from
{
    class FromExecute : IFrom
    {
        public string tableRefName { get; private set; }
        public IORecordID tableID { get; private set; }
        //SelectHQLStatement queryStatement;
        internal string executableName;
        internal List<string> execParams;
        internal FromExecute(string hqlTxt)
        {
            //this.queryStatement = queryStatement;
            var items = new MultiSymbolString<DictionaryValues<string>>(SymbolOrder.symbolbefore, hqlTxt, new string[1] { From.splitTableName }).segmentItems;
            executableName = stringHelper.GetStringBetween(hqlTxt, "(", ",").Trim();
            tableRefName = items[From.splitTableName];
            string paramPortion = hqlTxt.Substring(hqlTxt.IndexOf(",")+1, hqlTxt.Length - hqlTxt.IndexOf(",")-1);
            paramPortion = paramPortion.Substring(0, paramPortion.LastIndexOf(")"));
            execParams = new MultiSymbolString<PlainList<string>>(SymbolOrder.symbolbefore, paramPortion, new string[1] { From.splitTableName }).segmentItems;
            tableID = -1011;
        }
        public IEnumerable<IRogueRow> StreamIRows(IMultiRogueRow parentRow)
        {
            throw new NotImplementedException("This stream is from an executable From and should never be called as the results come from tableStatement as full multiroguerows not irogueRow");
        }
    }
}
