using System;
using rogue_core.rogueCore.id;

namespace rogue_core.codeGenerator
{
    public class CodeGenerator
    {
        private static String NewColumn(int columnID, dataTypeIDs dataTypeID, String ColumnIDName, columnTypes columnType){
            
            String col = "public static readonly FilledColumn RogueColumnID = new FilledColumn(" + columnID.ToString() + ", SystemIDs.IOTableRecords.Tables.DataTypeTables.rogueIDTableID, ColumnCols.ColColumnTypeColumnName.ID, Numbers.numberOne.ID, RogueColumnIDName.ID, SystemIDs.IOTableRecords.Bundles.rootBundleID);";
            return col;
        }
        public enum dataTypeIDs{
            varchar = SystemIDs.IOTableRecords.Tables.DataTypeTables.varcharTableID, rogueID = SystemIDs.IOTableRecords.Tables.DataTypeTables.rogueIDTableID
            //, number = SystemIDs.IOTableRecords.Tables.DataTypeTables.numberTableID, rogueID = SystemIDs.IOTableRecords.Tables.DataTypeTables.rogueIDTableID, date = SystemIDs.IOTableRecords.Tables.DataTypeTables.dateTableID
        }
        public enum columnTypes{
            column, parentTableRef, childTableRef
        }
    }
}