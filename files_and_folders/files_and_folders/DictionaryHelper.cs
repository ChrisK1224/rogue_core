using System;
using System.Collections.Generic;
using System.Text;

namespace files_and_folders
{
   
    public static class DictionaryHelper
    {
        public static vtype TryFindReturn<ktype, vtype>(this Dictionary<ktype, vtype> dict, ktype key) where vtype : new()
            {
                vtype ret;
                if (dict.TryGetValue(key, out ret))
                {
                    return ret;
                }
                else
                {
                    return new vtype();
                }
            }
        public static List<vType> FindAddIfNotFound<vType>(this Dictionary<int, List<vType>> dict, int key)
            {
                List<vType> ret;
                if (dict.TryGetValue(key, out ret))
                {
                    return ret;
                }
                else
                {
                    List<vType> newList = new List<vType>();
                    dict.Add(key, newList);
                    return newList;
                }
            }
        public static void FindAddIfNotFound<kType, vType>(this Dictionary<kType, vType> dict, kType key, vType val)
        {
            vType ret;
            if (!dict.TryGetValue(key, out ret))
            {
                dict.Add(key, val);
            }
        }
        public static List<vType> FindAddIfNotFound<kType, vType>(this Dictionary<kType, List<vType>> dict, kType key, vType val)
        {
            List<vType> ret;
            if (dict.TryGetValue(key, out ret))
            {
                ret.Add(val);
                return ret;
            }
            else
            {
                List<vType> newList = new List<vType>();
                dict.Add(key, newList);
                newList.Add(val);
                return newList;
            }
        }
        public static List<vType> FindAddIfNotFound<vType>(this Dictionary<long, List<vType>> dict, int key)
        {
            List<vType> ret;
            if (dict.TryGetValue(key, out ret))
            {
                return ret;
            }
            else
            {
                List<vType> newList = new List<vType>();
                dict.Add(key, newList);
                return newList;
            }
        }
        public static List<string> FindAddIfNotFound<vType>(this Dictionary<vType, List<string>> dict, vType key)
        {
            List<string> ret;
            if (dict.TryGetValue(key, out ret))
            {
                return ret;
            }
            else
            {
                List<string> newList = new List<string>();
                dict.Add(key, newList);
                return newList;
            }
        }
        public static vType FindAddIfNotFound<vType>(this Dictionary<long, vType> dict, int key) where vType : new()
        {
            vType ret;
            if (dict.TryGetValue(key, out ret))
            {
                return ret;
            }
            else
            {
                vType newList = new vType();
                dict.Add(key, newList);
                return newList;
            }
        }
        public static List<vType> FindAddIfNotFound<vType>(this Dictionary<string, List<vType>> dict, string key)
        {
            List<vType> ret;
            if (dict.TryGetValue(key, out ret))
            {
                return ret;
            }
            else
            {
                List<vType> newList = new List<vType>();
                dict.Add(key, newList);
                return newList;
            }
        }
        public static Dictionary<vType, xType> FindAddIfNotFound<aType, vType, xType>(this Dictionary<aType, Dictionary<vType, xType>> dict, aType key)
        {
            Dictionary<vType, xType> ret;
            if (dict.TryGetValue(key, out ret))
            {
                return ret;
            }
            else
            {
                Dictionary<vType, xType> newList = new Dictionary<vType, xType>();
                dict.Add(key, newList);
                return newList;
            }
        }
        public static void FindChangeIfNotFound<vType>(this Dictionary<int, vType> dict, int key, vType val)
        {
            vType ret;
            if (dict.TryGetValue(key, out ret))
            {
                dict[key] = val;
            }
            else
            {
                //vType newList = n
                dict.Add(key, val);
            }
        }
        public static void FindChangeIfNotFound<vType>(this Dictionary<string, vType> dict, string key, vType val)
        {
            vType ret;
            if (dict.TryGetValue(key, out ret))
            {
                dict[key] = val;
            }
            else
            {
                //vType newList = n
                dict.Add(key, val);
            }
        }
        //public static void FindChangeIfNotFound<vType>(this List<vType> dict, string key, vType val)
        //{
        //    vType ret;
        //    if (dict.TryGetValue(key, out ret))
        //    {
        //        dict[key] = val;
        //    }
        //    else
        //    {
        //        //vType newList = n
        //        dict.Add(key, val);
        //    }
        //}
    }
    
}
