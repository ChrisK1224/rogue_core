using rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands;
using rogue_core.rogueCore.hqlSyntaxV4;
using rogue_core.rogueCore.hqlSyntaxV4.group.convert;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq;
namespace rogue_core.rogueCore.misc.reflector
{
    public static class Reflector
    {
        static Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
        static Dictionary<string, Type> commandColumnsTypes = GetCommandColumnTypes();
        static Dictionary<string, Type> groupConvertTypes = GetGroupConvertTypes();
        static Dictionary<string, Type> GetCommandColumnTypes()
        {
            
            Type parentType = typeof(ColumnCommand);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType)).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            subclasses.ForEach(x => classKeys.Add(x.GetProperty("CodeMatchName").GetValue(x, null).ToString(), x));
            return classKeys;
        }
        static Dictionary<string, Type> GetGroupConvertTypes()
        {
            var type = typeof(IGroupConvert);
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            types.ForEach(x => classKeys.Add(x.GetProperty("codeMatchName").GetValue(x, null).ToString(), x));
            return classKeys;
        }
        public static void test()
        {
            var cols = Reflector.commandColumnsTypes;
            Type myType = cols.First().Value;
            var cls = Activator.CreateInstance(myType, new string[1] { "HEY" });            
        }
        public static IGroupConvert GetNewGroupConvert(string classNm, string hqlTxt, string idName, QueryMetaData metaData)
        {
            var grps = Reflector.groupConvertTypes;
            Type myType = grps[classNm];
            return (IGroupConvert)Activator.CreateInstance(myType, new object[2] { hqlTxt, metaData });
        }
    }
}
