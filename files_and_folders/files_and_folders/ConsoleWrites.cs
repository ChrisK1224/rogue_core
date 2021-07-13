using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FilesAndFolders
{
     public static class ConsoleWrites
    {
        public static void writeErrorLine(string text, [CallerMemberName] string callerName = "", Boolean shouldStop = false)
        {
            string className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            // Get call stack
            StackTrace stackTrace = new StackTrace();
            // Get calling method name
            Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);

            Console.WriteLine(callerName + "called me.");

            if (shouldStop == true)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void writeProgressLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
        }

        public static void writeSuccessLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
        }

        public static void writeLogLine(string text,  [CallerMemberName] string memberName = "",[CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
{
    // we'll just use a simple Console write for now    
    Console.WriteLine("{0}({1}):{2} - {3}", fileName, lineNumber, memberName, text);

    string className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            // Get call stack
            StackTrace stackTrace = new StackTrace();
            // Get calling method name
            Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);

            Console.WriteLine(memberName + "called me.");
            Console.WriteLine("myClaassNme:" + className);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
        }
    }
}
