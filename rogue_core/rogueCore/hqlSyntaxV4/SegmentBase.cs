//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace rogue_core.rogueCore.hqlSyntaxV4
//{
//    public abstract class SegmentBase : ITempBase
//    {
//        public abstract string[] splitters { get; }
//        string origTxt { get; }
//        //protected const string asKey = "AS";
//        protected const string startKey = "";
//        protected List<KeyValuePair<string, string>> splitList { get; }
//        protected abstract bool includeWhitespace { get; }
//        //protected virtual bool includeParenthesis { get { return false; } }
//        public SegmentBase(string txt, QueryMetaData metaData)
//        {
//            try
//            {
//                txt = txt.Trim();
//                this.origTxt = txt;                
//                metaData.AddSegment(this);
//                splitList = CreateSplitList(Transformer(txt));
//            }
//            catch(Exception ex)
//            {

//            }            
//        }
//        protected virtual string Transformer(string origTxt) { return origTxt; }
       
//        //protected virtual SplitTypes splitType { get { return SplitTypes.match; } }
//        public void PrintSegment()
//        {
//            Console.WriteLine(this.GetType().Name + ":" + origTxt + "|" + PrintDetails());
//        }
//        public abstract string PrintDetails();
//        //also works especially for starts with space or start and ends with space or end (?<=\s|^)(SELECT)(?=\s|$)(?=(?:[^\"]|\"[^\"]*?\")*?$)(?![^\(]*\))
//        //static string outsideQuotesPatternNonParenthesis = @"(?<=\s)(@TYPES)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)";
//        //*This still has probs
//        static string outsideQuotesAndParenPattern = @"((?<=\s)|^)(@TYPES)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
//        static string outsideQuotesAndParenPatternNoSpace = @"(@TYPES)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
        
//        List<KeyValuePair<string, string>> CreateSplitList(string txt)
//        {
//            //*These to flags handle adding start line need check if missed first one OR if none set then add as default
//            bool isFirst = true;
//            int firstIndex = 0;
//            bool missedFirst = false;
//            var segments = new List<KeyValuePair<string, string>>();
//            string patternStart = (includeWhitespace) ? outsideQuotesAndParenPattern : outsideQuotesAndParenPatternNoSpace;
//            string regexPattern = patternStart.Replace("@TYPES", CreateRegexKeyPattern(splitters));
//            var matches = Regex.Matches(txt, regexPattern, RegexOptions.IgnoreCase);
//            List<Match> validMatches = matches.Cast<Match>().Where(J => !String.IsNullOrEmpty(J.Value)).ToList();
//            //**Probably Sould strip key splitter off string instead of the afterFirstKey bullshit. Not sure where it would negative affect.
//            for (int i = 0; i < validMatches.Count; i++)
//            {                
//                var index = validMatches[i].Index + validMatches[i].Value.Length;
//                var name = validMatches[i].Value.ToUpper().Trim();
//                if(txt.Substring(0,index).ToUpper() != name && isFirst)
//                {
//                    missedFirst = true;
//                    firstIndex = index - validMatches[i].Value.Length;
//                }
//                isFirst = false;
//                if (i < (validMatches.Count - 1))
//                {
//                    var endIndex = validMatches[i + 1].Index;
//                    segments.Add(new KeyValuePair<string, string>(name, txt.Substring(index, endIndex - index).Trim()));
//                }
//                else
//                {
//                    segments.Add(new KeyValuePair<string, string>(name, txt.Substring(index).Trim()));
//                }
//            }
//            firstIndex = (validMatches.Count == 0) ? txt.Length : firstIndex;
//            if (missedFirst || isFirst)
//            {
//                segments.Insert(0, new KeyValuePair<string, string>(startKey, txt.Substring(0, firstIndex).Trim()));
//            }
//            return segments;
//            //switch (splitType)
//            //{
//            //    case SplitTypes.match:
//            //        return MatchSplit(regexPattern, txt);
//            //    case SplitTypes.split:
//            //        return SplitSplit(regexPattern, txt);
//            //    default:
//            //        throw new Exception("Unknown Split type");
//            //}
//        }
//        //List<KeyValuePair<string, string>> MatchSplit(string regexPattern, string txt)
//        //{
//        //    var segments = new List<KeyValuePair<string, string>>();
//        //    var matches = Regex.Matches(txt, regexPattern, RegexOptions.IgnoreCase);
//        //    List<Match> validMatches = matches.Cast<Match>().Where(J => !String.IsNullOrEmpty(J.Value)).ToList();
//        //    for (int i = 0; i < validMatches.Count; i++)
//        //    {
//        //        var index = validMatches[i].Index;
//        //        var name = validMatches[i].Value;
//        //        if (i < (validMatches.Count - 1))
//        //        {
//        //            var endIndex = validMatches[i + 1].Index;
//        //            segments.Add(new KeyValuePair<string, string>(name, txt.Substring(index, endIndex - index)));
//        //        }
//        //        else
//        //        {
//        //            segments.Add(new KeyValuePair<string, string>(name, txt.Substring(index)));
//        //        }
//        //    }
//        //    return segments;
//        //}
        
//        //protected enum SplitTypes
//        //{
//        //    match, split
//        //}
//        static string CreateRegexKeyPattern(string[] splitters)
//        {
//            String keyStr = "";
//            foreach (string sep in splitters)
//            {
//                //if (sep.Any(ch => !Char.IsLetterOrDigit(ch)))
//                //{
//                //    keyStr += "\\" + sep;
//                //}
//                if (sep.Length == 1)
//                {
//                    keyStr += "\\" + sep;
//                }               
//                else
//                {
//                    keyStr += sep;
//                }
//                keyStr += "|";
//            }
//            if(splitters.Length > 0)
//            {
//                keyStr = keyStr.Substring(0, keyStr.Length - 1);
//            }            
//            return keyStr;
//        }
//    }
//}
