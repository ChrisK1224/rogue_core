using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public class SplitKey
    {
        public enum ReplaceTypes { none, firstInstance, lastInstance }
        public ReplaceTypes replaceType { get; }
        public string keyTxt { get; }
        public string splitTxt { get; }
        public WhiteSpaceOptions includeWhiteSpace { get; }
        public bool includeKeyName { get; }
        public string replaceKey { get; }
        public bool attachFirstDefault { get; }
        public bool includeEmpty {get;}
        public bool includeInSplit { get; }
        public bool takeAfter { get; } 
        public Func<List<Match>, int, int, string,string> GetValue { get;}
        public enum WhiteSpaceOptions { none, include, includeFront}
        public SplitKey(string keyTxt, WhiteSpaceOptions includeWhiteSpace, bool includeKeyName, bool attachFirstDefault, bool includeEmpty = false, string replaceKey = "", ReplaceTypes replaceType = ReplaceTypes.none, bool takeAfter = true, bool includeInSplit = true)
        {
            this.keyTxt = keyTxt.ToUpper();
            this.splitTxt = (replaceKey == "") ? keyTxt : replaceKey;
            this.includeWhiteSpace = includeWhiteSpace;
            this.includeKeyName = includeKeyName;
            this.attachFirstDefault = attachFirstDefault;
            this.replaceKey = replaceKey;
            this.includeEmpty = includeEmpty;
            this.replaceType = replaceType;
            this.includeInSplit = includeInSplit;
            //this.takeAfter = takeAfter;
            //GetValue = GetAfterValue;
            this.GetValue = (takeAfter) ? GetAfterValue : this.GetValue = GetBeforeValue;
        }
        string GetBeforeValue(List<Match> validMatches,int i,int index, string txt)
        {
            if (i !=0)
            {
                var startIndex = validMatches[i - 1].Index;
                return txt.Substring(startIndex, index - startIndex).Trim();
            }
            else
            {
                return txt.Substring(index).Trim();
            }
        }
        string GetAfterValue(List<Match> validMatches, int i, int index, string txt)
        {
            if (i < (validMatches.Count - 1))
            {
                var endIndex = validMatches[i + 1].Index;
                return txt.Substring(index, endIndex - index).Trim();
            }
            else
            {
                return txt.Substring(index).Trim();
            }
        }
        //void ReplaceKey(string replaceKey)
        //{

        //}
    }
    public static class KeyNames
    {
        public const string with = "WITH";
        public const string withEnd = "END";

        public const string convert = "CONVERT";
        public const string pipe = "|";
        public const string questionMark = "?";

        public const string from = "FROM";
        public const string select = "SELECT";
        public const string combine = "COMBINE";
        public const string insert = "INSERT";
        public const string delete = "DELETE";
        public const string update = "UPDATE";
        public const string classify = "CLASSIFY";

        public const string where = "WHERE";
        public const string join = "JOIN";
        public const string limit = "LIMIT";
        public const string into = "INTO";
        public const string byKey = "BY";

        public const string asKey = "AS";
        public const string period = ".";
        public const string comma = ",";
        public const string concat = "&";

        public const string joinEqual = "=";
        public const string joinOn = "ON";

        public const string whereEqual = "=";
        public const string whereNotEqual = "!=";

        public const string whereAndKey = "AND";
        public const string whereOrKey = "OR";
        public const string openParenthensis = "(";
        public const string openParenthensisReplacement = "";
        public const string closeParenthesis = ")";

        public const string openBracket = "[";
        public const string closeBracket = "]";



        public const string addKey = "+";
        public const string minusKey = "-";
        public const string multiplyKey = "*";
        public const string divideKey = "/";
        public const string greatThanKey = ">";
        public const string lessThanKey = "<";

        public const string startKey = "";
    }
    public static class GroupSplitters
    {
        public static readonly SplitKey withKey = new SplitKey(KeyNames.with, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey convertKey = new SplitKey(KeyNames.convert, SplitKey.WhiteSpaceOptions.include, false, false, true);
        public static readonly SplitKey withEndKey = new SplitKey(KeyNames.withEnd, SplitKey.WhiteSpaceOptions.include, false, true, true);
        public static readonly SplitKey openCommand = new SplitKey(KeyNames.openParenthensis, SplitKey.WhiteSpaceOptions.none, false, false, false, KeyNames.pipe, SplitKey.ReplaceTypes.firstInstance);
        public static readonly SplitKey closeCommand = new SplitKey(KeyNames.closeParenthesis, SplitKey.WhiteSpaceOptions.none, false, false, true, KeyNames.questionMark, SplitKey.ReplaceTypes.lastInstance);
    }
    public static class LevelSplitters
    {
        //Level split items
        public static readonly SplitKey fromKey = new SplitKey(KeyNames.from, SplitKey.WhiteSpaceOptions.include, true, false);
        public static readonly SplitKey selectKey = new SplitKey(KeyNames.select, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey combineKey = new SplitKey(KeyNames.combine, SplitKey.WhiteSpaceOptions.include, true, false);
        public static readonly SplitKey insertKey = new SplitKey(KeyNames.insert, SplitKey.WhiteSpaceOptions.include, true, false);
        public static readonly SplitKey deleteKey = new SplitKey(KeyNames.delete, SplitKey.WhiteSpaceOptions.include, true, false);
        public static readonly SplitKey updateKey = new SplitKey(KeyNames.update, SplitKey.WhiteSpaceOptions.include, true, false);
        public static readonly SplitKey classifyKey = new SplitKey(KeyNames.classify, SplitKey.WhiteSpaceOptions.include, false, false);
    }
    public static class TableSplitters
    {
        
        //Table split items
        public static readonly SplitKey whereKey = new SplitKey(KeyNames.where, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey joinKey = new SplitKey(KeyNames.join, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey limitKey = new SplitKey(KeyNames.limit, SplitKey.WhiteSpaceOptions.include, false, false);
        //Same as level with differnt setting to remove key in string
        public static readonly SplitKey fromKey = new SplitKey(KeyNames.from, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey combineKey = new SplitKey(KeyNames.combine, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey insertKey = new SplitKey(KeyNames.insert, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey deleteKey = new SplitKey(KeyNames.delete, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey updateKey = new SplitKey(KeyNames.update, SplitKey.WhiteSpaceOptions.include, false, false);

    }
    public static class LocationSplitters
    {
        
        //All named Locations
        public static readonly SplitKey AsKey = new SplitKey(KeyNames.asKey, SplitKey.WhiteSpaceOptions.include, false, false);
        //Location divider and separater
        public static readonly SplitKey colDivider = new SplitKey(KeyNames.period, SplitKey.WhiteSpaceOptions.none, false, true);
        public static readonly SplitKey colSeparator = new SplitKey(KeyNames.comma, SplitKey.WhiteSpaceOptions.none, false, true);
        public static readonly SplitKey colConcat = new SplitKey(KeyNames.concat, SplitKey.WhiteSpaceOptions.none, false, true, true);
        public static readonly SplitKey colAdd = new SplitKey(KeyNames.addKey, SplitKey.WhiteSpaceOptions.none, false, true, true);
        public static readonly SplitKey colMinus = new SplitKey(KeyNames.minusKey, SplitKey.WhiteSpaceOptions.none, false, true, true);
        public static readonly SplitKey colMultiply = new SplitKey(KeyNames.multiplyKey, SplitKey.WhiteSpaceOptions.none, false, true, true);
        public static readonly SplitKey colDivide = new SplitKey(KeyNames.divideKey, SplitKey.WhiteSpaceOptions.none, false, true, true);
        

        public static readonly SplitKey openCommand = new SplitKey(KeyNames.openParenthensis, SplitKey.WhiteSpaceOptions.none, false, true, true);
        public static readonly SplitKey closeCommand = new SplitKey(KeyNames.closeParenthesis, SplitKey.WhiteSpaceOptions.none, false, false, true);
    }
    public static class ClassifySplitters
    {
        
    }
    public static class CalcGroupSplitters
    {
        public static readonly SplitKey openGroup = new SplitKey(KeyNames.openBracket, SplitKey.WhiteSpaceOptions.none, false, true, true);
        public static readonly SplitKey closeGroup = new SplitKey(KeyNames.closeBracket, SplitKey.WhiteSpaceOptions.none, false, false, true);
    }
    public static class JoinSplitters
    {
        public static readonly SplitKey joinOn = new SplitKey(KeyNames.joinOn, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey joinEqual = new SplitKey(KeyNames.joinEqual, SplitKey.WhiteSpaceOptions.none, false, false);
    }
    public static class WhereSplitters
    {
        public static readonly SplitKey whereEqual = new SplitKey(KeyNames.whereEqual, SplitKey.WhiteSpaceOptions.none, false, false);
        public static readonly SplitKey whereNotEqual = new SplitKey(KeyNames.whereNotEqual, SplitKey.WhiteSpaceOptions.none, false, false);
        public static readonly SplitKey whereGreaterThan = new SplitKey(KeyNames.greatThanKey, SplitKey.WhiteSpaceOptions.none, false, false);
        public static readonly SplitKey whereLessThan = new SplitKey(KeyNames.lessThanKey, SplitKey.WhiteSpaceOptions.none, false, false);
    }
    public static class WhereClauseSplitters
    {
        public static readonly SplitKey whereAnd = new SplitKey(KeyNames.whereAndKey, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey whereOr = new SplitKey(KeyNames.whereOrKey, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey whereOpenParen = new SplitKey(KeyNames.openParenthensis, SplitKey.WhiteSpaceOptions.none, false, true);
        public static readonly SplitKey whereCloseParen = new SplitKey(KeyNames.closeParenthesis, SplitKey.WhiteSpaceOptions.none, false, false);
    }
    public static class CommandSplitters
    {
        public static readonly SplitKey openCommand = new SplitKey(KeyNames.openParenthensis, SplitKey.WhiteSpaceOptions.none, false, true, false, KeyNames.comma,  SplitKey.ReplaceTypes.firstInstance);
        public static readonly SplitKey closeCommand = new SplitKey(KeyNames.closeParenthesis, SplitKey.WhiteSpaceOptions.none, false, false, false, "", SplitKey.ReplaceTypes.lastInstance, true, false);
        public static readonly SplitKey colSeparator = new SplitKey(KeyNames.comma, SplitKey.WhiteSpaceOptions.none, false, false);
    }
    public static class InsertSplitters
    {
        public static readonly SplitKey intoKey = new SplitKey(KeyNames.into, SplitKey.WhiteSpaceOptions.include, false, false);
        public static readonly SplitKey byKey = new SplitKey(KeyNames.byKey, SplitKey.WhiteSpaceOptions.include, false, false);
    }
    public static class LimitSpiltters
    {

    }
}

