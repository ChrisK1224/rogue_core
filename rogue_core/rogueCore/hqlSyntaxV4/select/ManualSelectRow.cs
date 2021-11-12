using rogue_core.rogueCore.binary.prefilled;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    class ManualSelectRow : ISelectRow
    {
        public List<ISelectColumn> selectColumns { get; } = new List<ISelectColumn>();
        public Dictionary<string, ISelectColumn> columnsByName { get; } = new Dictionary<string, ISelectColumn>();
        public ManualSelectRow(IORecordID tableId, QueryMetaData metaData, bool includeIdCols = false)
        {
            Func<long, List<IColumnRow>> getCols = includeIdCols ? getCols = binary.BinaryDataTable.columnTable.AllColumnsPerTable : binary.BinaryDataTable.columnTable.AllColumnsPerTableWOIds;
            foreach (var id in getCols(tableId))
            {
                var col = new DirectColumn(metaData.DefaultTableName(), id.ColumnIDName(), id.rowID);
                selectColumns.Add(col);
                columnsByName.Add(id.ColumnIDNameID(), col);
            }
        }
    }
}
