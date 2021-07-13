using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.id.rogueID.ioRecord;

namespace rogue_core.rogueCore.id
{
    public static class SystemIDs
    {
        public static class ObjectTypes
        {
            //public const int bundle = -302;
            //public const int database = -303;
            //public const int table = -304;
            public static readonly IDPair bundle = new IDPair(-302, "Bundle");
            public static readonly IDPair database = new IDPair(-303, "Database");
            public static readonly IDPair table = new IDPair(-304, "Table");
            //            public static readonly FilledVarcharRow database = new FilledVarcharRow(new VarcharRowID(-303), "Database");
            //            public static readonly FilledVarcharRow table = new FilledVarcharRow(new VarcharRowID(-304), "Table");
            //public static readonly FilledVarcharRow bundle = new FilledVarcharRow(new VarcharRowID(-302), "Bundle");
            //public static readonly FilledVarcharRow database = new FilledVarcharRow(new VarcharRowID(-303), "Database");
            //public static readonly FilledVarcharRow table = new FilledVarcharRow(new VarcharRowID(-304), "Table");
        }
        //public static class ColumnTypes
        //{
        //    //public const int bundle = -302;
        //    //public const int database = -303;
        //    //public const int table = -304;
        //    public static readonly IDPair bundle = new IDPair(-302, "Bundle");
        //    public static readonly IDPair database = new IDPair(-303, "Database");
        //    public static readonly IDPair table = new IDPair(-304, "Table");
        //    //            public static readonly FilledVarcharRow database = new FilledVarcharRow(new VarcharRowID(-303), "Database");
        //    //            public static readonly FilledVarcharRow table = new FilledVarcharRow(new VarcharRowID(-304), "Table");
        //    //public static readonly FilledVarcharRow bundle = new FilledVarcharRow(new VarcharRowID(-302), "Bundle");
        //    //public static readonly FilledVarcharRow database = new FilledVarcharRow(new VarcharRowID(-303), "Database");
        //    //public static readonly FilledVarcharRow table = new FilledVarcharRow(new VarcharRowID(-304), "Table");
        //}
        public static class IOTableRecords{
            public static class Tables{
                public static class DataTypeTables{
                    public const int rogueIDTableID = -1000;
                    public const int dateTableID = -1001;
                    public const int numberTableID = -1002;
                    public const int varcharTableID = -1003;
                }

                public static class SystemTables{
                    public const int ioRecordTableID = -1010;
                    public const int columnTableID = -1011;
                }
                public static class StockTables{
                    public const int uiControlsTableID = -1026;
                    public const int uiControlsAttributesTableID = -2033;
                }
            }
            
            public static class Bundles{
                public const int rootBundleID = -1004;
                public const int systemBundleID = -1005;
                public const int stockBundleID = -1006;
                public const int dataBundle = -1007;
            }
            public static class Databases{
                public const int dataTypeDatabaseID = -1008;
                public const int metaDatabase = -1009;
                public const int uiDatabase = -1029;
            }
        }
        public static class Columns{
            public const int rogueColumnID = -1012;
            public const int dateAddedID = -1013;
            //public const int metaDescriptionID = -1014;
            //public const int metaForeignID = -1015;
            //public const int ownerIOItemID = -1016;
            //public const int metaRecordTypeID = -1017;
            //public const int metaFolderPathID = -1018;
            //public const int metaTablePathID = -1019;
            public const int metaDescriptionID = -1014;
            public const int metaForeignID = -1015;
            public const int ownerIOItemID = -1016;
            public const int metaRecordTypeID = -1017;
            public const int metaFolderPathID = -1018;
            public const int metaTablePathID = -1019;
            public const int columnTypeID = -1020;
            public const int dataTypeID = -1021;
            public const int columnOrdinalID = -1022;
            public const int columnNameID = -1023;
            public const int ownerColumnItemID = -1024;
            public const int literalValueID = -1025;
            public const int parentTableID = 6297;
            public const int isEnumerated = 7379;
            public const int recordIO_OID = -5111;
            public const int recordRef_OID = -5112;
            public const int recordCol_OID = -5113;
            public const int recordPosition = -5114;
            public const int recordComplexIndex = -5115;
            public class UIControls{
                public const int uiControlType = -1027;
                public const int uiControlName = -1028;
                
            }
        }
        // public static IORecordID SystemRootBundle = -512;
        // public static IORecordID DataRootBundle = -504;
        // public static IORecordID MetaDatabase = -506;
        // //public static IORecordID MetaTable = -4;
        // public static IORecordID StockBundle = -503;
        //public static IORecordID DataTypeDatabase = -5;
        // public static IORecordID RootBundle = -517;
        // public static IORecordID DateDataTypeID = -7;
        // public static IORecordID NumberDataTypeID = -8;
        // public static IORecordID VarcharDataTypeID = -9;
        // public static ColumnRowID RogueColumnID = -10;
        // public static ColumnRowID RogueDateAdded = -11;
        // public static ColumnRowID ColumnColumnType = -12;
        // public static ColumnRowID ColumnOrdinal = -13;
        // public static ColumnRowID ColumnDataType = -14;
        // public static ColumnRowID ColumnNameID = -15;
        // public static ColumnRowID ColumnOwnerIoItem = -16;
        // public static ColumnRowID EncodingLiteralValue = -17;
        // public static ColumnRowID MetaDescription = -18;
        // public static ColumnRowID MetaForeignID = -19;
        // public static ColumnRowID MetaIOItemID = -20;
        // public static ColumnRowID MetaOwnerIOItemID = -21;
        // public static class DecodedRows
        // {
        //     public static class ColumnTypes
        //     {
        //         public static DecodedRowID Column = -22;
        //         public static DecodedRowID ParentTableRef = -23;
        //         public static DecodedRowID ChildTableRef = -24;
        //     }
        //     public static class MetaRecordTypes
        //     {
        //         public static DecodedRowID bundle = -26;
        //         public static DecodedRowID database = -27;
        //         public static DecodedRowID table = -28;
        //     }

        // }
        // public static class DataTypeIDs
        // {
        //     public const int rogueIDTableID = -800;
            
        // }
        //public static IORecordID ColumnTable = -25;
    }
    public struct IDPair
    {
        //public long id { get; }
        public string name { get;}
        public IDPair(long id, string name)
        {
            //this.id = id;
            this.name = name;
        }
    }
}