using System.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using rogueCore.streaming;
using rogueCore.api;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.id.rogueID;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace rogueCore.apiV3.formats.json
{
    public class jsonTester : StreamInterpreter
    {
        List<JsonToken> jsonItems = new List<JsonToken>();
        Boolean inArrayContainer = false;
        JsonToken lastJsonItem { get { return jsonItems.Count > 1 ? jsonItems[jsonItems.Count - 1] : JsonToken.StartObject; } }
        public jsonTester(string filePath, string default_nm, string dbId) : base(dbId, default_nm)
        {           
            RunJsonInsert(new StreamReader(filePath), default_nm);
        }
        public jsonTester(APIConnect thsAPIConnect, Dictionary<string,string> topTableConstants ) : base(thsAPIConnect.apiDB, topTableConstants, thsAPIConnect.source_NM)
        {
            //this.topTableConst = topTableConstants;
            //lastPropertyName = thsAPIConnect.segment_NM;
            //this.thsAPIConnect = new APIConnect("","");
            //this.database = new RogueDatabase<DataRowID>(6247);
            // APIConnect ths_api_connect = new APIConnect("","");
            //var token = JToken.Parse(full_json_testTwo());
            /* String blah = token.SelectToken("[0].id_str").Value<string>();
             String blah2 = token.SelectToken("[0].entities.hashtags[0].text").Value<string>();
             token["[0].entities.hashtags[0].indices"].ToObject<List<List<string>>>();
             JObject obj = JObject.Parse(json);
             JToken token = obj["car"]["type"][0]["sedan"]["make"];*/
            //Console.WriteLine(token.Path + " -> " + token.ToString());
            //JsonTextReader reader = new JsonTextReader(new StringReader(full_json_testTwo()));
            //string test = thsAPIConnect.stream_read.ReadToEnd();
            //JsonTextReader reader = new JsonTextReader(thsAPIConnect.stream_read);
            RunJsonInsert(thsAPIConnect.stream_read, thsAPIConnect.segment_NM);
            //while (reader.Read())
            //{
            //    switch (reader.TokenType)
            //    {
            //        case JsonToken.StartArray:
            //            //Console.WriteLine("Open Container: " + lastPropertyName);
            //            //propertyNames.Add(lastPropertyName);
            //            //*MIGHT NEED TO BRING BACK
            //            //parentNames.Add(lastPropertyName);
            //            break;
            //        case JsonToken.StartObject:
            //            //Console.WriteLine("Open Container: " + lastPropertyName);
            //            //propertyNames.Add(lastPropertyName);
            //            //if (!(((JObject)reader.Value).Parent is JArray)){
            //            parentNames.Add(lastPropertyName);
            //            //base.NewTable(lastPropertyName);
            //            //}                        
            //            var tmr = new Stopwatch();
            //            tmr.Start();
            //            NewRow(lastPropertyName);
            //            tmr.Stop();
            //            var tmrBlah = tmr.ElapsedMilliseconds;
            //            break;
            //        case JsonToken.PropertyName:
            //            lastPropertyName = reader.Value.ToString();
            //            //propertyNames.Add(reader.Value.ToString());
            //            break;
            //        case JsonToken.String:
            //        case JsonToken.Boolean:
            //        case JsonToken.Integer:
            //        case JsonToken.Date:
            //        case JsonToken.Float:
            //            String colName = lastPropertyName;
            //            if (lastJsonItem != JsonToken.PropertyName)
            //            {
            //                if (lastJsonItem == JsonToken.StartArray)
            //                {
            //                    NewRow(lastPropertyName);
            //                    inArrayContainer = true;
            //                }
            //                colName = colName + arrayOccurenceNum;
            //                arrayOccurenceNum++;
            //            }
            //            var tmr2 = new Stopwatch();
            //            tmr2.Start();
            //            //Console.WriteLine("New Pair - " + colName + ":" + reader.Value);
            //            AddKeyValuePair(colName, reader.Value.ToString());
            //            tmr2.Stop();
            //            var ll = tmr2.ElapsedMilliseconds;
            //            break;
            //        case JsonToken.EndArray:
            //            //closeContainer(ItemTypes.endArray);
            //            //Console.WriteLine("Close Container - " + parentNames[parentNames.Count - 1]);
            //            if(inArrayContainer){
            //                CloseRow();
            //            }
            //            //lastPropertyName = parentNames[parentNames.Count - 1];
            //            //parentNames.RemoveAt(parentNames.Count - 1);
            //            //propertyNames.RemoveAt(propertyNames.Count-1);
            //            //lastPropertyName = lastProperty;
            //            //lastPropertyName = parentNames[parentNames.Count-1];
            //            arrayOccurenceNum = 1;
            //            inArrayContainer = false;
            //            break;
            //        case JsonToken.EndObject:
            //            //closeContainer(ItemTypes.endObjectContainer);
            //            //Console.WriteLine("Close Container - " + parentNames[parentNames.Count - 1]);
            //            var tmr3 = new Stopwatch();
            //            tmr3.Start();                       
            //            CloseRow();
            //            tmr3.Stop();
            //            var llsadf = tmr3.ElapsedMilliseconds;
            //            lastPropertyName = parentNames[parentNames.Count - 1];
            //            parentNames.RemoveAt(parentNames.Count - 1);

            //            //propertyNames.RemoveAt(propertyNames.Count-1);
            //            //lastPropertyName = lastProperty;
            //            break;
            //        default:
            //            break;
            //    }
            //    jsonItems.Add(reader.TokenType);
            //}            
            //reader.Close();
            //base.Close();
            //base.CloseStreamAndWrite();
        }
        void RunJsonInsert(StreamReader jsonStream, string defaultNM)
        {
            string lastPropertyName = defaultNM;
            List<String> parentNames = new List<string>();
            int arrayOccurenceNum = 1;
            JsonTextReader reader = new JsonTextReader(jsonStream);
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartArray:
                        break;
                    case JsonToken.StartObject:
                        parentNames.Add(lastPropertyName);
                        NewRow(lastPropertyName);
                        break;
                    case JsonToken.PropertyName:
                        lastPropertyName = reader.Value.ToString();
                        break;
                    case JsonToken.String:
                    case JsonToken.Boolean:
                    case JsonToken.Integer:
                    case JsonToken.Date:
                    case JsonToken.Float:
                        String colName = lastPropertyName;
                        if (lastJsonItem != JsonToken.PropertyName)
                        {
                            if (lastJsonItem == JsonToken.StartArray)
                            {
                                NewRow(lastPropertyName);
                                inArrayContainer = true;
                            }
                            colName = colName + arrayOccurenceNum;
                            arrayOccurenceNum++;
                        }
                        AddKeyValuePair(colName, reader.Value.ToString());
                        break;
                    case JsonToken.EndArray:
                        if (inArrayContainer)
                        {
                            CloseRow();
                        }
                        arrayOccurenceNum = 1;
                        inArrayContainer = false;
                        break;
                    case JsonToken.EndObject:
                        CloseRow();
                        lastPropertyName = parentNames[parentNames.Count - 1];
                        parentNames.RemoveAt(parentNames.Count - 1);
                        break;
                    default:
                        break;
                }
                jsonItems.Add(reader.TokenType);
            }
            reader.Close();
            base.Close();
        }
        public string full_json_test()
        {
            return @"[
	{
        ""hashtags"": [
                {
                    ""text"": ""spectacles"",
                    ""indices"": [
                        43,
                        54
                    ]
                }
            ],
		""id"": ""0001"",
		""type"": ""donut"",
		""name"": ""Cake"",
		""ppu"": 0.55,
		""batters"":
			{
				""batter"":
					[
						{ ""id"": ""1001"", ""type"": ""Regular"" },
						{ ""id"": ""1002"", ""type"": ""Chocolate"" },
						{ ""id"": ""1003"", ""type"": ""Blueberry"" },
						{ ""id"": ""1004"", ""type"": ""Devil's Food"" }
					]
			},
		""topping"":
			[
				{ ""id"": ""5001"", ""type"": ""None"" },
				{ ""id"": ""5002"", ""type"": ""Glazed"" }
			]
	},
	{
        ""bounding_box"": {
                    ""type"": ""Polygon"",
                    ""coordinates"": [
                        [
                            [
                                -87.940033,
                                41.644102
                            ],
                            [
                                -87.523993,
                                41.644102
                            ],
                            [
                                -87.523993,
                                42.0230669
                            ],
                            [
                                -87.940033,
                                42.0230669
                            ]
                        ]
                    ]
                },
		""id"": ""0002"",
		""type"": ""donut"",
		""name"": ""Raised"",
		""ppu"": 0.55,
		""batters"":
			{
				""batter"":
					[
						{ ""id"": ""1001"", ""type"": ""Regular"" }
					]
			},
		""topping"":
			[
				{ ""id"": ""5001"", ""type"": ""None"" },
				{ ""id"": ""5002"", ""type"": ""Glazed"" },
				{ ""id"": ""5005"", ""type"": ""Sugar"" },
				{ ""id"": ""5003"", ""type"": ""Chocolate"" },
				{ ""id"": ""5004"", ""type"": ""Maple"" }
			]
	    }
    ]";
        }
        public String full_json_testTwo()
        {
            return @"[
    {
        ""created_at"": ""Sat Dec 24 02:20:06 +0000 2016"",
        ""id"": 812482955473920000,
        ""id_str"": ""812482955473920000"",
        ""full_text"": ""RT @rtehrani: Yes, I am back in the  @snap #spectacles line because I either love  my kids or am a masochist :-) https://t.co/xE40hJtfll"",
        ""truncated"": false,
        ""display_text_range"": [
            0,
            136
        ],
        ""entities"": {
            ""hashtags"": [
                {
                    ""text"": ""spectacles"",
                    ""indices"": [
                        43,
                        54
                    ]
                }
            ],
            ""symbols"": [],
            ""user_mentions"": [
                {
                    ""screen_name"": ""rtehrani"",
                    ""name"": ""Rich Tehrani"",
                    ""id"": 5654932,
                    ""id_str"": ""5654932"",
                    ""indices"": [
                        3,
                        12
                    ]
                },
                {
                    ""screen_name"": ""Snap"",
                    ""name"": ""Snap Inc."",
                    ""id"": 751560747566510081,
                    ""id_str"": ""751560747566510081"",
                    ""indices"": [
                        37,
                        42
                    ]
                }
            ],
            ""urls"": [],
            ""media"": [
                {
                    ""id"": 812384993636978688,
                    ""id_str"": ""812384993636978688"",
                    ""indices"": [
                        113,
                        136
                    ],
                    ""media_url"": ""http://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                    ""media_url_https"": ""https://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                    ""url"": ""https://t.co/xE40hJtfll"",
                    ""display_url"": ""pic.twitter.com/xE40hJtfll"",
                    ""expanded_url"": ""https://twitter.com/rtehrani/status/812384997755748352/photo/1"",
                    ""type"": ""photo"",
                    ""sizes"": {
                        ""thumb"": {
                            ""w"": 150,
                            ""h"": 150,
                            ""resize"": ""crop""
                        },
                        ""medium"": {
                            ""w"": 900,
                            ""h"": 1200,
                            ""resize"": ""fit""
                        },
                        ""small"": {
                            ""w"": 510,
                            ""h"": 680,
                            ""resize"": ""fit""
                        },
                        ""large"": {
                            ""w"": 1536,
                            ""h"": 2048,
                            ""resize"": ""fit""
                        }
                    },
                    ""source_status_id"": 812384997755748352,
                    ""source_status_id_str"": ""812384997755748352"",
                    ""source_user_id"": 5654932,
                    ""source_user_id_str"": ""5654932""
                }
            ]
        },
        ""extended_entities"": {
            ""media"": [
                {
                    ""id"": 812384993636978688,
                    ""id_str"": ""812384993636978688"",
                    ""indices"": [
                        113,
                        136
                    ],
                    ""media_url"": ""http://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                    ""media_url_https"": ""https://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                    ""url"": ""https://t.co/xE40hJtfll"",
                    ""display_url"": ""pic.twitter.com/xE40hJtfll"",
                    ""expanded_url"": ""https://twitter.com/rtehrani/status/812384997755748352/photo/1"",
                    ""type"": ""photo"",
                    ""sizes"": {
                        ""thumb"": {
                            ""w"": 150,
                            ""h"": 150,
                            ""resize"": ""crop""
                        },
                        ""medium"": {
                            ""w"": 900,
                            ""h"": 1200,
                            ""resize"": ""fit""
                        },
                        ""small"": {
                            ""w"": 510,
                            ""h"": 680,
                            ""resize"": ""fit""
                        },
                        ""large"": {
                            ""w"": 1536,
                            ""h"": 2048,
                            ""resize"": ""fit""
                        }
                    },
                    ""source_status_id"": 812384997755748352,
                    ""source_status_id_str"": ""812384997755748352"",
                    ""source_user_id"": 5654932,
                    ""source_user_id_str"": ""5654932""
                }
            ]
        },
        ""source"": ""<a href=\""http://tapbots.com/tweetbot\"" rel=\""nofollow\"">Tweetbot for iΟS</a>"",
        ""in_reply_to_status_id"": null,
        ""in_reply_to_status_id_str"": null,
        ""in_reply_to_user_id"": null,
        ""in_reply_to_user_id_str"": null,
        ""in_reply_to_screen_name"": null,
        ""user"": {
            ""id"": 751560747566510081,
            ""id_str"": ""751560747566510081"",
            ""name"": ""Snap Inc."",
            ""screen_name"": ""Snap"",
            ""location"": ""Venice, CA"",
            ""description"": ""Snap Inc. is a camera company. We make @Snapchat and @Spectacles."",
            ""url"": ""https://t.co/4TUw1awnN9"",
            ""entities"": {
                ""url"": {
                    ""urls"": [
                        {
                            ""url"": ""https://t.co/4TUw1awnN9"",
                            ""expanded_url"": ""http://snap.com"",
                            ""display_url"": ""snap.com"",
                            ""indices"": [
                                0,
                                23
                            ]
                        }
                    ]
                },
                ""description"": {
                    ""urls"": []
                }
            },
            ""protected"": false,
            ""followers_count"": 62696,
            ""friends_count"": 2,
            ""listed_count"": 373,
            ""created_at"": ""Fri Jul 08 23:37:00 +0000 2016"",
            ""favourites_count"": 1,
            ""utc_offset"": null,
            ""time_zone"": null,
            ""geo_enabled"": false,
            ""verified"": true,
            ""statuses_count"": 139,
            ""lang"": null,
            ""contributors_enabled"": false,
            ""is_translator"": false,
            ""is_translation_enabled"": false,
            ""profile_background_color"": ""F5F8FA"",
            ""profile_background_image_url"": null,
            ""profile_background_image_url_https"": null,
            ""profile_background_tile"": false,
            ""profile_image_url"": ""http://pbs.twimg.com/profile_images/779559400847441920/WEsRmsyA_normal.jpg"",
            ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/779559400847441920/WEsRmsyA_normal.jpg"",
            ""profile_banner_url"": ""https://pbs.twimg.com/profile_banners/751560747566510081/1474696409"",
            ""profile_link_color"": ""1DA1F2"",
            ""profile_sidebar_border_color"": ""C0DEED"",
            ""profile_sidebar_fill_color"": ""DDEEF6"",
            ""profile_text_color"": ""333333"",
            ""profile_use_background_image"": true,
            ""has_extended_profile"": false,
            ""default_profile"": true,
            ""default_profile_image"": false,
            ""following"": false,
            ""follow_request_sent"": false,
            ""notifications"": false,
            ""translator_type"": ""none""
        },
        ""geo"": null,
        ""coordinates"": null,
        ""place"": null,
        ""contributors"": null,
        ""retweeted_status"": {
            ""created_at"": ""Fri Dec 23 19:50:51 +0000 2016"",
            ""id"": 812384997755748352,
            ""id_str"": ""812384997755748352"",
            ""full_text"": ""Yes, I am back in the  @snap #spectacles line because I either love  my kids or am a masochist :-) https://t.co/xE40hJtfll"",
            ""truncated"": false,
            ""display_text_range"": [
                0,
                98
            ],
            ""entities"": {
                ""hashtags"": [
                    {
                        ""text"": ""spectacles"",
                        ""indices"": [
                            29,
                            40
                        ]
                    }
                ],
                ""symbols"": [],
                ""user_mentions"": [
                    {
                        ""screen_name"": ""Snap"",
                        ""name"": ""Snap Inc."",
                        ""id"": 751560747566510081,
                        ""id_str"": ""751560747566510081"",
                        ""indices"": [
                            23,
                            28
                        ]
                    }
                ],
                ""urls"": [],
                ""media"": [
                    {
                        ""id"": 812384993636978688,
                        ""id_str"": ""812384993636978688"",
                        ""indices"": [
                            99,
                            122
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                        ""url"": ""https://t.co/xE40hJtfll"",
                        ""display_url"": ""pic.twitter.com/xE40hJtfll"",
                        ""expanded_url"": ""https://twitter.com/rtehrani/status/812384997755748352/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""medium"": {
                                ""w"": 900,
                                ""h"": 1200,
                                ""resize"": ""fit""
                            },
                            ""small"": {
                                ""w"": 510,
                                ""h"": 680,
                                ""resize"": ""fit""
                            },
                            ""large"": {
                                ""w"": 1536,
                                ""h"": 2048,
                                ""resize"": ""fit""
                            }
                        }
                    }
                ]
            },
            ""extended_entities"": {
                ""media"": [
                    {
                        ""id"": 812384993636978688,
                        ""id_str"": ""812384993636978688"",
                        ""indices"": [
                            99,
                            122
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0Yr2PaXEAA1_ld.jpg"",
                        ""url"": ""https://t.co/xE40hJtfll"",
                        ""display_url"": ""pic.twitter.com/xE40hJtfll"",
                        ""expanded_url"": ""https://twitter.com/rtehrani/status/812384997755748352/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""medium"": {
                                ""w"": 900,
                                ""h"": 1200,
                                ""resize"": ""fit""
                            },
                            ""small"": {
                                ""w"": 510,
                                ""h"": 680,
                                ""resize"": ""fit""
                            },
                            ""large"": {
                                ""w"": 1536,
                                ""h"": 2048,
                                ""resize"": ""fit""
                            }
                        }
                    }
                ]
            },
            ""source"": ""<a href=\""http://www.hootsuite.com\"" rel=\""nofollow\"">Hootsuite</a>"",
            ""in_reply_to_status_id"": null,
            ""in_reply_to_status_id_str"": null,
            ""in_reply_to_user_id"": null,
            ""in_reply_to_user_id_str"": null,
            ""in_reply_to_screen_name"": null,
            ""user"": {
                ""id"": 5654932,
                ""id_str"": ""5654932"",
                ""name"": ""Rich Tehrani"",
                ""screen_name"": ""rtehrani"",
                ""location"": ""Trumbull, CT"",
                ""description"": ""CEO TMC: #Futurist #Influencer #Keynote #Speaker #Cloud #IoT #Blockchain #Cybersecurity #VoIP #CRM #UCaaS #BigData #Analytics #AI #MachineLearning #SDWAN #IIoT"",
                ""url"": ""https://t.co/jXncTWevnI"",
                ""entities"": {
                    ""url"": {
                        ""urls"": [
                            {
                                ""url"": ""https://t.co/jXncTWevnI"",
                                ""expanded_url"": ""http://www.tehrani.com"",
                                ""display_url"": ""tehrani.com"",
                                ""indices"": [
                                    0,
                                    23
                                ]
                            }
                        ]
                    },
                    ""description"": {
                        ""urls"": []
                    }
                },
                ""protected"": false,
                ""followers_count"": 24362,
                ""friends_count"": 25415,
                ""listed_count"": 813,
                ""created_at"": ""Mon Apr 30 14:30:41 +0000 2007"",
                ""favourites_count"": 23763,
                ""utc_offset"": null,
                ""time_zone"": null,
                ""geo_enabled"": true,
                ""verified"": false,
                ""statuses_count"": 121253,
                ""lang"": null,
                ""contributors_enabled"": false,
                ""is_translator"": false,
                ""is_translation_enabled"": false,
                ""profile_background_color"": ""9AE4E8"",
                ""profile_background_image_url"": ""http://abs.twimg.com/images/themes/theme1/bg.png"",
                ""profile_background_image_url_https"": ""https://abs.twimg.com/images/themes/theme1/bg.png"",
                ""profile_background_tile"": false,
                ""profile_image_url"": ""http://pbs.twimg.com/profile_images/378800000447877184/f7896fca0648407dd797c817d4481e5b_normal.jpeg"",
                ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/378800000447877184/f7896fca0648407dd797c817d4481e5b_normal.jpeg"",
                ""profile_link_color"": ""FF691F"",
                ""profile_sidebar_border_color"": ""87BC44"",
                ""profile_sidebar_fill_color"": ""E0FF92"",
                ""profile_text_color"": ""000000"",
                ""profile_use_background_image"": true,
                ""has_extended_profile"": false,
                ""default_profile"": false,
                ""default_profile_image"": false,
                ""following"": false,
                ""follow_request_sent"": false,
                ""notifications"": false,
                ""translator_type"": ""none""
            },
            ""geo"": null,
            ""coordinates"": null,
            ""place"": null,
            ""contributors"": null,
            ""is_quote_status"": false,
            ""retweet_count"": 9,
            ""favorite_count"": 81,
            ""favorited"": false,
            ""retweeted"": false,
            ""possibly_sensitive"": false,
            ""lang"": ""en""
        },
        ""is_quote_status"": false,
        ""retweet_count"": 9,
        ""favorite_count"": 0,
        ""favorited"": false,
        ""retweeted"": false,
        ""possibly_sensitive"": false,
        ""lang"": ""en""
    },
    {
        ""created_at"": ""Sat Dec 24 02:19:24 +0000 2016"",
        ""id"": 812482779376066560,
        ""id_str"": ""812482779376066560"",
        ""full_text"": ""RT @DanNieves: In and out @Snap @Spectacles store this AM! Wristband system was 👍🏽👍🏽 https://t.co/vsY7u6bYIl"",
        ""truncated"": false,
        ""display_text_range"": [
            0,
            108
        ],
        ""entities"": {
            ""hashtags"": [],
            ""symbols"": [],
            ""user_mentions"": [
                {
                    ""screen_name"": ""DanNieves"",
                    ""name"": ""Dan Nieves"",
                    ""id"": 50832208,
                    ""id_str"": ""50832208"",
                    ""indices"": [
                        3,
                        13
                    ]
                },
                {
                    ""screen_name"": ""Snap"",
                    ""name"": ""Snap Inc."",
                    ""id"": 751560747566510081,
                    ""id_str"": ""751560747566510081"",
                    ""indices"": [
                        26,
                        31
                    ]
                },
                {
                    ""screen_name"": ""Spectacles"",
                    ""name"": ""Spectacles"",
                    ""id"": 775495523142819840,
                    ""id_str"": ""775495523142819840"",
                    ""indices"": [
                        32,
                        43
                    ]
                }
            ],
            ""urls"": [],
            ""media"": [
                {
                    ""id"": 812315454895259648,
                    ""id_str"": ""812315454895259648"",
                    ""indices"": [
                        85,
                        108
                    ],
                    ""media_url"": ""http://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                    ""media_url_https"": ""https://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                    ""url"": ""https://t.co/vsY7u6bYIl"",
                    ""display_url"": ""pic.twitter.com/vsY7u6bYIl"",
                    ""expanded_url"": ""https://twitter.com/DanNieves/status/812315467868229632/photo/1"",
                    ""type"": ""photo"",
                    ""sizes"": {
                        ""thumb"": {
                            ""w"": 150,
                            ""h"": 150,
                            ""resize"": ""crop""
                        },
                        ""small"": {
                            ""w"": 510,
                            ""h"": 680,
                            ""resize"": ""fit""
                        },
                        ""medium"": {
                            ""w"": 900,
                            ""h"": 1200,
                            ""resize"": ""fit""
                        },
                        ""large"": {
                            ""w"": 1536,
                            ""h"": 2048,
                            ""resize"": ""fit""
                        }
                    },
                    ""source_status_id"": 812315467868229632,
                    ""source_status_id_str"": ""812315467868229632"",
                    ""source_user_id"": 50832208,
                    ""source_user_id_str"": ""50832208""
                }
            ]
        },
        ""extended_entities"": {
            ""media"": [
                {
                    ""id"": 812315454895259648,
                    ""id_str"": ""812315454895259648"",
                    ""indices"": [
                        85,
                        108
                    ],
                    ""media_url"": ""http://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                    ""media_url_https"": ""https://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                    ""url"": ""https://t.co/vsY7u6bYIl"",
                    ""display_url"": ""pic.twitter.com/vsY7u6bYIl"",
                    ""expanded_url"": ""https://twitter.com/DanNieves/status/812315467868229632/photo/1"",
                    ""type"": ""photo"",
                    ""sizes"": {
                        ""thumb"": {
                            ""w"": 150,
                            ""h"": 150,
                            ""resize"": ""crop""
                        },
                        ""small"": {
                            ""w"": 510,
                            ""h"": 680,
                            ""resize"": ""fit""
                        },
                        ""medium"": {
                            ""w"": 900,
                            ""h"": 1200,
                            ""resize"": ""fit""
                        },
                        ""large"": {
                            ""w"": 1536,
                            ""h"": 2048,
                            ""resize"": ""fit""
                        }
                    },
                    ""source_status_id"": 812315467868229632,
                    ""source_status_id_str"": ""812315467868229632"",
                    ""source_user_id"": 50832208,
                    ""source_user_id_str"": ""50832208""
                },
                {
                    ""id"": 812315454861705216,
                    ""id_str"": ""812315454861705216"",
                    ""indices"": [
                        85,
                        108
                    ],
                    ""media_url"": ""http://pbs.twimg.com/media/C0XsmjSXgAAjs6W.jpg"",
                    ""media_url_https"": ""https://pbs.twimg.com/media/C0XsmjSXgAAjs6W.jpg"",
                    ""url"": ""https://t.co/vsY7u6bYIl"",
                    ""display_url"": ""pic.twitter.com/vsY7u6bYIl"",
                    ""expanded_url"": ""https://twitter.com/DanNieves/status/812315467868229632/photo/1"",
                    ""type"": ""photo"",
                    ""sizes"": {
                        ""thumb"": {
                            ""w"": 150,
                            ""h"": 150,
                            ""resize"": ""crop""
                        },
                        ""small"": {
                            ""w"": 510,
                            ""h"": 680,
                            ""resize"": ""fit""
                        },
                        ""medium"": {
                            ""w"": 900,
                            ""h"": 1200,
                            ""resize"": ""fit""
                        },
                        ""large"": {
                            ""w"": 1536,
                            ""h"": 2048,
                            ""resize"": ""fit""
                        }
                    },
                    ""source_status_id"": 812315467868229632,
                    ""source_status_id_str"": ""812315467868229632"",
                    ""source_user_id"": 50832208,
                    ""source_user_id_str"": ""50832208""
                }
            ]
        },
        ""source"": ""<a href=\""http://tapbots.com/tweetbot\"" rel=\""nofollow\"">Tweetbot for iΟS</a>"",
        ""in_reply_to_status_id"": null,
        ""in_reply_to_status_id_str"": null,
        ""in_reply_to_user_id"": null,
        ""in_reply_to_user_id_str"": null,
        ""in_reply_to_screen_name"": null,
        ""user"": {
            ""id"": 751560747566510081,
            ""id_str"": ""751560747566510081"",
            ""name"": ""Snap Inc."",
            ""screen_name"": ""Snap"",
            ""location"": ""Venice, CA"",
            ""description"": ""Snap Inc. is a camera company. We make @Snapchat and @Spectacles."",
            ""url"": ""https://t.co/4TUw1awnN9"",
            ""entities"": {
                ""url"": {
                    ""urls"": [
                        {
                            ""url"": ""https://t.co/4TUw1awnN9"",
                            ""expanded_url"": ""http://snap.com"",
                            ""display_url"": ""snap.com"",
                            ""indices"": [
                                0,
                                23
                            ]
                        }
                    ]
                },
                ""description"": {
                    ""urls"": []
                }
            },
            ""protected"": false,
            ""followers_count"": 62696,
            ""friends_count"": 2,
            ""listed_count"": 373,
            ""created_at"": ""Fri Jul 08 23:37:00 +0000 2016"",
            ""favourites_count"": 1,
            ""utc_offset"": null,
            ""time_zone"": null,
            ""geo_enabled"": false,
            ""verified"": true,
            ""statuses_count"": 139,
            ""lang"": null,
            ""contributors_enabled"": false,
            ""is_translator"": false,
            ""is_translation_enabled"": false,
            ""profile_background_color"": ""F5F8FA"",
            ""profile_background_image_url"": null,
            ""profile_background_image_url_https"": null,
            ""profile_background_tile"": false,
            ""profile_image_url"": ""http://pbs.twimg.com/profile_images/779559400847441920/WEsRmsyA_normal.jpg"",
            ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/779559400847441920/WEsRmsyA_normal.jpg"",
            ""profile_banner_url"": ""https://pbs.twimg.com/profile_banners/751560747566510081/1474696409"",
            ""profile_link_color"": ""1DA1F2"",
            ""profile_sidebar_border_color"": ""C0DEED"",
            ""profile_sidebar_fill_color"": ""DDEEF6"",
            ""profile_text_color"": ""333333"",
            ""profile_use_background_image"": true,
            ""has_extended_profile"": false,
            ""default_profile"": true,
            ""default_profile_image"": false,
            ""following"": false,
            ""follow_request_sent"": false,
            ""notifications"": false,
            ""translator_type"": ""none""
        },
        ""geo"": null,
        ""coordinates"": null,
        ""place"": null,
        ""contributors"": null,
        ""retweeted_status"": {
            ""created_at"": ""Fri Dec 23 15:14:34 +0000 2016"",
            ""id"": 812315467868229632,
            ""id_str"": ""812315467868229632"",
            ""full_text"": ""In and out @Snap @Spectacles store this AM! Wristband system was 👍🏽👍🏽 https://t.co/vsY7u6bYIl"",
            ""truncated"": false,
            ""display_text_range"": [
                0,
                69
            ],
            ""entities"": {
                ""hashtags"": [],
                ""symbols"": [],
                ""user_mentions"": [
                    {
                        ""screen_name"": ""Snap"",
                        ""name"": ""Snap Inc."",
                        ""id"": 751560747566510081,
                        ""id_str"": ""751560747566510081"",
                        ""indices"": [
                            11,
                            16
                        ]
                    },
                    {
                        ""screen_name"": ""Spectacles"",
                        ""name"": ""Spectacles"",
                        ""id"": 775495523142819840,
                        ""id_str"": ""775495523142819840"",
                        ""indices"": [
                            17,
                            28
                        ]
                    }
                ],
                ""urls"": [],
                ""media"": [
                    {
                        ""id"": 812315454895259648,
                        ""id_str"": ""812315454895259648"",
                        ""indices"": [
                            70,
                            93
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                        ""url"": ""https://t.co/vsY7u6bYIl"",
                        ""display_url"": ""pic.twitter.com/vsY7u6bYIl"",
                        ""expanded_url"": ""https://twitter.com/DanNieves/status/812315467868229632/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""small"": {
                                ""w"": 510,
                                ""h"": 680,
                                ""resize"": ""fit""
                            },
                            ""medium"": {
                                ""w"": 900,
                                ""h"": 1200,
                                ""resize"": ""fit""
                            },
                            ""large"": {
                                ""w"": 1536,
                                ""h"": 2048,
                                ""resize"": ""fit""
                            }
                        }
                    }
                ]
            },
            ""extended_entities"": {
                ""media"": [
                    {
                        ""id"": 812315454895259648,
                        ""id_str"": ""812315454895259648"",
                        ""indices"": [
                            70,
                            93
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0XsmjaXgAAYkQY.jpg"",
                        ""url"": ""https://t.co/vsY7u6bYIl"",
                        ""display_url"": ""pic.twitter.com/vsY7u6bYIl"",
                        ""expanded_url"": ""https://twitter.com/DanNieves/status/812315467868229632/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""small"": {
                                ""w"": 510,
                                ""h"": 680,
                                ""resize"": ""fit""
                            },
                            ""medium"": {
                                ""w"": 900,
                                ""h"": 1200,
                                ""resize"": ""fit""
                            },
                            ""large"": {
                                ""w"": 1536,
                                ""h"": 2048,
                                ""resize"": ""fit""
                            }
                        }
                    },
                    {
                        ""id"": 812315454861705216,
                        ""id_str"": ""812315454861705216"",
                        ""indices"": [
                            70,
                            93
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0XsmjSXgAAjs6W.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0XsmjSXgAAjs6W.jpg"",
                        ""url"": ""https://t.co/vsY7u6bYIl"",
                        ""display_url"": ""pic.twitter.com/vsY7u6bYIl"",
                        ""expanded_url"": ""https://twitter.com/DanNieves/status/812315467868229632/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""small"": {
                                ""w"": 510,
                                ""h"": 680,
                                ""resize"": ""fit""
                            },
                            ""medium"": {
                                ""w"": 900,
                                ""h"": 1200,
                                ""resize"": ""fit""
                            },
                            ""large"": {
                                ""w"": 1536,
                                ""h"": 2048,
                                ""resize"": ""fit""
                            }
                        }
                    }
                ]
            },
            ""source"": ""<a href=\""http://twitter.com/download/iphone\"" rel=\""nofollow\"">Twitter for iPhone</a>"",
            ""in_reply_to_status_id"": null,
            ""in_reply_to_status_id_str"": null,
            ""in_reply_to_user_id"": null,
            ""in_reply_to_user_id_str"": null,
            ""in_reply_to_screen_name"": null,
            ""user"": {
                ""id"": 50832208,
                ""id_str"": ""50832208"",
                ""name"": ""Dan Nieves"",
                ""screen_name"": ""DanNieves"",
                ""location"": ""NYC via SF, DC"",
                ""description"": ""Curious about tech, employee experience & happiness | Customers @WorkplacebyFB | Ex @Frame_io @Zinc @Deloitte | Love breakfast, coffee, and a fresh fade 💯"",
                ""url"": ""https://t.co/f5qmaPmocc"",
                ""entities"": {
                    ""url"": {
                        ""urls"": [
                            {
                                ""url"": ""https://t.co/f5qmaPmocc"",
                                ""expanded_url"": ""http://www.linkedin.com/in/dnieves"",
                                ""display_url"": ""linkedin.com/in/dnieves"",
                                ""indices"": [
                                    0,
                                    23
                                ]
                            }
                        ]
                    },
                    ""description"": {
                        ""urls"": []
                    }
                },
                ""protected"": false,
                ""followers_count"": 2907,
                ""friends_count"": 3446,
                ""listed_count"": 430,
                ""created_at"": ""Fri Jun 26 00:12:01 +0000 2009"",
                ""favourites_count"": 23415,
                ""utc_offset"": null,
                ""time_zone"": null,
                ""geo_enabled"": true,
                ""verified"": false,
                ""statuses_count"": 36077,
                ""lang"": null,
                ""contributors_enabled"": false,
                ""is_translator"": false,
                ""is_translation_enabled"": false,
                ""profile_background_color"": ""FFFFFF"",
                ""profile_background_image_url"": ""http://abs.twimg.com/images/themes/theme9/bg.gif"",
                ""profile_background_image_url_https"": ""https://abs.twimg.com/images/themes/theme9/bg.gif"",
                ""profile_background_tile"": false,
                ""profile_image_url"": ""http://pbs.twimg.com/profile_images/1074876771848372225/Hl8A4GQz_normal.jpg"",
                ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/1074876771848372225/Hl8A4GQz_normal.jpg"",
                ""profile_banner_url"": ""https://pbs.twimg.com/profile_banners/50832208/1405450501"",
                ""profile_link_color"": ""080826"",
                ""profile_sidebar_border_color"": ""000000"",
                ""profile_sidebar_fill_color"": ""686466"",
                ""profile_text_color"": ""000000"",
                ""profile_use_background_image"": true,
                ""has_extended_profile"": false,
                ""default_profile"": false,
                ""default_profile_image"": false,
                ""following"": false,
                ""follow_request_sent"": false,
                ""notifications"": false,
                ""translator_type"": ""none""
            },
            ""geo"": null,
            ""coordinates"": null,
            ""place"": null,
            ""contributors"": null,
            ""is_quote_status"": false,
            ""retweet_count"": 17,
            ""favorite_count"": 98,
            ""favorited"": false,
            ""retweeted"": false,
            ""possibly_sensitive"": false,
            ""lang"": ""en""
        },
        ""is_quote_status"": false,
        ""retweet_count"": 17,
        ""favorite_count"": 0,
        ""favorited"": false,
        ""retweeted"": false,
        ""possibly_sensitive"": false,
        ""lang"": ""en""
    },
    {
        ""created_at"": ""Thu Dec 22 18:13:50 +0000 2016"",
        ""id"": 811998194146144260,
        ""id_str"": ""811998194146144260"",
        ""full_text"": ""RT @karishustad: .@Snap's @Spectacles popped up at the @DaveandBusters in @VillageOrlandPk. Sold out in 3 hours. We snagged a pair: https:/…"",
        ""truncated"": false,
        ""display_text_range"": [
            0,
            140
        ],
        ""entities"": {
            ""hashtags"": [],
            ""symbols"": [],
            ""user_mentions"": [
                {
                    ""screen_name"": ""karishustad"",
                    ""name"": ""Karis Hustad"",
                    ""id"": 22335937,
                    ""id_str"": ""22335937"",
                    ""indices"": [
                        3,
                        15
                    ]
                },
                {
                    ""screen_name"": ""Snap"",
                    ""name"": ""Snap Inc."",
                    ""id"": 751560747566510081,
                    ""id_str"": ""751560747566510081"",
                    ""indices"": [
                        18,
                        23
                    ]
                },
                {
                    ""screen_name"": ""Spectacles"",
                    ""name"": ""Spectacles"",
                    ""id"": 775495523142819840,
                    ""id_str"": ""775495523142819840"",
                    ""indices"": [
                        26,
                        37
                    ]
                },
                {
                    ""screen_name"": ""DaveandBusters"",
                    ""name"": ""Dave & Buster's"",
                    ""id"": 50728264,
                    ""id_str"": ""50728264"",
                    ""indices"": [
                        55,
                        70
                    ]
                },
                {
                    ""screen_name"": ""VillageOrlandPk"",
                    ""name"": ""Village of Orland Pk"",
                    ""id"": 49628920,
                    ""id_str"": ""49628920"",
                    ""indices"": [
                        74,
                        90
                    ]
                }
            ],
            ""urls"": []
        },
        ""source"": ""<a href=\""http://tapbots.com/tweetbot\"" rel=\""nofollow\"">Tweetbot for iΟS</a>"",
        ""in_reply_to_status_id"": null,
        ""in_reply_to_status_id_str"": null,
        ""in_reply_to_user_id"": null,
        ""in_reply_to_user_id_str"": null,
        ""in_reply_to_screen_name"": null,
        ""user"": {
            ""id"": 751560747566510081,
            ""id_str"": ""751560747566510081"",
            ""name"": ""Snap Inc."",
            ""screen_name"": ""Snap"",
            ""location"": ""Venice, CA"",
            ""description"": ""Snap Inc. is a camera company. We make @Snapchat and @Spectacles."",
            ""url"": ""https://t.co/4TUw1awnN9"",
            ""entities"": {
                ""url"": {
                    ""urls"": [
                        {
                            ""url"": ""https://t.co/4TUw1awnN9"",
                            ""expanded_url"": ""http://snap.com"",
                            ""display_url"": ""snap.com"",
                            ""indices"": [
                                0,
                                23
                            ]
                        }
                    ]
                },
                ""description"": {
                    ""urls"": []
                }
            },
            ""protected"": false,
            ""followers_count"": 62696,
            ""friends_count"": 2,
            ""listed_count"": 373,
            ""created_at"": ""Fri Jul 08 23:37:00 +0000 2016"",
            ""favourites_count"": 1,
            ""utc_offset"": null,
            ""time_zone"": null,
            ""geo_enabled"": false,
            ""verified"": true,
            ""statuses_count"": 139,
            ""lang"": null,
            ""contributors_enabled"": false,
            ""is_translator"": false,
            ""is_translation_enabled"": false,
            ""profile_background_color"": ""F5F8FA"",
            ""profile_background_image_url"": null,
            ""profile_background_image_url_https"": null,
            ""profile_background_tile"": false,
            ""profile_image_url"": ""http://pbs.twimg.com/profile_images/779559400847441920/WEsRmsyA_normal.jpg"",
            ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/779559400847441920/WEsRmsyA_normal.jpg"",
            ""profile_banner_url"": ""https://pbs.twimg.com/profile_banners/751560747566510081/1474696409"",
            ""profile_link_color"": ""1DA1F2"",
            ""profile_sidebar_border_color"": ""C0DEED"",
            ""profile_sidebar_fill_color"": ""DDEEF6"",
            ""profile_text_color"": ""333333"",
            ""profile_use_background_image"": true,
            ""has_extended_profile"": false,
            ""default_profile"": true,
            ""default_profile_image"": false,
            ""following"": false,
            ""follow_request_sent"": false,
            ""notifications"": false,
            ""translator_type"": ""none""
        },
        ""geo"": null,
        ""coordinates"": null,
        ""place"": null,
        ""contributors"": null,
        ""retweeted_status"": {
            ""created_at"": ""Wed Dec 21 22:02:03 +0000 2016"",
            ""id"": 811693237844918273,
            ""id_str"": ""811693237844918273"",
            ""full_text"": "".@Snap's @Spectacles popped up at the @DaveandBusters in @VillageOrlandPk. Sold out in 3 hours. We snagged a pair: https://t.co/WqpPxrUVfT https://t.co/p8b87wC9FM"",
            ""truncated"": false,
            ""display_text_range"": [
                0,
                138
            ],
            ""entities"": {
                ""hashtags"": [],
                ""symbols"": [],
                ""user_mentions"": [
                    {
                        ""screen_name"": ""Snap"",
                        ""name"": ""Snap Inc."",
                        ""id"": 751560747566510081,
                        ""id_str"": ""751560747566510081"",
                        ""indices"": [
                            1,
                            6
                        ]
                    },
                    {
                        ""screen_name"": ""Spectacles"",
                        ""name"": ""Spectacles"",
                        ""id"": 775495523142819840,
                        ""id_str"": ""775495523142819840"",
                        ""indices"": [
                            9,
                            20
                        ]
                    },
                    {
                        ""screen_name"": ""DaveandBusters"",
                        ""name"": ""Dave & Buster's"",
                        ""id"": 50728264,
                        ""id_str"": ""50728264"",
                        ""indices"": [
                            38,
                            53
                        ]
                    },
                    {
                        ""screen_name"": ""VillageOrlandPk"",
                        ""name"": ""Village of Orland Pk"",
                        ""id"": 49628920,
                        ""id_str"": ""49628920"",
                        ""indices"": [
                            57,
                            73
                        ]
                    }
                ],
                ""urls"": [
                    {
                        ""url"": ""https://t.co/WqpPxrUVfT"",
                        ""expanded_url"": ""http://chicagoinno.streetwise.co/2016/12/21/snapchats-spectacles-sold-out-in-chicago-but-we-got-our-hands-on-a-pair/"",
                        ""display_url"": ""chicagoinno.streetwise.co/2016/12/21/sna…"",
                        ""indices"": [
                            115,
                            138
                        ]
                    }
                ],
                ""media"": [
                    {
                        ""id"": 811693087277752320,
                        ""id_str"": ""811693087277752320"",
                        ""indices"": [
                            139,
                            162
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0O2j_XUoAAxtWj.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0O2j_XUoAAxtWj.jpg"",
                        ""url"": ""https://t.co/p8b87wC9FM"",
                        ""display_url"": ""pic.twitter.com/p8b87wC9FM"",
                        ""expanded_url"": ""https://twitter.com/karishustad/status/811693237844918273/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""small"": {
                                ""w"": 510,
                                ""h"": 680,
                                ""resize"": ""fit""
                            },
                            ""medium"": {
                                ""w"": 900,
                                ""h"": 1200,
                                ""resize"": ""fit""
                            },
                            ""large"": {
                                ""w"": 1250,
                                ""h"": 1667,
                                ""resize"": ""fit""
                            }
                        }
                    }
                ]
            },
            ""extended_entities"": {
                ""media"": [
                    {
                        ""id"": 811693087277752320,
                        ""id_str"": ""811693087277752320"",
                        ""indices"": [
                            139,
                            162
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0O2j_XUoAAxtWj.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0O2j_XUoAAxtWj.jpg"",
                        ""url"": ""https://t.co/p8b87wC9FM"",
                        ""display_url"": ""pic.twitter.com/p8b87wC9FM"",
                        ""expanded_url"": ""https://twitter.com/karishustad/status/811693237844918273/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""small"": {
                                ""w"": 510,
                                ""h"": 680,
                                ""resize"": ""fit""
                            },
                            ""medium"": {
                                ""w"": 900,
                                ""h"": 1200,
                                ""resize"": ""fit""
                            },
                            ""large"": {
                                ""w"": 1250,
                                ""h"": 1667,
                                ""resize"": ""fit""
                            }
                        }
                    },
                    {
                        ""id"": 811693172967358464,
                        ""id_str"": ""811693172967358464"",
                        ""indices"": [
                            139,
                            162
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0O2o-lUQAA6U8R.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0O2o-lUQAA6U8R.jpg"",
                        ""url"": ""https://t.co/p8b87wC9FM"",
                        ""display_url"": ""pic.twitter.com/p8b87wC9FM"",
                        ""expanded_url"": ""https://twitter.com/karishustad/status/811693237844918273/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""large"": {
                                ""w"": 1250,
                                ""h"": 703,
                                ""resize"": ""fit""
                            },
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""medium"": {
                                ""w"": 1200,
                                ""h"": 675,
                                ""resize"": ""fit""
                            },
                            ""small"": {
                                ""w"": 680,
                                ""h"": 382,
                                ""resize"": ""fit""
                            }
                        }
                    },
                    {
                        ""id"": 811693215623450625,
                        ""id_str"": ""811693215623450625"",
                        ""indices"": [
                            139,
                            162
                        ],
                        ""media_url"": ""http://pbs.twimg.com/media/C0O2rdfUkAEZcSK.jpg"",
                        ""media_url_https"": ""https://pbs.twimg.com/media/C0O2rdfUkAEZcSK.jpg"",
                        ""url"": ""https://t.co/p8b87wC9FM"",
                        ""display_url"": ""pic.twitter.com/p8b87wC9FM"",
                        ""expanded_url"": ""https://twitter.com/karishustad/status/811693237844918273/photo/1"",
                        ""type"": ""photo"",
                        ""sizes"": {
                            ""thumb"": {
                                ""w"": 150,
                                ""h"": 150,
                                ""resize"": ""crop""
                            },
                            ""large"": {
                                ""w"": 1250,
                                ""h"": 703,
                                ""resize"": ""fit""
                            },
                            ""small"": {
                                ""w"": 680,
                                ""h"": 382,
                                ""resize"": ""fit""
                            },
                            ""medium"": {
                                ""w"": 1200,
                                ""h"": 675,
                                ""resize"": ""fit""
                            }
                        }
                    }
                ]
            },
            ""source"": ""<a href=\""http://twitter.com\"" rel=\""nofollow\"">Twitter Web Client</a>"",
            ""in_reply_to_status_id"": null,
            ""in_reply_to_status_id_str"": null,
            ""in_reply_to_user_id"": null,
            ""in_reply_to_user_id_str"": null,
            ""in_reply_to_screen_name"": null,
            ""user"": {
                ""id"": 22335937,
                ""id_str"": ""22335937"",
                ""name"": ""Karis Hustad"",
                ""screen_name"": ""karishustad"",
                ""location"": ""London, England"",
                ""description"": ""Financial journalist @Debtwire Europe. Previously covered tech/startups/vc @ChicagoInno. Freelanced @time, @euronews, @PRI, etc. Views mine, not my employer's."",
                ""url"": null,
                ""entities"": {
                    ""description"": {
                        ""urls"": []
                    }
                },
                ""protected"": false,
                ""followers_count"": 2520,
                ""friends_count"": 1783,
                ""listed_count"": 186,
                ""created_at"": ""Sun Mar 01 05:36:55 +0000 2009"",
                ""favourites_count"": 8550,
                ""utc_offset"": null,
                ""time_zone"": null,
                ""geo_enabled"": true,
                ""verified"": true,
                ""statuses_count"": 8681,
                ""lang"": null,
                ""contributors_enabled"": false,
                ""is_translator"": false,
                ""is_translation_enabled"": false,
                ""profile_background_color"": ""131516"",
                ""profile_background_image_url"": ""http://abs.twimg.com/images/themes/theme6/bg.gif"",
                ""profile_background_image_url_https"": ""https://abs.twimg.com/images/themes/theme6/bg.gif"",
                ""profile_background_tile"": false,
                ""profile_image_url"": ""http://pbs.twimg.com/profile_images/1158486510515175424/Zer4vVnL_normal.jpg"",
                ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/1158486510515175424/Zer4vVnL_normal.jpg"",
                ""profile_banner_url"": ""https://pbs.twimg.com/profile_banners/22335937/1411296364"",
                ""profile_link_color"": ""129696"",
                ""profile_sidebar_border_color"": ""EEEEEE"",
                ""profile_sidebar_fill_color"": ""EFEFEF"",
                ""profile_text_color"": ""333333"",
                ""profile_use_background_image"": true,
                ""has_extended_profile"": true,
                ""default_profile"": false,
                ""default_profile_image"": false,
                ""following"": false,
                ""follow_request_sent"": false,
                ""notifications"": false,
                ""translator_type"": ""none""
            },
            ""geo"": null,
            ""coordinates"": null,
            ""place"": {
                ""id"": ""1d9a5370a355ab0c"",
                ""url"": ""https://api.twitter.com/1.1/geo/id/1d9a5370a355ab0c.json"",
                ""place_type"": ""city"",
                ""name"": ""Chicago"",
                ""full_name"": ""Chicago, IL"",
                ""country_code"": ""US"",
                ""country"": ""United States"",
                ""contained_within"": [],
                ""bounding_box"": {
                    ""type"": ""Polygon"",
                    ""coordinates"": [
                        [
                            [
                                -87.940033,
                                41.644102
                            ],
                            [
                                -87.523993,
                                41.644102
                            ],
                            [
                                -87.523993,
                                42.0230669
                            ],
                            [
                                -87.940033,
                                42.0230669
                            ]
                        ]
                    ]
                },
                ""attributestwo"": {}
            },
            ""contributors"": null,
            ""is_quote_status"": false,
            ""retweet_count"": 22,
            ""favorite_count"": 115,
            ""favorited"": false,
            ""retweeted"": false,
            ""possibly_sensitive"": false,
            ""lang"": ""en""
        },
        ""is_quote_status"": false,
        ""retweet_count"": 22,
        ""favorite_count"": 0,
        ""favorited"": false,
        ""retweeted"": false,
        ""lang"": ""en""
    }]";
        }     
    }
    public static class JsonReaderExtensions
    {
        public static IEnumerable<T> SelectTokensWithRegex<T>(
            this JsonReader jsonReader, Regex regex)
        {
            JsonSerializer serializer = new JsonSerializer();
            while (jsonReader.Read())
            {
                if (regex.IsMatch(jsonReader.Path)
                    && jsonReader.TokenType != JsonToken.PropertyName)
                {
                    yield return serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}