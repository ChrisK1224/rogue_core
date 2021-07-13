using System.Collections.Generic;
using rogue_core.rogueCore.StoredProcedures.StoredQuery;

namespace rogueCore.StoredProcedures
{
    class StoredUIQuery : StoredHQLQuery
    {
        internal Dictionary<int, string> selectDependentQueryIDs = new Dictionary<int, string>();
        internal string sectionID;
        internal StoredUIQuery(string baseQry, int queryID, string sectionID) : base(baseQry, queryID){ this.sectionID = sectionID;}
        internal StoredUIQuery(string baseQry, int queryID,string sectionID, Dictionary<string, string> lstParams) : base(baseQry, queryID, lstParams){ this.sectionID = sectionID;}
        internal void SelectCommand(Dictionary<string,string> pageContent){
            
        }
    }
}