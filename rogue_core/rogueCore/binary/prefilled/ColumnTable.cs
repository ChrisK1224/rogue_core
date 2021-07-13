using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.install;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using files_and_folders;
using System.Linq;
using System.IO;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4;

namespace rogue_core.rogueCore.binary.prefilled
{
    public class ColumnTable : PreFilledTable<ColumnRow>
    {
        public static readonly IColumnRow rogueColumnIDRow = new RogueColumnIDRow();
        public static readonly IColumnRow rogueDateAddedRow = new RogueDateAddedRow();
        public Dictionary<long, Dictionary<string, IColumnRow>> columns { get; } = new Dictionary<long, Dictionary<string, IColumnRow>>();
        public Dictionary<long, IColumnRow> columnsByID { get; } = new Dictionary<long, IColumnRow>();
        public ColumnTable() : base("-1011")
        {
            foreach (var row in StreamDataRows())
            {
                var colRow = new ColumnRow(row);
                var owner = long.Parse(colRow.OwnerIOItem());
                var lst = columns.FindAddIfNotFound(owner);
                lst.Add(colRow.ColumnIDNameID(), colRow);
                columnsByID.Add(row.rowID.ToInt(), new ColumnRow(row));
            }
            var foundLst = columns.FindAddIfNotFound(long.Parse(recordIO_OID_Row.OwnerIOItem()));
            foundLst.Add(recordIO_OID_Row.ColumnIDNameID(), recordIO_OID_Row);
            columnsByID.Add(recordIO_OID_Row.rowID.ToInt(), recordIO_OID_Row);

            foundLst.Add(record_Col_OID_Row.ColumnIDNameID(), record_Col_OID_Row);
            columnsByID.Add(record_Col_OID_Row.rowID.ToInt(), record_Col_OID_Row);

            foundLst.Add(refRecordRow_OID.ColumnIDNameID(), refRecordRow_OID);
            columnsByID.Add(refRecordRow_OID.rowID.ToInt(), refRecordRow_OID);

            foundLst.Add(recordComplexIndex.ColumnIDNameID(), recordComplexIndex);
            columnsByID.Add(recordComplexIndex.rowID.ToInt(), recordComplexIndex);

            foundLst.Add(record_Position.ColumnIDNameID(), record_Position);
            columnsByID.Add(record_Position.rowID.ToInt(), record_Position);
        }
        public override void DeleteRewrite(List<IReadOnlyRogueRow> deletedRows)
        {
            throw new Exception("No deletes on column table yet.");
        }
        public override IEnumerable<IReadOnlyRogueRow> StreamDataRows()
        {
            foreach(var row in base.StreamDataRows())
            {
                yield return row;
            }
            //yield return recordIO_OID_Row;
            //yield return record_Col_OID_Row;
            //yield return refRecordRow_OID;
            //yield return record_Position;
            //yield return recordComplexIndex;
        }
        public List<IColumnRow> AllColumnsPerTable(long ioRecordID)
        {
            //**Poor logic for a new table that has no columns yet. Should probably add rogueColumnId automatically when table is made to prevent this
            //TODO **TODO Make table insert insert rogue colum asutomaitcally.**
            List<IColumnRow> tempCols;
            if (columns.ContainsKey(ioRecordID))
            {
               tempCols = new List<IColumnRow>(columns[ioRecordID].Values.ToList());
            }
            else
            {
                tempCols = new List<IColumnRow>();
            }
            tempCols.Add(ColumnTable.rogueColumnIDRow);
            tempCols.Add(ColumnTable.rogueDateAddedRow);
            return tempCols;
        }
        IColumnRow WriteNewColumn(long ownerIOItem, ColumnTypes col_type, string col_nm, long parentTableID = 0, string test_value = "")
        {
            short rowCount = (short)((parentTableID == 0) ? 6 : 7);
            //*Check if below replacement and getting rid of addwriterow works right
            var newRow = base.NewWriteRow();// new BinaryDataRow(complexWordTable);
            newRow.pairs.Add(SystemIDs.Columns.columnNameID, new BinaryDataPair(rowCount, SystemIDs.Columns.columnNameID, col_nm, 0,complexWordTable));
            newRow.pairs.Add(SystemIDs.Columns.columnOrdinalID, new BinaryDataPair(rowCount, SystemIDs.Columns.columnOrdinalID, "1", 0, complexWordTable));
            newRow.pairs.Add(SystemIDs.Columns.columnTypeID, new BinaryDataPair(rowCount, SystemIDs.Columns.columnTypeID, col_type.ToString(), 0, complexWordTable));
            newRow.pairs.Add(SystemIDs.Columns.dataTypeID, new BinaryDataPair(rowCount, SystemIDs.Columns.dataTypeID, BinaryDataPair.GetDataType(test_value).ToString(), 0, complexWordTable));
            newRow.pairs.Add(SystemIDs.Columns.ownerColumnItemID, new BinaryDataPair(rowCount, SystemIDs.Columns.ownerColumnItemID, ownerIOItem.ToString(), 0, complexWordTable));
            if(parentTableID != 0)
                newRow.pairs.Add(SystemIDs.Columns.parentTableID, new BinaryDataPair(rowCount, SystemIDs.Columns.parentTableID, parentTableID.ToString(), 0, complexWordTable));
            newRow.pairs.Add(SystemIDs.Columns.isEnumerated, new BinaryDataPair(rowCount, SystemIDs.Columns.isEnumerated, "false", 1, complexWordTable));
            //AddWriteRow(newRow);
            Write();
            //var foundCols = columns.FindAddIfNotFound((newRow.rowID.ToInt()));
            var foundCols = columns.FindAddIfNotFound(ownerIOItem);
            var col = new ColumnRow(newRow);
            foundCols.Add(col_nm.ToUpper(),col);
            return col;
        }
        public IColumnRow GetWriteColumn(IORecordID ownerIOObject, ColumnTypes col_typ, String col_nm, String test_value = "")
        {
            IColumnRow ths_col = null;
            var foundLst = columns.TryFindReturn(ownerIOObject);
            foundLst.TryGetValue(col_nm.ToUpper(), out ths_col);
            if (ths_col == null)
                ths_col = WriteNewColumn(ownerIOObject, col_typ, col_nm, 0, test_value);            
            return ths_col;
        }
        public IColumnRow GetWriteColumn(IORecordID ownerIOObject, String col_nm, IORecordID parentTableID)
        {
            IColumnRow ths_col = null;
            var foundLst = columns.TryFindReturn(ownerIOObject);
            foundLst.TryGetValue(col_nm.ToUpper(), out ths_col);
            if (ths_col == null)
                ths_col = WriteNewColumn(ownerIOObject,  ColumnTypes.parentColumnRef,col_nm,  parentTableID);
            return ths_col;
        }
        #region QuickQueriesForHQLBuilder
        public ColumnRowID GuessColumnIDByName(String colName, List<string> potentialOwnerIds)
        {
            //* FIXME Terrible rogue columnID code FIX THIS
            if (colName.ToUpper() == "ROGUECOLUMNID")
            {
                return new ColumnRowID(-1012);
            }
            //HQLQuery idQry = new HQLQuery(columnIDBNameAndOwner.Replace("@COLNAME", colName).Replace("@OWNERTABLE", ownerID.ToString()));
            var rows = columnsByID.Where(x => x.Value.Equals(colName.ToUpper()) && potentialOwnerIds.Contains(x.Value.OwnerIOItem())).Select(x => x.Value);
            //HQLQuery idQry = new HQLQuery(columnIDBNameAndOwner.Replace("@COLNAME", colName).Replace("@OWNERTABLE", ownerID.ToString()));
            if (rows.Count() > 1)
            {
                throw new Exception("NON UNIQUE COLUMN NAME");
            }
            return new ColumnRowID(rows.First().rowID.ToInt());
        }
        public ColumnRowID GuessColumnIDByName(String colName, string ownerId)
        {
            
            //* FIXME Terrible rogue columnID code FIX THIS
            if (colName.ToUpper() == "ROGUECOLUMNID")
            {
                return new ColumnRowID(-1012);
            }
            //HQLQuery idQry = new HQLQuery(columnIDBNameAndOwner.Replace("@COLNAME", colName).Replace("@OWNERTABLE", ownerID.ToString()));
            var rows = columnsByID.Where(x => x.Value.ColumnIDNameID().Equals(colName.ToUpper()) && x.Value.OwnerIOItem() == ownerId).Select(x => x.Value);
            //HQLQuery idQry = new HQLQuery(columnIDBNameAndOwner.Replace("@COLNAME", colName).Replace("@OWNERTABLE", ownerID.ToString()));
            if (rows.Count() > 1)
            {
                throw new Exception("NON UNIQUE COLUMN NAME");
            }
            return new ColumnRowID(rows.First().rowID.ToInt());
        }
        public String GetColumnNameByID(long colID)
        {            
            return columnsByID[colID].ColumnIDName();
        }
        public long GetColumnIDByFullName(string fullName)
        {
            string[] AllRecords = fullName.Split(new string[1] { KeyNames.period }, StringSplitOptions.RemoveEmptyEntries);
            String allIOObjects = fullName.Substring(0, fullName.LastIndexOf(KeyNames.period));
            String colName = fullName.Substring(fullName.LastIndexOf(KeyNames.period) + 1, fullName.Length - fullName.LastIndexOf(KeyNames.period) - 1);
            int ownerTableID = BinaryDataTable.ioRecordTable.DecodeTableName(allIOObjects);
            return GetColumnIDByNameAndOwnerID(colName, ownerTableID);
            //String whereClause = "";
            //for(int i =0; i < (allIORecordIDs.Length-1);i++)
            //{
            //    whereClause += SystemIDs.Columns.ownerColumnItemID + "=" + allIORecordIDs[i] + ",";
            //}
            //whereClause = whereClause.Substring(0, whereClause.Length - 1);
            //return new HQLQuery(tableColumnQuery.Replace("@PARENTOBJECTID", parentObjectID.ToString()).Replace("@COLNAME", colName)).hierarchyGrid[0].Value.rowID;
        }
        public long GetColumnIDByNameAndOwnerID(String colName, long parentObjectID)
        {            
            switch (colName.ToUpper())
            {
                case "ROGUECOLUMNID":
                    return -1012;
                case "ROGUEDATEADDED":
                    return -1013;
                case "ROGUE_VALUE":
                    return 8619;
                case "ROGUE_KEY":
                    return 8676;
                case "ROGUE_ROW_ID":
                    return 8637;
                case "ROGUEVALUE":
                    return 0;
            }
            return columns[parentObjectID][colName.ToUpper()].rowID.ToInt();
            //return new HQLQuery(tableColumnQuery.Replace("@PARENTOBJECTID", parentObjectID.ToString()).Replace("@COLNAME", colName)).hierarchyGrid[0].Value.rowID.ToString();
        }
        public static RecordIO_OID_Row recordIO_OID_Row  = new RecordIO_OID_Row();
        public static Record_Col_OID_Row record_Col_OID_Row = new Record_Col_OID_Row();
        public static RefRecordRow_OID refRecordRow_OID = new RefRecordRow_OID();
        public static Record_Position_Index record_Position = new Record_Position_Index();
        public static RefRecordComplexCount_OID recordComplexIndex = new RefRecordComplexCount_OID();

