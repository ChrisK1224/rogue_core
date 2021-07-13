using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using rogueCore.rogueUIV2;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.from
{
    class FromExecute : IFrom
    {
        public string tableRefName { get; private set; }
        public IORecordID tableID { get; private set; }
        HQLMetaData metaData;
        internal string executableName;
        internal List<string> execParams;
        internal FromExecute(string hqlTxt, HQLMetaData metaData)
        {
            this.metaData = metaData;
            var items = new MultiSymbolString<DictionaryValues<string>>(SymbolOrder.symbolbefore, hqlTxt, new string[1] { From.splitTableName }, metaData).segmentItems;
            executableName = stringHelper.GetStringBetween(hqlTxt, "(", ",").Trim();
            tableRefName = items[From.splitTableName];
            string paramPortion = hqlTxt.Substring(hqlTxt.IndexOf(",")+1, hqlTxt.Length - hqlTxt.IndexOf(",")-1);
            paramPortion = paramPortion.Substring(0, paramPortion.LastIndexOf(")"));
            execParams = new MultiSymbolString<PlainList<string>>(SymbolOrder.symbolbefore, paramPortion, new string[1] { From.splitTableName }, metaData).segmentItems;
            tableID = -1011;
        }
        public IEnumerable<IRogueRow> StreamIRows(MultiRogueRow parentRow)
        {
            throw new NotImplementedException("This stream is from an executable From and should never be called as the results come from tableStatement as full multiroguerows not irogueRow");
        }
    }
}
