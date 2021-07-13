using rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace rogue_core.rogueCore.misc.reflector
{
    public static class Reflector
    {
        static Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
        public static Dictionary<string, Type> commandColumnsTypes = GetCommandColumnTypes();
        public static Dictionary<string, Type> GetCommandColumnTypes()
        {
            
            Type parentType = typeof(ColumnCommand);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType)).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            subclasses.ForEach(x => classKeys.Add(x.GetProperty("CodeMatchName").GetValue(x, null).ToString(), x));
            return classKeys;
        }
        public static void test()
        {
            var cols = Reflector.commandColumnsTypes;
            Type myType = cols.First().Value;
            //var item = Activator.CreateInstance<CurrentDate>();
            var cls = Activator.CreateInstance(myType, new string[1] { "HEY" });
            
        }
    }
}
