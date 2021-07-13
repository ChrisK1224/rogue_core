//using rogueCore.hqlSyntax;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace rogueCore.rogueUI
//{
//    class RogueSectionQuery
//    {
//        FilledSelectRow qryRow;
//        List<HQLParam> parameters = new List<HQLParam>();
//        internal RogueSectionQuery(FilledSelectRow qryRow)
//        {
//            this.qryRow = qryRow;
//            foreach(FilledSelectRow paramRow in qryRow.childRows)
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
