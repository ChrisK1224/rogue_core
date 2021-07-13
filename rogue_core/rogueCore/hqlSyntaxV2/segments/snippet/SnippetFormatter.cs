using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.snippet
{
    public class SnippetFormatter
    {
        const string snipStart = " SNIPPET ";
        const string snipEnd = ";";
        int uniqueEnforcer = 1;
        string fixedHQL;
        internal StringBuilder hqlBuilder;
        public SnippetFormatter(string fullHQL, int queryID = 0)
        {
            hqlBuilder = new StringBuilder(fullHQL);
            var snippets = new MultiSymbolSegment<PlainList<Snippet>, Snippet>(SymbolOrder.randombetweensymbols, fullHQL, new string[2] { snipStart, snipEnd }, (x, y) => new Snippet(x, y), null).segmentItems;
            foreach(Snippet thsSnip in snippets)
            {
                ReplaceSnip(thsSnip);
                //string changeSnip = thsSnip.SnipText();
                //changeSnip = changeSnip.Replace("@UNIQUE", uniqueEnforcer.ToString());
                ////changeSnip = changeSnip.Replace("@QUERYID", queryID.ToString());
                //int start = Regex.Match(hqlBuilder.ToString(), MultiSymbolSegment<PlainList<Snippet>, Snippet>.GetOutsideQuotesPattern(new string[1] { snipStart}), RegexOptions.IgnoreCase).Groups[0].Index;
                //int end = Regex.Match(hqlBuilder.ToString(), MultiSymbolSegment<PlainList<Snippet>, Snippet>.GetOutsideQuotesPattern(new string[1] { snipEnd }), RegexOptions.IgnoreCase).Groups[0].Index;
                //hqlBuilder.Remove(start, (end - start) + 1);
                //hqlBuilder.Insert(start, changeSnip);
                //uniqueEnforcer++;
            }
        }
        void SnipWithinSnipLoop(string hqlSegment)
        {
            var snippets = new MultiSymbolSegment<PlainList<Snippet>, Snippet>(SymbolOrder.randombetweensymbols, hqlSegment, new string[2] { snipStart, snipEnd }, (x, y) => new Snippet(x, y), null).segmentItems;
            foreach (Snippet thsSnip in snippets)
            {
                ReplaceSnip(thsSnip);
            }
        }
        void ReplaceSnip(Snippet thsSnip)
        {
            string changeSnip = thsSnip.SnipText();
            changeSnip = changeSnip.Replace("@UNIQUE", uniqueEnforcer.ToString());
            //changeSnip = changeSnip.Replace("@QUERYID", queryID.ToString());
            int start = Regex.Match(hqlBuilder.ToString(), MultiSymbolSegment<PlainList<Snippet>, Snippet>.GetOutsideQuotesPattern(new string[1] { snipStart }), RegexOptions.IgnoreCase).Groups[0].Index;
            int end = Regex.Match(hqlBuilder.ToString(), MultiSymbolSegment<PlainList<Snippet>, Snippet>.GetOutsideQuotesPattern(new string[1] { snipEnd }), RegexOptions.IgnoreCase).Groups[0].Index;
            hqlBuilder.Remove(start, (end - start) + 1);
            hqlBuilder.Insert(start, changeSnip);
            uniqueEnforcer++;
            SnipWithinSnipLoop(changeSnip);
        }
    }
}
