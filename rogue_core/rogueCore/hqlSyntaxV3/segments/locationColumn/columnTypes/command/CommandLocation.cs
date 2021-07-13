using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.segments.namedLocation;
using rogueCore.rogueUIV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes
{
    public class CommandLocation
    {
        string commandName;
        public string[] commandParams { get; private set; }
        public string name { get; private set; }
        public string upperName { get; private set; }
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        string origTxt { get; }
        public CommandLocation(string locationTxt)
        {
            origTxt = locationTxt;
            try
            {
                NamedLocation namedLoc = new NamedLocation(locationTxt);
                commandName = locationTxt.BeforeFirstChar('(');
                commandName = commandName.ToUpper();
                name = namedLoc.isNameSet ? namedLoc.Name : commandName;
                upperName = name.ToUpper();
                //string splitReg = MutliSegmentEnum.GetOutsideQuotesPattern(new string[1] { "," });
                string splitReg = ",(?= (?:[^\"]*\"[^\"]*\")*[^\"]*$)";
                string paramTxt = stringHelper.get_string_between_2(locationTxt, "(", ")");
                commandParams = Regex.Split(paramTxt, splitReg).Where(x => x !="").ToArray();
                commandParams = commandParams.Select(cmd => cmd.Trim()).ToArray();
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }            
        }
        public void LoadSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            LocalSyntaxParts(parentRow, syntaxCommands);
            //try
            //{
            //    syntaxCommands.GetLabel(parentRow, commandName + "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //    syntaxCommands.GetLabel(parentRow, commandParams[0], IntellsenseDecor.MyColors.black, IntellsenseDecor.Boldness.bold);
            //    for (int i = 1; i < commandParams.Length; i++)
            //    {
            //        syntaxCommands.GetLabel(parentRow, ",&nbsp;" + commandParams[i], IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //    }
            //    syntaxCommands.GetLabel(parentRow, ")&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //    LocalSyntaxParts = StandardSyntaxParts;
            //}
            //catch(Exception ex)
            //{
            //    LocalSyntaxParts = ErrorSyntaxParts;
            //}
        }
        public void StandardSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, commandName + "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            syntaxCommands.GetLabel(parentRow, commandParams[0], IntellsenseDecor.MyColors.black, IntellsenseDecor.Boldness.bold);
            for (int i = 1; i < commandParams.Length; i++)
            {
                syntaxCommands.GetLabel(parentRow, ",&nbsp;" + commandParams[i], IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            }
            syntaxCommands.GetLabel(parentRow, ")&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
        }
        public void ErrorSyntaxParts(IMultiRogueRow parentRow, ISyntaxPartCommands syntaxCommands)
        {
            syntaxCommands.GetLabel(parentRow, origTxt, IntellsenseDecor.MyColors.red, IntellsenseDecor.Boldness.bolder, IntellsenseDecor.FontSize.regular, IntellsenseDecor.Underline.underline);
            //syntaxCommands.GetLabel(parentRow, commandName + "(", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //syntaxCommands.GetLabel(parentRow, commandParams[0], IntellsenseDecor.MyColors.black, IntellsenseDecor.Boldness.bold);
            //for (int i = 1; i < commandParams.Length; i++)
            //{
            //    syntaxCommands.GetLabel(parentRow, ",&nbsp;" + commandParams[i], IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
            //}
            //syntaxCommands.GetLabel(parentRow, ")&nbsp;", IntellsenseDecor.MyColors.blue, IntellsenseDecor.Boldness.bold);
        }
        //public abstract IEnumerable<IReadOnlyRogueRow> RunExecProcedure(IMultiRogueRow parentRow);
        public IEnumerable<IReadOnlyRogueRow> RunExecProcedure(IMultiRogueRow parentRow)
        {
            object[] parameters = new object[2];
            parameters[0] = commandParams;
            parameters[1] = parentRow;
            foreach (var row in (List<IReadOnlyRogueRow>)CodeCaller.RunProcedure(commandName, parameters))
            {
                yield return row;
            }
        }
        public IEnumerable<IReadOnlyRogueRow> RunExecProcedure(Func<IMultiRogueRow, IEnumerable<IReadOnlyRogueRow>>  exec, IMultiRogueRow parentRow)
        {
            object[] parameters = new object[2];
            parameters[0] = commandParams;
            parameters[1] = parentRow;
            foreach (var row in exec(parentRow))
            {
                yield return row;
            }
            //foreach (var row in (List<IReadOnlyRogueRow>)CodeCaller.RunProcedure(commandName, parameters))
            //{
            //    yield return row;
            //}
        }
    }
}
