using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV2.segments.snippet
{
    class SnippetParam
    {
        internal string paramID;
        internal string paramValue;
        const char paramSplit = '=';
        internal SnippetParam(string paramHql, HQLMetaData metaData)
        {
            int index = paramHql.IndexOf(paramSplit);
            paramID = paramHql.Substring(0, index).Trim().ToUpper();
            paramValue = paramHql.Substring(index + 1);
            paramValue = paramValue.Trim().Substring(1, paramValue.Length - 3);
        }
    }
}
