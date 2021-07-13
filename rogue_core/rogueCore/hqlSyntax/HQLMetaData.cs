using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntax
{
    public class HQLMetaData
    {
        internal Dictionary<string, int> tableRefIDs { get; private set; }= new Dictionary<string, int>();
        internal string currTableRefName;
        internal List<string> encodedTableStatements = new List<string>();
        internal FilledTable rootTable;
        internal void AddTableRefID(string key, int tableID)
        {
            tableRefIDs.Add(key, tableID);
        }
        internal void AddChangeTableRefID(string key, int tableID)
        {
            if (tableRefIDs.ContainsKey(key))
            {
                tableRefIDs[key] = tableID;
            }
            else
            {
                tableRefIDs.Add(key, tableID);
            }
        }
        public HQLMetaData()
        {
            rootTable = new FilledTable(this, FilledSelectRow.BaseRow());
        }
    }
}
