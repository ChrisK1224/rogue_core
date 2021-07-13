using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column
{
    class ConstantColumn : ConstantLocation, IColumn
    {
        public string columnName { get; }
        public string upperColumnName { get { return columnName.ToUpper(); } }
        public ConstantColumn(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            try
            {
                columnName = (GetAliasName() == "") ? metaData.NextUnnamedColumn() : GetAliasName();
            }
            catch (Exception ex)
            {
                //LocalSyntaxParts = ErrorSyntaxParts;
            }
        }
        public string RetrieveStringValue(Dictionary<string, IReadOnlyRogueRow> rows) { return constValue; }
    }
}
