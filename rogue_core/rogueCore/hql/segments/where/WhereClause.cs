using System;
using System.Collections.Generic;
using System.Text;
using FilesAndFolders;
using rogue_core.rogueCore.hql.segments;
using rogue_core.rogueCore.hql.segments.selects;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;

namespace rogue_core.RogueCode.hql.hqlSegments.where
{
    public class WhereClause
    {
        public ColumnRowID localRowColID;
        public EvaluationTypes evalType;
        public String thsValue;
        public DecodedRowID value;
        public LocationColumn localColumn;
        public WhereClause(ColumnRowID localRowColID, EvaluationTypes evalType, String thsValue)
        {
            this.localRowColID = localRowColID;
            this.evalType = evalType;
            this.thsValue = thsValue;
            SetRealValue();
        }
        public WhereClause(String whereSnippet)
        {
            int charIndex = whereSnippet.IndexOfAny(WhereClause.evalChars());
            localRowColID = new ColumnRowID(whereSnippet.Substring(0, charIndex));
            evalType = (WhereClause.EvaluationTypes)whereSnippet[charIndex];
            thsValue = whereSnippet.Substring(charIndex + 1, (whereSnippet.Length - charIndex) - 1);
            SetRealValue();
        }
        private void SetRealValue()
        {
            int testValue;
            if (int.TryParse(thsValue, out testValue))
            {
                //* TODO bad code lots of if staemnts to determine if rogueID data type then send as number else as encoded number
                if (localRowColID.ColumnDataTypeID().Equals(SystemIDs.IOTableRecords.Tables.DataTypeTables.rogueIDTableID))
                {
                    value = testValue;
                }
                else
                {
                    value = testValue.ToDecodedRowID();
                }

            }
            else
            {
                value = thsValue.ToDecodedRowID();
            }
        }
        public String GetHQLText()
        {
            String hql = localRowColID.ToString() + (char)evalType + thsValue;
            return hql;
        }
        public String GetHQLFullText()
        {
            return HQLEncoder.GetColumnNameByID(localRowColID) + " " + (char)evalType + " \"" + thsValue + "\"";
        }
        public enum EvaluationTypes
        {
            equal = '=', notEqual = '!', merge = '$', valuePair = '?'
        }
        public static char[] evalChars()
        {
            char[] chars = new char[4];
            chars[0] = '=';
            chars[1] = '!';
            chars[2] = '$';
            chars[3] = '?';
            return chars;
        }
        public static string[] evalSeperators()
        {
            string[] chars = new string[4];
            chars[0] = " = ";
            chars[1] = " ! ";
            chars[2] = " $ ";
            chars[3] = " ? ";
            return chars;
        }
        public static WhereClause HumanToEncodedHQL(String humanWhereHQL, Dictionary<String, int> tableRefIDs, String lastTableRefName)
        {
            String[] whereParts = humanWhereHQL.Split(new char[0]);
            ColumnRowID thsColID = new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(whereParts[0], tableRefIDs[lastTableRefName]));
            EvaluationTypes tp = ((WhereClause.EvaluationTypes)whereParts[1][0]);
            return new WhereClause(thsColID, ((WhereClause.EvaluationTypes)whereParts[1][0]), whereParts[2].TrimFirstAndLastChar());
            //return  thsColID + " " + (WhereClause.EvaluationTypes)humanWhereHQL[1] + "\"" + whereParts[2] + "\"";
        }
    }
}
