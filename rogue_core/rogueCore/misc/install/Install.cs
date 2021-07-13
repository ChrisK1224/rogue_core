using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FilesAndFolders;
using System.Collections.Generic;
using rogue_core.rogueCore.id;

namespace rogue_core.rogueCore.install
{
    public static class Install
    {
        public static void InstallAll()
        {
            
            String root = RootVariables.rootPath;
            //String insertPath = install + "inserts.hql";
            // Path.GetFullPath(Directory.GetCurrentDirectory())
            System.IO.DirectoryInfo di = new DirectoryInfo(root);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            DirectoryHelper.createFolderCheckExists(root);
            DirectoryHelper.createFileCheckExists(RootVariables.insertsScriptPath);
            IDIncrement.InstallStart();
            //rogue_core.rogueCore.row.decoded.varchar.filled.FilledVarcharRow testRow = rogue_core.rogueCore.row.decoded.varchar.filled.VarcharIORecord.
            // //*Write all RogueID rows to RogueID Table
            // InstallTable<DecodedRow<RowID>, int, DecodedRowID> number = new InstallTable<DecodedRow<int>, int, DecodedRowID>(metaTablePaths.numberTableFilePath[SystemIDs.EncodingLiteralValue].valueID);
            // foreach (var val in GetStaticClasses<rogue_core.rogueCore.row.decoded.number.filled.FilledNumberRow>())
            // {
            //     number.writeRows.Add(val);
            // }
            //// number.Write();
            //Type t = typeof(rogue_core.rogueCore.row.decoded.varchar.filled.FilledVarcharRow);
            //FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);

            //foreach (FieldInfo fi in fields)
            //{
            //    Console.WriteLine(fi.Name);
            //    Console.WriteLine(fi.GetValue(null).ToString());
            //}
            ////*Write all numbers to Number Table */
            //InstallTable<DecodedRow<int>, int, DecodedRowID> number = new InstallTable<DecodedRow<int>, int, DecodedRowID>(RootVariables.rootPath + metaTablePaths.rogueIDTableFilePath[SystemIDs.Columns.literalValueID].valueID);
            ////foreach (var val in GetStaticClasses<rogue_core.rogueCore.row.decoded.number.filled.FilledNumberRow>())
            ////{
            ////    number.writeRows.Add(val);
            ////}
            ////number.Write();
            ////*Write all Columns to Column Table */
            //InstallTable<ColumnRow, DecodedRowID, ColumnRowID> columns = new InstallTable<ColumnRow, DecodedRowID, ColumnRowID>(RootVariables.rootPath + metaTablePaths.columnTableFilePath[SystemIDs.Columns.literalValueID].valueID);
            //foreach (var val in GetStaticClasses<rogue_core.rogueCore.row.encoded.column.FilledColumn>())
            //{
            //    columns.writeRows.Add(val);
            //}
            //columns.Write();
            ////*Write all date to date Table */
            ////InstallTable<DecodedRow<DateTime>, DateTime, DecodedRowID> date = new InstallTable<DecodedRow<DateTime>, DateTime, DecodedRowID>(metaTablePaths.dateTableFilePath[SystemIDs.Columns.literalValueID].valueID);
            //// foreach (var val in GetStaticClasses<rogue_core.rogueCore.row.decoded.number.filled.FilledNumberRow>())
            //// {
            ////     date.writeRows.Add(val);
            //// }
            //// date.Write();
            ////*Write all IO Record Rows */
            //InstallTable<IORecordRow, DecodedRowID, IORecordID> ioRecords = new InstallTable<IORecordRow, DecodedRowID, IORecordID>(RootVariables.rootPath + metaTablePaths.IORecordTableFilePath[SystemIDs.Columns.literalValueID].valueID);
            //foreach (var val in GetStaticClasses<rogue_core.rogueCore.row.encoded.ioRecordRow.filled.FilledIORecord>())
            //{
            //    ioRecords.writeRows.Add(val);
            //}
            //ioRecords.Write();
           
            ////*Write all varchars to Varchar Table */
            //InstallTable<DecodedRow<String>, String, DecodedRowID> varchar = new InstallTable<DecodedRow<String>, String, DecodedRowID>(RootVariables.rootPath + metaTablePaths.varcharTableFilePath[SystemIDs.Columns.literalValueID].valueID);
            //foreach (var val in GetStaticClasses<rogue_core.rogueCore.row.decoded.varchar.filled.FilledVarcharRow>())
            //{
            //    varchar.writeRows.Add(val);
            //}
            //varchar.Write();
            ////*Write all date to date Table */
            ////InstallTable<DecodedRow<DateTime>, DateTime, DecodedRowID> date = new InstallTable<DecodedRow<DateTime>, DateTime, DecodedRowID>(metaTablePaths.dateTableFilePath[SystemIDs.Columns.literalValueID].valueID);
            //// foreach (var val in GetStaticClasses<rogue_core.rogueCore.row.decoded.number.filled.FilledNumberRow>())
            //// {
            ////     date.writeRows.Add(val);
            //// }
            //// date.Write();
           
            
            ////*Write all UI Control Record Rows */
            //InstallTable<RogueDataRow, DecodedRowID, DataRowID> uiRecords = new InstallTable<RogueDataRow, DecodedRowID, DataRowID>(RootVariables.rootPath + varchars.uiControlTablePath[SystemIDs.Columns.literalValueID].valueID);
            //foreach (var val in GetStaticClasses<rogue_core.rogueCore.codeGenerator.GeneratedRows.FilledUIRecord>())
            //{
            //    uiRecords.writeRows.Add(val);
            //}
            //uiRecords.Write();
            ////*Write all UI Attribute Record Rows
            //InstallTable<RogueDataRow, DecodedRowID, DataRowID> uiAttributes = new InstallTable<RogueDataRow, DecodedRowID, DataRowID>(RootVariables.rootPath + varchars.uiAttributesTablePath[SystemIDs.Columns.literalValueID].valueID);
            //foreach (var val in GetStaticClasses<rogue_core.rogueCore.codeGenerator.GeneratedRows.FilledAttributeRecord>())
            //{
            //    uiAttributes.writeRows.Add(val);
            //}
            //uiAttributes.Write();
           // IORecordTable test = new IORecordTable();
           // IORecordRow ioRecordRow = test.rows[-1006];
           // IORecordRow ioRecordRow2 = FullTables.ioRecordTable.rows[-1006];
           // //FilledTables.IORecords.
           // DataTypeDatabase ds = new DataTypeDatabase();
           //// IORecordTable test = new IORecordTable();
           // StockBundle blah = new StockBundle();
           // IORecordTable tb = new IORecordTable();
            
           // FullTables.ioRecordTable.Description();
           // StockBundle tbl = new StockBundle();
            //RogueDatabase<DataRowID> uiDB = FullTables.stockBundle.GetDatabase<DataRowID>("UIConfig", "This Database hold all tables related to building User Intefaces");
            //IRogueTable uiControls = uiDB.GetTable("UIControls", "This table holds all the ui controls for use in HQL queries to create different kinds of UIs.");
            
            // ioTbl.writeRows.Add(ioTbl);
            // ioTbl.writeRows.Add(varchars);
            //ioTbl.writeRows.Add(cols);
            //ioTbl.write();
            //var stuff = ReflectiveEnumerator.GetEnumerableOfType<rogue_core.rogueCore.row.decoded.varchar.filled.FilledVarcharRow>();
            //  var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            // foreach (var thsAssembly in assemblies)
            // {
            //     // var theList = Assembly.GetExecutingAssembly().GetTypes()
            //     //           .Where(t => t.Namespace == "rogue_core.rogueCore.row.decoded.varchar.filled")
            //     //           .ToList();
            //     // foreach (var assembly in assemblies)
            //     // {
            //         var values = GetStaticClasses<rogue_core.rogueCore.row.decoded.varchar.filled.FilledVarcharRow>(thsAssembly);
            //         foreach (var val in values)
            //         {
            //             varchars.writeRows.Add(val);
            //         }
            //     //}
            // }

            // Assembly myAsses = typeof(rogue_core.rogueCore.row.encoded.column.rootBundleCols).Assembly;
            // var values = from type in myAsses.GetTypes()
            //              from field in type.GetFields(BindingFlags.Static |
            //                                           BindingFlags.Public |
            //                                           BindingFlags.NonPublic)
            //              where field.IsInitOnly &&
            //                    field.FieldType == typeof(rogue_core.rogueCore.row.encoded.column.FilledColumn)
            //              select (rogue_core.rogueCore.row.encoded.column.FilledColumn)field.GetValue(null);

            // foreach (var type in values)
            // {
            //     varchars.writeRows.Add(type);
            //     Console.WriteLine(type.ToString());
            // }
            // varchars.write();
            // var output = FindAllDerivedTypes<rogue_core.rogueCore.row.encoded.column.FilledColumn>(myAsses);
            // foreach (var type in output)
            // {
            //     Console.WriteLine(type.Name);
            // }


            //             Assembly myAsses = Assembly.GetCallingAssembly();
            // Type myType = myAsses.GetType("");

            //         var types = Assembly
            // .GetExecutingAssembly()
            // .GetTypes()
            // .Where(t => typeof(myType).IsAssignableFrom(t) &&
            //             t != typeof(myType))
            // .ToArray();


        }
        public static List<T> GetStaticClasses<T>()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<T> allValues = new List<T>();
            foreach (var thsAssembly in assemblies)
            {
                //Assembly myAsses = typeof(rogue_core.rogueCore.row.encoded.column.rootBundleCols).Assembly;
                var values = from type in thsAssembly.GetTypes()
                             from field in type.GetFields(BindingFlags.Static |
                                                          BindingFlags.Public |
                                                          BindingFlags.NonPublic)
                             where field.IsInitOnly &&
                                   field.FieldType == typeof(T)
                             select (T)field.GetValue(null);
                if (values != null)
                {
                    foreach (T thsT in values)
                    {
                        allValues.Add(thsT);
                    }
                }
                //allValues.Concat(values ?? Enumerable.Empty<T>());
            }
            return allValues;
        }
        public static IEnumerable<T> GetStaticClassesFromOneClass<T, L>(Type typ)
        {
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //List<T> allValues = new List<T>();
            // foreach (var thsAssembly in assemblies)
            // {
                //L = rogue_core.rogueCore.row.encoded.column.rootBundleCols
                Assembly myAsses = typeof(L).Assembly;
                var values = from type in myAsses.GetTypes() where type.IsSubclassOf(typ)
                             from field in type.GetFields(BindingFlags.Static |
                                                          BindingFlags.Public |
                                                          BindingFlags.NonPublic)
                             where field.IsInitOnly && 
                                   field.FieldType == typeof(T)
                             select (T)field.GetValue(null);
                // foreach (T thsT in values)
                // {
                //     allValues.Add(thsT);
                // }
                //allValues.Concat(values ?? Enumerable.Empty<T>());
            //}
            return values;
        }
        static void test(String nspace)
        {
            // var q = from t in Assembly.GetExecutingAssembly().GetTypes()
            //                     where t.IsClass && t.Namespace == nspace
            //                     select t;
            //             q.ToList().ForEach(t => Console.WriteLine(t.Name));

            var q2 = from t in Assembly.GetCallingAssembly().GetTypes()
                     where t.IsClass && t.Namespace == nspace
                     select t;
            q2.ToList().ForEach(t => Console.WriteLine(t.Name));


        }
        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly
                .GetTypes()
                .Where(t =>
                    t != derivedType &&
                    derivedType.IsAssignableFrom(t)
                    ).ToList();

        }
    }
    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>() where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type));
            }
            objects.Sort();
            return objects;
        }
        public static IEnumerable<T> GetStatics<T>(Assembly myAsses) where T : class
        {

            var values = from type in myAsses.GetTypes()
                         from field in type.GetFields(BindingFlags.Static |
                                                      BindingFlags.Public |
                                                      BindingFlags.NonPublic)
                         where field.IsInitOnly &&
                               field.FieldType == typeof(T)
                         select (T)field.GetValue(null);
            return values;
        }
    }
    public static class ReflectionHelpers
    {
        public static System.Type[] GetAllDerivedTypes(this System.AppDomain aAppDomain, System.Type aType)
        {
            var result = new List<System.Type>();
            var assemblies = aAppDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(aType))
                        result.Add(type);
                }
            }
            return result.ToArray();
        }
    }
}