//using rogue_core.rogueCore.hqlSyntaxV2.segments;
//using rogueCore.api;
//using rogueCore.hqlSyntax;
//using rogueCore.hqlSyntaxV2.filledSegments;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace rogueCore.rogueUIV2
//{
//    public class CodeCaller
//    {
//        static Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
//        public static object RunProcedure(string methodName, string[] parameters)
//        {
//           // Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
//            var myCodeCaller = co.GetType("rogueCore.rogueUIV2.CodeCaller");
//            return InvokeStringMethod(myCodeCaller, methodName, parameters);
//        }
//        public static object RunProcedure(string methodName, object[] parameters)
//        {
//            // Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
//            var myCodeCaller = co.GetType("rogueCore.rogueUIV2.CodeCaller");
//            return InvokeStringMethod(myCodeCaller, methodName, parameters);
//        }
//        public static object RunProcedure(string methodName, MultiRogueRow[] parameters)
//        {
//            // Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
//            var myCodeCaller = co.GetType("rogueCore.rogueUIV2.CodeCaller");
//            return InvokeStringMethod(myCodeCaller, methodName, parameters);
//        }
//        static object InvokeStringMethod(Type calledType, string methodName, string[] parameters)
//        {
//            MethodInfo methodInfo = calledType.GetMethod(methodName);
//            return methodInfo.Invoke(null, parameters);
//        }
//        static string InvokeStringMethod(Type calledType, string methodName, MultiRogueRow[] parameters)
//        {
//            MethodInfo methodInfo = calledType.GetMethod(methodName);
//            return (string)methodInfo.Invoke(null, parameters);
//        }
//        static object InvokeStringMethod(Type calledType, string methodName, object[] parameters)
//        {
//            MethodInfo methodInfo = calledType.GetMethod(methodName);
//            return methodInfo.Invoke(null, parameters);
//        }
//        public List<String> IntelliOptions(string[] origQryParam)
//        {
//            List<string> options = new List<string> { "FROM", "SELECT", "JOIN", "WHERE", "SNIPPET" };
//            return options;
//        }
//        public static List<MultiRogueRow> UI_QRY_TEXT(string[] origQryParam, MultiRogueRow parentRow)
//        {
//            string origQry = origQryParam[0];
//            string finalQry;
//            if (origQry.StartsWith("\"")){
//                finalQry = origQry.Trim().Substring(1, origQry.Length - 2);
//            }
//            else
//            {
//                finalQry = FilledHQLQuery.GetQueryByID(int.Parse(origQry));
//            }
//            var qry = new FilledHQLQuery(finalQry);
//            return QueryDecoration.GetUIQueryResults(parentRow, qry.nonModifiedQry);
//        }
//        public static string RUN_API(MultiRogueRow topRows)
//        {
//            new APIConnect(topRows);
//            return "SUCCESS";
//        }
//    }
//}
