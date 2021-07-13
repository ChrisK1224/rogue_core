using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace rogueCore.hqlSyntax.segments.split
{
    //*UNUSED but should probably try to use later. THis is better than current split technique since it auto sets actual values sent as REF
    class SplitProperty<mycollection, returnType> where mycollection : IMyList<returnType>, new()
    { 
        protected string[] keys { get; private set; }
        String splitTxt {get; set;}
        Func<String, returnType> CreateInstance;
        Dictionary<SymbolOrder, Func<List<String>, mycollection>> SymbolFuncs = new Dictionary<SymbolOrder, Func<List<string>, mycollection>>();
        SymbolOrder thsOrder;
        public SplitProperty(string[] keys, SymbolOrder thsOrder, String splitTxt, ref mycollection setValue, Func<String, returnType> CreateInstance)
        {
            this.splitTxt = splitTxt;
            this.keys = keys;
            this.CreateInstance = CreateInstance;
            SymbolFuncs.Add(SymbolOrder.symbolbefore, SymbolBeforeTransform);
            SymbolFuncs.Add(SymbolOrder.symbolafter, SymbolAfterTransform);
            this.thsOrder = thsOrder;
            //segmentItems = Split(input);
            setValue = Split(splitTxt);
        }
        public SplitProperty(string[] keys,SymbolOrder thsOrder, String splitTxt, ref returnType setValue, Func<String, returnType> CreateInstance)
        {
            this.keys = keys;
            this.splitTxt = splitTxt;
            this.CreateInstance = CreateInstance;
            SymbolFuncs.Add(SymbolOrder.symbolbefore, SymbolBeforeTransform);
            SymbolFuncs.Add(SymbolOrder.symbolafter, SymbolAfterTransform);
            this.thsOrder = thsOrder;
            //segmentItems = Split(input);
            setValue = Split(splitTxt).FirstValue();
        }
        mycollection SymbolBeforeTransform(List<String> splits)
        {
            var ret = new mycollection();
            //*This take care of symbol before split when it has a value before first split key.
            if (splits.Count % 2 != 0)
            {
                splits.Insert(0, "START");
            }
            for (int i = 1; i < splits.Count + 1; i = i + 2)
            {
                ret.Add(splits[i - 1], CreateInstance(splits[i].Trim()));
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
                ret.Add(splits[i], CreateInstance(splits[i - 1].Trim()));
            }
            return ret;
        }
        mycollection Split(String input)
        {
            String pattern = GetOutsideQuotesPattern(keys);
            List<String> splits = Regex.Split(input, pattern).Where(s => s != String.Empty).ToList<string>();
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
        public enum SymbolOrder
        {
            symbolbefore, symbolafter
        }
    }
    abstract class StringSplitProperty<collectionType> : SplitProperty<collectionType, String> where collectionType : IMyList<String>, new()
    {
        public StringSplitProperty(string[] keys, SymbolOrder thsOrder, String splitTxt, ref collectionType setValue) : base(keys,thsOrder, splitTxt,ref setValue, CreateInstanceString){ }
        static String CreateInstanceString(String value) { return value;  }
    }
    public class DictionaryValues<type> : Dictionary<String, type>, IMyList<type>
    {
        public virtual type GetValue(String key)
        {
            return this[key];
        }
        public type FirstValue()
        {
            var e = this.GetEnumerator();
            e.MoveNext();
            return e.Current.Value;
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
    public class ListValues<type> : Dictionary<String, List<type>>, IMyList<type>
    {
        public void Add(string key, type value)
        {
            if (!this.ContainsKey(key))
            {
                this.Add(key, new List<type>());
            }
            this[key].Add(value);
        }
        public type FirstValue()
        {
            var e = this.GetEnumerator();
            e.MoveNext();
            return e.Current.Value[0];
        }
    }
    public class PlainList<type, retType> : List<type>, IMyList<type>
    {
        public void Add(string key, type value)
        {
            this.Add(value);
        }
        public type FirstValue()
        {
            return this[0];
        }
    }
    public class SingleString<type> : List<type>, IMyList<type>
    {
        public void Add(string key, type value)
        {
            this.Add(value);
        }
        public type FirstValue()
        {
            return this[0];
        }
    }
    public interface IMyList<type>
    {
        void Add(String key, type value);
        type FirstValue();
    }
}
