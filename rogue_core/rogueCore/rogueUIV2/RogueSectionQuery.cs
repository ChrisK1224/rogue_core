//using rogueCore.hqlSyntaxV2.filledSegments;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace rogueCore.rogueUIV2
//{
//    class RogueSectionQuery
//    {
//        MultiRogueRow qryRow;
//        List<HQLParam> parameters = new List<HQLParam>();
//        internal RogueSectionQuery(MultiRogueRow qryRow)
//        {
//            this.qryRow = qryRow;
//            foreach(MultiRogueRow paramRow in qryRow.childRows)
//            {
//                parameters.Add(new HQLParam(paramRow));
//            }
//        }
//        internal int QueryOID()
//        {
//            return int.Parse(qryRow.values["QUERY_OID"].Value);
//        }
//        internal string QueryText()
//        {
//            return qryRow.values["QUERY_TXT"].Value;
//        }
//        internal string SectionIDText()
//        {
//            return qryRow.values["SECTION_PARAM_TXT"].Value;
//        }
//    }
//}
