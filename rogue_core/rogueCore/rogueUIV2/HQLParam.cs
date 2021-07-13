//using rogueCore.hqlSyntax;
//using rogueCore.hqlSyntaxV2.filledSegments;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace rogueCore.rogueUIV2
//{
//    class HQLParam
//    {
//        MultiRogueRow paramRow;
//        internal string value { private set; get; }
//        internal HQLParam(MultiRogueRow thsRow)
//        {
//            this.paramRow = thsRow;
//            value = DefaultValue();
//        }
//        internal void SetParamValue(string val)
//        {
//            this.value = val;
//        }
//        internal string ParamReplaceTxt()
//        {
//            return paramRow.values["PARM_TXT_ID"].Value;
//        }
//        internal string DefaultValue()
//        {
//            return paramRow.GetValue("DEFAULT_VALUE");
//        }
//        internal string ParamOID()
//        {
//            return paramRow.GetValue("PARAM_OID");
//        }
//    }
//}
