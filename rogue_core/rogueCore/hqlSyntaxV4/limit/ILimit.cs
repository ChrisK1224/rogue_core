using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.limit
{
    public interface ILimit : ITempBase
    {
        int limitRows { get; }
    }
}
