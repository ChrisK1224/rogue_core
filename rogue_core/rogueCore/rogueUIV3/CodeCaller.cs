using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.apiV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.row;
using rogueCore.hqlSyntaxV3.segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace rogueCore.rogueUIV3
{
    public class CodeCaller
    {
        static Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
        public static object RunProcedure(string methodName, string[] parameters)
        {
           // Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
            var myCodeCaller = co.GetType("rogueCore.rogueUIV3.CodeCaller");
            return InvokeStringMethod(myCodeCaller, methodName, parameters);
        }
        public static object RunProcedure(string methodName, object[] parameters)
        {
            // Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
            var myCodeCaller = co.GetType("rogueCore.rogueUIV3.CodeCaller");
            return InvokeStringMethod(myCodeCaller, methodName, parameters);
        }
        public static object RunProcedure(string methodName, IMultiRogueRow[] parameters)
        {
            // Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
            var myCodeCaller = co.GetType("rogueCore.rogueUIV3.CodeCaller");
            return InvokeStringMethod(myCodeCaller, methodName, parameters);
        }
        static object InvokeStringMethod(Type calledType, string methodName, string[] parameters)
        {
            MethodInfo methodInfo = calledType.GetMethod(methodName);
            return methodInfo.Invoke(null, parameters);
        }
        static string InvokeStringMethod(Type calledType, string methodName, MultiRogueRow[] parameters)
        {
            MethodInfo methodInfo = calledType.GetMethod(methodName);
            return (string)methodInfo.Invoke(null, parameters);
        }
        static object InvokeStringMethod(Type calledType, string methodName, object[] parameters)
        {
            MethodInfo methodInfo = calledType.GetMethod(methodName);
            return methodInfo.Invoke(null, parameters);
        }
        public List<String> IntelliOptions(string[] origQryParam)
        {
            List<string> options = new List<string> { "FROM", "SELECT", "JOIN", "WHERE", "SNIPPET" };
            return options;
        }
        public static List<IMultiRogueRow> UI_PARAMETER_FIELDS(string[] origQryParam, IMultiRogueRow parentRow)
        {
            string origQry = origQryParam[0].Trim();
            origQry = origQry.Replace('\0', ' ');
            var qry = new SelectHQLStatement(origQry);
            return qry.GenerateParameterParts(parentRow, qry.ParamPart).childRows;
        }
        public static List<IMultiRogueRow> UI_QRY_TEXT(string[] origQryParam, IMultiRogueRow parentRow)
        {
            string origQry = origQryParam[0].Trim();
            origQry = origQry.Replace('\0', ' ');
            string finalQry;
            //if (origQry.StartsWith("\""))
            //{
            //    //finalQry = hqlTxt.Trim().Substring(1, hqlTxt.Length - 2);
            //    //if()
            //}
            //else
            //{
            //finalQry = SelectHQLStatement.GetQueryByID(int.Parse(origQry));
            //}
            var qry = new SelectHQLStatement(origQry);
            return qry.GenerateIntellisenseParts(parentRow, new SyntaxCommands()).childRows;
        }
        public static List<IMultiRogueRow> UI_QRY_BASIC_RESULTS(string[] origQryParam, IMultiRogueRow parentRow)
        {
            string origQry = origQryParam[0].Trim();
            origQry = origQry.Replace('\0', ' ');
            return new UIQueryResultsBuilder(origQry).Build(parentRow).childRows;
        }
        public static List<IMultiRogueRow> UI_FOLDER_EXPLORER(string[] origQryParam, IMultiRogueRow parentRow)
        {
            string path = origQryParam[0];
            string clickEvent = origQryParam[1];
            return new UIFileExplorer(path,clickEvent, parentRow).topDiv.childRows;
        }
        public static List<IReadOnlyRogueRow> RUN_API(string[] origQryParam, IMultiRogueRow topRow)
        {
            var apiRun = new APIConnect(topRow);
            //var newRow = new ManualMultiRogueRow(topRow);
            var newRow = new ManualBinaryRow();
            newRow.AddPair(2076417, apiRun.resultPath + "data.json");
            newRow.AddPair(2076445, apiRun.resultPath + "metadata.json");
            newRow.AddPair(2076472, apiRun.resultPath);
            newRow.AddPair(2094214, apiRun.segment_NM);
            newRow.AddPair(2094252, apiRun.database_ID);
            return new List<IReadOnlyRogueRow>() { newRow };            
        }
        public static string EXECUTE_API()
        {
            return "";
        }
    }
}
