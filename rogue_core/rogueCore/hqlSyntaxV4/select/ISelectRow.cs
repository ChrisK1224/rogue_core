﻿using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.select
{
    public interface ISelectRow
    {
        List<ISelectColumn> selectColumns { get; }
    }
}
