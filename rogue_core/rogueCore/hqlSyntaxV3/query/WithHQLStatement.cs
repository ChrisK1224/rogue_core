using FilesAndFolders;
using rogueCore.hqlSyntaxV3.segments;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV3.query
{
    public class WithHQLStatement : IHQLStatement
    {
        public string withName { get; }
        SelectHQLStatement selectStatement { get; }
        public WithHQLStatement(string qry)
        {
            withName = qry.BeforeFirstSpace();
            selectStatement = new SelectHQLStatement(qry.get_string_between_2("(", ")"));
            selectStatement.Fill();
        }
    }
}
