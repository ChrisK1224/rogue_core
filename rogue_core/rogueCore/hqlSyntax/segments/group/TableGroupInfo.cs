using FilesAndFolders;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.pair;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.table.encoded;
using rogueCore.hqlSyntax.segments.join;
using rogueCore.hqlSyntax.segments.select;
using System;
using System.Collections.Generic;
using System.Linq;
using static rogueCore.hqlSyntax.HQLQueryTwo;

namespace rogueCore.hqlSyntax.segments.table
{
    public class TableGroupInfo
    {
        public const String splitKey = "FORMAT";
        public List<FilledSelectRow> Transformer(List<FilledSelectRow> rows, Dictionary<String, FilledTable> joinTables, Dictionary<string, TableStatement> tableStatements)
        {
            switch (formatType)
            {
                case FormatTypes.heirarchytable:
                    return HierarchyTableTransform(rows, joinTables, tableStatements);
                case FormatTypes.standard:
                    return rows;
                default:
                    return rows;
            }
        }
        protected FormatTypes formatType { get; set; }
        public String groupRefName { get; set; }
        HQLMetaData metaData;
        public TableGroupInfo(String txt, HQLMetaData metaData)
        {
            this.metaData = metaData;
            //*If group format and not not specificed. usually if there is only one group
            if (txt == "")
            {
                formatType = FormatTypes.standard;
                groupRefName = "roguedefaultgroup";
            }
            else
            {
                //Enum.TryParse(txt.BeforeFirstSpace().ToLower(), out FormatTypes thsType);
                formatType = FormatTypeByName(txt.BeforeFirstSpace());
                groupRefName = txt.AfterLastSpace();
            }
        }
        public string origStatement()
        {
            return TableGroupInfo.splitKey + " " +  formatType + " AS " + groupRefName;
        }

        //List<FilledSelectRow> KeyValuePairsToRows(List<FilledSelectRow> allRows, Dictionary<String, FilledTable> joinTables)
        //{
        //    foreach(FilledSelectRow row in allRows)
        //    {

