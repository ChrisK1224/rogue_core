using FilesAndFolders;
using files_and_folders;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using static rogueCore.hqlSyntaxV3.MutliSegmentEnum;
using rogue_core.rogueCore.hqlSyntaxV3.segments;
using System.Text.RegularExpressions;
using rogue_core.rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogue_core.rogueCore.syntaxCommand;

namespace rogueCore.hqlSyntaxV3.segments.select
{
    public class SelectColumn  : ComplexColumn, ISelectColumn, ISelectColOrStar
    {
        public string columnName { get; set; }
        public string upperColumnName { get; set; }
        public string colTableRefName { get { return column.colTableRefName; } }
        public string upperColumnTableRefName { get { return column.colTableRefName; } }
        public List<ISelectColumn> generatedColumns { get { return new List<ISelectColumn>() { this }; } }
        public ColumnRowID baseColumnID { get { return base.column.columnRowID; } }
        string origTxt;
        Action<IMultiRogueRow, ISyntaxPartCommands> LocalSyntaxParts;
        public SelectColumn(string colTxt) : base(colTxt)
        {
            origTxt = colTxt;
            
            //upperColumnTableRefName = columnTableRefName.ToUpper();
            //var lstSplitTxt = StripAliasName(colTxt);
            //columnName = lstSplitTxt.Count > 1 ? lstSplitTxt[1].Trim() : base.column.columnRowID.ToColumnName();
        }
        public new void PreFill(QueryMetaData metaData, string levelName)
        {
            try
            {
                base.PreFill(metaData, levelName);
                //if(columnName == "")
                //{
                columnName = columns[columns.Count - 1].columnName;
                //}
                upperColumnName = columnName.ToUpper();
                LocalSyntaxParts = StandardSyntaxParts;
            }
            catch(Exception ex)
            {
                LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public void ResetColName(string colName)
        {
            columnName = colName;
            upperColumnName = colName.ToUpper();
        }
        
    }
}
