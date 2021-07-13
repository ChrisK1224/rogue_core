using rogue_core.rogueCore.queryResults;
using rogueCore.hqlSyntax;
using rogueCore.hqlSyntaxV3.segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.StoredProcedures.StoredQuery
{
    public class StoredHQLQuery
    {
        public int queryID;
        String runQry;
        String origQry;
        Dictionary<String, String> hqlParams = new Dictionary<string, string>();
        public StoredHQLQuery(String baseQry, int queryID)
        {
            this.queryID = queryID;
            this.origQry = baseQry;
            //this.runQry = baseQry.Replace("@QUERYID", queryID.ToString());
        }
        //Query with Parameters
        public StoredHQLQuery(String baseQry, int queryID, Dictionary<String, String> lstParams)
        {
            this.queryID = queryID;
            this.origQry = baseQry;
            hqlParams = lstParams;
        }
        public SelectHQLStatement RunQuery()
        {
            var tbl = new SelectHQLStatement(FinalHQLStatement());
            tbl.Fill();
            return tbl;
        }
        String FinalHQLStatement()
        {
            this.runQry = origQry.Replace("@QUERYID", queryID.ToString());
            foreach (KeyValuePair<String, String> param in hqlParams)
            {
                runQry = runQry.Replace(param.Key, param.Value);
            }
            return runQry;
        }
        internal void ResetParameters(Dictionary<String, String> lstParams = null)
        {
            if(lstParams == null)
            {
                lstParams = new Dictionary<string, string>();
            }
            hqlParams = lstParams;
        }
    }
}