        //    }
        //}
        List<FilledSelectRow> HierarchyTableTransform(List<FilledSelectRow> allRows, Dictionary<String, FilledTable> joinTables, Dictionary<string, TableStatement> grpTables)
        {
            List<FilledSelectRow> tblList = new List<FilledSelectRow>();
            String topJoinNm = groupRefName + "_TableTop";
            FilledTable parentTbl;
            joinTables.TryGetValue(topJoinNm, out parentTbl);
            FilledSelectRow parent = parentTbl.rows.First();

            string headerGroup = groupRefName + "_HeaderGroup";
            FilledTable headerGroupTbl = new FilledTable(metaData, headerGroup, JoinClause.JoinAll(topJoinNm, metaData), parentTbl);
            joinTables.Add(headerGroup, headerGroupTbl);
            FilledSelectRow headerGroupTblRow = new FilledSelectRow(headerGroup);
            headerGroupTbl.AddRow(headerGroupTblRow);
            parent.childRows.Add(headerGroupTblRow);
            headerGroupTblRow.SetParent(parent);

            string headerName = groupRefName + "_HeaderRow";
            FilledTable headerTbl = new FilledTable(metaData, headerName, JoinClause.JoinAll(topJoinNm, metaData), headerGroupTbl);
            joinTables.Add(headerName, headerTbl);
            FilledSelectRow headerTblRow = new FilledSelectRow(headerName);
            headerTbl.AddRow(headerTblRow);
            headerGroupTblRow.childRows.Add(headerTblRow);
            headerTblRow.SetParent(headerGroupTblRow);

            string headerColName = groupRefName + "_HeaderCol";
            FilledTable headerCols = new FilledTable(metaData, headerColName, JoinClause.JoinAll(headerName, metaData), headerTbl);
            Dictionary<string, bool> colEnumInfo = new Dictionary<string, bool>();
            Dictionary<bool, List<string>> colStats = new Dictionary<bool, List<string>>();
            colStats.Add(true, new List<string>());
            colStats.Add(false, new List<string>());
            
            var colEnums = new Dictionary<string, List<KeyValuePair<SelectColumn, KeyValuePair<string,string>>>>();
            List<SelectColumn> allCols = new List<SelectColumn>();
            grpTables.Values.Where(x => x.selectRow != null).ToList().ForEach(tbl => allCols.AddRange(tbl.selectRow.SelectColumns.Values.ToList()));
            //*Set header row just use first row since assumed all have same columns
            if (allRows.Count > 0)
            {
                foreach (SelectColumn thsCol in allCols)
                {
                    FilledSelectRow headerCol = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(thsCol.columnName));
                    headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(thsCol, thsCol.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                    headerCols.AddRow(headerCol);
                    headerTblRow.childRows.Add(headerCol);
                    headerCol.SetParent(headerTblRow);
                    var enumVals = new List<KeyValuePair<SelectColumn, KeyValuePair<string,string>>>();
                    //* Notsure if needed to bring back to add blank value in case value not set. Might already be taken care of
                    //enumVals.Add(new KeyValuePair<SelectColumn, KeyValuePair<string, string>>(pair.Value.Key,new KeyValuePair<string,string>("","")));
                    colEnums.Add(thsCol.columnName.ToUpper(), enumVals);
                    Action<rowstatus, FilledSelectRow> act = (rowstatus stat, FilledSelectRow row) => Console.WriteLine("");

                    foreach (var thsColName in (new HumanHQLStatement("FROM COLUMNENUMERATIONS SELECT * WHERE COLUMN_OID = \"" + thsCol.BaseColumnID + "\"").IterateRows(act)))
                    {
                        //FilledSelectRow enumVal = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(pair.Key));
                        //headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, pair.Value.Key.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                        //headerCols.AddRow(headerCol);
                        //headerTblRow.childRows.Add(headerCol);
                        //headerCol.SetParent(headerTblRow);
                        enumVals.Add(new KeyValuePair<SelectColumn, KeyValuePair<string,string>>(thsCol, new KeyValuePair<string,string>(thsColName.Value.GetValue("ENUMERATION_VALUE"),thsColName.Value.GetValue("ENUMERATION_VALUE"))));
                    }
                    foreach (var thsColName in (new HumanHQLStatement("FROM Root.System.MetaRecords.Column AS COLUMN WHERE COLUMN.ColumnType = \"ParentTableRef\" AND RogueColumnID = \"" + thsCol.BaseColumnID + "\"  FROM IORECORDS AS COLID JOIN MERGE * = COLUMN.ROGUECOLUMNID SELECT NAME_COLUMN_OID WHERE NAME_COLUMN_OID != \"\" AND ROGUECOLUMNID = COLUMN.ParentTableID FROM[{ COLUMN.PARENTTABLEID }] AS PARENTCOLENUM JOIN MERGE * = COLID.ROGUECOLUMNID SELECT[{ COLID.NAME_COLUMN_OID}] as ENUMERATION_VALUE, PARENTCOLENUM.ROGUECOLUMNID AS ENUMERATION_ID WHERE COLUMN.ColumnType = \"ParentTableRef\" FROM IORECORDS AS COLIDBlank JOIN MERGE * = COLUMN.ROGUECOLUMNID SELECT NAME_COLUMN_OID WHERE NAME_COLUMN_OID = \"\" AND ROGUECOLUMNID = COLUMN.ParentTableID FROM[{ COLUMN.PARENTTABLEID}] AS PARENTCOLENUMID JOIN MERGE * = COLIDBlank.ROGUECOLUMNID SELECT COLIDBlank.ROGUECOLUMNID as ENUMERATION_VALUE, PARENTCOLENUMID.ROGUECOLUMNID AS ENUMERATION_ID WHERE COLUMN.ColumnType = \"ParentTableRef\"").IterateRows(act)))
                    {
                        //FilledSelectRow enumVal = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(pair.Key));
                        //headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, pair.Value.Key.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                        //headerCols.AddRow(headerCol);
                        //headerTblRow.childRows.Add(headerCol);
                        //headerCol.SetParent(headerTblRow);
                        enumVals.Add(new KeyValuePair<SelectColumn, KeyValuePair<string, string>>(thsCol, new KeyValuePair<string, string>(thsColName.Value.GetValue("ENUMERATION_ID"), thsColName.Value.GetValue("ENUMERATION_VALUE"))));
                    }
                    //*For when 
                    // foreach (var thsColName in (new HumanHQLStatement("FROM [" + pair.Value.Key.BaseColumnID.ToColumnType() + "] SELECT * WHERE COLUMN_OID = \"" + pair.Value.Key.BaseColumnID + "\"").IterateRows(act)))
                    // {
                    //     //FilledSelectRow enumVal = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(pair.Key));
                    //     //headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, pair.Value.Key.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                    //     //headerCols.AddRow(headerCol);
                    //     //headerTblRow.childRows.Add(headerCol);
                    //     //headerCol.SetParent(headerTblRow);
                    //     enumVals.Add(new KeyValuePair<SelectColumn, string>(pair.Value.Key, thsColName.Value.GetValue("ENUMERATION_VALUE")));
                    // }
                    //nonCol.Add(pair.Value.Key.BaseColumnID);
                    //colEnumInfo.Add(pair.Value.Key.columnName, pair.Value.Key.BaseColumnID.Is_Enumerated());
                    /*colStats[pair.Value.Key.BaseColumnID.Is_Enumerated()].Add(pair.Value.Key.columnName.ToUpper());*/
                    /*if (pair.Value.Key.BaseColumnID.Is_Enumerated() && !colEnums.ContainsKey(pair.Value.Key.columnName.ToUpper()))
                    {
                        colEnums.Add(pair.Value.Key.columnName.ToUpper(), new List<string>());
                    };*/

                    //colEnumChecks.Add(pair.Value.Key.)
                }
            }

            joinTables.Add(headerColName, headerCols);

            string dataGroupNm = groupRefName + "_DataGroup";
            FilledTable dataGroupTbl = new FilledTable(metaData, dataGroupNm, JoinClause.JoinAll(topJoinNm, metaData), parentTbl);
            joinTables.Add(dataGroupNm, dataGroupTbl);
            FilledSelectRow dataGroupTblRow = new FilledSelectRow(dataGroupNm);
            dataGroupTbl.AddRow(dataGroupTblRow);
            parent.childRows.Add(dataGroupTblRow);
            dataGroupTblRow.SetParent(parent);

            string dataRowTblNM = groupRefName + "_DataRow";

            string dataColTblNM = groupRefName + "_DataCol";
            string dataColDDLItemTblNM = groupRefName + "_DDLDataCol";
            string dataColDDLItemValue = groupRefName + "_DDLDataColValue";
            FilledTable dataRowTbl = new FilledTable(metaData, dataRowTblNM, JoinClause.JoinAll(topJoinNm, metaData), parentTbl);
            //dataRowTbl.level = dataGroupTbl.level +1;
            joinTables.Add(dataRowTblNM, dataRowTbl);

            string dataCellTblNM = groupRefName + "_DataCell";
            FilledTable dataCellTbl = new FilledTable(metaData, dataCellTblNM, JoinClause.JoinAll(dataRowTblNM, metaData), dataRowTbl);
            joinTables.Add(dataCellTblNM, dataCellTbl);

            FilledTable dataColTbl = new FilledTable(metaData, dataColTblNM, JoinClause.JoinAll(dataCellTblNM, metaData), dataCellTbl);
            //dataColTbl.level = dataRowTbl.level =1;
            joinTables.Add(dataColTblNM, dataColTbl);

            FilledTable dataColDDLItemTbl = new FilledTable(metaData, dataColDDLItemTblNM, JoinClause.JoinAll(dataColTblNM, metaData), dataColTbl);
            //dataColTbl.level = dataRowTbl.level =1;
            joinTables.Add(dataColDDLItemTblNM, dataColDDLItemTbl);

            FilledTable dataColDDLItemValueTbl = new FilledTable(metaData, dataColDDLItemValue, JoinClause.JoinAll(dataColDDLItemTblNM, metaData), dataColDDLItemTbl);
            joinTables.Add(dataColDDLItemValue, dataColDDLItemValueTbl);
            //Just to find selected enumeration value
            string selectedItemTblNM = groupRefName + "_chosenDDLItem";
            FilledTable selectedItemTbl = new FilledTable(metaData, selectedItemTblNM, JoinClause.JoinAll(dataColDDLItemTblNM, metaData), dataColDDLItemTbl);
            joinTables.Add(selectedItemTblNM, selectedItemTbl);

            //*Iterate each row 
            //* Needto query for column meta data and then create new fake table named DDLITem and create child table with enumeration values. Then just merge whatever into here like others
            foreach (FilledSelectRow origRow in allRows)
            {
                FilledSelectRow dataRow = new FilledSelectRow(dataRowTblNM, dataGroupTblRow, origRow.baseTableRow);
                dataRowTbl.AddRow(dataRow);
                //dataGroupTblRow.childRows.Add(dataRow);
                //dataRow.SetParent(dataGroupTblRow);
                //dataRow.tableRefRows.Add(dataGroupNm, dataGroupTblRow.baseTableRow);

                /* foreach(string thsColID in colStats[false]){
                     FilledSelectRow dataCol = new FilledSelectRow(dataColTblNM, ValueRogueRow.GetFakeRogueRow(origRow.values[thsColID].Value));
                     dataCol.values.Add("VALUE", origRow.values[thsColID]);
                     dataColTbl.AddRow(dataCol);
                     dataRow.childRows.Add(dataCol);
                     dataCol.SetParent(dataRow);

                 }
                 foreach(string thsColID in colStats[true]){
                     FilledSelectRow dataCol = new FilledSelectRow(dataColDDLTblNM, ValueRogueRow.GetFakeRogueRow(origRow.values[thsColID].Value));
                     dataCol.values.Add("VALUE", origRow.values[thsColID]);
                     dataColDDLTbl.AddRow(dataCol);
                     dataRow.childRows.Add(dataCol);
                     dataCol.SetParent(dataRow);
                 }*/
                foreach (var pair in origRow.values)
                {
                    FilledSelectRow cellRow = new FilledSelectRow(dataCellTblNM, dataRow, null);
                    dataCellTbl.AddRow(cellRow);
                    //dataRow.childRows.Add(cellRow);
                    //cellRow.SetParent(dataRow);
                    //cellRow.tableRefRows.Add(dataRowTblNM, dataRow.baseTableRow);
                    //bool isEnum = false;
                    string isEnum = "false";
                    string realVal = pair.Value.Value;
                    if (pair.Value.Key.BaseColumnID.ToColumnType().ToDecodedString().ToUpper() == "PARENTTABLEREF" || pair.Value.Key.BaseColumnID.Is_Enumerated())
                    {
                        isEnum = "true";
                        realVal = "";
                        var enums = colEnums[pair.Key];
                        string findVal = origRow.GetValue(pair.Value.Key.columnName);
                        if(findVal == "0")
                        {
                            realVal = "";
                        }
                        else if(findVal == "")
                        {
                            realVal = "";
                        }
                        else
                        {
                            var test = enums.Where(x => x.Value.Key == findVal);
                            if(test.Any())
                            {
                                realVal = test.First().Value.Value;
                            }
                            else
                            {
                                realVal = "roguenotvalid";
                            }
                        }
                    }
                    FilledSelectRow dataCol = new FilledSelectRow(dataColTblNM,cellRow, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), realVal }, { new ColumnRowID(7379), isEnum }, { new ColumnRowID(-1012), pair.Value.Key.BaseColumnID.ToString() }, { new ColumnRowID(-1020), pair.Value.Key.BaseColumnID.ToColumnType().ToDecodedString() } }));
                    dataCol.values.Add("VALUE",new KeyValuePair<SelectColumn, string>(pair.Value.Key,realVal));
                    dataColTbl.AddRow(dataCol);
                    //cellRow.childRows.Add(dataCol);
                    //dataCol.SetParent(cellRow);
                    //dataCol.tableRefRows.Add(dataCellTblNM, cellRow.baseTableRow);
                    if(colEnums[pair.Key].Count > 0){
                        FilledSelectRow enumVal = new FilledSelectRow(dataColDDLItemTblNM, dataCol, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), ""} }));
                        enumVal.values.Add("VALUE", new KeyValuePair<SelectColumn,string>(colEnums[pair.Key][0].Key, ""));
                        dataColDDLItemTbl.AddRow(enumVal);

                        FilledSelectRow enumValID = new FilledSelectRow(dataColDDLItemValue, enumVal, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), "0" } }));
                        enumValID.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(colEnums[pair.Key][0].Key, ""));
                        dataColDDLItemValueTbl.AddRow(enumValID);
                    }
                    foreach (KeyValuePair<SelectColumn, KeyValuePair<string,string>> enumPair in colEnums[pair.Key])
                    {
                        FilledSelectRow enumVal = new FilledSelectRow(dataColDDLItemTblNM, dataCol, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), enumPair.Value.Value } }));
                        enumVal.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Key));
                        //enumVal.values.Add("DISPLAYVALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Key));
                        dataColDDLItemTbl.AddRow(enumVal);

                        FilledSelectRow enumValID = new FilledSelectRow(dataColDDLItemValue, enumVal, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), enumPair.Value.Key } }));
                        enumValID.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(colEnums[pair.Key][0].Key, ""));
                        dataColDDLItemValueTbl.AddRow(enumValID);
                        //dataCol.childRows.Add(enumVal);
                        //enumVal.SetParent(dataCol);
                        //enumVal.tableRefRows.Add(dataColTblNM, dataCol.baseTableRow);

                        if (origRow.GetValue(enumPair.Key.columnName) == enumPair.Value.Key)
                        {
                            FilledSelectRow selectedVal = new FilledSelectRow(selectedItemTblNM,enumVal, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), enumPair.Value.Value } }));
                            selectedVal.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Value));
                            //selectedVal.values.Add("DISPLAYVALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Key));
                            selectedItemTbl.AddRow(selectedVal);
                            //enumVal.childRows.Add(selectedVal);
                            //selectedVal.SetParent(enumVal);
                        }
                    }
                    //FilledSelectRow rw  = new FilledSelectRow();


                    /*switch (pair.Value.Key.BaseColumnID.Is_Enumerated())
                    {
                         
                        case true:
                            FilledSelectRow dataCol = new FilledSelectRow(dataColDDLTblNM, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>(){{new ColumnRowID(0),pair.Value.Value}, {new ColumnRowID(7379), pair.Value.Key.BaseColumnID.Is_Enumerated().ToString()}}));
                            dataCol.values.Add("VALUE", pair.Value);
                            dataColDDLTbl.AddRow(dataCol);
                            dataRow.childRows.Add(dataCol);
                            dataCol.SetParent(dataRow);
                            break;
                        case false:
                            FilledSelectRow dataColDDL = new FilledSelectRow(dataColTblNM, ValueRogueRow.GetFakeRogueRow(pair.Value.Value));
                            dataColDDL.values.Add("VALUE", pair.Value);
                            dataColTbl.AddRow(dataColDDL);
                            dataRow.childRows.Add(dataColDDL);
                            dataColDDL.SetParent(dataRow);
                            break;
                    }*/
                    /*FilledSelectRow dataCol = new FilledSelectRow(dataColTblNM, ValueRogueRow.GetFakeRogueRow(pair.Value.Value));
                    dataCol.values.Add("VALUE", pair.Value);
                    dataColTbl.AddRow(dataCol);
                    dataRow.childRows.Add(dataCol);
                    dataCol.SetParent(dataRow);*/
                }
            }
            tblList.AddRange(parentTbl.rows);
            //LoopPrintHierachy(parent, 0, true);
            //parentTbl.
            return tblList;
        }
        List<FilledSelectRow> HierarchyTableTransformOLD(List<FilledSelectRow> allRows, Dictionary<String, FilledTable> joinTables)
        {
            List<FilledSelectRow> tblList = new List<FilledSelectRow>();
            String topJoinNm = groupRefName + "_TableTop";
            FilledTable parentTbl;
            joinTables.TryGetValue(topJoinNm, out parentTbl);
            FilledSelectRow parent = parentTbl.rows.First();

            string headerGroup = groupRefName + "_HeaderGroup";
            FilledTable headerGroupTbl = new FilledTable(metaData, headerGroup, JoinClause.JoinAll(topJoinNm, metaData), parentTbl);
            joinTables.Add(headerGroup, headerGroupTbl);
            FilledSelectRow headerGroupTblRow = new FilledSelectRow(headerGroup);
            headerGroupTbl.AddRow(headerGroupTblRow);
            parent.childRows.Add(headerGroupTblRow);
            headerGroupTblRow.SetParent(parent);

            string headerName = groupRefName + "_HeaderRow";
            FilledTable headerTbl = new FilledTable(metaData, headerName, JoinClause.JoinAll(topJoinNm, metaData), headerGroupTbl);
            joinTables.Add(headerName, headerTbl);
            FilledSelectRow headerTblRow = new FilledSelectRow(headerName);
            headerTbl.AddRow(headerTblRow);
            headerGroupTblRow.childRows.Add(headerTblRow);
            headerTblRow.SetParent(headerGroupTblRow);

            string headerColName = groupRefName + "_HeaderCol";
            FilledTable headerCols = new FilledTable(metaData, headerColName, JoinClause.JoinAll(headerName, metaData), headerTbl);
            Dictionary<string, bool> colEnumInfo = new Dictionary<string, bool>();
            Dictionary<bool, List<string>> colStats = new Dictionary<bool, List<string>>();
            colStats.Add(true, new List<string>());
            colStats.Add(false, new List<string>());
            //List<ColumnRowID> enumCols = new List<ColumnRowID>();
            //Dictionary<bool, List<ColumnRowID>> enumCols = new Dictionary<bool, List<ColumnRowID>>();
            var colEnums = new Dictionary<string, List<KeyValuePair<SelectColumn, KeyValuePair<string, string>>>>();

            //*Set header row just use first row since assumed all have same columns
            if (allRows.Count > 0)
            {
                foreach (var pair in allRows[0].values)
                {
                    FilledSelectRow headerCol = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(pair.Key));
                    headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, pair.Value.Key.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                    headerCols.AddRow(headerCol);
                    headerTblRow.childRows.Add(headerCol);
                    headerCol.SetParent(headerTblRow);
                    var enumVals = new List<KeyValuePair<SelectColumn, KeyValuePair<string, string>>>();
                    //* Notsure if needed to bring back to add blank value in case value not set. Might already be taken care of
                    //enumVals.Add(new KeyValuePair<SelectColumn, KeyValuePair<string, string>>(pair.Value.Key,new KeyValuePair<string,string>("","")));
                    colEnums.Add(pair.Value.Key.columnName.ToUpper(), enumVals);
                    Action<rowstatus, FilledSelectRow> act = (rowstatus stat, FilledSelectRow row) => Console.WriteLine("");

                    foreach (var thsColName in (new HumanHQLStatement("FROM COLUMNENUMERATIONS SELECT * WHERE COLUMN_OID = \"" + pair.Value.Key.BaseColumnID + "\"").IterateRows(act)))
                    {
                        //FilledSelectRow enumVal = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(pair.Key));
                        //headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, pair.Value.Key.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                        //headerCols.AddRow(headerCol);
                        //headerTblRow.childRows.Add(headerCol);
                        //headerCol.SetParent(headerTblRow);
                        enumVals.Add(new KeyValuePair<SelectColumn, KeyValuePair<string, string>>(pair.Value.Key, new KeyValuePair<string, string>(thsColName.Value.GetValue("ENUMERATION_VALUE"), thsColName.Value.GetValue("ENUMERATION_VALUE"))));
                    }
                    foreach (var thsColName in (new HumanHQLStatement("FROM Root.System.MetaRecords.Column AS COLUMN WHERE COLUMN.ColumnType = \"ParentTableRef\" AND RogueColumnID = \"" + pair.Value.Key.BaseColumnID + "\"  FROM IORECORDS AS COLID JOIN MERGE * = COLUMN.ROGUECOLUMNID SELECT NAME_COLUMN_OID WHERE NAME_COLUMN_OID != \"\" AND ROGUECOLUMNID = COLUMN.ParentTableID FROM[{ COLUMN.PARENTTABLEID }] AS PARENTCOLENUM JOIN MERGE * = COLID.ROGUECOLUMNID SELECT[{ COLID.NAME_COLUMN_OID}] as ENUMERATION_VALUE, PARENTCOLENUM.ROGUECOLUMNID AS ENUMERATION_ID WHERE COLUMN.ColumnType = \"ParentTableRef\" FROM IORECORDS AS COLIDBlank JOIN MERGE * = COLUMN.ROGUECOLUMNID SELECT NAME_COLUMN_OID WHERE NAME_COLUMN_OID = \"\" AND ROGUECOLUMNID = COLUMN.ParentTableID FROM[{ COLUMN.PARENTTABLEID}] AS PARENTCOLENUMID JOIN MERGE * = COLIDBlank.ROGUECOLUMNID SELECT COLIDBlank.ROGUECOLUMNID as ENUMERATION_VALUE, PARENTCOLENUMID.ROGUECOLUMNID AS ENUMERATION_ID WHERE COLUMN.ColumnType = \"ParentTableRef\"").IterateRows(act)))
                    {
                        //FilledSelectRow enumVal = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(pair.Key));
                        //headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, pair.Value.Key.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                        //headerCols.AddRow(headerCol);
                        //headerTblRow.childRows.Add(headerCol);
                        //headerCol.SetParent(headerTblRow);
                        enumVals.Add(new KeyValuePair<SelectColumn, KeyValuePair<string, string>>(pair.Value.Key, new KeyValuePair<string, string>(thsColName.Value.GetValue("ENUMERATION_ID"), thsColName.Value.GetValue("ENUMERATION_VALUE"))));
                    }
                    //*For when 
                    // foreach (var thsColName in (new HumanHQLStatement("FROM [" + pair.Value.Key.BaseColumnID.ToColumnType() + "] SELECT * WHERE COLUMN_OID = \"" + pair.Value.Key.BaseColumnID + "\"").IterateRows(act)))
                    // {
                    //     //FilledSelectRow enumVal = new FilledSelectRow(headerColName, ValueRogueRow.GetFakeRogueRow(pair.Key));
                    //     //headerCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, pair.Value.Key.columnName));//new KeyValuePair<select.SelectColumn, string>(pair.Key, pair.Key.columnName));
                    //     //headerCols.AddRow(headerCol);
                    //     //headerTblRow.childRows.Add(headerCol);
                    //     //headerCol.SetParent(headerTblRow);
                    //     enumVals.Add(new KeyValuePair<SelectColumn, string>(pair.Value.Key, thsColName.Value.GetValue("ENUMERATION_VALUE")));
                    // }
                    //nonCol.Add(pair.Value.Key.BaseColumnID);
                    //colEnumInfo.Add(pair.Value.Key.columnName, pair.Value.Key.BaseColumnID.Is_Enumerated());
                    /*colStats[pair.Value.Key.BaseColumnID.Is_Enumerated()].Add(pair.Value.Key.columnName.ToUpper());*/
                    /*if (pair.Value.Key.BaseColumnID.Is_Enumerated() && !colEnums.ContainsKey(pair.Value.Key.columnName.ToUpper()))
                    {
                        colEnums.Add(pair.Value.Key.columnName.ToUpper(), new List<string>());
                    };*/

                    //colEnumChecks.Add(pair.Value.Key.)
                }
            }

            joinTables.Add(headerColName, headerCols);

            string dataGroupNm = groupRefName + "_DataGroup";
            FilledTable dataGroupTbl = new FilledTable(metaData, dataGroupNm, JoinClause.JoinAll(topJoinNm, metaData), parentTbl);
            joinTables.Add(dataGroupNm, dataGroupTbl);
            FilledSelectRow dataGroupTblRow = new FilledSelectRow(dataGroupNm);
            dataGroupTbl.AddRow(dataGroupTblRow);
            parent.childRows.Add(dataGroupTblRow);
            dataGroupTblRow.SetParent(parent);

            string dataRowTblNM = groupRefName + "_DataRow";

            string dataColTblNM = groupRefName + "_DataCol";
            string dataColDDLItemTblNM = groupRefName + "_DDLDataCol";
            string dataColDDLItemValue = groupRefName + "_DDLDataColValue";
            FilledTable dataRowTbl = new FilledTable(metaData, dataRowTblNM, JoinClause.JoinAll(topJoinNm, metaData), parentTbl);
            //dataRowTbl.level = dataGroupTbl.level +1;
            joinTables.Add(dataRowTblNM, dataRowTbl);

            string dataCellTblNM = groupRefName + "_DataCell";
            FilledTable dataCellTbl = new FilledTable(metaData, dataCellTblNM, JoinClause.JoinAll(dataRowTblNM, metaData), dataRowTbl);
            joinTables.Add(dataCellTblNM, dataCellTbl);

            FilledTable dataColTbl = new FilledTable(metaData, dataColTblNM, JoinClause.JoinAll(dataCellTblNM, metaData), dataCellTbl);
            //dataColTbl.level = dataRowTbl.level =1;
            joinTables.Add(dataColTblNM, dataColTbl);

            FilledTable dataColDDLItemTbl = new FilledTable(metaData, dataColDDLItemTblNM, JoinClause.JoinAll(dataColTblNM, metaData), dataColTbl);
            //dataColTbl.level = dataRowTbl.level =1;
            joinTables.Add(dataColDDLItemTblNM, dataColDDLItemTbl);

            FilledTable dataColDDLItemValueTbl = new FilledTable(metaData, dataColDDLItemValue, JoinClause.JoinAll(dataColDDLItemTblNM, metaData), dataColDDLItemTbl);
            joinTables.Add(dataColDDLItemValue, dataColDDLItemValueTbl);
            //Just to find selected enumeration value
            string selectedItemTblNM = groupRefName + "_chosenDDLItem";
            FilledTable selectedItemTbl = new FilledTable(metaData, selectedItemTblNM, JoinClause.JoinAll(dataColDDLItemTblNM, metaData), dataColDDLItemTbl);
            joinTables.Add(selectedItemTblNM, selectedItemTbl);

            //*Iterate each row 
            //* Needto query for column meta data and then create new fake table named DDLITem and create child table with enumeration values. Then just merge whatever into here like others
            foreach (FilledSelectRow origRow in allRows)
            {
                FilledSelectRow dataRow = new FilledSelectRow(dataRowTblNM, dataGroupTblRow, origRow.baseTableRow);
                dataRowTbl.AddRow(dataRow);
                //dataGroupTblRow.childRows.Add(dataRow);
                //dataRow.SetParent(dataGroupTblRow);
                //dataRow.tableRefRows.Add(dataGroupNm, dataGroupTblRow.baseTableRow);

                /* foreach(string thsColID in colStats[false]){
                     FilledSelectRow dataCol = new FilledSelectRow(dataColTblNM, ValueRogueRow.GetFakeRogueRow(origRow.values[thsColID].Value));
                     dataCol.values.Add("VALUE", origRow.values[thsColID]);
                     dataColTbl.AddRow(dataCol);
                     dataRow.childRows.Add(dataCol);
                     dataCol.SetParent(dataRow);

                 }
                 foreach(string thsColID in colStats[true]){
                     FilledSelectRow dataCol = new FilledSelectRow(dataColDDLTblNM, ValueRogueRow.GetFakeRogueRow(origRow.values[thsColID].Value));
                     dataCol.values.Add("VALUE", origRow.values[thsColID]);
                     dataColDDLTbl.AddRow(dataCol);
                     dataRow.childRows.Add(dataCol);
                     dataCol.SetParent(dataRow);
                 }*/
                foreach (var pair in origRow.values)
                {
                    FilledSelectRow cellRow = new FilledSelectRow(dataCellTblNM, dataRow, null);
                    dataCellTbl.AddRow(cellRow);
                    //dataRow.childRows.Add(cellRow);
                    //cellRow.SetParent(dataRow);
                    //cellRow.tableRefRows.Add(dataRowTblNM, dataRow.baseTableRow);
                    //bool isEnum = false;
                    string isEnum = "false";
                    string realVal = pair.Value.Value;
                    if (pair.Value.Key.BaseColumnID.ToColumnType().ToDecodedString().ToUpper() == "PARENTTABLEREF" || pair.Value.Key.BaseColumnID.Is_Enumerated())
                    {
                        isEnum = "true";
                        realVal = "";
                        var enums = colEnums[pair.Key];
                        string findVal = origRow.GetValue(pair.Value.Key.columnName);
                        if (findVal == "0")
                        {
                            realVal = "";
                        }
                        else if (findVal == "")
                        {
                            realVal = "";
                        }
                        else
                        {
                            var test = enums.Where(x => x.Value.Key == findVal);
                            if (test.Any())
                            {
                                realVal = test.First().Value.Value;
                            }
                            else
                            {
                                realVal = "roguenotvalid";
                            }
                        }
                    }
                    FilledSelectRow dataCol = new FilledSelectRow(dataColTblNM, cellRow, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), realVal }, { new ColumnRowID(7379), isEnum }, { new ColumnRowID(-1012), pair.Value.Key.BaseColumnID.ToString() }, { new ColumnRowID(-1020), pair.Value.Key.BaseColumnID.ToColumnType().ToDecodedString() } }));
                    dataCol.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(pair.Value.Key, realVal));
                    dataColTbl.AddRow(dataCol);
                    //cellRow.childRows.Add(dataCol);
                    //dataCol.SetParent(cellRow);
                    //dataCol.tableRefRows.Add(dataCellTblNM, cellRow.baseTableRow);
                    if (colEnums[pair.Key].Count > 0)
                    {
                        FilledSelectRow enumVal = new FilledSelectRow(dataColDDLItemTblNM, dataCol, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), "" } }));
                        enumVal.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(colEnums[pair.Key][0].Key, ""));
                        dataColDDLItemTbl.AddRow(enumVal);

                        FilledSelectRow enumValID = new FilledSelectRow(dataColDDLItemValue, enumVal, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), "0" } }));
                        enumValID.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(colEnums[pair.Key][0].Key, ""));
                        dataColDDLItemValueTbl.AddRow(enumValID);
                    }
                    foreach (KeyValuePair<SelectColumn, KeyValuePair<string, string>> enumPair in colEnums[pair.Key])
                    {
                        FilledSelectRow enumVal = new FilledSelectRow(dataColDDLItemTblNM, dataCol, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), enumPair.Value.Value } }));
                        enumVal.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Key));
                        //enumVal.values.Add("DISPLAYVALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Key));
                        dataColDDLItemTbl.AddRow(enumVal);

                        FilledSelectRow enumValID = new FilledSelectRow(dataColDDLItemValue, enumVal, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), enumPair.Value.Key } }));
                        enumValID.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(colEnums[pair.Key][0].Key, ""));
                        dataColDDLItemValueTbl.AddRow(enumValID);
                        //dataCol.childRows.Add(enumVal);
                        //enumVal.SetParent(dataCol);
                        //enumVal.tableRefRows.Add(dataColTblNM, dataCol.baseTableRow);

                        if (origRow.GetValue(enumPair.Key.columnName) == enumPair.Value.Key)
                        {
                            FilledSelectRow selectedVal = new FilledSelectRow(selectedItemTblNM, enumVal, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>() { { new ColumnRowID(0), enumPair.Value.Value } }));
                            selectedVal.values.Add("VALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Value));
                            //selectedVal.values.Add("DISPLAYVALUE", new KeyValuePair<SelectColumn, string>(enumPair.Key, enumPair.Value.Key));
                            selectedItemTbl.AddRow(selectedVal);
                            //enumVal.childRows.Add(selectedVal);
                            //selectedVal.SetParent(enumVal);
                        }
                    }
                    //FilledSelectRow rw  = new FilledSelectRow();


                    /*switch (pair.Value.Key.BaseColumnID.Is_Enumerated())
                    {
                         
                        case true:
                            FilledSelectRow dataCol = new FilledSelectRow(dataColDDLTblNM, ValueRogueRow.GetFakeRogueRow(new Dictionary<ColumnRowID, string>(){{new ColumnRowID(0),pair.Value.Value}, {new ColumnRowID(7379), pair.Value.Key.BaseColumnID.Is_Enumerated().ToString()}}));
                            dataCol.values.Add("VALUE", pair.Value);
                            dataColDDLTbl.AddRow(dataCol);
                            dataRow.childRows.Add(dataCol);
                            dataCol.SetParent(dataRow);
                            break;
                        case false:
                            FilledSelectRow dataColDDL = new FilledSelectRow(dataColTblNM, ValueRogueRow.GetFakeRogueRow(pair.Value.Value));
                            dataColDDL.values.Add("VALUE", pair.Value);
                            dataColTbl.AddRow(dataColDDL);
                            dataRow.childRows.Add(dataColDDL);
                            dataColDDL.SetParent(dataRow);
                            break;
                    }*/
                    /*FilledSelectRow dataCol = new FilledSelectRow(dataColTblNM, ValueRogueRow.GetFakeRogueRow(pair.Value.Value));
                    dataCol.values.Add("VALUE", pair.Value);
                    dataColTbl.AddRow(dataCol);
                    dataRow.childRows.Add(dataCol);
                    dataCol.SetParent(dataRow);*/
                }
            }
            tblList.AddRange(parentTbl.rows);
            //LoopPrintHierachy(parent, 0, true);
            //parentTbl.
            return tblList;
        }
        void LoopPrintHierachy(FilledSelectRow topRow, int currLvl, bool printBase = false)
        {
            topRow.PrintRow(currLvl, printBase);
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
        public IRoguePair IGetBasePair(ColumnRowID colRowID)
        {
            return new ValueRoguePair(colRowID, vals[colRowID]);
        }
        public IEnumerable<IRoguePair> pairs()
        {
            throw new NotImplementedException();
        }
        public IRoguePair ITryGetValue(ColumnRowID colRowID)
        {
            if (vals.ContainsKey(colRowID))
            {
                return new ValueRoguePair(vals[colRowID]);
            }
            else
            {
                return null;
            }
        }
        public IRoguePair NewWritePair(IORecordID ownerTblID, ColumnTypes colType, string columnName, RowID value)
        {
            throw new NotImplementedException();
        }
        public IRoguePair NewWritePair(IORecordID ownerTblID, string columnName, RowID value, IORecordID parentRecord)
        {
            throw new NotImplementedException();
        }
        public IRoguePair NewWritePair(ColumnRowID colID, string colValue)
        {
            throw new NotImplementedException();
        }
        public string GetValueByColumn(ColumnRowID thsCol)
        {
            string ret = "";
            vals.TryGetValue(thsCol, out ret);
            return ret;
        }
        public void SetValue(ColumnRowID col, string value)
        {
            throw new NotImplementedException();
        }

        public IRoguePair NewWritePair(IORecordID ownerTblID, string colNM, string colValue)
        {
            throw new NotImplementedException();
        }
    }
    class ValueRoguePair : IRoguePair
    {
        string val;
        public ColumnRowID KeyColumnID { get; set; }
        public ValueRoguePair(String val) { this.val = val; }
        public ValueRoguePair(ColumnRowID id, String val) { this.val = val; KeyColumnID = id; }
        public string DisplayValue()
        {
            return val;
        }
        public string WriteValue()
        {
            return val.ToDecodedRowID().ToString();
        }
    }
}
