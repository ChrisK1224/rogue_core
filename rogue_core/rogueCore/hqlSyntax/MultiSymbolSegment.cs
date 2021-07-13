using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace rogueCore.hqlSyntax
{
    public abstract class MultiSymbolSegment<mycollection, segmentType> where mycollection : IMyList<segmentType>, new()
    {
        protected abstract string[] keys { get; }
        SymbolOrder thsOrder;
        protected string firstEntrySymbol {get {return "START";}}
        //internal QueryFormatDecoration itemDecor;
        internal string origStatement { get;private set; }
        protected mycollection segmentItems { get; private set; }
        Dictionary<SymbolOrder, Func<List<String>, mycollection>> SymbolFuncs = new Dictionary<SymbolOrder, Func<List<string>, mycollection>>();
        protected MultiSymbolSegment(SymbolOrder thsOrder, String input)
        {
            SymbolFuncs.Add(SymbolOrder.symbolbefore, SymbolBeforeTransform);
            SymbolFuncs.Add(SymbolOrder.symbolafter, SymbolAfterTransform);
            SymbolFuncs.Add(SymbolOrder.betweensymbols, SymbolBetweenTransform);
            this.thsOrder = thsOrder;
            segmentItems = Split(input);
            origStatement = input;
        }
        //* FOR setting a segment directly that doesn't have text to be parsed. Mainly when initiating through code instead of text
        protected MultiSymbolSegment(){ segmentItems = new mycollection(); origStatement = ""; }
        mycollection SymbolBetweenTransform(List<String> splits)
        {
            var ret = new mycollection();
            //*For now assume to add one blank to end since all groups (where caluse) will not end in symbol might need to change in future
            splits.Add("");
            for (int i = 1; i < splits.Count; i = i + 4)
            {
                ret.Add(splits[i+2].ToUpper(), ItemParse(splits[i].Trim()));
            }
            return ret;
        }
        mycollection SymbolBeforeTransform(List<String> splits)
        {
            var ret = new mycollection();
            //*This take care of symbol before split when it has a value before first split key.
            if (splits.Count % 2 != 0)
            {
                splits.Insert(0, firstEntrySymbol);
            }
            for (int i = 1; i < splits.Count + 1; i = i + 2)
            {
                ret.Add(splits[i - 1].ToUpper(), ItemParse(splits[i].Trim()));
            }
            return ret;
        }
        mycollection SymbolAfterTransform(List<String> splits)
        {
            var ret = new mycollection();
            splits.Add("");
            //*Start at 1 so if there are no segments otherwise blank will get added.  Which will cause errors when initlaizing split segment
            for (int i = 1; i < splits.Count; i = i + 2)
            {
                ret.Add(splits[i].ToUpper(), ItemParse(splits[i - 1].Trim()));
            }
            return ret;
        }
        mycollection Split(String input)
        {
            String pattern = GetOutsideQuotesPattern(keys);
            List<String> splits = Regex.Split(input, pattern, RegexOptions.IgnoreCase).Where(s => s != String.Empty).ToList<string>();
            return SymbolFuncs[thsOrder](splits);
        }
        protected String GetOutsideQuotesPattern(String[] seps)
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
            keyStr = keyStr.Substring(0, keyStr.Length - 1);
            return "(" + keyStr + ")(?=(?:[^\"]|\"[^\"]*\")*$)";
        }
        protected String GetOutsideQuotesPattern(String sep)
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
            keyStr = keyStr.Substring(0, keyStr.Length - 1);
            return "(" + keyStr + ")(?=(?:[^\"]|\"[^\"]*\")*$)";
        }
        protected abstract segmentType ItemParse(String txt);
        //protected virtual QueryFormatDecoration SetTextDecor()
        //{
        //    switch (thsOrder)
        //    {
        //        case SymbolOrder.symbolbefore:
        //            itemDecor = new QueryFormatDecoration();
        //            break;
        //        case SymbolOrder.symbolafter:
                    
        //            break;
        //        case SymbolOrder.betweensymbols:

        //            break;
        //    }
        //}
        protected enum SymbolOrder
        {
            symbolbefore, symbolafter, betweensymbols
        }
    }
    //public class QueryFormatDecoration
    //{
    //    List<QueryFormatDecoration> childItems = new List<QueryFormatDecoration>();
    //    List<DecorationOption> decorations = new List<DecorationOption>();
    //    string text;
    //    internal QueryFormatDecoration(string text) { this.text = text; }
    //    internal class DecorationOption
    //    {
    //        string decorType;
    //        string value;
    //        DecorationOption(string decorType, string decorValue)
    //        {
    //            this.decorType = decorType;
    //            this.value = decorValue;
    //        }
    //        public enum DecorType
    //        {
    //            bold, color
    //        }
    //    }
    //}
    //public class QuickSplit<mycollection> : MultiSymbolSegment<mycollection, string> where mycollection : IMyList<string>, new()
    //{
    //    protected override string[] keys { get { return new string[2] { "{", "}"}; } }
    //    QuickSplit(string val) : base(SymbolOrder.betweensymbols, val)
    //    {
            
    //    }

    //    protected override string ItemParse(string txt)
    //    {
    //        return txt;
    //    }
    //    public static QuickSplit<DictionaryListValues<string>> BracketSplit(string val)
    //    {
    //        return new QuickSplit<DictionaryListValues<string>>(val);
    //    }
    //}
    //public class FormatSplit : MultiSymbolSegment<DictionaryValues<string>, string>
    //{
    //    protected override string[] keys { get { return new string[5] { "FROM", "SELECT", "LIMIT", "WHERE", "JOIN" }; } }
    //    internal FormatSplit(string val) : base(SymbolOrder.symbolbefore, val)
    //    {
           
    //    }
    //    protected override string ItemParse(string txt)
    //    {
    //        return txt;
    //    }
    //    internal Dictionary<string,string> qryItems()
    //    {
    //        return segmentItems;
    //    }
    //}
    //public static class QuerySplit
    //{
    //    public static Dictionary<string, string> GetStrings(string val)
    //    {
    //        return new FormatSplit(val).qryItems();
    //    }
    //}
    
}
