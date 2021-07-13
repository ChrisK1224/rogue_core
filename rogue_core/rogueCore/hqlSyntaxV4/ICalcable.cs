using rogue_core.rogueCore.binary;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4
{
    public interface ICalcable 
    {
        string RetrieveStringValue(IEnumerable<Dictionary<string, IReadOnlyRogueRow>> parentRows);
    }
}
