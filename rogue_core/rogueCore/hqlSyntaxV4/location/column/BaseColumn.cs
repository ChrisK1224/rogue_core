using rogue_core.rogueCore.hqlSyntaxV4.column;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location.column
{
    public class BaseColumn
    {
        public static IColumn ParseColumn(string locationTxt, QueryMetaData metaData)
        {
            locationTxt = locationTxt.Trim();
            if (locationTxt.Contains("{"))
            {
                return new EncodedColumn(locationTxt, metaData);
            }
            else if (locationTxt.StartsWith("\""))
            {
                return new ConstantColumn(locationTxt, metaData);
            }
            else if (locationTxt.ToUpper().StartsWith("EXECUTE("))
            {
                throw new Exception("No code for executeColumn");
            }
            //else if (new NamedLocation(locationTxt, metaData).remainingTxt.EndsWith(")"))
            else if(locationTxt.Contains("(") && locationTxt.Contains(")"))
            {
                return CommandLocation.GetCommandColumn(locationTxt, metaData);
            }
            //else if(locationTxt.StartsWith("(") && locationTxt.EndsWith(")"))
            //{
            //    return new CommandLocation(locationTxt, queryStatement);
            //}
            else if (locationTxt.Equals("*"))
            {
                return null;
               // return new StarColumn(locationTxt, metaData);
            }
            else
            {
                return new StandardColumn(locationTxt, metaData);
            }
        }
    }
}
