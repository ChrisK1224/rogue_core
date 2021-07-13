using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilesAndFolders
{/// <summary>
/// This class has various common requests with text strings or text files such as reading a file and returning the lines or splitting a long string 
/// into separate lines by new line
/// </summary>
   public static class TXT
    {
        /// <summary>
        /// This method takes in a  full file path and returns the text in the file as a list of strings
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> getLinesFromText(string fileName)
        {
            int counter = 0;
            string line;

            List<string> fullText = new List<string>();
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                fullText.Add(line);
                counter++;
            }
            file.Close();

            return fullText;
        }

        /// <summary>
        /// This method takes in a string and return the string split into a list of string by each new line
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> getLinesFromPlanText(string input)
        {
            List<string> list = new List<string>(Regex.Split(input, Environment.NewLine));
            return list;
        }
    }
}
