using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.from
{
    interface IIdableFrom : IFrom
    {
        IORecordID tableId {get;}
    }
}
