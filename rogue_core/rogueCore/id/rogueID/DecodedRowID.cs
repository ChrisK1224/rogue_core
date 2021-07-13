using rogue_core.rogueCore.binary;
using System;

namespace rogue_core.rogueCore.id.rogueID
{
    public class DecodedRowID : RowID
    {
        public Boolean IsLiteralNumber = false;
        public DecodedRowID(int id) : base(id) { }
        public static implicit operator int(DecodedRowID id)
        {
            return id._intVal;
        }
        public static implicit operator DecodedRowID(int id)
        {
            return new DecodedRowID(id);
        }


        // public override bool Equals(object obj)
        // {
        //     if (obj == null || GetType() != obj.GetType())
        //         return false;

        //     var other = (RowID)obj;
        //     return this._intVal == other._intVal;
        // }
    }
    public static class dataTypeConversions
    {
        //public static string ToDecodedString(this DecodedRowID thsID)
        //{
        //    var row = FullTables.varcharTable.rows[thsID];
        //    var pair = row.IGetBasePair(DecodedCols.LiteralValue.ID);
        //    //*Recent changed pair.WriteValue();
        //    return pair.DisplayValue();
        //}
        /// <summary>
        /// WARNING this is bad code potential forbugs. Coming from syntax only but doesn't give tableID towrite reference for all datatypes that come in. Need to fix to be able to index by complex Values since complex stoe=red per table
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static DecodedRowID ToDecodedRowID(this string strValue)
        {
            return new DecodedRowID((int)BinaryDataPair.GetValueOID(strValue).valueID);
            //int i = 0;
            //bool result = int.TryParse(strValue, out i); //i now = 108  
            //if (result)
            //{
            //    DecodedRowID newID = new DecodedRowID(i);
            //    newID.IsLiteralNumber = true;
            //    return newID;
            //}
            //else
            //{
            //    DecodedRowID foundRow = null;
            //    //* TODO need better way to check the encoded version of string
            //    string checkVal = strValue.Replace(",", "@RCOMMA").Replace(":", "@ROGUECOLON").Replace(";", "@ROGUESEMICOLON").Replace(Environment.NewLine, "@RNEWLINE").Replace("|", "@RBAR").Replace("\n", "@RNEWLINE");
            //    if (FullTables.varcharTable.valueLookup.TryGetValue(checkVal, out foundRow))
            //    {
            //        return foundRow;
            //    }
            //    else
            //    {
            //        return FullTables.varcharTable.WriteNewRow(strValue).ID;
            //        //return new VarcharTable().WriteNewRow(strValue).ID;
            //    }
            //}
        }
        //*NNED TO FIX
        //public static DecodedRowID ToDecodedRowID(this string strValue, ColumnRowID colID)
        //{
        //    var col = BinaryDataTable.columnTable.columnsByID[colID];
        //    //var complexTbl = 
        //        //BinaryDataTable.ioRecordTable.idRows[long.Parse(col.OwnerIOItem())].com;
        //    return new DecodedRowID((int)BinaryDataPair.GetValueOID(strValue, ioRecords[9]).valueID);

        //}
        //public static CustomDecoded*ID ToBinaryDecodedRowID(this string strValue)
        //{
        //    int i = 0;
        //    bool result = int.TryParse(strValue, out i); //i now = 108  
        //    if (result)
        //    {
        //        DecodedRowID newID = new DecodedRowID(i);
        //        newID.IsLiteralNumber = true;
        //        return newID;
        //    }
        //    else
        //    {
        //        DecodedRowID foundRow = null;
        //        //* TODO need better way to check the encoded version of string
        //        string checkVal = strValue.Replace(",", "@RCOMMA").Replace(":", "@ROGUECOLON").Replace(";", "@ROGUESEMICOLON").Replace(Environment.NewLine, "@RNEWLINE").Replace("|", "@RBAR").Replace("\n", "@RNEWLINE");
        //        if (FullTables.varcharTable.valueLookup.TryGetValue(checkVal, out foundRow))
        //        {
        //            return foundRow;
        //        }
        //        else
        //        {
        //            return FullTables.varcharTable.WriteNewRow(strValue).ID;
        //            //return new VarcharTable().WriteNewRow(strValue).ID;
        //        }
        //    }
        //}
        public static int ToInt(this string strValue)
        {
            return int.Parse(strValue);
        }
        public static DecodedRowID ToDecodedRowID(this int intValue)
        {
            DecodedRowID newID = new DecodedRowID(intValue);
            newID.IsLiteralNumber = true;
            return newID;
            //DecodedRowID foundRow = null;
            //if (FullTables.varcharTable.valueLookup.TryGetValue(intValue.ToString(), out foundRow))
            //{
            //    return foundRow;
            //}
            //else
            //{
            //    return FullTables.varcharTable.WriteNewRow(intValue.ToString()).ID;
            //}
        }
        //public static DecodedRowID ToDecodedRowID(this DateTime dateValue)
        //{
        //    DecodedRowID foundRow = null;
        //    if (FullTables.varcharTable.valueLookup.TryGetValue(dateValue.ToString(), out foundRow))
        //    {
        //        return foundRow;
        //    }
        //    else
        //    {
        //        return FullTables.varcharTable.WriteNewRow(dateValue.ToString()).ID;
        //        //return new VarcharTable().WriteNewRow(dateValue.ToString()).ID;
        //    }
        //}
        public static DecodedRowID ToDecodedRowID(this RowID rowID)
        {
            DecodedRowID newID = new DecodedRowID(rowID.ToInt());
            newID.IsLiteralNumber = true;
            return newID;

        }
        //public static String ToDisplayString(this DecodedRowID encodedRowID)
        //{
        //    //if(int.TryParse(encodedRow))
        //}
    }

}