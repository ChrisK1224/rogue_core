using System;
using System.Collections.Generic;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.binary;
using System.Diagnostics;
using files_and_folders;
using System.Threading.Tasks;

namespace rogueCore.streaming
{
    public abstract class StreamInterpreter
    {
        RogueDatabase<DataRowID> stream_db;
        List<IRogueTable> activeTables = new List<IRogueTable>();
        Dictionary<string, IRogueTable> writeTables = new Dictionary<string, IRogueTable>();
        IRogueRow activeRow { get { return activeTables[activeTables.Count - 1].currWriteRows[activeTables[activeTables.Count - 1].currWriteRows.Count - 1]; } }
        public IRogueRow topRow { get; private set; } = null;// **Need to fix this to write all rows at once
        internal StreamInterpreter(string dbId, string topTableName)
        {
            stream_db = new RogueDatabase<DataRowID>(new IORecordID(dbId));
            //var topTbl = stream_db.GetTable(topTableName);
            //topRow = topTbl.NewWriteRow();
            //activeTables.Add(topTbl);
        } 
        internal StreamInterpreter(RogueDatabase<DataRowID> ths_db, Dictionary<string,string> topLevelValues, string topTableName)
        {
            stream_db = ths_db;
            //var topTbl = stream_db.GetTable(topTableName);
            var topTbl = GetRogueTable(topTableName);            
            topRow = topTbl.NewWriteRow();
            foreach (var pair in topLevelValues)
            {
                topRow.NewWritePair(topTbl.ioItemID, pair.Key, pair.Value);
            }
            activeTables.Add(topTbl);
        }
        protected void NewRow(String tableName)
        {
            IRogueTable rowTbl = GetRogueTable(tableName);
            IRogueRow newRow = rowTbl.NewWriteRow();
            if(activeTables.Count > 0)
            {
                String columnName = activeTables[activeTables.Count-1].ioItemID.TableName() + "_OID";
                //Problem is here writing number instead of string need to pass col type as well
                newRow.NewWritePair(rowTbl.ioItemID, columnName, activeRow.rowID.ToInt().ToString(), new IORecordID(activeTables[activeTables.Count - 1].ioItemID));
            } else if(topRow == null)
            {
                topRow = newRow;
            }
            activeTables.Add(rowTbl);
        }
        protected void AddKeyValuePair(String key, String value)
        {
            activeRow.NewWritePair(activeTables[activeTables.Count-1].ioItemID,key, value);
        }
        protected void CloseRow()
        {
            //activeTables[activeTables.Count-1].Write();
            activeTables.RemoveAt(activeTables.Count-1);
        }
        protected void Close()
        {
            //for(int i =activeTables.Count-1; i >-1; i--)
            //{
            //    activeTables[i].Write();
            //}
            foreach(var tbl in writeTables.Values)
            {
                tbl.Write();
            }
            //Parallel.ForEach(writeTables, x => x.Value.Write());
        }
        public IRogueTable GetRogueTable(string tableName)
        {
            tableName = tableName.ToUpper();
            if (!writeTables.ContainsKey(tableName))
            {
                var newTbl = stream_db.GetTable(tableName);
                writeTables.Add(tableName, newTbl);
                return newTbl;
            }
            else
            {
                return writeTables[tableName];
            }
        }
    }
}