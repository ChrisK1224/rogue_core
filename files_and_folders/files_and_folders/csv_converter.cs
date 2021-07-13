using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace files_and_folders
{
    public static class csv_converter
    {
        /// <summary>
        /// This is simply a function that takes a string and attempts a conversion to another Type, on a success, it drops the value into output and returns true
        /// on failure it returns false, and output should be null.
        /// </summary>
        /// <param name="data">Input string data to convert</param>
        /// <param name="escapechar">Character to escape on. Gets passed in so that your function can remove escape characters if it makes sense.</param>
        /// <param name="output">Output object after conversion</param>
        /// <returns>If the conversion was successful.</returns>
        public delegate bool TypeConverter(string data, char escapechar, out object output);
        /// <summary>
        /// A Type Dictionary for determining a data string Data's probable Type.
        /// <para>The TypeDictionary, and the places it's used are probably the most complicated parts of this library, but suffice to say, that the functions here will both test for and perform conversion from a string to a given data Type</para>
        /// <para>They must be placed in order from most restrictive to least restrictive, meaning that for example, integers must be placed before doubles, because if a column works for multiple Types,
        /// it gets set to the first one in the list, and you would never get integers out, as all integers can parse as doubles.</para>
        /// <para>As an aside, note that these are expected to trim their own escape characters if necessary, so that a 6 is not treated the same as "6" (The former would be an int, and the latter would be a string)</para>
        /// <para>The default String, gives an example of trimming escape characters, so that "Hello World" is treated the same as Hello World</para>
        /// </summary>
        public static readonly List<KeyValuePair<Type, TypeConverter>> DefaultTypeDictionary = new List<KeyValuePair<Type, TypeConverter>>()
        {
            {new KeyValuePair<Type,TypeConverter>(typeof(Boolean), (string data, char escapechar, out object output)=>{
                string upperdata = data.PruneEscapeCharacters(escapechar).ToUpper();
                if(data == "0" || upperdata == "F" || upperdata == "N")
                {
                    output = false;
                    return true;
                }
                if(data == "1" || upperdata == "T" || upperdata == "Y")
                {
                    output = true;
                    return true;
                }
                bool toReturn = Boolean.TryParse(data, out Boolean toutput);
                output = toReturn? (object)toutput:null;
                return toReturn;
            })},
            {new KeyValuePair<Type, TypeConverter>(typeof(Int64),(string data, char escapechar, out object output)=>{ bool toReturn =  Int64.TryParse(data, out Int64 toutput); output = toReturn? (object)toutput:null; return toReturn; }) },
            {new KeyValuePair<Type, TypeConverter>(typeof(Double),(string data, char escapechar, out object output)=>{ bool toReturn = Double.TryParse(data, out Double toutput); output = toReturn? (object)toutput:null; return toReturn; }) },
            {new KeyValuePair<Type, TypeConverter>(typeof(DateTime), (string data, char escapechar, out object output)=>{
                bool toReturn = DateTime.TryParseExact(data.PruneEscapeCharacters(escapechar),
                    new string[]{"yyyy-MM-dd HH:mm:ss.FFFFFFFK","yyyy-MM-dd\\THH:mm:ss.FFFFFFFK","yyyy-MM-dd HH:mmK","yyyy-MM-dd\\THH:mmK","yyyy-MM-ddK"},
                    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime toutput);
                output = toReturn? (object)toutput:null; return toReturn;
            })},
            {new KeyValuePair<Type,TypeConverter> (typeof(String), (string data, char escapechar, out object output)=>{output = data.PruneEscapeCharacters(escapechar); return true; })}
        };
        /// <summary>
        /// Any object can be converted to a string, so this is less important than the reverse, but in special cases, the default toString() method is insufficient.
        /// <para>This allows an override of sorts so that while writing CSVs, data is written as desired.</para>
        /// </summary>
        /// <param name="o">The object to convert.</param>
        /// <returns>The string representation of that object.</returns>
        public delegate string SpecialStringConversion(object o);
        /// <summary>
        /// This is a Default String Conversion Dictionary. It handles the special cases
        /// </summary>
        public static readonly Dictionary<Type, SpecialStringConversion> DefaultStringConversionDictionary = new Dictionary<Type, SpecialStringConversion>()
        {
            {typeof(DateTime),(o)=> {return (o == null || DBNull.Value.Equals(o))?"":((DateTime)o).ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK");} },//Without using ISO8601, we can't aways get DateTimes right.
            {typeof(Double),(o)=> {return (o == null || DBNull.Value.Equals(o))?"":((Double)o).ToString("G17");} }//Without using the G17 Format, some doubles fail to roundtrip.
        };
        /// <summary>
        /// Reads a CSV file, separated by a separator, escaped by an escape character into a DataTable, and attempts to guess the Type of each Column based on the typeDictionary which is editable.
        /// <para>The defaults are appropriate for most csv files, but if you have custom data types you want to read, you'll have to make a typeDictionary. Use the default for guidance.</para>
        /// <para>Does not name the table, but will not change the name if a name exists.</para>
        /// <para>Can also append data to an existing table if the columns and datatypes match.</para>
        /// </summary>
        /// <param name="baseTable">Table to operate on.</param>
        /// <param name="input">TextReader to use as input.</param>
        /// <param name="separator">Character to separate on. Usually a comma.</param>
        /// <param name="headers">Is the first line a header line?</param>
        /// <param name="nullValues">Values to treat as null. This defaults to only situations where there are no characters between two separators.</param>
        /// <param name="typeDictionary">An ordered dictionary of types with methods for processing them from the underlying data. See the DefaultTypeDictionary for more info.</param>
        /// <param name="escapechar">Character to escape on. Usually a Double Quote " but may be a single ' or back quote `.</param>
        /// <param name="startLine">How many lines to skip? This will skip after reading the header(if there is one), but before reading the data.</param>
        /// <param name="lineLimit">Maximum data lines to read. (Not counting the headerline, nor newlines within string values.)</param>
        /// <returns>Number of lines actually read.</returns>
        public static DataTable ReadCSV(TextReader input, char separator = ',', bool headers = true, string[] nullValues = null, List<KeyValuePair<Type, TypeConverter>> typeDictionary = null, char escapechar = '"', int startLine = 0, int? lineLimit = null)
        {
            DataTable baseTable = new DataTable();
            if (nullValues == null) { nullValues = new string[] { "" }; }
            if (typeDictionary == null) { typeDictionary = DefaultTypeDictionary; }
            Dictionary<Type, TypeConverter> _typeDictionary = new Dictionary<Type, TypeConverter>();
            Type[] typeDictionaryArray = new Type[typeDictionary.Count];
            for (int i = 0; i < typeDictionary.Count; i++)
            {
                typeDictionaryArray[i] = typeDictionary[i].Key;
            }
            List<object[]> readData = new List<object[]>();
            bool completedReading = false;
            string firstLine = input.AdvancedReadLine(escapechar);
            if (firstLine == null)
            {
                return null;
            }
            string[] firstLineArray = firstLine.AdvancedSplit(separator, nullValues, escapechar);
            int columns = firstLineArray.Length;
            int linesRead = 0;
            if (!headers && startLine <= 0 && lineLimit != 0)
            {
                readData.Add(firstLineArray);
                linesRead = 1;
            }
            bool presetDataTypes = false;
            if (baseTable.Columns.Count == 0)
            {
                for (int i = 0; i < columns; i++)
                {
                    baseTable.Columns.Add(headers ? firstLineArray[i] : null);
                }
            }
            else if (baseTable.Rows.Count > 0)
            {
                presetDataTypes = true;
            }
            int DataTypeCount = typeDictionaryArray.Count();
            if (presetDataTypes)
            {
                DataTypeCount = 1;
            }
            Task<object[]>[,] DataPossibilities = new Task<object[]>[columns, DataTypeCount];
            for (int i = 0; i < columns; i++)
            {
                if (presetDataTypes)
                {
                    DataPossibilities[i, 0] = new Task<object[]>((object o) => ProcessStringsAsType(ref readData, ref completedReading, o), new object[] { i, _typeDictionary[baseTable.Columns[i].DataType], escapechar });
                    DataPossibilities[i, 0].Start();
                }
                else
                {
                    for (int j = 0; j < DataTypeCount; j++)
                    {
                        DataPossibilities[i, j] = new Task<object[]>((object o) => ProcessStringsAsType(ref readData, ref completedReading, o), new object[] { i, _typeDictionary[typeDictionaryArray[j]], escapechar });
                        DataPossibilities[i, j].Start();
                    }
                }
            }
            for (int i = 0; i < startLine; i++)
            {
                input.AdvancedReadLine();
            }
            string currLine;
            while ((lineLimit == null || linesRead++ < lineLimit) && (currLine = input.AdvancedReadLine(escapechar)) != null)
            {
                string[] currLineArray = currLine.AdvancedSplit(separator, null, escapechar);
                lock (readData) { readData.Add(currLineArray); }
            }
            lock (readData) { completedReading = true; }
            object[][] dataToAdd = new object[columns][];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < DataTypeCount; j++)
                {
                    DataPossibilities[i, j].Wait();
                }
            }
            readData = null;//saves some memory
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < DataTypeCount; j++)
                {
                    if (DataPossibilities[i, j].IsCompleted && DataPossibilities[i, j].Result != null)
                    {
                        dataToAdd[i] = DataPossibilities[i, j].Result;
                        if (!presetDataTypes)
                        {
                            baseTable.Columns[i].DataType = typeDictionaryArray[j];
                        }
                        break;
                    }
                }
            }
            if (dataToAdd.Length > 0 && dataToAdd[0] != null)
            {
                for (int i = 0; i < dataToAdd[0].Length; i++)
                {
                    object[] toAdd = new object[columns];
                    for (int j = 0; j < columns; j++)
                    {
                        toAdd[j] = dataToAdd[j][i];
                    }
                    baseTable.Rows.Add(toAdd);
                }
            }
            return baseTable;
        }
        /// <summary>
        /// Multithread function that does the actual processing on objects loaded in readData. If it fails at any point, it returns null.
        /// </summary>
        /// <param name="readData">Data to attempt to read</param>
        /// <param name="completedReading">Boolean to test if the parent function is done updating the readData function.</param>
        /// <param name="state">Has 3 parameters encapsulated in it.<para>[0] The column number we're responsible for.</para><para>[1] The TypeConverter to process strings with.</para><para>[2]The escape character to pass into the TypeConverter.</para></param>
        /// <returns>An array of objects that are the converted string, unless we failed somewhere, in which case we're null.</returns>
        private static object[] ProcessStringsAsType(ref List<object[]> readData, ref bool completedReading, object state)
        {
            int columnNumber = (int)((object[])state)[0];
            TypeConverter tc = (TypeConverter)((object[])state)[1];
            char escapechar = (char)((object[])state)[2];

            int currentPos = 0;
            bool _completedReading = false;
            string[] toParse = null;
            List<object> toReturn = new List<object>();
            while (!_completedReading)
            {
                lock (readData)
                {
                    _completedReading = completedReading;
                    if (readData.Count > currentPos)
                    {
                        toParse = new string[readData.Count - currentPos];
                        for (int k = 0; k < toParse.Length; k++)
                        {
                            toParse[k] = (string)readData[currentPos++][columnNumber];
                        }
                    }
                    else
                    {
                        toParse = null;
                    }
                }
                if (toParse != null)
                {
                    for (int k = 0; k < toParse.Length; k++)
                    {
                        if (toParse[k] == null)
                        {
                            toReturn.Add(null);
                            continue;
                        }
                        if (tc(toParse[k], escapechar, out object output))
                        {
                            toReturn.Add(output);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            return toReturn.ToArray();
        }
        /// <summary>
        /// Writes a DataTable back out to a CSV.
        /// <para>specialToStrings is a special Dictionary for DataTypes that have a more complicated way of converting to Strings than the default toString().</para>
        /// <para>The most obvious example (Which is included as ExampleStringConversionDictionary) is that of a DateTime, where you want a specific format.</para>
        /// <para>If you don't have any special situations that you can think of, I recommend at least using the ExampleStringConversionDictionary</para>
        /// </summary>
        /// <param name="baseTable">The table to write.</param>
        /// <param name="output">TextWriter object to write to.</param>
        /// <param name="specialToStrings">A list of special Type cases more complicated than toString() This defaults to the DefaultStringConversionDictionary with special cases for DateTime and Double</param>
        /// <param name="separator">Character to separate on. Usually a comma.</param>
        /// <param name="headers">Should we first write a header line?</param>
        /// <param name="escapechar">Character to escape on. Usually a Double Quote " but may be a single ' or back quote `.</param>
        public static void WriteCSV(this DataTable baseTable, TextWriter output, Dictionary<Type, SpecialStringConversion> specialToStrings = null, char separator = ',', bool headers = true, char escapechar = '"')
        {
            if (specialToStrings == null)
            {
                specialToStrings = DefaultStringConversionDictionary;
            }
            SpecialStringConversion[] TypeConversion = new SpecialStringConversion[baseTable.Columns.Count];
            SpecialStringConversion defaultConversion = new SpecialStringConversion((o) =>
            {
                if (o == null || DBNull.Value.Equals(o))
                {
                    return "";
                }
                string s = o.ToString();
                if (o is String || s.Contains(separator) || s.Contains("\n") || s.Contains("\r") || s.Contains(escapechar))
                {
                    return s.EscapeString(escapechar);
                }
                return s;
            });
            String[] LineToWrite = new string[baseTable.Columns.Count];
            for (int j = 0; j < baseTable.Columns.Count; j++)
            {
                TypeConversion[j] = specialToStrings.ContainsKey(baseTable.Columns[j].DataType) ? specialToStrings[baseTable.Columns[j].DataType] : defaultConversion;
                if (headers)
                {
                    LineToWrite[j] = baseTable.Columns[j].ColumnName.EscapeString(escapechar);
                }
            }
            if (headers) { output.WriteLine(string.Join(separator.ToString(), LineToWrite)); }
            for (int i = 0; i < baseTable.Rows.Count; i++)
            {
                for (int j = 0; j < baseTable.Columns.Count; j++)
                {
                    LineToWrite[j] = TypeConversion[j](baseTable.Rows[i][j]);
                }
                output.WriteLine(string.Join(separator.ToString(), LineToWrite));
            }
        }
        /// <summary>
        /// Suggested to call from certain custom TypeConverters to remove external escape characters and replace double escaped escape characters with just one copy.
        /// </summary>
        /// <param name="input">String to prune escape characters from.</param>
        /// <param name="escapechar"></param>
        /// <returns></returns>
        public static string PruneEscapeCharacters(this string input, char escapechar)
        {
            int firstpos;
            if ((firstpos = input.IndexOf(escapechar)) > -1)
            {
                input = input.Remove(firstpos, 1);
                input = input.Remove(input.LastIndexOf(escapechar), 1);
            }
            input = input.Replace(new string(escapechar, 2), new string(escapechar, 1));
            return input;
        }
        /// <summary>
        /// Escapes the String prepatory to writing to a CSV File
        /// </summary>
        /// <param name="input">String to escape.</param>
        /// <param name="escapechar">Character to escape on. Usually a Double Quote " but may be a single ' or back quote `.</param>
        /// <returns></returns>
        public static string EscapeString(this string input, char escapechar)
        {
            return escapechar + input.Replace(new string(escapechar, 1), new string(escapechar, 2)) + escapechar;
        }
        /// <summary>
        /// Reads a line from the TextReader, but ignores newlines that have been escaped.
        /// </summary>
        /// <param name="input">TextReader to use as input.</param>
        /// <param name="escapechar">Character to escape on. Usually a Double Quote " but may be a single ' or back quote `.</param>
        /// <returns>Line from TextReader</returns>
        public static string AdvancedReadLine(this TextReader input, char escapechar = '"')
        {
            string toReturn = input.ReadLine();
            if (toReturn == null)
            {
                return null;
            }
            int currCount = toReturn.Count(c => c == escapechar);
            while (currCount % 2 != 0)
            {
                string nextLine = input.ReadLine();
                if (nextLine == null)
                {
                    return toReturn;
                }
                toReturn += "\r\n" + nextLine;
                currCount += nextLine.Count(c => c == escapechar);
            }
            return toReturn;
        }
        /// <summary>
        /// Like String.Split, but ignores separators that have been escaped, and returns certain values as null.
        /// </summary>
        /// <param name="input">String to split</param>
        /// <param name="separator">Character to separate on. Usually a comma.</param>
        /// <param name="nullValues">Values to treat as null. This defaults to only situations where there are no characters between two separators.</param>
        /// <param name="escapechar">Character to escape on. Usually a Double Quote " but may be a single ' or back quote `.</param>
        /// <returns>Array of strings that have been singled out like String.Split would.</returns>
        public static string[] AdvancedSplit(this string input, char separator = ',', string[] nullValues = null, char escapechar = '"')
        {
            if (nullValues == null) { nullValues = nullValues = new string[] { "" }; }
            string[] naiveArray = input.Split(separator);
            List<string> parsedLine = new List<string>();
            for (int i = 0; i < naiveArray.Length; i++)
            {
                string toAdd = naiveArray[i];
                int currCount = toAdd.Count(c => c == escapechar);
                while (currCount % 2 != 0 && i < naiveArray.Length - 1)
                {
                    toAdd += "," + naiveArray[++i];
                    currCount += naiveArray[i].Count(c => c == escapechar);
                }
                if (nullValues.Contains(toAdd))
                {
                    parsedLine.Add(null);
                    continue;
                }
                parsedLine.Add(toAdd);
            }
            return parsedLine.ToArray();
        }
    }
}
