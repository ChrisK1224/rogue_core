using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogueCore.hqlSyntax;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntax.MutliSegmentEnum;

namespace rogueCore.hqlSyntax.segments.from
{
    //public abstract class From : MultiSymbolSegment<PlainList<String>,String>, ISplitSegment
    public class From
    {
        public const String splitKey = "FROM";
        protected string[] keys { get { return new string[1] { " AS " }; } }
        public IORecordID tableID { get; protected set; }
        public String tableRefName { get; protected set; }
        internal bool isEncoded = false;
        public From(String fromTxt, HQLMetaData metaData) 
        {
            var segmentItems  = new MultiSymbolSegmentNew<PlainList<string>, string>(SymbolOrder.symbolafter, fromTxt, keys, metaData).segmentItems;
            //**
            if (segmentItems.Count > 1)
            {
                tableRefName = segmentItems[1];
            }
            else
            {
                String[] ioNames = segmentItems[0].Split('.');
                tableRefName = ioNames[ioNames.Length - 1].Trim();
            }
            if (fromTxt.StartsWith("{") || fromTxt.StartsWith("[{")) 
            {
                metaData.encodedTableStatements.Add(tableRefName);
                //isEncoded = true;
            }
           
            else if (segmentItems[0].Contains("."))
            {
                tableID = HQLEncoder.DecodeTableName(segmentItems[0]);
            }
            else if (segmentItems[0].StartsWith("["))
            {
                //*To handle a default avlue when a parameter is set here instead of an int. Needed for columnqry later
                int id = 0;
                int.TryParse(stringHelper.GetStringBetween(segmentItems[0], "[", "]"), out id);
                if (id == 0) { id = -1010; }
                tableID = id;
            }
            else
            {
                // * this is for when full name is not specified
                tableID = new IORecordID(HQLEncoder.GuessTableIDByName(segmentItems[0]));
            }
           
        }
        public From(IORecordID tableID) : base() { 
            this.tableID = tableID;
            this.tableRefName = HQLEncoder.GetTableNameByRogueID(tableID);
        }
    }
}