        #endregion
        #region IORecordRowInfo

        #endregion
        public class ColumnTypes : EnumBaseType<ColumnTypes>
        {
            //public static readonly ColumnTypes column = new ColumnTypes(BinaryDataTable.simpleTable.GetValue("Column").valueID,VarColumnCols.ColColumnTypeColumnName.rowID.ToInt(), "Column");
            public static readonly ColumnTypes column = new ColumnTypes(BinaryDataTable.simpleTable.GetValue("Column").valueID, "Column");
            public static readonly ColumnTypes parentColumnRef = new ColumnTypes(BinaryDataTable.simpleTable.GetValue("parentColumnRef").valueID, "parentColumnRef");
            //public static readonly ColumnTypes parentColumnRef = new ColumnTypes(VarIORecordCols.ColumnParentTableRefName.rowID.ToInt(), "parentColumnRef");
            public ColumnTypes(long key, string value) : base(key, value)
            {

            }
            public static ReadOnlyCollection<ColumnTypes> GetValues()
            {
                return GetBaseValues();
            }
            public static ColumnTypes GetByKey(long key)
            {
                return GetBaseByKey(key);
            }
        }
    }
    
    public class ColumnRow : IColumnRow
    {
        IReadOnlyRogueRow baseRow { get; }
        public ColumnRowID rowID { get { return new ColumnRowID(baseRow.rowID.ToInt()); } }
        public ColumnRow(IReadOnlyRogueRow baseRow)
        {
            this.baseRow = baseRow;
        }        
        public string ColumnType()
        {
            return baseRow.GetValueByColumn(new ColumnRowID(SystemIDs.Columns.columnTypeID));
        }
        public string DataType()
        {
            return baseRow.GetValueByColumn(-1021);
        }
        public string Ordinal()
        {
            return baseRow.GetValueByColumn(-1022);
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return baseRow.GetValueByColumn(-1023);
        }
        public string ColumnIDNameID()
        {
            return baseRow.GetValueByColumn(-1023).ToUpper();
        }
        public string OwnerIOItem()
        {
            return baseRow.GetValueByColumn(-1024);
        }
    }
    public class RogueColumnIDRow : IColumnRow
    {
        public ColumnRowID rowID { get { return SystemIDs.Columns.rogueColumnID; } }        
        public string ColumnType()
        {
            return ColumnTable.ColumnTypes.parentColumnRef.Value;
        }
        public string DataType()
        {
            return "int";
        }
        public string Ordinal()
        {
            return "1";
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return "RogueColumnID";
        }
        public string ColumnIDNameID()
        {
            return "ROGUECOLUMNID";
        }
        public string OwnerIOItem()
        {
            return SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString();
        }
    }
    public class RecordIO_OID_Row : IColumnRow
    {
        public ColumnRowID rowID { get { return SystemIDs.Columns.recordIO_OID; } }
        public string ColumnType()
        {
            return ColumnTable.ColumnTypes.parentColumnRef.Value;
        }
        public string DataType()
        {
            return "int";
        }
        public string Ordinal()
        {
            return "1";
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return "RecordIO_OID";
        }
        public string ColumnIDNameID()
        {
            return "RECORDOID_OID";
        }
        public string OwnerIOItem()
        {
            return SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString();
        }
    }
    public class Record_Col_OID_Row : IColumnRow
    {
        public ColumnRowID rowID { get { return SystemIDs.Columns.recordCol_OID; } }
        public string ColumnType()
        {
            return ColumnTable.ColumnTypes.parentColumnRef.Value;
        }
        public string DataType()
        {
            return "int";
        }
        public string Ordinal()
        {
            return "2";
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return "RecordCol_OID";
        }
        public string ColumnIDNameID()
        {
            return "RECORDCOL_OID";
        }
        public string OwnerIOItem()
        {
            return SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString();
        }
    }
    public class RefRecordRow_OID : IColumnRow
    {
        public ColumnRowID rowID { get { return SystemIDs.Columns.recordRef_OID; } }
        public string ColumnType()
        {
            return ColumnTable.ColumnTypes.parentColumnRef.Value;
        }
        public string DataType()
        {
            return "int";
        }
        public string Ordinal()
        {
            return "3";
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return "RefRecordRow_OID";
        }
        public string ColumnIDNameID()
        {
            return "REFRECORDROW_OID";
        }
        public string OwnerIOItem()
        {
            return SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString();
        }
    }
    public class RefRecordComplexCount_OID : IColumnRow
    {
        public ColumnRowID rowID { get { return SystemIDs.Columns.recordComplexIndex; } }
        public string ColumnType()
        {
            return ColumnTable.ColumnTypes.column.Value;
        }
        public string DataType()
        {
            return "int";
        }
        public string Ordinal()
        {
            return "5";
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return "RefRecordComplexIndex_OID";
        }
        public string ColumnIDNameID()
        {
            return "REFRECORDCOMPLEXINDEX";
        }
        public string OwnerIOItem()
        {
            return SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString();
        }
    }
    public class Record_Position_Index : IColumnRow
    {
        public ColumnRowID rowID { get { return SystemIDs.Columns.recordPosition; } }
        public string ColumnType()
        {
            return ColumnTable.ColumnTypes.column.Value;
        }
        public string DataType()
        {
            return "int";
        }
        public string Ordinal()
        {
            return "4";
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return "RecordPosition";
        }
        public string ColumnIDNameID()
        {
            return "RECORDPOSITION";
        }
        public string OwnerIOItem()
        {
            return SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString();
        }
    }
    public class RogueDateAddedRow : IColumnRow
    {
        public ColumnRowID rowID { get { return SystemIDs.Columns.dateAddedID; } }
        public string ColumnType()
        {
            return ColumnTable.ColumnTypes.column.Value;
        }
        public string DataType()
        {
            return "date";
        }
        public string Ordinal()
        {
            return "1";
        }
        public string ColumnIDName()
        {
            //string blah = columnsByID[7376].ColumnIDNameID();
            return "RogueDateAdded";
        }
        public string ColumnIDNameID()
        {
            return "ROGUEDATEADDED";
        }
        public string OwnerIOItem()
        {
            return SystemIDs.IOTableRecords.Bundles.rootBundleID.ToString();
        }
    }
    public interface IColumnRow
    {
        public ColumnRowID rowID { get; }
        public string ColumnType();
        public string DataType();
        public string Ordinal();
        public string ColumnIDName();
        public string ColumnIDNameID();
        public string OwnerIOItem();
    }
}
