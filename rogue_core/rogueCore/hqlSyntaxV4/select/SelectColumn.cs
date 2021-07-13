using FilesAndFolders;
using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    public class SelectColumn  : CalcableGroups, ISelectColumn, ISelectColOrStar
    {
        public string columnName { get; private set; }
        public string upperColumnName { get { return columnName.ToUpper(); } }
        public List<ISelectColumn> generatedColumns { get { return new List<ISelectColumn>() { this }; } }
        public ColumnRowID baseColumnID => throw new NotImplementedException();
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public SelectColumn(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            //columnName = base.columns[base.columns.Count - 1].columnName;
            columnName = base.name;
        }
        public void ResetColName(string colName)
        {
            columnName = colName;
        }
        public override string PrintDetails()
        {
            return "SelectName:" + columnName;
        }
    }
}
