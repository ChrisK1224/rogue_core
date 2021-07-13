using FilesAndFolders;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.namedLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    abstract class IDableLocation<IDType> : IPotentialParam where IDType : RowID 
    {
        internal const string fullColumnSplitter = ".";
        internal const string columnAliasSep = " AS ";
        protected bool isDirectID { get; set; }
        protected NamedLocation aliasName { get; private set; }
        protected string name = "";
        protected string[] items;
        //public abstract List<string> paramOptions { get; }
        protected string upperName { get; set; }
        protected string origTxt { get; private set; }
        protected IDType ID { get; set; }
        protected Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public IDableLocation(string locTxt)
        {
            try
            {
                origTxt = locTxt;
                aliasName = new NamedLocation(locTxt);
                locTxt = aliasName.remainingTxt.Trim();
                //** Known probably can't handle a encoded col with a diredtID within brackets 
                items = Regex.Split(locTxt, @"\.(?=([^(\]|\}|\"")]*(\[|\{|\"")[^(\[|\{|\"")]*(\]|\}|\""))*[^(\]|\}|\"")]*$)", RegexOptions.ExplicitCapture);
                //string lastSeg = items[items.Length - 1];
                isDirectID = SetDirectID();
                //SetNameAndID(items);
                //upperName = name.ToUpper();
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch (Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        protected IDableLocation(IDType ID) { this.ID = ID;isDirectID = false;  }
        bool SetDirectID()
        {
            //if(Regex.IsMatch(@"^\[.*\]?", colTxt))
            if (items[items.Length-1].StartsWith("[") && items[items.Length - 1].EndsWith("]"))
            {
                items[items.Length - 1] = items[items.Length - 1].TrimFirstAndLastChar();
                //ID = NameToID(colTxt);
                //ID = int.Parse(colTxt);             
                return true;
            }
            else
            {
                return false;
            }
        }
        //public void NormSyntaxParts(IMultiRogueRow parentRow)
        //{
        //    //StandardSyntaxParts(parentRow);
        //    aliasName.LoadSyntaxParts(parentRow);
        //}
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //if (isDirectID)
            //{
            //    //syntaxCommands.GetLabel(parentRow, "[", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //    LocalSyntaxParts(parentRow);
            //    syntaxCommands.GetLabel(parentRow, "]", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bolder);
            //    aliasName.LoadSyntaxParts(parentRow);
            //}
            //else
            //{

            //}
            //var directNmLbl = syntaxCommands.GetLabel(parentRow, "&nbsp;" + String.Join('.',items) + "&nbsp;", IntellsenseDecor.MyColors.black);
            //aliasName.LoadSyntaxParts(directNmLbl);
        }
        protected void AddSyntaxNamePart(IMultiRogueRow parentRow,string loc_NM, IntellsenseDecor.MyColors color, ISyntaxPartCommands syntaxCommands)
        {
            if (loc_NM.StartsWith("@"))
            {
                syntaxCommands.GetLabel(parentRow, loc_NM, IntellsenseDecor.MyColors.yellow);
            }
            else
            {
                syntaxCommands.GetLabel(parentRow, loc_NM, color);
            }
        }
        protected abstract void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands);
        protected void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, "&nbsp;" + origTxt + "&nbsp;", IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //aliasName.LoadSyntaxParts(parentRow);
            //var directNmLbl = syntaxCommands.GetLabel(parentRow, "&nbsp;" + String.Join('.', items) + "&nbsp;", IntellsenseDecor.MyColors.black);
            //aliasName.LoadSyntaxParts(directNmLbl);
        }
        public IEnumerable<string> UnsetParams()
        {
            foreach(string nameTest in items)
            {
                if (nameTest.StartsWith("@"))
                {
                    yield return nameTest;
                }
            }            
        }
    }
}
