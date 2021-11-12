using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    public interface ISelectRow : ITempBase
    {
        List<ISelectColumn> selectColumns { get; }
        Dictionary<string, ISelectColumn> columnsByName { get; }
    }
}
