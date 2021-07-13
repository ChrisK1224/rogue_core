using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hql.segments
{
    interface IHQLSegment
    {
        String HumanToEncodedHQL(String humanHQL);
    }
}
