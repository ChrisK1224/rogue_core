using System;
using System.IO;
using FilesAndFolders;

namespace rogue_core.rogueCore.id
{
    public static class IDIncrement
    {
        //static String path = install.RootVariables.rootPath + Path.DirectorySeparatorChar + "id.bin";
        //static String insertPath = install.RootVariables.rootPath + "inserts.hql";
        public static void InstallStart()
        {
            DirectoryHelper.createFileCheckExists(install.RootVariables.incrementIDPath);
            //DirectoryHelper.createFileCheckExists(insertPath);
            write_id_bin(0);
        }
        public static int NextID()
        {
            int newId = read_id_bin() +1;
            write_id_bin(newId);
            return newId;
        }
        static void write_id_bin(long last_id)
        {
            using (BinaryWriter b = new BinaryWriter(
            File.Open(install.RootVariables.incrementIDPath, FileMode.Create)))
            {
                // Use foreach and write all 12 integers.
                b.Write(last_id);
            }
        }
        static int read_id_bin()
        {
            //*This only checks if the file is empty. if empty assu
            using (BinaryReader b = new BinaryReader(File.Open(install.RootVariables.incrementIDPath, FileMode.Open)))
            {
                return (b.ReadInt32());
            }
        }
        public static void PresetID(int lastID)
        {
            write_id_bin(lastID);
        }
    }
}