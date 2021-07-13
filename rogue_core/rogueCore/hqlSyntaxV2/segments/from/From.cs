using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using rogueCore.hqlSyntaxV2.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.hqlSyntaxV2.MutliSegmentEnum;

namespace rogueCore.hqlSyntaxV2.segments.from
{
    //public abstract class From : MultiSymbolSegment<PlainList<String>,String>, ISplitSegment
    public class From : IFrom
    {
        public const String splitKey = "FROM";
        public const String splitTableName = " AS ";
        protected string[] keys { get { return new string[1] { " AS " }; } }
        public IORecordID tableID { get; private set; }
        public string tableRefName { get; private set; }
        bool hasAlias = false;
        public string origTxt { get; }
        //internal bool isEncoded = false;
        public From(String fromTxt, HQLMetaData metaData) 
        {
            origTxt = fromTxt;
            var segmentItems  = new  MultiSymbolString<PlainList<string>>(SymbolOrder.symbolafter, fromTxt, keys,metaData).segmentItems;
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
                metaData.AddEncodedTableRef(tableRefName);
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
        public From(IORecordID tableID) : base()
        { 
            this.tableID = tableID;
            this.tableRefName = HQLEncoder.GetTableNameByRogueID(tableID);
        }
        public IEnumerable<IRogueRow> StreamIRows(MultiRogueRow parentRow)
        {
            return tableID.ToTable().StreamIRows();
        }
        public List<UIDecoratedTextItem> txtItems()
        {
            List<UIDecoratedTextItem> items = new List<UIDecoratedTextItem>();
            items.Add(new UIDecoratedTextItem(splitKey, "red", "bold"));
            if (hasAlias)
            {
                items.Add(new UIDecoratedTextItem(splitKey, "red", "bold"));
                items.Add(new UIDecoratedTextItem(splitKey, "red", "bold"));
            }
            return items;
        }
    }
}
