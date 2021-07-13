using rogueCore.hqlSyntax;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3
{
    class HQLParam
    {
        IMultiRogueRow paramRow;
        internal string value { private set; get; }
        internal HQLParam(IMultiRogueRow thsRow)
        {
            this.paramRow = thsRow;
            value = DefaultValue();
        }
        internal void SetParamValue(string val)
        {
            this.value = val;
        }
        internal string ParamReplaceTxt()
        {
            return paramRow.GetValue("PARM_TXT_ID");
        }
        internal string DefaultValue()
        {
            return paramRow.GetValue("DEFAULT_VALUE");
        }
        internal string ParamOID()
        {
            return paramRow.GetValue("PARAM_OID");
        }
    }
}
