using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FilesAndFolders
{
    public class DirectoryHelper
    {
        public static Boolean createFileCheckExists(string thsFile)
        {
            if (!Directory.Exists(Path.GetDirectoryName(thsFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(thsFile));
            }
            if (!File.Exists(thsFile))
            {
                File.Create(thsFile).Close();
                return false;
            }
            else
            {
                return true;
            }
        }
        public static Boolean createFolderCheckExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                return false;
            }
            else
            {
                return true;
            }
        }
        public static List<string> getFileListFromFolder(string folderPath)
        {
            string[] fileEntries = Directory.GetFiles(folderPath);
            return fileEntries.ToList();
        }
        public static string createFileNotExists(string thsFile)
        {
            if (!Directory.Exists(Path.GetDirectoryName(thsFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(thsFile));
            }
            if (!File.Exists(thsFile))
            {
                File.Create(thsFile).Close();
            }
            return thsFile;
        }
        public static bool createFileCheckNotExists(string thsFile)
        {
            if (!Directory.Exists(Path.GetDirectoryName(thsFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(thsFile));
            }
            if (!File.Exists(thsFile))
            {
                File.Create(thsFile).Close();
                return true; 
            }
            else
            {
                return false;
            }
        }
        public static void createFolderNotExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            //return folderPath.ToString();
        }
        public static Boolean isFileEmpty(string thsFilePath)
        {
            StreamReader sr = new StreamReader(thsFilePath); 
            if(sr.ReadLine() == null)
            {
                sr.Close();
                sr.Dispose();
                return true;
            }
            else
            {
                sr.Close();
                sr.Dispose();
                return false;
            }
            //while ((line = sr.ReadLine()) != null)
            //{ 
            //    return true;
            //}      
        }
        public static List<string> getFoldersContainingKey(string base_path, string containsKey, Boolean retFullDirectory = false, Boolean checkAlldirectories = false)
        {
            string[] dirs;
            containsKey = "*" + containsKey + "*";
           
            if(checkAlldirectories == false)
            {
                dirs = Directory.GetDirectories(base_path, containsKey, SearchOption.TopDirectoryOnly);
            }
            else
            {
                dirs = Directory.GetDirectories(base_path, containsKey, SearchOption.AllDirectories);
            }
            List<string> singleDirNames;
            if (retFullDirectory == true)
            {
                singleDirNames = dirs.Select(x => Path.GetDirectoryName(x)).Distinct().ToList();
            }
            else
            {
                singleDirNames = dirs.Select(x => Path.GetFileName(x)).Distinct().ToList();
            }
            return singleDirNames;
        }
        public static List<string> getLastFoldersContainingKey(string base_path, string containsKey)
        {
            List<string> retList = new List<string>();
            foreach (string folder in Directory.GetDirectories(base_path,"*", SearchOption.TopDirectoryOnly))
            {
                if (Path.GetFileName(folder).Contains(containsKey))
                {
                    retList.Add(folder);
                }
            }
            return retList;
        }
    }
}
