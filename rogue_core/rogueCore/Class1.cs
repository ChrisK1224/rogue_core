//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using rogue_core.rogueCore.id.rogueID;
//using rogue_core.rogueCore.row;
//using rogue_core.rogueCore.table;
//using rogueCore.hqlSyntax;

//namespace rogue_core
//{

//    // public interface Itable<in TRow> where TRow : row
//    // {
//    //     // IfindLst();
//    // }
//    //class Sample<A> : IContravariant<A> { }
//    public static class Roguetester
//    {
//        public static void testOne()
//        {
//            IRogueTable tbl = new IORecordID(6291).ToTable();
//            foreach (IRogueRow row in tbl.StreamIRows())
//            {
//                String blah = row.rowID.ToString();
//                row.PrintRow();
//            }
//        }
//        public static void runShit()
//        {
//            Action<rowstatus, FilledSelectRow> act = (rowstatus stat, FilledSelectRow row) => Console.WriteLine("");
//            foreach (var row in new HumanHQLStatement("FROM COLUMNENUMERATIONS SELECT ENUMERATION_VALUE WHERE COLUMN_OID = \"-1020\"").IterateRows(act))
//            {
//                string bb = "";
//            }
//        }
//    }
//    public abstract class table<TRow> where TRow : row
//    {
//        public Dictionary<int, TRow> hereLst = new Dictionary<int, TRow>();
//        public table()
//        {

//        }
//        public Dictionary<int, TRow> IfindLst()
//        {
//            return hereLst;
//        }

//    }
//    //Derive instance of base table class
//    public class tableInstOne : table<rowInstOne>//, Itable<rowInstOne>
//    {

//    }
//    //Base class for row that all derived rows are guarantee to be of
//    public abstract class row
//    {

//    }
//    public class rowInstOne : row
//    {
//        public rowInstOne() { }
//    }
//    public class tester
//    {
//        public static void main()
//        {
//            tableInstOne tblInstOne = new tableInstOne();
//            IEnumerable<row> baseLst3 = tblInstOne.IfindLst().Values;
//            //*For checking intefaces */
//            // var type = typeof(IMyInterface);
//            // var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => type.IsAssignableFrom(p));

//            // var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType == typeof(row));
//            // foreach (var x in types)
//            // {
//            //     MethodInfo methodInfo = x.GetMethod(methodName);

//            //     if (methodInfo != null)
//            //     {
//            //         object result = null;
//            //         ParameterInfo[] parameters = methodInfo.GetParameters();
//            //         object classInstance = Activator.CreateInstance(type, null);

//            //         if (parameters.Length == 0)
//            //         {
//            //             // This works fine
//            //             result = methodInfo.Invoke(classInstance, null);
//            //         }
//            //         else
//            //         {
//            //             object[] parametersArray = new object[] { "Hello" };

//            //             // The invoke does NOT work;
//            //             // it throws "Object does not match target type"             
//            //             result = methodInfo.Invoke(classInstance, parametersArray);
//            //         }

//            //     }
//            //     Console.Read();
//            // Itable<row> tblInstOne = new tableInstOne();
//            // List<row> baseLst = tblInstOne.IfindLst();
//            // //*This will not work since the table is instantiated with a derived class of row but all i need are the base row calls.
//            // AcceptDerviedRowTypeAsBase(tblInstOne);
//            //}
//        }

//    }
//}
