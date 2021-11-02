using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using files_and_folders;
using FilesAndFolders;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public abstract class SplitSegment : ITempBase
    {
        public abstract List<SplitKey> splitKeys { get; }
        //public abstract IEnumerable<string> SyntaxSuggestions();
        string origTxt { get; }
        string modifiedTxt { get; set; }
        protected List<KeyValuePair<string, string>> splitList { get; } = new List<KeyValuePair<string, string>>();
        public SplitSegment(string txt, QueryMetaData metaData)
        {
            try
            {
                this.origTxt = txt;
                modifiedTxt = txt.Trim();                
                metaData.AddSegment(this);
                var replaceKeys = splitKeys.Where(x => x.replaceType != SplitKey.ReplaceTypes.none).ToList();
                replaceKeys.ForEach(x => SetReplacements(x));
                CreateSplitList(modifiedTxt);
                //Initialize(metaData);
                //SyntaxCommand = LoadSyntaxParts;
            }
            catch (Exception ex)
            {
                SyntaxCommand = ErrorSyntaxParts;
            }
        }
        //protected abstract void Initialize(QueryMetaData metaData);
        public void PrintSegment()
        {
            Console.WriteLine(this.GetType().Name + ":" + origTxt + "|" + PrintDetails());
        }
        public abstract string PrintDetails();
        //*This still has probs
        static string outsideQuotesAndParenPattern = @"((?<=\s)|^)(@TYPES)((?=\s)|$)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
        //public static string outsideQuotesAndParenPatternNoSpace = @"(@TYPES)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
        //***CHANGED FROM ABOVE MIGHT NOT NEED IGNORE PARENT DOESN"T WORK ANYWAY
        //**Changed recently to inlcude not between paren
        public static string outsideQuotesAndParenPatternNoSpace = @"(@TYPES)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
        //public static string outsideQuotesAndParenPatternNoSpace = @"(@TYPES)(?=(?:[^\""]|\""[^\""]*?\"")*?$)";
        //THis might work for not between quotes unless ESCAPED:: \+(?=([^"\\]*(\\.|"([^"\\]*\\.)*[^"\\]*"))*[^"]*$)
        //public static string outsideQuotesPatternNoSpace = @"(@TYPES)(?=(?:[^\""]|\""[^\""]*?\"")*?$)(?![^\(]*\))";
        //static string outsideQuotesAndParenPatternNoSpace = @"(@TYPES)";
        void CreateSplitList(string txt)
        {
            bool isFirst = true;
            int firstIndex = 0;
            bool missedFirst = false;
            //var splitList = new List<KeyValuePair<string, string>>();
            //string patternStart = (includeWhitespace) ? outsideQuotesAndParenPattern : outsideQuotesAndParenPatternNoSpace;
            string regexPattern = outsideQuotesAndParenPatternNoSpace.Replace("@TYPES", CreateRegexKeyPattern());
            var stt = new Stopwatch();
            stt.Start();
            var matches = Regex.Matches(txt, regexPattern, RegexOptions.IgnoreCase);
            stt.Stop();
            Console.WriteLine("Regex match:" + stt.ElapsedMilliseconds); 
            List<Match> validMatches = matches.Cast<Match>().Where(J => !String.IsNullOrEmpty(J.Value)).ToList(); 
            //**Probably Sould strip key splitter off string instead of the afterFirstKey bullshit. Not sure where it would negative affect.
            for (int i = 0; i < validMatches.Count; i++)
            {
                var name = validMatches[i].Value.ToUpper().Trim();
                var splitKey = splitKeys.Where(x => x.splitTxt == name).FirstOrDefault();
                var index = validMatches[i].Index + name.Length;                
                if (txt.Substring(0, index).ToUpper() != name && isFirst)
                {
                    missedFirst = true;
                    firstIndex = index - name.Length;
                }
                isFirst = false;
                //**Change text 
                index = (splitKey.includeKeyName) ? validMatches[i].Index : validMatches[i].Index + validMatches[i].Value.Length;
                string value = splitKey.GetValue(validMatches, i, index, txt);
                if (value != "" || splitKey.includeEmpty)
                {
                    splitList.Add(new KeyValuePair<string, string>(splitKey.keyTxt,value ));
                }                
                //if (i < (validMatches.Count - 1))
                //{
                //    var endIndex = validMatches[i + 1].Index;                    
                //    splitList.Add(new KeyValuePair<string,string>(name,txt.Substring(index, endIndex - index).Trim()));
                //}
                //else
                //{
                //    splitList.Add(new KeyValuePair<string, string>(name, txt.Substring(index).Trim()));
                //}
            }
            firstIndex = (validMatches.Count == 0) ? txt.Length : firstIndex;
            if (missedFirst || isFirst)
            {
                var defaultItem = splitKeys.Where(x => x.attachFirstDefault).FirstOrDefault();
                if(defaultItem != null)
                {
                    //splitList.Add(new KeyValuePair<string, string>(defaultItem.keyTxt,  splitKey.GetValue(validMatches, i, index, txt)));
                    splitList.Insert(0, new KeyValuePair<string, string>(defaultItem.keyTxt, txt.Substring(0, firstIndex).Trim()));
                }
                else
                {
                    splitList.Insert(0, new KeyValuePair<string, string>(KeyNames.startKey, txt.Substring(0, firstIndex).Trim()));
                }         
            }

            //*Remove empty entries**
            //splitList = splitList.Where(x => x.Value != "").ToList();
        }
        void SetReplacements(SplitKey key)
        {
            switch (key.replaceType)
            {
                case SplitKey.ReplaceTypes.firstInstance:
                    modifiedTxt = modifiedTxt.ReplaceFirstOccurrence(key.keyTxt, key.replaceKey);
                    break;
                case SplitKey.ReplaceTypes.lastInstance:
                    modifiedTxt = modifiedTxt.ReplaceLastOccurrence(key.keyTxt, key.replaceKey);
                    break;
            }
        }
        string CreateRegexKeyPattern()
        {
            List<string> keyStrs = new List<string>();
            //.Where(x => x.replaceType == SplitKey.ReplaceTypes.none)
            foreach (var key in splitKeys.Where(x => x.splitTxt != ""))
            {
                if (key.includeInSplit)
                {
                    string keyStr = "";
                    if (key.keyTxt.Length == 1)
                    {
                        keyStr += "(\\" + key.splitTxt + ")";
                    }
                    else
                    {
                        keyStr += key.splitTxt;
                    }
                    if (key.includeWhiteSpace == SplitKey.WhiteSpaceOptions.include)
                    {
                        keyStr = "((^|\\s)" + keyStr + "(\\s|$))";
                    }
                    else if (key.includeWhiteSpace == SplitKey.WhiteSpaceOptions.includeFront)
                    {
                        keyStr = "((^|\\s)" + keyStr + ")";
                    }
                    keyStrs.Add(keyStr);
                }                
            }
            return String.Join("|", keyStrs);
        }
        protected Action<IMultiRogueRow, ISyntaxPartCommands> SyntaxCommand { get; set; }
        private void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp; "  + " " + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
        }
        //public abstract void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
    }
}
