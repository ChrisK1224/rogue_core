using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.prefilled;
using rogue_core.rogueCore.hqlSyntaxV4.level.command.uiCommands;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select.command
{
    class UITemplate : SplitSegment, ISelectRow
    {
        public List<ISelectColumn> selectColumns { get; } = new List<ISelectColumn>();
        //public List<CalcableGroups> commandParams { get; private set; } = new List<CalcableGroups>();
        public Dictionary<string, ISelectColumn> columnsByName { get; } = new Dictionary<string, ISelectColumn>();
        public const string commandNameIDConst = "UI_CONTROL";
        public static string CodeMatchName { get { return commandNameIDConst; } }
        public override List<SplitKey> splitKeys { get { return new List<SplitKey>() { LocationSplitters.AsKey, CommandSplitters.colSeparator, CommandSplitters.openCommand, CommandSplitters.closeCommand }; } }
        public UITemplate(string txt, QueryMetaData metaData) : base(txt, metaData)
        {
            var lst = splitList.Where(x => x.Key == KeyNames.comma).ToList();
            var colnames = new List<string>() {  BinaryDataTable.columnTable.GetColumnNameByID(UICommand.uiColumn_ControlValue),
            BinaryDataTable.columnTable.GetColumnNameByID(UICommand.uiColumn_ControlStyle) };
            var autoColName = BinaryDataTable.columnTable.GetColumnNameByID(UICommand.uiColumn_ControlType);
            var autoCol = new SelectColumn("\"tr\"", metaData, autoColName); 
            selectColumns.Add(autoCol); 
            columnsByName.Add(autoColName, autoCol);
            for (int i =0; i < lst.Count; i++)
            {
                var col = new SelectColumn(lst[i].Value, metaData, colnames[i]); selectColumns.Add(col); columnsByName.Add(colnames[i], col);
            }
        }
        public override string PrintDetails()
        {
            return "";
        }
    }
}
