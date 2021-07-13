using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Linq;
using static rogue_core.rogueCore.binary.prefilled.ColumnTable;

namespace rogueCore.hqlSyntaxV3.segments.table
{
    public class TableGroupInfo
    {
        public const String splitKey = "FORMAT";
        //internal List<MultiRogueRow> Transformer(List<MultiRogueRow> rows, Dictionary<String, ITableStatement> joinTables, Dictionary<string, LevelStatement> levelStatements)
        //{
        //    switch (formatType)
        //    {
        //        //case FormatTypes.heirarchytable:
        //        //    return HierarchyTableTransform(rows, joinTables, levelStatements);
        //        case FormatTypes.standard:
        //            return rows;
        //        default:
        //            return rows;
        //    }
        //}
        protected FormatTypes formatType { get; set; }
        public String groupRefName { get; set; }
        public TableGroupInfo(String txt)
        {
            //this.metaData = metaData;
            //*If group format and not not specificed. usually if there is only one group
            if (txt == "")
            {
                formatType = FormatTypes.standard;
                groupRefName = "roguedefaultgroup";
            }
            else
            {
                formatType = FormatTypeByName(txt.BeforeFirstSpace());
                groupRefName = txt.AfterLastSpace();
            }
        }
        public string origStatement()
        {
            return TableGroupInfo.splitKey + " " +  formatType + " AS " + groupRefName;
        }
        void LoopPrintHierachy(IMultiRogueRow topRow, int currLvl, bool printBase = false)
        {
            topRow.PrintRow(printBase);
            currLvl++;
            foreach (var childRow in topRow.childRows)
            {
                LoopPrintHierachy(childRow, currLvl);
            }
        }
        protected enum FormatTypes
        {
            standard, heirarchytable
        }
        protected FormatTypes FormatTypeByName(String evalName)
        {
            switch (evalName.ToLower())
            {
                case "hierarchytable":
                    return FormatTypes.heirarchytable;
                case "standard":
                    return FormatTypes.standard;
                default:
                    return FormatTypes.standard;
            }
        }
    }
    class ValueRogueRow : IRogueRow
    {
        String val;
        Dictionary<ColumnRowID, string> vals;
        public static ValueRogueRow GetFakeRogueRow(String val) { return new ValueRogueRow(val); }
        public static ValueRogueRow GetFakeRogueRow(Dictionary<ColumnRowID, string> vals) { return new ValueRogueRow(vals); }
        ValueRogueRow(String val) { vals = new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), val } }; }
        ValueRogueRow(Dictionary<ColumnRowID, string> vals)
        {
            this.vals = vals;
        }
        public RowID rowID {get{return new UnKnownID(vals[-1012]);}}
        public byte[] WriteBytes()
        {
            throw new Exception("NEED TO UPDATE THIS TO GET WRITE BYteS MAYBEE??");
        }
        Dictionary<long, BinaryDataPair> IRogueRow.pairs => throw new NotImplementedException();

        //public IRoguePair IGetBasePair(ColumnRowID colRowID)
        //{
        //    return new ValueRoguePair(colRowID, vals[colRowID]);
        //}

        //public IEnumerable<IRoguePair> pairs()
        //{
        //    throw new NotImplementedException();
        //}

        //public IRoguePair ITryGetValue(ColumnRowID colRowID)
        //{
        //    if (vals.ContainsKey(colRowID))
        //    {
        //        return new ValueRoguePair(vals[colRowID]);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public IRoguePair NewWritePair(IORecordID ownerTblID, ColumnTypes colType, string columnName, RowID value)
        //{
        //    throw new NotImplementedException();
        //}
        //public IRoguePair NewWritePair(IORecordID ownerTblID, string columnName, RowID value, IORecordID parentRecord)
        //{
        //    throw new NotImplementedException();
        //}
        //public IRoguePair NewWritePair(ColumnRowID colID, string colValue)
        //{
        //    throw new NotImplementedException();
        //}
        //**This hsould probably be changed to direct return value and assume exists but not sure if used under that assumption
        public string GetValueByColumn(ColumnRowID thsCol)
        {
            string ret = "";
            vals.TryGetValue(thsCol, out ret);
            return ret;
        }
        public string ITryGetValueByColumn(ColumnRowID colRowID)
        {
            string ret = "";
            vals.TryGetValue(colRowID, out ret);
            return ret;
        }
        public void SetValue(ColumnRowID col, string value)
        {
            throw new NotImplementedException();
        }

        //public IRoguePair NewWritePair(IORecordID ownerTblID, string colNM, string colValue)
        //{
        //    throw new NotImplementedException();
        //}

        IGenericValue IRogueRow.ITryGetValue(ColumnRowID colRowID)
        {
            throw new NotImplementedException();
        }

        IGenericValue IRogueRow.IGetBasePair(ColumnRowID colRowID)
        {
            throw new NotImplementedException();
        }

        public IGenericValue NewWritePair(IORecordID ownerTblID, ColumnTypes colType, string columnName, RowID value, byte dataTypeID)
        {
            throw new NotImplementedException();
        }

        public IGenericValue NewWritePair(IORecordID ownerTblID, string columnName, RowID value, byte dataTypeID, IORecordID parentTableID)
        {
            throw new NotImplementedException();
        }

        IGenericValue IRogueRow.NewWritePair(ColumnRowID colID, string colValue)
        {
            throw new NotImplementedException();
        }

        IGenericValue IRogueRow.NewWritePair(IORecordID ownerTblID, string colNM, string colValue)
        {
            throw new NotImplementedException();
        }

        public void PrintRow()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IValueReference> GetDataTypeValList()
        {
            throw new NotImplementedException();
        }

        public IGenericValue NewWritePair(IORecordID ownerTblID, ColumnTypes colType, string columnName, string value)
        {
            throw new NotImplementedException();
        }

        public IGenericValue NewWritePair(IORecordID ownerTblID, string columnName, string value, IORecordID parentTableID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<ColumnRowID, string>> GetPairs()
        {
            throw new NotImplementedException();
        }
    }
    class ValueRoguePair : IGenericValue
    {
        string val;
        public ColumnRowID KeyColumnID { get; set; }

        public byte dataTypeID { get; }

        public long valueID { get; }

        public ValueRoguePair(String val) { this.val = val; this.valueID = val.ToDecodedRowID(); this.dataTypeID = BinaryDataPair.GetDataType(val); }
        public ValueRoguePair(ColumnRowID id, String val) { this.val = val; KeyColumnID = id; val.ToDecodedRowID(); this.dataTypeID = BinaryDataPair.GetDataType(val); }
        public string WriteValue()
        {
            return val.ToDecodedRowID().ToString();
        }

        public string StringValue(ComplexWordTable complexTbl)
        {
            return val;
        }
    }
}
