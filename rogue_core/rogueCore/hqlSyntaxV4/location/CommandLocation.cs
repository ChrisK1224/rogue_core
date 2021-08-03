using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.column.command;
using rogue_core.rogueCore.hqlSyntaxV4.select;
using rogueCore.rogueUIV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    public class CommandLocation : BaseLocation, ILocation, IWithinParenthesis
    {
        string commandName { get; }
        public List<CalcableGroups> commandParams { get; private set; } = new List<CalcableGroups>();
        public string name { get; private set; }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LocationSplitters.AsKey, CommandSplitters.colSeparator, CommandSplitters.openCommand, CommandSplitters.closeCommand }; } }        
        public CommandLocation(string locationTxt, QueryMetaData metaData) : base(locationTxt, metaData)
        {
            //**TODO Need to add check for not inlcuding not between queote in regex pattern*******
            commandName = splitList.Where(x => x.Key == KeyNames.openParenthensis).FirstOrDefault().Value;
            commandName = commandName.ToUpper();
            name = (GetAliasName() == "") ? metaData.NextUnnamedColumn() : GetAliasName();
            name = name.ToUpper();
            //splitList.Where(x => x.Key == BaseLocation.colSeparator || x.Key == SegmentBase.startKey).ToList().ForEach(x => commandParams.Add(new ComplexColumn(x.Value, metaData)));
            splitList.Where(x => x.Key == KeyNames.comma).ToList().ForEach(x => commandParams.Add(new CalcableGroups(x.Value, metaData)));
        }
        public static IColumn GetCommandColumn(string colTxt, QueryMetaData metaData)
        {
            Type parentType = typeof(CommandLocation);
            Type iColumnType = typeof(IColumn);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType) && t.IsSubclassOf(iColumnType)).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            subclasses.ForEach(x => classKeys.Add(x.GetProperty("CodeMatchName").GetValue(x, null).ToString(), x));
            string parseTxt = colTxt.ToUpper();
            string idName = colTxt.BeforeFirstChar('(').ToUpper();
            switch (idName)
            {
                case "CURRENT_DATE":
                    return new CurrentDate(colTxt, metaData);
                case "FILE_CONTENT":
                    return new FileContent(colTxt, metaData);
                case "DATE_TO_EPOCH":
                    return new DateToEpoch(colTxt, metaData);
                case "EPOCH_TO_DATE":
                    return new EpochToDate(colTxt, metaData);
                case "SENTIMENT_ANALYSIS":
                    return new SentimentAnalysis(colTxt, metaData);
                case "SENTIMENT_CLASSIFICATION":
                    return new SentimentClassification(colTxt, metaData);
                case "GENERATE_ML_MODEL":
                    return new GenerateMLModel(colTxt, metaData);
                case "ADD_DAYS":
                    return new AddDays(colTxt, metaData);
                default:
                    throw new Exception("Unknown command location type");
            }
        }
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
        }
        public override string PrintDetails()
        {
            string details = "commandName:" + commandName;
            //foreach(var param in commandParams)
            //{
            //    details += ", paramName:" + param.columns[0].columnName;
            //}
            return details;
        }
    }
}
