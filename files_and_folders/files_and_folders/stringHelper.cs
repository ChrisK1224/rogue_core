using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilesAndFolders
{
    /// <summary>
    /// this class has several methods for does common things with strings
    /// </summary>
    public static class stringHelper
    {
        public static int FindCharIndexAfterString(this string source, string findValue, char nextChar)
        {
            int sourceLocation = source.EndIndexOf(findValue);
            String partialSource = source.Substring(sourceLocation);
            int partialLocation = partialSource.IndexOf(nextChar);
            return sourceLocation + partialLocation + 1;
        }
        public static int EndIndexOf(this string source, string value)
        {
            int index = source.IndexOf(value);
            if (index >= 0)
            {
                index += value.Length;
            }
            return index;
        }
        public static String[] SectionOffByChar(this string input, char sepChar)
        {
            List<int> startIndexes = new List<int>();
            // string startPattern = "(\\()(?=(?:[^\"]|\"[^\"]*\")*$)";
            string startPattern = "(\\" + sepChar + ")(?=(?:[^\"]|\"[^\"]*\")*$)";
            //string endPattern = "(\\))(?=(?:[^\"]|\"[^\"]*\")*$)";
            foreach (Match match in Regex.Matches(input, startPattern))
            {
                startIndexes.Add(match.Index);
            }
            String[] finalSegments = new String[startIndexes.Count];
            startIndexes.Add(input.Length);
            for (int i = 0; i < startIndexes.Count - 1; i++)
            {
                finalSegments[i] = input.Substring(startIndexes[i], startIndexes[i + 1] - startIndexes[i]).Trim();
            }
            return finalSegments;
        }
        public static String AfterFirstSpace(this String thsSegment)
        {
            return thsSegment.Substring(thsSegment.IndexOf(" ") + 1, (thsSegment.Length - thsSegment.IndexOf(" ")) - 1);
        }
        public static String AfterFirstKey(this String thsSegment, string key)
        {
            if (thsSegment.ToUpper().Contains(key.ToUpper()))
            {
                return thsSegment.Substring(thsSegment.ToUpper().IndexOf(key.ToUpper()) + key.Length).Trim();
            }
            else
            {
                return thsSegment.Trim();
            }
        }
        public static string ReplaceLastOccurrence(this string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);
            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
        public static string ReplaceFirstOccurrence(this string Source, string Find, string Replace)
        {
            int place = Source.IndexOf(Find);
            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
        public static String BeforeFirstKey(this String thsSegment, string key)
        {
            if (thsSegment.ToUpper().Contains(key.ToUpper()))
            {
                return thsSegment.Substring(0, thsSegment.ToUpper().IndexOf(key.ToUpper())).Trim();
            }
            else
            {
                return thsSegment.Trim();
            }
        }
        public static String BeforeFirstSpace(this String thsSegment)
        {
            if(thsSegment.Contains(" "))
            {
                return thsSegment.Substring(0, thsSegment.IndexOf(" "));
            }
            else
            {
                return thsSegment;
            }
        }
        public static String BeforeLastSpace(this String thsSegment)
        {
            if (thsSegment.Contains(" "))
            {
                return thsSegment.Substring(0, thsSegment.LastIndexOf(" "));
            }
            else
            {
                return thsSegment;
            }
        }
        public static String BeforeFirstSpaceExceptInQuotesOrParenthesis(this String thsSegment)
        {
            return Regex.Split(thsSegment, @"\s(?![^\(\""]*[\""\)])")[0];
            //var parts = Regex.Matches(thsSegment, )
            //    .Cast<Match>()
            //    .Select(m => m.Value)
            //    .ToList();
            //return parts[0];
        }
        public static String BeforeFirstChar(this String thsSegment, char End)
        {
            if (thsSegment.Contains(End))
            {
                return thsSegment.Substring(0, thsSegment.IndexOf(End));
            }
            else
            {
                return thsSegment;
            }
        }
        //public static String BeforeFirstChar(this String thsSegment, List<char> End)
        //{
            
        //    if (thsSegment.Any(End))
        //    {
        //        return thsSegment.Substring(0, thsSegment.IndexOf(End));
        //    }
        //    else
        //    {
        //        return thsSegment;
        //    }
        //}
        public static String AfterFirstChar(this String thsSegment, char End)
        {
            if (thsSegment.Contains(End))
            {
                return thsSegment.Substring(thsSegment.IndexOf(End)+1);
            }
            else
            {
                return thsSegment;
            }
        }
        public static String AfterLastChar(this String thsSegment, char End)
        {
            if (thsSegment.Contains(End))
            {
                return thsSegment.Substring(thsSegment.LastIndexOf(End) + 1);
            }
            else
            {
                return thsSegment;
            }
        }
        public static String AfterLastSpace(this String thsSegment)
        {
            return thsSegment.Substring(thsSegment.LastIndexOf(" ") + 1);
        }
        public static String TrimFirstAndLastChar(this String thsSegment)
        {
            return thsSegment.Substring(1, thsSegment.Length - 2);
        }
        public static string SplitBetween(this string token, string first, string second)
        {
            if (token.Contains(first) && token.Contains(second))
            {
                int start = token.IndexOf(first) + first.Length;
                return token.Substring(start, token.LastIndexOf(second) - start);
            }
            else
            {
                return "";
            }

        }
        public static string getBetweenCharLastOccurance(string token, char firstChar, char secondChar, Boolean toEnd = false)
        {
            if(toEnd == false)
            {
                return token.Substring(token.LastIndexOf(firstChar) + 1, (token.LastIndexOf(secondChar) - token.LastIndexOf(firstChar)) - 1);
            }
            else
            {
                return token.Substring(token.LastIndexOf(firstChar) + 1, (token.Length - token.LastIndexOf(firstChar)) - 1);
            }
            
        }
        public static string get_between_last_occurance_to_end(string token, String end_key)
        {
                return token.Substring(token.LastIndexOf(end_key) + 1, (token.Length - token.LastIndexOf(end_key)) - 1);
        }
        //public static string get_string_between_2(string token, string first, string second)
        //{
        //    if(token.Contains(first) && token.Contains(second))
        //    {
        //        int start = token.IndexOf(first) + first.Length;
        //        return token.Substring(start, token.LastIndexOf(second) - start);
        //    }
        //    else
        //    {
        //        return "";
        //    }
            
        //}
        public static string get_string_between_2(this string token, string first, string second)
        {
            if (token.Contains(first) && token.Contains(second))
            {
                int start = token.IndexOf(first) + first.Length;
                return token.Substring(start, token.LastIndexOf(second) - start);
            }
            else
            {
                return "";
            }

        }
        public static string get_string_between_first_occurs(string token, string first, string second)
        {
            if (token.Contains(first) && token.Contains(second))
            {
                int start = token.IndexOf(first) + first.Length;
                return token.Substring(start, token.IndexOf(second) - start);
            }
            else
            {
                return "";
            }

        }
        public static string get_string_between_first_occurs(string token, string first, List<string> second)
        {
            token = token.ToUpper();
            first = first.ToUpper();
            List<int> indexes = new List<int>();
            second.Where(x => token.Contains(x.ToUpper())).ToList().ForEach(x => indexes.Add(x.IndexOf(x)));
            indexes.Sort();
            int endIndex = (indexes.Count > 0) ? indexes[indexes.Count-1] : token.Length;
            var max = second.OrderByDescending(x => token.IndexOf(x.ToUpper())).FirstOrDefault();
            
            //second.ForEach(x => token.IndexOf(x.ToUpper()).OrderByDescending(item => item.Height).First();
            if (token.Contains(first.ToUpper()) && token.Contains(second[0]))
            {
                int start = token.IndexOf(first) + first.Length;
                return token.Substring(start, token.IndexOf(second[0]) - start);
            }
            else
            {
                return "";
            }

        }
        /// <summary>
        /// This method takes in a string called token and returns a value between the first and second variables. Or if toENd is set to true
        /// it will return string between first value and the end of the line
        /// </summary>
        /// <param name="token"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="toEnd"></param>
        /// <returns></returns>
        public static string GetStringBetween(string token, string first, string second, Boolean toEnd = false)
        {
            if (!token.Contains(first)) return "";

            //var afterFirst = token.Split(new[] { first }, StringSplitOptions.None)[1];
            int index = token.IndexOf(first);
            var afterFirst = token.Substring(index + 1, token.Length - (index+1));
            if (!afterFirst.Contains(second)) return "";

            if (toEnd == false)
            {
                return afterFirst.Split(new[] { second }, StringSplitOptions.None)[0];
            }
            else
            {
                return afterFirst;
            }

            //return result;
        }
        /// <summary>
        /// This method returns a string between the first and last occurance of the same character
        /// </summary>
        /// <param name="line"></param>
        /// <param name="splitchar"></param>
        /// <returns></returns>
        public static string getStringBetweenFirstLastOccurance(string line, char splitChar)
        {
            return line.Substring(line.IndexOf(splitChar) + 1, (line.LastIndexOf(splitChar) - line.IndexOf(splitChar)) - 1);
        }
        public static string getStringBetweenBothLastOccurance(string token, string first, string second)
        {
            if (token.Contains(first) && token.Contains(second))
            {
                int start = token.LastIndexOf(first) + first.Length;
                return token.Substring(start, token.LastIndexOf(second) - start);
            }
            else
            {
                return "";
            }
        }
        public static string replaceFirstOccurance(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        /// <summary>
        /// This method takes in a string and removes lead and trailing quote for csv files usually
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string removeLeadTrailQuote(string val)
        {
            if (val.Length > 2)
            {
                if (val.Substring(0, 1) == "\"")
                {
                    val = val.Substring(1, val.Length - 1);
                }
                if (val.Substring(val.Length - 1, 1) == "\"")
                {
                    val = val.Substring(0, val.Length - 1);
                }
            }
            return val;
        }
        /// <summary>
        /// This method creates a lisst of list from hardcodedd values sent to it. It createes up to five lists of string. mainly just for
        /// adding list quickly on one line of code
        /// </summary>      
        public static List<List<string>> iniListOfList(string name1, string val1, string name2 = "", string val2 = "", string name3 = "", string val3 = "", string name4 = "", string val4 = "", string name5 = "", string val5 = "")
        {
            List<List<string>> thsList = new List<List<string>>();
            List<string> tempLst = new List<string>();
            tempLst.Add(name1);
            tempLst.Add(val1);
            thsList.Add(tempLst);

            if (name2 != "")
            {
                tempLst = new List<string>();
                tempLst.Add(name2);
                tempLst.Add(val2);
                thsList.Add(tempLst);
            }

            if (name3 != "")
            {
                tempLst = new List<string>();
                tempLst.Add(name3);
                tempLst.Add(val3);
                thsList.Add(tempLst);
            }

            if (name4 != "")
            {
                tempLst = new List<string>();
                tempLst.Add(name4);
                tempLst.Add(val4);
                thsList.Add(tempLst);
            }

            if (name5 != "")
            {
                tempLst = new List<string>();
                tempLst.Add(name5);
                tempLst.Add(val5);
                thsList.Add(tempLst);
            }

            return thsList;
        }
        /// <summary>
        /// This method takes in up to 30 string and returns them as a list of string. Basiclly just a way to create a list on one line of code
        /// </summary>       
        public static List<string> iniList(string Val1, string Val2 = "", string Val3 = "", string Val4 = "", string Val5 = "", string Val6 = "", string Val7 = "", string Val8 = "", string Val9 = "", string Val10 = "", string Val11 = "", string Val12 = "", string Val13 = "", string Val14 = "", string Val15 = "", string Val16 = "", string Val17 = "", string Val18 = "", string Val19 = "", string Val20 = "", string Val21 = "", string Val22 = "", string Val23 = "", string Val24 = "", string Val25 = "", string Val26 = "", string Val27 = "", string Val28 = "", string Val29 = "", string Val30 = "", string Val31 = "", string Val32 = "", string Val33 = "", string Val34 = "", string Val35 = "")
        {
            List<string> thsList = new List<string>();
            
                thsList.Add(Val1);

                if (Val2 != "")
            {
                thsList.Add(Val2);
            }

            if (Val3 != "")
            {
                thsList.Add(Val3);
            }

            if (Val4 != "")
            {
                thsList.Add(Val4);
            }

            if (Val5 != "")
            {
                thsList.Add(Val5);
            }

            if (Val6 != "")
            {
                thsList.Add(Val6);
            }

            if (Val7 != "")
            {
                thsList.Add(Val7);
            }

            if (Val8 != "")
            {
                thsList.Add(Val8);
            }

            if (Val9 != "")
            {
                thsList.Add(Val9);
            }
            if (Val10 != "")
            {
                thsList.Add(Val10);
            }
            if (Val11 != "")
            {
                thsList.Add(Val11);
            }
            if (Val12 != "")
            {
                thsList.Add(Val12);
            }
            if (Val13 != "")
            {
                thsList.Add(Val13);
            }
            if (Val14 != "")
            {
                thsList.Add(Val14);
            }
            if (Val15 != "")
            {
                thsList.Add(Val15);
            }
            if (Val16 != "")
            {
                thsList.Add(Val16);
            }
            if (Val17 != "")
            {
                thsList.Add(Val17);
            }
            if (Val18 != "")
            {
                thsList.Add(Val18);
            }
            if (Val19 != "")
            {
                thsList.Add(Val19);
            }
            if (Val20 != "")
            {
                thsList.Add(Val20);
            }
            if (Val21 != "")
            {
                thsList.Add(Val21);
            }
            if (Val22 != "")
            {
                thsList.Add(Val22);
            }
            if (Val23 != "")
            {
                thsList.Add(Val23);
            }
            if (Val24 != "")
            {
                thsList.Add(Val24);
            }
            if (Val25 != "")
            {
                thsList.Add(Val25);
            }
            if (Val26 != "")
            {
                thsList.Add(Val26);
            }
            if (Val27 != "")
            {
                thsList.Add(Val27);
            }
            if (Val28 != "")
            {
                thsList.Add(Val28);
            }
            if (Val29 != "")
            {
                thsList.Add(Val29);
            }
            if (Val30 != "")
            {
                thsList.Add(Val30);
            }
            if (Val31 != "")
            {
                thsList.Add(Val31);
            }

            if (Val32 != "")
            {
                thsList.Add(Val32);
            }

            if (Val33 != "")
            {
                thsList.Add(Val33);
            }

            if (Val34 != "")
            {
                thsList.Add(Val34);
            }

            if (Val35 != "")
            {
                thsList.Add(Val35);
            }
            
            return thsList;
        }
        /// <summary>
        /// This method takes in up to 35 string and returns them as a list with all values set to uppercase to be used with genIntializeTable to 
        /// exempt certain column makes sure they are upper 
        /// </summary>
        /// <param name="Val1"></param>
        /// <param name="Val2"></param>
        /// <param name="Val3"></param>
        /// <param name="Val4"></param>
        /// <param name="Val5"></param>
        /// <param name="Val6"></param>
        /// <param name="Val7"></param>
        /// <param name="Val8"></param>
        /// <param name="Val9"></param>
        /// <param name="Val10"></param>
        /// <param name="Val11"></param>
        /// <param name="Val12"></param>
        /// <param name="Val13"></param>
        /// <param name="Val14"></param>
        /// <param name="Val15"></param>
        /// <param name="Val16"></param>
        /// <param name="Val17"></param>
        /// <param name="Val18"></param>
        /// <param name="Val19"></param>
        /// <param name="Val20"></param>
        /// <param name="Val21"></param>
        /// <param name="Val22"></param>
        /// <param name="Val23"></param>
        /// <param name="Val24"></param>
        /// <param name="Val25"></param>
        /// <param name="Val26"></param>
        /// <param name="Val27"></param>
        /// <param name="Val28"></param>
        /// <param name="Val29"></param>
        /// <param name="Val30"></param>
        /// <param name="Val31"></param>
        /// <param name="Val32"></param>
        /// <param name="Val33"></param>
        /// <param name="Val34"></param>
        /// <param name="Val35"></param>
        /// <returns></returns>
        public static List<string> iniListUpper(string Val1, string Val2 = "", string Val3 = "", string Val4 = "", string Val5 = "", string Val6 = "", string Val7 = "", string Val8 = "", string Val9 = "", string Val10 = "", string Val11 = "", string Val12 = "", string Val13 = "", string Val14 = "", string Val15 = "", string Val16 = "", string Val17 = "", string Val18 = "", string Val19 = "", string Val20 = "", string Val21 = "", string Val22 = "", string Val23 = "", string Val24 = "", string Val25 = "", string Val26 = "", string Val27 = "", string Val28 = "", string Val29 = "", string Val30 = "", string Val31 = "", string Val32 = "", string Val33 = "", string Val34 = "", string Val35 = "")
        {
            List<string> thsList = new List<string>();

            thsList.Add(Val1.ToUpper());

            if (Val2 != "")
            {
                thsList.Add(Val2.ToUpper());
            }

            if (Val3 != "")
            {
                thsList.Add(Val3.ToUpper());
            }

            if (Val4 != "")
            {
                thsList.Add(Val4.ToUpper());
            }

            if (Val5 != "")
            {
                thsList.Add(Val5.ToUpper());
            }

            if (Val6 != "")
            {
                thsList.Add(Val6.ToUpper());
            }

            if (Val7 != "")
            {
                thsList.Add(Val7.ToUpper());
            }

            if (Val8 != "")
            {
                thsList.Add(Val8.ToUpper());
            }

            if (Val9 != "")
            {
                thsList.Add(Val9.ToUpper());
            }
            if (Val10 != "")
            {
                thsList.Add(Val10.ToUpper());
            }
            if (Val11 != "")
            {
                thsList.Add(Val11.ToUpper());
            }
            if (Val12 != "")
            {
                thsList.Add(Val12.ToUpper());
            }
            if (Val13 != "")
            {
                thsList.Add(Val13.ToUpper());
            }
            if (Val14 != "")
            {
                thsList.Add(Val14.ToUpper());
            }
            if (Val15 != "")
            {
                thsList.Add(Val15.ToUpper());
            }
            if (Val16 != "")
            {
                thsList.Add(Val16.ToUpper());
            }
            if (Val17 != "")
            {
                thsList.Add(Val17.ToUpper());
            }
            if (Val18 != "")
            {
                thsList.Add(Val18.ToUpper());
            }
            if (Val19 != "")
            {
                thsList.Add(Val19.ToUpper());
            }
            if (Val20 != "")
            {
                thsList.Add(Val20.ToUpper());
            }
            if (Val21 != "")
            {
                thsList.Add(Val21.ToUpper());
            }
            if (Val22 != "")
            {
                thsList.Add(Val22.ToUpper());
            }
            if (Val23 != "")
            {
                thsList.Add(Val23.ToUpper());
            }
            if (Val24 != "")
            {
                thsList.Add(Val24.ToUpper());
            }
            if (Val25 != "")
            {
                thsList.Add(Val25.ToUpper());
            }
            if (Val26 != "")
            {
                thsList.Add(Val26.ToUpper());
            }
            if (Val27 != "")
            {
                thsList.Add(Val27.ToUpper());
            }
            if (Val28 != "")
            {
                thsList.Add(Val28.ToUpper());
            }
            if (Val29 != "")
            {
                thsList.Add(Val29.ToUpper());
            }
            if (Val30 != "")
            {
                thsList.Add(Val30.ToUpper());
            }
            if (Val31 != "")
            {
                thsList.Add(Val31.ToUpper());
            }

            if (Val32 != "")
            {
                thsList.Add(Val32.ToUpper());
            }

            if (Val33 != "")
            {
                thsList.Add(Val33.ToUpper());
            }

            if (Val34 != "")
            {
                thsList.Add(Val34.ToUpper());
            }

            if (Val35 != "")
            {
                thsList.Add(Val35.ToUpper());
            }

            return thsList;
        }
        /// <summary>
        /// This method takes in a string value and outputs its datatype can be a string, number, or date maybe boolean
        /// </summary>
        public static string getDataType(string val)
        {
            string thsType = "string";
            double result;
            DateTime isDate;
            // For suitable values of text, style and culture...
            bool isDoubleValid = double.TryParse(val, out result);
            bool isDateValid = DateTime.TryParse(val, out isDate);
            if (isDoubleValid == true)
            {
                thsType = "number";
            }
            else if(isDateValid == true)
            {
                thsType = "date";
            }
            else if (val.ToUpper() == "TRUE" || val.ToUpper() == "FALSE")
            {
                thsType = "boolean";
            }
            return thsType;
        }
        /// <summary>
        /// This method takes in a string value and outputs its datatype can be a string, number, or date maybe boolean
        /// </summary>
        public static Boolean validateDataType(string thsTyp,string val)
        {
            
            double result;
            DateTime isDate;
            // For suitable values of text, style and culture...
            bool isDoubleValid = double.TryParse(val, out result);
            bool isDateValid = DateTime.TryParse(val, out isDate);
            switch (thsTyp)
            {
                case "number":
                    if (isDoubleValid == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "date":
                    if (isDateValid == true)
                    {
                        return true; ;
                    }
                    else
                    {
                        return false;
                    }
                case "boolean":
                    if (val.ToUpper() == "TRUE" || val.ToUpper() == "FALSE")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "string":
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// This method takes in a string value and checks if it of the data type that is sent in. Different than
        /// get data type becuase it validates against a type that it is assumed to be. The list of potential data types
        /// has to be the same list of from the get Data Type method
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Boolean checkDataType(string val, string dataType)
        {
            switch (dataType)
            {
                case "string":
                    return true;
                case "date":
                    DateTime isDate;
                    bool isDateValid = DateTime.TryParse(val, out isDate);
                    if (isDateValid == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "number":
                    double result;
                    bool isDoubleValid = double.TryParse(val, out result);
                    if (isDoubleValid == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "boolean":
                    if(val == "0" || val == "0" || val.ToLower() == "false" || val.ToLower() == "true")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return false;
        }
        //public static List<KeyValuePair<int, KeyValuePair<int, string>>> groupByDuplicates(List<KeyValuePair<int,string>> thsList)
        //{
        //    List<KeyValuePair<int,KeyValuePair<int, string>>> retList = new List<KeyValuePair<int, KeyValuePair<int, string>>>();
        //    List<KeyValuePair<int, string>> foundList = new List<KeyValuePair<int, string>>();

        //    foreach (KeyValuePair<int, string> thsKey in thsList)
        //    {
        //        if (!foundList.Contains(thsKey))
        //        {
        //            foundList.Add(thsKey);
        //            retList.Add(new KeyValuePair<int, KeyValuePair<int, string>>(1, thsKey));
        //        }
        //        else
        //        {
        //            foreach(KeyValuePair<int, KeyValuePair<int, string>> thsRetListItem in retList)
        //            {
        //                if(thsRetListItem.Value.Key == thsKey.Key && thsRetListItem.Value.Value == thsKey.Value)
        //                {
        //                    var newEntry = new KeyValuePair<int, string>(oldEntry.Key, newValue);
        //                    thsRetListItem.Key = 2;
        //                }
        //            }
        //        }
        //    }
        //    return retList;
        //}
    }
}
