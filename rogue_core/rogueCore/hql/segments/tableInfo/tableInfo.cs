using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hql.segments.tableInfo
{
    public class TableInfo : IHQLSegment
    {
        private const char startTableIDSeparator = '<';
        private const char endTableIDSeparator = '>';
        public const String humanHQLSplitter = "FROM ";
        public IORecordID tableID;
        public String tableRefName = "";
        public TableInfo(String fullSegment)
        {
            String IORecordStr = stringHelper.GetStringBetween(fullSegment, startTableIDSeparator.ToString(), endTableIDSeparator.ToString());
            if (IORecordStr.Contains("."))
            {
                String[] ids = IORecordStr.Split('.');
                tableID = new IORecordID(ids[0]);
                tableRefName = ids[1].Trim();
            }
            else
            {
                tableID = new IORecordID(IORecordStr);
                tableRefName = tableID.ToString();
                //tableRefName = HQLEncoder.GetTableNameByRogueID(tableID);
            }
        }
        public TableInfo(IORecordID tableID, String tableRefName)
        {
            this.tableID = tableID;
            this.tableRefName = tableRefName;
            //this.matrixTableID = matrixTableID;
        }
        public TableInfo(int queryID)
        {

        }
        public static TableInfo FromHumanHQL(String EncodedSegment)
        {
            //EncodedSegment = EncodedSegment.ToUpper();
            int indexOfSpace = EncodedSegment.IndexOf(" ");
            if(indexOfSpace == -1)
            {
                indexOfSpace = EncodedSegment.Length;
            }
            String tableIDPortion = EncodedSegment.Substring(0, indexOfSpace);
            IORecordID tableID;
            if (tableIDPortion.Contains(".")){
                tableID = HQLEncoder.DecodeTableName(tableIDPortion);
            }
            else
            {
                // * this is for when full name is not specified
                tableID = new IORecordID(HQLEncoder.GuessTableIDByName(tableIDPortion));
                //return new TableInfo(tableID, tableIDPortion);
            }
            //int indexOfJoin = EncodedSegment.ToUpper().IndexOf("JOIN");
            //String tableSegment = EncodedSegment.Substring(0, indexOfJoin);
            String[] names = EncodedSegment.ToUpper().Split(new string[] { " AS " }, StringSplitOptions.None);
            
            String tableRefName;
            if(names.Length > 1)
            {
                tableRefName = names[1].Trim();
            }
            else
            {
                String[] ioNames = EncodedSegment.Split('.');
                tableRefName = ioNames[ioNames.Length - 1].Trim();
            }
            return new TableInfo(tableID, tableRefName);
        }
        public String GetFullHQLText()
        {
            String encodedSegment = " FROM ";
            //String tableSegment = stringHelper.GetStringBetween(EncodedSegment, "FROM", "JOIN");
            String fullIDName = HQLEncoder.GetDecodedFullTableName(this.tableID);
            //String[] names = tableSegment.Split(new string[] { " AS " }, StringSplitOptions.None);
            //IORecordID tableID = HQLEncoder.GetTableNameByRogueID(names[0]);
            encodedSegment += fullIDName + " AS " + this.tableRefName + " ";
            return encodedSegment;
        }
        public String GetTableHQLText()
        {
            return startTableIDSeparator + tableID.ToString() + "." + tableRefName + endTableIDSeparator;
        }

        public string HumanToEncodedHQL(string humanHQL)
        {
            throw new NotImplementedException();
        }
    }
}
