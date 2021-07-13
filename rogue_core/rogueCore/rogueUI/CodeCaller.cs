//using rogueCore.api;
//using rogueCore.hqlSyntax;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace rogue_core.rogueCore.rogueUI
//{
//    public class CodeCaller
//    {
//        static Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
//        public static string RunProcedure(string methodName, string[] parameters)
//        {
//           // Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
//            var myCodeCaller = co.GetType("rogue_core.rogueCore.rogueUI.CodeCaller");
//            return InvokeStringMethod(myCodeCaller, methodName, parameters);
//        }
//        static string InvokeStringMethod(Type calledType, string methodName, string[] parameters)
//        {
//            MethodInfo methodInfo = calledType.GetMethod(methodName);
//            return (string)methodInfo.Invoke(null, parameters);
//        }
//        static string FORMATTED_QRY_TEXT(string origQry)
//        {
//            if(origQry == "")
//            {
//                return "";
//            }
//            return String.Join("&#13;&#10;", HumanHQLStatement.FormattedText(origQry));
//        }
        
//    }
//}
