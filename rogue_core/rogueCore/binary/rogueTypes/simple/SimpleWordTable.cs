using rogue_core.rogueCore.binary.rogueTypes;
using rogue_core.rogueCore.binary.word;
using rogue_core.rogueCore.binary.word.complex;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace rogue_core.rogueCore.binary
{
    public class SimpleWordTable : RogueTypeTable<SimpleWordValue>
    {
        internal const string lineSeparatorEncoded = "@RTAG";
        protected override Encoding encodingType { get { return Encoding.BigEndianUnicode; } }
        public SimpleWordTable() : base("SimpleString")
        {
            //AddValue(iorecordRowOIDStr);
            
            //if (!File.Exists(filePath))
            //    File.Create(filePath).Close();
            //byte[] bytes = File.ReadAllBytes(filePath);
            //values = (string[])ConvertByteArrayToObject(bytes);
            
            //byte[] bytes = File.ReadAllBytes(filePath);
            //values = (string[])ConvertByteArrayToObject(bytes);
            //foreach(string val in values){

            //}
            //var index = 0;
            //foreach (var row in StreamRows())
            //{
            //    AddValue(row);
            //    //index++;
            //}
            //wtch.Stop();
            //var millis = wtch.ElapsedMilliseconds;
        }
        protected override ISimpleValueReference NewReadValue(string line)
        {
            return new SimpleWordValue(line);
        }
        protected override ISimpleValueReference NewWriteValue(RowID rowID, string value)
        {
            return new SimpleWordValue(rowID, value);
        }
        //static IORecord_OID_ROW_STR iorecordRowOIDStr = new IORecord_OID_ROW_STR();
        //void AddValue(SimpleWordValue word)
        //{
        //    values.Add(word);
        //    readLookup.Add(word.valueID.ToInt(), values.Count - 1);
        //    //*Temp check for unrecognized char
        //    if (!writeLookup.ContainsKey(word.value))
        //    {
        //        writeLookup.Add(word.value, values.Count - 1);
        //    }
        //    else
        //    {
        //        string bll = word.value;
        //    }
        //}
        //SimpleWordValue WriteValue(string value)
        //{
        //    //value = value.Replace("�", "").Replace(@"\u2022", "").Replace("�","");
        //    DecodedRowID rowID = IDIncrement.NextID();
        //    SimpleWordValue val = new SimpleWordValue(rowID, value);
        //    using (var stream = new FileStream(filePath, FileMode.Append))
        //    {
        //        var bytes = Encoding.BigEndianUnicode.GetBytes(val.WriteValue() + lineSeparatorChar);
        //        stream.Write(bytes, 0, bytes.Length);
        //    }           
        //    //File.AppendAllText(filePath, val.WriteValue() + lineSeparatorChar, Encoding.BigEndianUnicode);
        //    values.Add(val);
        //    if (writeLookup.ContainsKey(value))
        //    {
        //        string ll = "EWH";
        //    }
        //    writeLookup.Add(value, values.Count-1);
        //    readLookup.Add(val.valueID.ToInt(), values.Count-1);

        //    //using (BinaryWriter writer = new BinaryWriter(new FileStream(filePath, FileMode.Append)))
        //    //{
        //    //    //val = new SimpleStringValue(rowID, value);
        //    //    //byte[] byts = BinaryReadWrite.ToByteSingle<SimpleStringValue>(val);
        //    //    //byte[] byts = ConvertObjectToByteArray(value);
        //    //    //File.WriteAllBytes(file, ba);                
        //    //    //writer.Write(byts);
        //    //    writer.Write((Int32)rowID.ToInt());
        //    //    writer.Write(value);
        //    //    //writer.Write((Int64)rowID);
        //    //    //writer.Write(value.Length);
        //    //    //writer.Write(value);
        //    //}
        //    return val;
        //}
        //public SimpleWordValue GetValue(string strValue)
        //{
        //    int foundID;
        //    //string checkVal = strValue.Replace(",", "@RCOMMA").Replace(":", "@ROGUECOLON").Replace(";", "@ROGUESEMICOLON").Replace(Environment.NewLine, "@RNEWLINE").Replace("|", "@RBAR").Replace("\n", "@RNEWLINE");
        //    if (writeLookup.TryGetValue(strValue, out foundID))
        //    {
        //        return values[foundID];
        //    }
        //    else
        //    {
        //        return WriteValue(strValue);
        //    }
        //}
        //public SimpleWordValue GetValue(long rowID)
        //{
        //    return values[readLookup[rowID]];
        //}
        //IEnumerable<SimpleWordValue> StreamRows()
        //{
        //    foreach(var line in File.ReadAllLines(filePath, Encoding.BigEndianUnicode))
        //    {
        //        yield return new SimpleWordValue(line);
        //    }
        //    //byte[] chunk;
        //    //int CHUNK_SIZE = 100000;
        //    //BufferedStream buff = new BufferedStream(File.Open(filePath, FileMode.Open), ((int)new FileInfo(filePath).Length));
        //    //BinaryReader br = new BinaryReader(buff, Encoding.ASCII);
        //    //BinaryReader br = new BinaryReader(new FileStream(filePath, FileMode.Open));
        //    //while ((chunk = br.ReadBytes(CHUNK_SIZE)).Length > 0)
        //    //{
        //    //    var id = br.ReadInt32();
        //    //    var value = br.ReadString();
        //    //    yield return new SimpleStringValue(new UnKnownID(id), value);
        //    //    //var pairs = BinaryReadWrite.FromByteArray<SimpleStringValue>(chunk);
        //    //    //foreach (var pair in pairs)
        //    //    //{
        //    //    //    yield return pair;
        //    //    //}
        //    //}
        //    //var wch = new Stopwatch();
        //    //wch.Start();
        //    //while (br.PeekChar() != -1)
        //    //{
        //    //    var id = br.ReadInt32();
        //    //    var value = br.ReadString();
        //    //    //Console.WriteLine(id + "," + value);
        //    //    yield return new SimpleStringValue(new UnKnownID(id), value);
        //    //}
        //    //br.Close();
        //    //wch.Stop();
        //    //var bll = wch.ElapsedMilliseconds;
        //}
        //public static byte[] ConvertObjectToByteArray(object ob)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    MemoryStream ms = new MemoryStream();
        //    bf.Serialize(ms, ob);
        //    return ms.ToArray();
        //}

        //public static object ConvertByteArrayToObject(byte[] ba)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    Stream stream = new MemoryStream(ba);
        //    return bf.Deserialize(stream);
        //}
        //public static void Test()
        //{
        //    string file = @"Y:\RogueDatabase\Pure\Shared\test.txt";
        //    //File.Create(file);
        //    string[] ob1 = new string[3] { "hey", "yo", "whatup" };
        //    byte[] ba = ConvertObjectToByteArray(ob1);
        //    File.WriteAllBytes(file, ba);

        //    byte[] ba2 = File.ReadAllBytes(file);
        //    object ob2 = ConvertByteArrayToObject(ba);
        //    string ll = "SDF";
        //}
        //private void write()
        //{
        //    string file = "C:\\file";
        //    string string1 = "John|Gold Membership|RegisterDate=2013-12-13";
        //    byte[] ba = Encoding.UTF8.GetBytes(string1);
        //    File.WriteAllBytes(file, ba);

        //    byte[] ba2 = File.ReadAllBytes(file);
        //    string string2 = Encoding.UTF8.GetString(ba2);
        //}
        //public static string[] IntPtrToStringArrayAnsi(IntPtr ptr)
        //{
        //    var lst = new List<string>();
        //    do
        //    {
        //        lst.Add(Marshal.PtrToStringAnsi(ptr));

        //        while (Marshal.ReadByte(ptr) != 0)
        //        {
        //            ptr = IntPtr.Add(ptr, 1);
        //        }

        //        ptr = IntPtr.Add(ptr, 1);
        //    }
        //    while (Marshal.ReadByte(ptr) != 0);

        //    // See comment of @zneak
        //    //if (lst.Count == 1 && lst[0] == string.Empty)
        //    //{
        //    //    return new string[0];
        //    //}

        //    return lst.ToArray();
        //}
    }
   
    //class IORecord_OID_ROW_STR : ISimpleValueReference
    //{
    //    public string value { get { return "IORecordRow_OID"; } }
    //    public int complexWordCount { get { return 1; } }
    //    public byte dataTypeID { get { return BinaryDataPair.dtSimpleString; } }
    //    public long valueID { get { return -5120; } }
    //    public string StringValue(ComplexWordTable complexTbl)
    //    {
    //        return value;
    //    }
    //    public string WriteValue()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}