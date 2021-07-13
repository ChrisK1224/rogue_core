using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3
{
    class RogueSectionQuery
    {
        MultiRogueRow qryRow;
        List<HQLParam> parameters = new List<HQLParam>();
        internal RogueSectionQuery(MultiRogueRow qryRow)
        {
            this.qryRow = qryRow;
            foreach(MultiRogueRow paramRow in qryRow.childRows)
            {
                parameters.Add(new HQLParam(paramRow));
            }
        }
        internal int QueryOID()
        {
            return int.Parse(qryRow.GetValue("QUERY_OID"));
        }
        internal string QueryText()
        {
            return qryRow.GetValue("QUERY_TXT");
        }
        internal string SectionIDText()
        {
            return qryRow.GetValue("SECTION_PARAM_TXT");
        }
    }
}
