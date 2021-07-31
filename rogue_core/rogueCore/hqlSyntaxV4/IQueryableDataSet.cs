using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    interface IQueryableDataSet
    {
        List<IMultiRogueRow> rows { get; set; }
        string dataSetName { get; }
    }
}
