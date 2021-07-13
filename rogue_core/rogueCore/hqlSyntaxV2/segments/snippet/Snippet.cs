using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV2.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.snippet
{
    class Snippet
    {
        const string paramSep = ",";
        const char snippetEnd = '[';
        string snippetName;
        List<SnippetParam> snipParams;
        static Dictionary<string, string> snippetQueries = SetQueries();
        public Snippet(string snippetHQL, HQLMetaData metaData) 
        {
            snippetName = stringHelper.BeforeFirstChar(snippetHQL, snippetEnd).Trim();
            string paramTxt = stringHelper.get_string_between_2(snippetHQL, "[", "]");
            snipParams = new MultiSymbolSegment<PlainList<SnippetParam>, SnippetParam> (SymbolOrder.symbolafter, paramTxt, new string[1] { paramSep}, (x, y) => new SnippetParam(x, y), metaData).segmentItems;
            SetQueries();
            //getSniptTxt = 
        }
        internal string SnipText()
        {
            string getSnipTxt = snippetQueries[snippetName];
            foreach (SnippetParam thsParam in snipParams)
            {
                getSnipTxt = getSnipTxt.Replace(thsParam.paramID, thsParam.paramValue);
            }
            return getSnipTxt;
        }
        static Dictionary<string, string> SetQueries()
        {
           Dictionary<string, string> qrys = new Dictionary<string, string>();
            foreach (var iRow in new IORecordID(7415).ToTable().StreamIRows())
            {
                if (!qrys.ContainsKey(iRow.GetValueByColumn(7420)))
                {
                    qrys.Add(iRow.GetValueByColumn(7420), iRow.GetValueByColumn(7422));
                }
            }
            return qrys;
        }
    }
}
