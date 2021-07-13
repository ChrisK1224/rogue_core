using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.limit
{
    public class EmptyLimit : ILimit
    {
        public int limitRows { get { return -1; } }
    }
}
