using rogue_core.rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.columnCommands;
using rogue_core.rogueCore.hqlSyntaxV4;
using rogue_core.rogueCore.hqlSyntaxV4.group.convert;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.command.executable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

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
        public static myType GetCommandInstance<myType>(string commandName, string hqlTxt, QueryMetaData metaData) where myType : Type
        {
            Type parentType = typeof(myType);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType)).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            subclasses.ForEach(x => classKeys.Add(x.GetProperty("CodeMatchName").GetValue(x, null).ToString().ToUpper(), x));
            Type type = typeof(myType);
            return (myType)Activator.CreateInstance(type, new object[2] { hqlTxt, metaData });
            //return classKeys[commandName.ToUpper()];
        }
        public static CommandLevel GetCommandInstance(string commandName, string hqlTxt, QueryMetaData metaData)
        {
            Type parentType = typeof(CommandLevel);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType)).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            subclasses.ForEach(x => classKeys.Add(x.GetProperty("CodeMatchName").GetValue(x, null).ToString().ToUpper(), x));
            Type type = typeof(CommandLevel);
            return (CommandLevel)Activator.CreateInstance(type, new object[2] { hqlTxt, metaData });
            //return classKeys[commandName.ToUpper()];
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
        static Dictionary<string, Type> GetCommandTypes()
        {
            var type = typeof(CommandLevel);
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
        public static CommandLevel GetCommandLevelType(string classNm, string hqlTxt, string idName, QueryMetaData metaData)
        {
            var grps = GetCommandTypes();
            Type myType = grps[classNm];
            return (CommandLevel)Activator.CreateInstance(myType, new object[2] { hqlTxt, metaData });
        }
        public static object InitiateClassByName(string className)
        {
            Type type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.Name.ToUpper() == className.ToUpper() && !p.IsInterface && !p.IsAbstract).ToList().First();
            
            //Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            //types.ForEach(x => classKeys.Add(x.GetProperty("codeMatchName").GetValue(x, null).ToString(), x));
            return Activator.CreateInstance(type);
        }
        public static string NameOfCallingClass()
        {
            string fullName;
            Type declaringType;
            int skipFrames = 2;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName;
        }
        public static object SetModelProperties(string className, IMultiRogueRow valueRow)
        {
            var obj = InitiateClassByName(className);
            var propLst = obj.GetType().GetProperties().ToList();
            var rowLst = valueRow.GetValueList();
            foreach(var prop in propLst)
            {
                prop.SetValue(obj, Convert.ChangeType(rowLst.Where(x => x.Key.ToUpper() == prop.Name.ToUpper()).First().Value, prop.PropertyType), null);
            }
            //foreach (var col in valueRow.GetValueList())
            //{
            //    PropertyInfo prop = propLst.Where(x => x.Name.ToUpper() == col.Key.ToUpper()).First();
            //    if (null != prop && prop.CanWrite)
            //    {
            //        prop.SetValue(obj, Convert.ChangeType(col.Value, prop.PropertyType), null);
            //    }
            //}
            return obj;
        }
    }
}
