using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using rogueCore.streaming;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.apiV3;

namespace rogueCore.api.formats.xml
{
    class xml_stream : StreamInterpreter
    {
        //write_funnel ths_funnel;
        XmlReader reader;
        internal xml_stream(APIConnect api_data, RogueDatabase<DataRowID> database) : base("FIX", "")
        {

        }
       /* internal xml_stream(api_connect api_data, RogueDatabase<DataRowID> database) : base(database)
        {
            //ths_funnel = new write_funnel(database);
            //*CHANGEBACK
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
          | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12;
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead("https://www.quandl.com/api/v3/datasets/FRED/NROUST.xml?api_key=x8LRTeBsGSxjdXjzcfJM");          
            reader = XmlReader.Create(stream);
            //reader = XmlReader.Create(ths_tbl.ths_IdableItem.file_path);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        openParents.Add(reader.Name);
                        //*If self closing then add an empty value
                        if (reader.IsEmptyElement)
                        {
                            String keyName = openParents[openParents.Count - 1];
                            openParents.RemoveAt(openParents.Count-1);
                            sync_value(keyName, reader.Value);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        //if this elements depth and name match the last unclosed parent then closed container
                        //if(reader.Name.Equals(ths_funnel.unclosed_parents[ths_funnel.unclosed_parents.Count-1].name) && reader.Depth == ths_funnel.unclosed_parents[ths_funnel.unclosed_parents.Count - 1].depth)
                        if (reader.Name.Equals(openParents[openParents.Count - 1]))
                        {
                            close_last_container();
                        }                                            
                        break;                        
                    case XmlNodeType.Text:
                        String keyNameTxt = openParents[openParents.Count - 1];
                        openParents.RemoveAt(openParents.Count-1);
                        sync_value(keyNameTxt, reader.Value);
                        break;
                }
            }
            //*Close any leftover containers after last element
            foreach(String ths_close_container in openParents)
            {
                close_last_container();
            }
            close_funnel();
        }*/
    }
}
