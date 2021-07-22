using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.prefilled;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using files_and_folders;
namespace rogue_core.rogueCore.hqlSyntaxV4.insert.insertMethods
{
    class Deduplicate : HQLInsert
    {
        public const string codeMatchName = "DEDUPLICATE";
        public static string insertType { get { return codeMatchName; } }
        Dictionary<IRogueTable, Dictionary<string, ColumnRowID>> columnIDs = new Dictionary<IRogueTable, Dictionary<string, ColumnRowID>>();
        List<IColumn> insertParameters = new List<IColumn>();
        //* TODO NEEd to spped this insert up and do all at once. Need like a close command here that writes each table..**
        public Deduplicate(string hql, QueryMetaData metaData) : base(hql, metaData)
        {            
            splitList.Where(x => x.Key == KeyNames.comma).ToList().ForEach(x => insertParameters.Add(BaseColumn.ParseColumn(x.Value, metaData)));
        }
        protected override IEnumerable<IReadOnlyRogueRow> Execute(IMultiRogueRow parentRow, ICalcableFromId tableFrom)
        {            
            var table = tableFrom.CalcTableID(parentRow).ToTable();
            SetColumnIDs(table);
            string checkval = CalcUniqueValue(parentRow);
            bool found = false;
            foreach (var row in table.StreamDataRows())
            {
                if(CalcUniqueValue(row, table).Equals(checkval))
                {
                    yield return row;
                    found = true;
                    break;
                }                    
            }
            //**If the row was not found then create a new one and return the new row**
            if (!found)
            {
                var newRow = table.NewWriteRow();
                foreach(var col in insertParameters)
                {
                    newRow.NewWritePair(table.ioItemID, col.columnName, parentRow.GetValue(col));
                }
                table.Write();
                yield return newRow;
            }
        }
        string CalcUniqueValue(IReadOnlyRogueRow row, IRogueTable table)
        {
            string val = "";
            foreach(var col in columnIDs[table])
            {
                val += row.GetValueByColumn(col.Value);
            }
            return val;
        }
        string CalcUniqueValue(IMultiRogueRow row)
        {
            string val = "";
            foreach (var col in insertParameters)
            {
                val += row.GetValue(col);
            }
            return val;
        }
        void SetColumnIDs(IRogueTable table)
        {
            if (!columnIDs.ContainsKey(table))
            {
                columnIDs.Add(table, new Dictionary<string, ColumnRowID>());
                var tableCols = columnIDs[table];
                foreach (var col in insertParameters)
                {
                    var colID = IORecordTable.columnTable.GetColumnIDByNameAndOwnerIDIfExist(col.columnName, table.ioItemID);
                    tableCols.FindAddIfNotFound(col.columnName, new ColumnRowID(colID.ToString()));
                }
            }            
        }
    }
}
