using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2
{
    public class MultiSymbolSegment<mycollection, myType> where mycollection : IMyList<myType>, new()
    {
        protected string[] keys { get; }
        SymbolOrder thsOrder;
        protected string firstEntrySymbol {get {return "START";}}
        internal string origStatement { get;private set; }
        internal mycollection segmentItems { get; private set; }
        Func<string, HQLMetaData, myType> NewItem;
        HQLMetaData metaData;
        Dictionary<SymbolOrder, Func<List<String>, mycollection>> SymbolFuncs = new Dictionary<SymbolOrder, Func<List<string>, mycollection>>();
        internal MultiSymbolSegment(SymbolOrder thsOrder, String input, string[] keys,Func<string, HQLMetaData, myType> NewItem, HQLMetaData metaData, string cutoffSymbol = "")
        {
            //if (cutoffSymbol != "" && Regex.IsMatch(input, GetOutsideQuotesPattern(cutoffSymbol)))
            //{
            //    bool test = true;
            //}
            //if (cutoffSymbol != "" && Regex.IsMatch(input, GetOutsideQuotesPattern(cutoffSymbol)))
            if (cutoffSymbol != "" && Regex.IsMatch(input, GetOutsideQuotesPattern(cutoffSymbol)))
            {
                int cutoffIndex = Regex.Match(input, GetOutsideQuotesPattern(cutoffSymbol)).Index;
                input = input.Substring(0, input.Length - (input.Length - cutoffIndex));
            }
            this.NewItem = NewItem;
            this.keys = keys;
            this.metaData = metaData;
            SymbolFuncs.Add(SymbolOrder.symbolbefore, SymbolBeforeTransform);
            SymbolFuncs.Add(SymbolOrder.symbolafter, SymbolAfterTransform);
            SymbolFuncs.Add(SymbolOrder.betweensymbols, SymbolBetweenTransform);
            SymbolFuncs.Add(SymbolOrder.randombetweensymbols, SymbolRandomBetweenTransform);
            this.thsOrder = thsOrder;
            segmentItems = Split(input);
            origStatement = input;
        }
        //internal KeyValuePair<int, int> StartEndIndexes(int segmentIndex)
        //{
            
        //}
        mycollection SymbolBetweenTransform(List<String> splits)
        {
            var ret = new mycollection();
            //*For now assume to add one blank to end since all groups (where caluse) will not end in symbol might need to change in future
            splits.Add("");
            for (int i = 1; i < splits.Count; i = i + 4)
            {
                AddSegmentItem(ret, splits[i + 2], splits[i]);
                //myType blah = (myType)Activator.CreateInstance(typeof(myType),"");
                //ret.Add(splits[i+2].ToUpper(), (myType)Activator.CreateInstance(typeof(myType),  splits[i].Trim() ));
            }
            return ret;
        }
        mycollection SymbolRandomBetweenTransform(List<String> splits)
        {
            var ret = new mycollection();
            //*For now assume to add one blank to end since all groups (where caluse) will not end in symbol might need to change in future
            splits.Add("");
            for (int i = 1; i < splits.Count; i++)
            {
                if(splits[i-1] == keys[0])
                {
                    AddSegmentItem(ret, splits[i], splits[i]);
                }
                //myType blah = (myType)Activator.CreateInstance(typeof(myType),"");
                //ret.Add(splits[i+2].ToUpper(), (myType)Activator.CreateInstance(typeof(myType),  splits[i].Trim() ));
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
                AddSegmentItem(ret, splits[i - 1], splits[i]);
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
                AddSegmentItem(ret, splits[i], splits[i-1]);
               // myType blah = (myType)Activator.CreateInstance(typeof(myType), ""
                //ret.Add(splits[i].ToUpper(), (myType)Activator.CreateInstance(typeof(myType), splits[i].Trim() ));
            }
            return ret;
        }
        protected virtual void AddSegmentItem(mycollection ret, string key, String item)
        {
            //if (typeof(myType) == typeof(string))
            //{
            //    ret.Add(key.ToUpper(), (myType)Convert.ChangeType(item.Trim(), typeof(myType)));
            //}
            //else
            //{
                ret.Add(key.ToUpper(), NewItem(item.Trim(), metaData));
            //}
        }
        mycollection Split(String input)
        {
            String pattern = GetOutsideQuotesPattern(keys);
            //var items = Regex.Split(input, pattern, RegexOptions.IgnoreCase).Where(s => s != String.Empty);
            List<String> splits = Regex.Split(input, pattern, RegexOptions.IgnoreCase).Where(s => s != String.Empty).ToList<string>();
            return SymbolFuncs[thsOrder](splits);
        }
        internal static String GetOutsideQuotesPattern(String[] seps)
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
        internal static String GetOutsideQuotesPattern(String sep)
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
            return "(" + keyStr + ")(?=(?:[^\"]|\"[^\"]*?\")*?$)";
        }
    }
    public class MultiSymbolString<mycollection> : MultiSymbolSegment<mycollection, string> where mycollection : IMyList<string>, new()
    {
        public MultiSymbolString(SymbolOrder thsOrder, string input, string[] keys, HQLMetaData metaData) : base(thsOrder, input, keys, BaseRet, metaData){ }
        protected override void AddSegmentItem(mycollection ret, string key, string item)
        {
            ret.Add(key.ToUpper(), item.Trim());
        }
        static  string BaseRet(string txt, HQLMetaData metaData) { return txt; }
    }
    public class MutliSegmentEnum
    {
        internal const string firstEntrySymbol = "START";
        
        public enum SymbolOrder
        {
            symbolbefore, symbolafter, betweensymbols,randombetweensymbols
        }
    }
    public class DictionaryValues<type> : Dictionary<string, type>, IMyList<type>
    {
        public virtual type GetValue(String key)
        {
            return this[key];
        }
        public new void Add(string key, type value)
        {
            if (!this.ContainsKey(key))
            {
               base.Add(key, value);
            }
        }
    }
    public class StringMyList : DictionaryValues<String>
    {
        public override String GetValue(String key)
        {
            String value;
            return this.TryGetValue(key, out value) ? value : "";
        }
    }
    public class DictionaryListValues<type> : Dictionary<String, List<type>>, IMyList<type>
    {
        public void Add(string key, type value)
        {
            if (!this.ContainsKey(key))
            {
                this.Add(key, new List<type>());
            }
            this[key].Add(value);
        }
    }
    public class PlainList<type> : List<type>, IMyList<type>
    {
        public void Add(string key, type value)
        {
            this.Add(value);
        }
    }
    public class ListKeyPairs<type> : List<KeyValuePair<String, type>>, IMyList<type>
    {
        public void Add(string key, type value)
        {
            this.Add(new KeyValuePair<String, type>(key, value));
        }
    }
    public interface IMyList<type>
    {
        void Add(String key, type value);

    }
    public interface iSplitSegment<myType>
    {
        myType NewSegmentItem(HQLMetaData metadata, string hqlSegment);
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
    //public class DictionaryValues<type> : Dictionary<String, type>, IMyList<type>
    //{
    //    public virtual type GetValue(String key) {

    //        return this[key];
    //    }
    //}
    //public class StringMyList : DictionaryValues<String>
    //{
    //    public override String GetValue(String key)
    //    {
    //        String value;
    //        return this.TryGetValue(key, out value) ? value : "";
    //    }
    //}
    //public class DictionaryListValues<type> : Dictionary<String, List<type>>, IMyList<type>
    //{
    //    public void Add(string key, type value)
    //    {
    //        if (!this.ContainsKey(key))
    //        {
    //            this.Add(key, new List<type>());
    //        }
    //        this[key].Add(value);
    //    }
    //}
    //public class PlainList<type> : List<type>, IMyList<type>
    //{
    //    public void Add(string key, type value)
    //    {
    //        this.Add(value);
    //    }
    //}
    //public class ListKeyPairs<type> : List<KeyValuePair<String, type>>, IMyList<type>
    //{
    //    public void Add(string key, type value)
    //    {
    //        this.Add(new KeyValuePair<String, type>(key, value));
    //    }
    //}
    //public interface IMyList<type>
    //{
    //    void Add(String key, type value);

    //}
}
