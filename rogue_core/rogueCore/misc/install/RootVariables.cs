using System;
using System.IO;

namespace rogue_core.rogueCore.install
{
    public static class RootVariables
    {
        //static string rootBase = "/home/chris/Development/RogueData/";
        //public static String rootPath = rootBase + "Pure";
        //public static String insertsScriptPath = rootBase + "inserts.hql";

        // public static String rootPath = Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "Pure";
        // public static String insertsScriptPath = Path.GetFullPath(Directory.GetCurrentDirectory()) + Path.DirectorySeparatorChar + "inserts.hql";
        //public static string testPath = Environment.CurrentDirectory;
#if DEBUG

        //static string path = Environment.CurrentDirectory;
        //public static string basePath = "Y:\\RogueDatabase";
        public static string basePath = "C:\\Users\\chris\\Documents\\RogueDataBase";
        public static string rootPath = basePath + Path.DirectorySeparatorChar + "Pure";
        public static string sharedDataPath = rootPath + Path.DirectorySeparatorChar + "Shared";
        public static string incrementIDPath = rootPath + Path.DirectorySeparatorChar + "id.bin";
        //public static String rootPath = path + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar  + "MyDatabase" + Path.DirectorySeparatorChar + "Pure";
        //#elif CONFIG = "ConsoleDebug"
        //        static string path = "Y:\\RogueDatabase" + Path.DirectorySeparatorChar + "Pure";
#else
        //static string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "wwwroot";
        //static string path = "Y:\\RogueDatabase" + Path.DirectorySeparatorChar + "Pure";
        //public static String rootPath = path;
         static string path = Environment.CurrentDirectory;
        public static String rootPath = path + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar  + "MyDatabase" + Path.DirectorySeparatorChar + "Pure";
#endif



        public static String insertsScriptPath = rootPath + Path.DirectorySeparatorChar + "MyDatabase" + Path.DirectorySeparatorChar + "inserts.hql";
        public static string archivePath = rootPath + Path.DirectorySeparatorChar + "Archives";
        //BRING BACK
        //public static String rootPath = "Y:\\RogueDatabase" + Path.DirectorySeparatorChar + "Pure";
        //public static String insertsScriptPath = "Y:\\RogueDatabase" + Path.DirectorySeparatorChar + "inserts.hql";
    }
}