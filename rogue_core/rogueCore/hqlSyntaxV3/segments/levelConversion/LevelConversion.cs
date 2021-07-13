//using FilesAndFolders;
//using rogue_core.rogueCore.hqlSyntaxV3.segments;
//using rogue_core.rogueCore.install;
//using rogue_core.rogueCore.queryResults;
//using rogue_core.rogueCore.row;
//using rogue_core.rogueCore.row.encoded.manual;
//using rogueCore.hqlSyntaxV3.filledSegments;
//using rogueCore.hqlSyntaxV3.segments.select;
//using rogueCore.hqlSyntaxV3.segments.table;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;

//namespace rogueCore.hqlSyntaxV3.segments.levelConversion
//{
//    //class LevelConversion
//    //{
//    //    internal const string splitter = "CONVERT";
//    //    internal Action<List<MultiRogueRow>> ConversionFunc;
//    //    internal Action<List<MultiRogueRow>> PreFillMod;
//    //    ConversionTypes conversionType;
//    //    SelectHQLStatement queryStatement;
//    //    SelectRow selectRow;
//    //    string baseLevelName { get; }
//    //    int baseLevelNum { get; }
//    //    internal LevelConversion(string hqlTxt, string levelName, int levelNum, SelectHQLStatement queryStatement) 
//    //    {
//    //        this.queryStatement = queryStatement;
//    //        this.baseLevelName = levelName;
//    //        this.baseLevelNum = levelNum;
//    //        conversionType = ConversionTypes.none;
//    //        ConversionFunc = NoConverstion;
//    //        PreFillMod = PreFillNone;
//    //        if (hqlTxt != "")
//    //        {
//    //            selectRow = new SelectRow(stringHelper.get_string_between_2(hqlTxt, "(", ")"), queryStatement);
//    //            conversionType = ConversionTypes.GetByString(hqlTxt.BeforeFirstChar('(').Trim().ToLower());
//    //            if(conversionType == ConversionTypes.rowToColumn)
//    //            {
//    //                InitalizeRowToColumn();
//    //            }
//    //            else if (conversionType == ConversionTypes.hierarchyTable)
//    //            {
//    //                InitalizeHierarchyTable();
//    //                PreFillMod = PreFillHierarchyTable;
//    //            }
//    //        }
//    //    }
//    //    internal static void NoConverstion(List<MultiRogueRow> thsRows) { }
//    //    void InitalizeRowToColumn()
//    //    {
//    //        ConversionFunc = RowToColumn;
//    //    }
//    //    void InitalizeHierarchyTable()
//    //    {
//    //        ConversionFunc = HierarchyTable;
//    //    }
//    //    void PreFillHierarchyTable(List<MultiRogueRow> parentRows)
//    //    {
//    //    }
//    //    void PreFillNone(List<MultiRogueRow> parentRows)
//    //    {
            
//    //    }
//    //    void RowToColumn(List<MultiRogueRow> rows)
//    //    {
//    //    }
//    //    void HierarchyTable(List<MultiRogueRow> rows)
//    //    {
//    //    }
//    //}
//    //public class ConversionTypes : EnumBaseType<ConversionTypes>
//    //{
//    //    public static readonly ConversionTypes rowToColumn = new ConversionTypes(1, "rowtocolumn");
//    //    public static readonly ConversionTypes hierarchyTable = new ConversionTypes(3, "hierarchytable");
//    //    public static readonly ConversionTypes none = new ConversionTypes(2, "none");
       
//    //    public ConversionTypes(int key, string value) : base(key, value)
//    //    {

//    //    }
//    //    public static ReadOnlyCollection<ConversionTypes> GetValues()
//    //    {
//    //        return GetBaseValues();
//    //    }

//    //    public static ConversionTypes GetByKey(int key)
//    //    {
//    //        return GetBaseByKey(key);
//    //    }
//    //}
//}
