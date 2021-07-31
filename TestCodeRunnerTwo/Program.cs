using rogue_core.rogueCore.hqlSyntaxV4;
using rogueCore.apiV3;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.rogueUIV3.web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace TestCodeRunnerTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            string qrysDel = @"DELETE IORECORDS  WHERE ROGUECOLUMNID = ""2563086"" SELECT ROGUECOLUMNID";
            //DELETE IORECORDS  WHERE ROGUECOLUMNID = ""2498397"" SELECT ROGUECOLUMNID
            //DELETE IORECORDS  WHERE ROGUECOLUMNID = ""2484893"" SELECT ROGUECOLUMNID
            //DELETE IORECORDS  WHERE ROGUECOLUMNID = ""2480850"" SELECT ROGUECOLUMNID";
            var heyDel = new HQLQuery(qrysDel);
            heyDel.Execute();
            heyDel.PrintQuery();
            //var calcTest = new CalcableGroups("[\"5\" + [\"4\" - \"2\"] + \"6\"] - [\"7\"]", new QueryMetaData());
            // var tmr3 = new Stopwatch();
            // tmr3.Start();
            // var blll = new List<SplitKey>() { SplitKeyTypes.AsSeparator, SplitKeyTypes.colDivider };
            //// tmr3.Stop();

            // SegmentSplitter seg = new SegmentSplitter("BLASH.hey AS YOOO",null, blll);
            // tmr3.Stop();
            // var t = tmr3.ElapsedMilliseconds.ToString();
            //INSERT INTO [-1010] as INS JSON_VALUE(""JSONN"")
            //BinaryDataTable.ioRecordTable.GuessTableIDByName("IORECORDS");
            var testQry4 = @"FROM StockHistory.CryptoCompare.APIRUNS  AS  api 
COMBINE StockHistory.CryptoCompare.DAYINTERVAL  as di JOIN ON di.APIRUNS_OID = api.ROGUECOLUMNID 
WHERE api.CRYPTO_ID = ""ETH""
SELECT api.CRYPTO_ID, api.RUN_TYPE , di.time, di.high, di.low, di.open, di.volumeto, di.close";

            var testQry = @"WITH csvTbl FROM STOCKHISTORY.CRYPTOCOMPARE.DAYINTERVAL 
                                        SELECT time, high, low, open, volumefrom, volumeto, close  CONVERT CSV END
   
            
             FROM csvTbl 
                SELECT ""HEY"", GENERATE_ML_MODEL(csvTbl.CSV_Path, ""close"", ""C:\Users\chris\Desktop\TestML"") ";

            var testQry3 = @"with csvTbl FROM StockHistory.CryptoCompare.APIRUNS  AS  api 
COMBINE StockHistory.CryptoCompare.DAYINTERVAL  as di JOIN ON di.APIRUNS_OID = api.ROGUECOLUMNID 
WHERE api.CRYPTO_ID = ""ETH""
SELECT api.CRYPTO_ID, api.RUN_TYPE , di.time, di.high, di.low, di.open, di.volumeto, di.close CONVERT CSV END 
FROM csvTbl 
 SELECT  GENERATE_ML_MODEL(csvTbl.CSV_Path, ""close"", ""C:\Users\chris\Desktop\TestML"") ";
            
            // SELECT ""HEY"", GENERATE_ML_MODEL(csvTbl.CSV_Path, ""Close"", ""C:\Users\chris\Desktop\TestML"") ";

            var testQry2 = @" FROM IORECORDS AS BUNDLES WHERE MetaRecordType = ""Bundle""
                            SELECT ROGUECOLUMNID, METAROW_NAME, ""BUNDLE""  AS TYP, CURRENT_DATE() 
                                FROM IORECORDS JOIN ON IORECORDS.OwnerIOItem = Bundles.ROGUECOLUMNID WHERE MetaRecordType = ""Database""
                                SELECT ROGUECOLUMNID, METAROW_NAME, ""DATABASE""  AS TYP
                                    FROM IORECORDS  AS TR JOIN ON TR.OwnerIOItem = IORECORDS.RogueColumnID
                                    SELECT ROGUECOLUMNID, METAROW_NAME, ""TABLE""  AS TYP
                                        FROM COLUMN JOIN ON COLUMN.OWNERIOITEM = TR.ROGUECOLUMNID
                                        SELECT COLUMNIDNAME  AS METAROW_NAME, ""COLUMN""  AS TYP ";
            //////var testQry = "FROM DATE_RANGE(\"06-20-2021\", \"day\", \"1\", \"1\") as DR SELECT DR.DATE_ITEM FROM IORECORDS as yo WHERE METAROW_NAME = \"COLUMN\" and NAME_COLUMN_OID != \"\" SELECT METAROW_NAME & \"YOLO\", MetaRecordType FROM COLUMN JOIN ON COLUMN.ROGUECOLUMNID = YO.OWNERIOITEM SELECT [{yo.NAME_COLUMN_OID}]";
            //////var testQry = "FROM DATE_RANGE(\"tseter\") as haha SELECT haha.hyoo, [23] as bll, {[45]} as encodeeee FROM blah where blah.hey = \"yo\" AND blah.yo = \"SDF\" LIMIT 100 COMBINE YO SELECT * FROM BLAHTWOOO JOIN ON blah.yo = BLAHTWOOD.hey SELECT BLAHTWOODs.blahCol INSERT INTO INS JSON_VALUES(\" FROM \") JOIN TO BLAHTWOOO SELECT *";
            ////var stopwatch3 = new Stopwatch();
            ////stopwatch3.Start();
            var test = new HQLQuery(testQry4);
            //////stopwatch3.Stop();
            //////Console.WriteLine(stopwatch3.ElapsedMilliseconds);
            test.Execute();
            test.PrintQuery();
            //test.PrintSegments();
            //string ll = "SDF";
            //var line = " FROM this.B FROMutton FROM .Value\nthiFROMs.value\ndocument.thisButton.Value";
            //var word = "FROM";
            //var rx = new Regex(string.Format(@"(?<=\s)\b{0}\b(?=\s)|\b{0}\b(?=\.)", word));
            ////var result = rx.Replace(line, "NEW_WORD");
            //var result = rx.Matches(line)[0].Index;
            //Console.WriteLine(result);

            ////List<JsonItem> openItems = new List<JsonItem>(); 
            //var cols = Reflector.commandColumnsTypes;
            //Reflector.test();
            //Type myType = cols.First().Value;

            //Activator.CreateInstance<myType>();
            //Object obj = Activator.CreateInstance(asstester, new Object[] { "HEYYY" });
            //var sentiment = "sell all your bitcoin";
            //var tmr = new Stopwatch();
            //tmr.Start();
            //var sentimentClassifier = new SentimentClassifier();
            //tmr.Stop();
            //Console.WriteLine("INIT:" + tmr.ElapsedMilliseconds);
            //tmr.Restart();
            /////// PositiveProbability method returns the positive probability of the sentiment.
            //var positiveProbability = sentimentClassifier.PositiveProbability(sentiment);
            //tmr.Stop();
            //Console.WriteLine("CALC" + tmr.ElapsedMilliseconds);
            //tmr.Restart();
            //var bl = sentimentClassifier.Classify(sentiment);
            //tmr.Stop();
            //Console.WriteLine("CLASSIFY:" + tmr.ElapsedMilliseconds);
            //Console.WriteLine($"Positive Probability of the sentiment { positiveProbability }");


            //var tbl = new SimpleRefInstance();
            //int p = 0;
            //foreach (var row in tbl.StreamDataRows())
            //{
            //    row.PrintRow();
            //    if (p > 20)
            //    {
            //        break;
            //    }
            //    p++;
            //}
            //UpdateModifier.UpdateTableV2();
            //var hey = new SelectHQLStatement("FROM IORECORDS SELECT *");
            //            INSERT INTO BLAH (
            //FROM \"APIINFO\" SELECT CURRENTDATE() AS ToDate, \"CRYPTOCOMPARE\" AS API_SOURCE_NM, \"SEG\" AS API_SEGMENT_NM, \"day\" as RUNTYPE, \"ETH\" AS CRYPTOID
            //  FROM RUN_API() as BLAH JOIN TO APIINFO SELECT FILE_CONTENT(DataFilePath) as DataFile
            //)
            // UpdateModifier.ArchiveRogueInstance();
            //string epochTime = DateHelper.DateToEpoch(DateTime.Now);
           
            //var qryyy = @"DELETE IORECORDS  WHERE ROGUECOLUMNID = ""2387034"" SELECT ROGUECOLUMNID
            //            DELETE IORECORDS  WHERE ROGUECOLUMNID = ""2387030"" SELECT ROGUECOLUMNID";
            //            var qryyy2 = @"DELETE [-1010] WHERE ROGUECOLUMNID = ""2442845"" SELECT ROGUECOLUMNID 
            //                            DELETE [-1010] WHERE ROGUECOLUMNID = ""2461147"" SELECT ROGUECOLUMNID 
            //DELETE [-1010] WHERE ROGUECOLUMNID = ""2480807"" SELECT ROGUECOLUMNID ";
            //            var del = new HQLQuery(qryyy2);
            //            del.Execute();
            //            del.PrintQuery();
            //SELECT EPOCH_TO_DATE(TO_DATE) as TO_DATE, API_SOURCE_NM, CRYPTO_ID, RUN_TYPE
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();



            // performs operations here


            //var qryyy3 = @"FROM STOCKHISTORY.CRYPTOCOMPARE.DAYINTERVAL as DI SELECT DI.open, DI.high, DI.low, DI.close, DI.close - DI.open as totalMovement, [[DI.high-DI.low]/ DI.open] * ""100"" as PercentMovement";
            //var qryyy3 = @"FROM STOCKHISTORY.CRYPTOCOMPARE.APIRUNS AS AR  
            //                COMBINE STOCKHISTORY.CRYPTOCOMPARE.DAYINTERVAL AS DI JOIN ON di.APIRUNS_OID = ar.ROGUECOLUMNID WHERE di.ROGUECOLUMNID != """" AND AR.Run_TYPE = ""day"" SELECT AR.ROGUECOLUMNID AS runID, Di.ROGUECOLUMNID AS rowID,  EPOCH_TO_DATE(AR.TO_DATE) as DATE,AR.Crypto_Id, AR.RUN_TYPE, DI.open, DI.high, DI.low, DI.close, DI.close - DI.open as totalMovement, [[DI.high-DI.low]/ DI.open] * ""100"" as PercentMovement ";

            var qryyy3 = @"FROM STOCKHISTORY.CRYPTOCOMPARE.APIRUNS AS AR  
                            COMBINE STOCKHISTORY.CRYPTOCOMPARE.DAYINTERVAL AS DI JOIN ON di.APIRUNS_OID = ar.ROGUECOLUMNID WHERE di.ROGUECOLUMNID != """" AND AR.Run_TYPE = ""day"" AND AR.CRYPTO_Id = ""ETH"" SELECT AR.Crypto_Id, DI.HIGH, EPOCH_TO_DATE(DI.time) as DATE";
            var hey3 = new HQLQuery(qryyy3);
            //var timer5 = new Stopwatch();
            //var before = System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;
            //timer5.Start();

            hey3.Execute();
            //timer5.Stop();
            //var after = System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;
            //Console.WriteLine("FULLMEMORY:" + (after- before));
            hey3.PrintQuery();
            //var after2 = System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64;
            //Console.WriteLine("FULLMEMORY:" + (after2 - after));
            //var hey2 = new HQLQuery(qryyy2);
            //var hey = new SelectHQLStatement("FROM SEG SELECT *");
            //  var hey = new SelectHQLStatement("INSERT INTO[2069090] as INSTBL JSON_VALUE(\"H:\\Development\\Visaul Studio Code\\RedditAPI\\ApiRuns\\512\\data.json\", \"TESTER5\") SELECT INSTBL.ROGUECOLUMNID");
            //string qryyy = @"FROM ""BLAH"" SELECT  DATE_TO_EPOCH(CURRENT_DATE()) ";
            string qryyy = @"FROM DATE_RANGE(""07-28-2021"", ""day"", ""-1"", ""1"") as DR SELECT DATE_TO_EPOCH(DR.DATE_ITEM) AS To_Date, ""CRYPTOCOMPARE"" AS API_SOURCE_NM, ""CRYPTOHISTORY"" as DEFAULT_TABLE_NM, ""DAY"" as RUN_TYPE, ""ETH"" AS CRYPTO_ID, 
                  FROM RUN_API() as APIResult JOIN TO DR SELECT DataFilePath as DataFile,Owner_Database_ID
                      INSERT INTO [{ APIResult.Owner_Database_ID }] as INSTBL BY JSON_VALUE(APIResult.DataFilePath, APIResult.Default_Table_NM) JOIN TO APIResult SELECT INSTBL.ROGUECOLUMNID";
            //INSERT INTO [-1010] as NewOrExistTable BY DEDUPLICATE(APIResult.Owner_Database_ID as OwnerIOItem, ""API_RUN_LINK"" as MetaRow_Name)
            //    INSERT INTO [{ NewOrExistTable.RogueTableID }] as InDBMetaRow BY DEDUPLICATE(APIResult.To_Date, APIResult.API_SOURCE_NM, APIResult.RUN_TYPE, ApiResult.CRYPTO_ID) ";

            //***WORKING API QUERY FOR FULL DAY ALMOST DONE
            string qryyy22 = @"FROM DATE_RANGE(""07-19-2021"", ""day"", ""-1"", ""1"") as DR
               COMBINE StockHistory.CryptoCompare.RunTypes  as rt JOIN TO DR 
               COMBINE StockHistory.CryptoCompare.CryptoIds as ci JOIN TO DR  SELECT DATE_TO_EPOCH(DR.DATE_ITEM) AS To_Date, ""CRYPTOCOMPARE"" AS API_SOURCE_NM, ""STOCKDATA"" as DEFAULT_TABLE_NM, rt.run_type as RUN_TYPE, ""2000"" as Limit, ci.cryptoId AS CRYPTO_ID, 
                  FROM RUN_API() as APIResult JOIN TO DR SELECT DataFilePath as DataFile,Owner_Database_ID
                      INSERT INTO [{ APIResult.Owner_Database_ID }] as INSTBL BY JSON_VALUE(APIResult.DataFilePath, APIResult.Default_Table_NM) JOIN TO APIResult SELECT INSTBL.ROGUECOLUMNID";
            //INSERT INTO [-1010] as NewOrExistTable BY DEDUPLICATE(APIResult.Owner_Database_ID as OwnerIOItem, ""API_RUN_LINK"" as MetaRow_Name)
            //    INSERT INTO [{ NewOrExistTable.RogueTableID }] as InDBMetaRow BY DEDUPLICATE(APIResult.To_Date, APIResult.API_SOURCE_NM, APIResult.RUN_TYPE, ApiResult.CRYPTO_ID) ";




            ////string qryyy = @"FROM DATE_RANGE(""07-01-2021"", ""day"", ""1"", ""1"") as DR SELECT DR.DATE_ITEM 
            //                    FROM ""APIINFO"" JOIN TO DR SELECT DATE_TO_EPOCH(DR.DATE_ITEM) AS To_Date, ""CRYPTOCOMPARE"" AS API_SOURCE_NM, ""DayInterval"" AS DEFAULT_TABLE_NM, ""day"" as RUN_TYPE, ""ETH"" AS CRYPTO_ID 
            //                        FROM RUN_API() as APIResult JOIN TO APIINFO SELECT DataFilePath as DataFile,Owner_Database_ID
            //                            INSERT INTO [{ APIResult.Owner_Database_ID }] as INSTBL BY JSON_VALUE(APIResult.DataFilePath, APIResult.Default_Table_NM) JOIN TO APIResult SELECT INSTBL.ROGUECOLUMNID";
            var hey = new HQLQuery(qryyy);
            //var hey = new SelectHQLStatement("FROM \"APIINFO\" SELECT DATE_TO_EPOCH(\"06-01-2021\") AS Start_Date, DATE_TO_EPOCH(\"06-09-2021\") AS To_Date, \"NewsAPI\" AS API_SOURCE_NM, \"ApiRuns\" AS API_SEGMENT_NM, \"Articles\" AS TOP_TABLE_NM, \"doge OR dogecoin\" AS SEARCH_TEXT FROM RUN_API() as APIResult JOIN TO APIINFO SELECT DataFilePath as DataFile INSERT INTO[{ APIResult.Owner_Database_ID }] as INSTBL JSON_VALUE(APIResult.DataFilePath, APIResult.Default_Table_NM) JOIN TO APIResult SELECT INSTBL.ROGUECOLUMNID");
            // var hey = new SelectHQLStatement("FROM \"APIINFO\" SELECT CURRENTDATE() AS ToDate, \"cryptocompare\" AS API_SOURCE_NM, \"DayInterval\" AS API_SEGMENT_NM, \"day\" as RUNTYPE, \"ETH\" AS CRYPTOID  FROM RUN_API() as APIResult JOIN TO APIINFO SELECT DataFilePath  INSERT INTO[{ APIResult.Owner_Database_ID }] as INSTBL JSON_VALUE(APIResult.DataFilePath, APIResult.Default_Table_NM) JOIN TO APIResult");
            //var hey = new SelectHQLStatement("FROM \"APIINFO\" SELECT CURRENTDATE() AS ToDate, \"CRYPTOCOMPARE\" AS API_SOURCE_NM, \"SEG\" AS API_SEGMENT_NM, \"day\" as RUNTYPE, \"ETH\" AS CRYPTOID  FROM RUN_API() as BLAH JOIN TO APIINFO SELECT FILE_CONTENT(DataFilePath) as DataFile ");
            //var hey = new SelectHQLStatement(@"FROM COLUMN WHERE OWNERIOITEM  = ""-1011"" SELECT * FROM COLUMNENUMERATIONS AS CE JOIN ON CE.COLUMN_OID = COLUMN.ROGUECOLUMNID SELECT *");
            hey.Execute();
            int i = 0;
            //foreach (var blw in hey.TopRows())
            //{                
            //    Console.WriteLine(i);
            //    i++;
            //}
            hey.PrintQuery();
             string lfl = "SF";
            //var emojis = "👹 in 🇯🇵";
            //var textParts = StringInfo.GetTextElementEnumerator(emojis);
            //while (textParts.MoveNext())
            //{
            //    Console.WriteLine(textParts.Current);
            //}
            //UpdateModifier.TestTemp();
            //string blahhhj = File.ReadAllText(@"H:\Documents\testchar.txt");
            //string bll = "’";
            //var bytedd = bll.ToCharArray()[0];
            //var s = bll.GetHashCode();
            //var wt = new Stopwatch();
            //wt.Start();           
            //UpdateModifier.ConvertToBinaryVOne();
            //UpdateModifier.TestTemp();
            //wt.Stop();
            //Console.WriteLine(wt.ElapsedMilliseconds);
            //UpdateModifier.TestTemp();
            //UpdateModifier.ReadBinary();            
            //ReadAndProcessLargeFile(@"H:\Development\Visaul Studio Code\BinaryTester\tester.bin");          
            //var fl = wt.ElapsedMilliseconds;
            //List<string> strs = new List<string>();
            //foreach (string blddah in File.ReadLines(@"Y:\RogueDatabase\Pure\-1005\-1008\-1003\-1003.rogue"))
            //{
            //    string[] split = blddah.Split("|");
            //    var id = split[0];
            //    var str = split[1];
            //    strs.Add(str);
            //}

            //string qryTest = "FROM COLUMN WHERE ROGUECOLUMNID = \"-1020\" SELECT * FROM COLUMNENUMERATIONS AS CE JOIN ON CE.COLUMNOID = COLUMN.ROGUECOLUMNID SELECT *";
            //APIConnect redditAPI = new APIConnect("reddit", );
            //string qryTest = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\qryTest.txt");
            //string qryTest = "FROM HQLQueries SELECT *";
            //string blahha = "WER";
            //var qryTwo = new SelectHQLStatement(qryTest);
            //qryTwo.Fill();
            //qryTwo.PrintQuery();
            //UpdateModifier.ArchiveRogueInstance();
            //var tm = new DateTime(2019, 2, 5,0,0,0, DateTimeKind.Utc);
            ////TimeSpan diff = tm.ToUniversalTime() - origin;
            ////return Math.Floor(diff.TotalSeconds);
            //int durationSeconds = 14400;
            var apiCon = new APIConnect("cryptocompare", "ETH", DateTime.Today, DateTime.Today.ToShortDateString());
            //for (int i = 0; i < 1000; i++)
            //{
            //    var apiCon = new APIConnect("REDDIT", tm, durationSeconds.ToString());
            //    tm = tm.AddSeconds(-(durationSeconds + 1));
            //    while (apiCon.runStatus == APIConnect.RunStatus.fail)
            //    {
            //        Thread.Sleep(30000);
            //        apiCon = new APIConnect("REDDIT", tm, durationSeconds.ToString());
            //    }
            //}

            //var apiCon = new APIConnect("REDDIT", new DateTime(2019, 2, 5));
            //APIConnect tweetConnect = new APIConnect(URL, tweetParameters, accessToken, accessTokenSecret, consumerKey, consumerSecret);
            //jsonTester test = new jsonTester(tweetConnect);
            //var frm = new From("COLUMN AS TST", new rogue_core.rogueCore.hqlSyntaxV3.TableRefIDs());
            //TestCol("\"YOYO\"");
            //TestCol("IORECORDS.[-1015]");
            //TestCol("METAROW_NAME");
            //TestCol("IORECORDS.METAROW_NAME AS TABLE");
            //TestCol("COLUMN.{IORECORDS.METAROW_NAME} AS encodeName");
            //TestCol("{IORECORDS.METAROW_NAME} AS TABLE");

            //var tst = new Column("COLID.[14]", null);
            //Dictionary<string, IRogueRow> rows = new Dictionary<string, IRogueRow>();
            //var row = new ManualRogueRow(4);
            //row.NewPair(14, "HEY");
            //rows.Add("COLID", row);
            //string testis = tst.RetrieveStringValue(rows);


            //string locTxt = "EXECUTE(.[{COLID.NAME_COLUMN_OID}]";
            //locTxt = locTxt.Trim();
            //var isdis = Regex.IsMatch(locTxt.ToUpper(), "(EXECUTE)(?=(?:[^\"]|\"[^\"]*?\")*?$)");
            //var items = Regex.Split(locTxt, @"\.(?=([^(\]|\}|\"")]*(\[|\{|\"")[^(\[|\{|\"")]*(\]|\}|\""))*[^(\]|\}|\"")]*$)", RegexOptions.ExplicitCapture);
            //var blahs = Regex.Split("PARENTCOLENUM.[{COLID.NAME_COLUMN_OID}] j.j {.} [.] \".\" l.l", @".(?=([^\]|\}|\""]*(\[|\{|\"")[^\[|\{|\""]*(\]|\}|\""))*[^\]|\}|\""]*$)", RegexOptions.ExplicitCapture);
            //var hhha = Regex.Split("PARENTCOLENUM.[{COLID.NAME_COLUMN_OID}]", @"\.(?=([^\]]*(\[)[^\[]*(\]))*[^\]]*$)|(?=([^\}]*(\{)[^\[\{]*(\}))*[^\}]*$)(?=([^\""]*(\"")[^\""]*(\""))*[^\""]*$)", RegexOptions.ExplicitCapture);
            //var ffd = Regex.Matches("PARENTCOLENUM.[{COLID.NAME_COLUMN_OID}]", @"\.(?=([^\]|\}|\""]* (\[|\{|\"")[^\[|\{|\""]*(\]|\}|\""))*[^\]|\}|\""]*$)", RegexOptions.ExplicitCapture);
            //string lastSeg = items[items.Length - 1];
            //bool isConstant = TestAndSet(@"^"".*""?", ref lastSeg);
            //bool isDirectID = TestAndSet(@"^\[.*\]?", ref lastSeg);
            //bool isEncoded = TestAndSet(@"^\{.*\}?", ref lastSeg);
            //bool isUnsetParam = locTxt.Contains("@") ? true : false;
            //Console.SetBufferSize(Console.BufferWidth, 32766);
            //APIConnect api = new APIConnect("https://www.quandl.com/api/v3/datasets/FRED/NROUST.json?api_key=x8LRTeBsGSxjdXjzcfJM", urlParams);
            //Dictionary<string, string> urlParams = new Dictionary<string, string>();
            //urlParams.Add("api_key", "x8LRTeBsGSxjdXjzcfJM");
            //APIConnect apiQuandle = new APIConnect("https://www.quandl.com/api/v3/datasets/FRED/NROUST.json", urlParams);



            //var statement = new HumanHQLStatement("FROM PAGE SELECT * WHERE ROGUECOLUMNID = \"7550\"  	FROM PAGE_OUTLINE JOIN ON PAGE_OUTLINE.ROGUECOLUMNID = PAGE.PAGE_OUTLINE_OID  		FROM HQL_QUERIES AS BASE_QUERY JOIN MERGE BASE_QUERY.ROGUECOLUMNID= PAGE_OUTLINE.HQL_QUERY_OID SELECT QUERY_TXT, BASE_QUERY.ROGUECOLUMNID AS QUERY_ID  			FROM QUERY_PARAM_LNK AS BASE_QUERY_PARAM_LNK JOIN ON BASE_QUERY_PARAM_LNK.HQL_QUERY_OID = BASE_QUERY.ROGUECOLUMNID  				FROM HQL_PARAMETERS AS BASE_PARAMS JOIN MERGE BASE_PARAMS.ROGUECOLUMNID = BASE_QUERY_PARAM_LNK.HQL_PARAMETER_OID SELECT PARM_TXT_ID, DEFAULT_VALUE, ROGUECOLUMNID AS PARAM_OID 	FROM PAGE_SECTION_LNK JOIN ON PAGE_SECTION_LNK.PAGE_OID = PAGE.PAGE_OUTLINE_OID  		FROM SECTION JOIN MERGE SECTION.ROGUECOLUMNID = PAGE_SECTION_LNK.SECTION_OID SELECT SECTION_NM, SECTION_PARAM_TXT, ROGUECOLUMNID AS SECTION_OID			FROM SECTION_QUERY_LNK JOIN MERGE SECTION_QUERY_LNK.SECTION_OID = SECTION.ROGUECOLUMNID WHERE PAGE_OID = PAGE.ROGUECOLUMNID  				FROM HQL_QUERIES JOIN MERGE HQL_QUERIES.ROGUECOLUMNID = SECTION_QUERY_LNK.HQL_QUERY_OID SELECT QUERY_TXT, HQL_QUERIES.ROGUECOLUMNID AS QUERY_ID  					FROM QUERY_PARAM_LNK JOIN ON QUERY_PARAM_LNK.HQL_QUERY_OID = HQL_QUERIES.ROGUECOLUMNID  						FROM HQL_PARAMETERS JOIN MERGE HQL_PARAMETERS.ROGUECOLUMNID= QUERY_PARAM_LNK.HQL_PARAMETER_OID SELECT PARM_TXT_ID, DEFAULT_VALUE, ROGUECOLUMNID AS PARAM_OID");
            //statement.LoadRows(null);
            //Console.WriteLine("Hello World!");
            //UIWebPage pageBuilder = new UIWebPage(8455);
            //string html = pageBuilder.BuildHTML();

            //var qryOne = new HumanHQLStatement("FROM SECTION_QUERY_LNK SELECT *");
            //qryOne.LoadRows(null);
            //string answer = CodeCaller.RunProcedure("FORMATTED_QRY_TEXT", new string[] { "YO" });
            // Type assem = Type.GetType("FD");
            //Assembly co = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "rogue_core");
            //var final = co.GetType("rogue_core.rogueCore.rogueUI.CodeCaller");
            // AssemblyName myAssembly = AssemblyName.GetAssemblyName("rogue_core");

            //var bl2 = Type.GetType("rogue_core.rogueCore.rogueUI.CodeCaller, rogue_core");
            /// string answer = InvokeStringMethod(final, "FORMATTED_QRY_TEXT", new string[] { "yo" });
            //string answer = InvokeStringMethod("rogue_core.rogueCore.rogueUI.CodeCaller", "FORMATTED_QRY_TEXT");
            // Type testType = Type.GetType("rogue_core.rogueCore.rogueUI");
            // MethodInfo toInvoke = testType.GetMethod("FORMATTED_QRY_TEXT", BindingFlags.Static | BindingFlags.Public);
            //var vla = toInvoke.Invoke(null, new object[] { "Goodbye, world!" });
            // //methodInfo.Invoke(classInstance, parametersArray);
            // string ret = CodeCaller.FORMATTED_QRY_TEXT("YO");
            //*TODO Need to swap Rows() when ecnoded to use parentRow. For now assumes joinAll so others would break if encoded query is used. Print to see results
            // string blah = "FROM IORECORDS AS COLID SELECT NAME_COLUMN_OID WHERE NAME_COLUMN_OID != \"\" FROM [{COLID.ROGUECOLUMNID}] AS PARENTCOLENUM JOIN ON * = COLID.ROGUECOLUMNID SELECT [{COLID.NAME_COLUMN_OID}] as COL_TXT, PARENTCOLENUM.ROGUECOLUMNID AS VALUEID";
            string blah = File.ReadAllText("Y:\\RogueDatabase\\HQLDeveloper\\qryTest.txt");
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            var qry = new SelectHQLStatement(blah);
            //var qry = new FilledHQLQuery(blah);
            //stopwatch.Stop();
            ////Console.WriteLine("Load: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Restart();
            //stopwatch.Start();//
            qry.Fill();
            //qry.PrintQuery();
            //var top = qry.GetUIQueryResults();
            //UIWebSection uitest = new UIWebSection(blah);
            //uitest.BuildQueryDecor();
            //string hhttml = uitest.finalHTML;
            stopwatch.Stop();
            Console.WriteLine("QUERYTIMER: " + stopwatch.ElapsedMilliseconds);
            //stopwatch.Restart();
            //qry.IterateRows();
            //stopwatch.Stop();
            //Console.WriteLine("IterationTimer: " + stopwatch.ElapsedMilliseconds);
            qry.PrintQuery(); // 1826, 1930, 2075
            //1978, 2008, 2276
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            UIWebSection webtest = new UIWebSection(blah);
            webtest.BuildSection();
            string httml = webtest.finalHTML;
            string bleeah = "SDF";
            //creates and start the instance of Stopwatch
            //var qry2 = HumanHQLStatement.StoredProcByID(7457);
            //*TableSelect
            //var qry2 = HumanHQLStatement.StoredProcByID(7609);
            //qry2 = qry2.Replace("@TABLEID", "-1010");
            ////var qry2 = HumanHQLStatement.StoredProcByID(7568);
            //var runQry = new HumanHQLStatement(qry2);

            stopwatch2.Stop();
            Console.WriteLine(stopwatch2.ElapsedMilliseconds);
            //runQry.PrintResults();
            //qry.PrintQuery();
            //qry.PrintResults();
            UIWebPage page = new UIWebPage(8455);
            page.BuildHTML();
            Dictionary<string, string> twtParams = new Dictionary<string, string>();
            //urlParams.Add("api_key", "x8LRTeBsGSxjdXjzcfJM");

            string consumerKey = "yiUAbDbDWhzD0u67ARqlHt2wb";
            string consumerSecret = "SPGtW2rz7dNP8Qj0B4Rzt60yLFRHX2CuIDhFChCxS7EFnInNwF";
            string accessToken = "732600599200555008-l4eMlTCxF7y7YNbZCKIgc9kj2OXv5r6";
            string accessTokenSecret = "kthDsFRVaOAIx4eApKuiFJrwANf2TGNNOiikQbyiH01Za";

            string URL = "https://api.twitter.com/1.1/search/tweets.json";
            Dictionary<string, string> tweetParameters = new Dictionary<string, string>() { { "q", "lang:en" }, { "tweet_mode", "extended" } };
            APIConnect tweetConnect = new APIConnect(URL, tweetParameters, accessToken, accessTokenSecret, consumerKey, consumerSecret);
            //jsonTester test = new jsonTester(tweetConnect, new KeyValuePair<string, string>(), new Dictionary<string, string>());
            //var qry = new HumanHQLStatement("FROM SECTION_QUERY_LNK SELECT *");
            //qry.LoadRows(null);
        }
        const int megabyte = 1024 * 1024;
        public static List<KeyValuePair<int,KeyValuePair<string,string>>> WhereSplit(string splitTxt)
        {
            const string start = "(";
            const string end = "(";
            

            var grps = new List<KeyValuePair<int, KeyValuePair<string, string>>>();
            string currTxt = splitTxt;
            int currLevel = 0;
            int currGrp = 0;
            List<KeyValuePair<string, string>> splits = new List<KeyValuePair<string, string>>();
            foreach(var item in splits)
            {
                if(item.Value.StartsWith(start) && currTxt.EndsWith(end))
                {
                    currLevel++;
                    grps.Add(new KeyValuePair<int, KeyValuePair<string,string>>(currLevel, item));
                    currLevel--;
                }
                if (item.Value.StartsWith(start))
                {
                    currLevel++;
                    grps.Add(new KeyValuePair<int, KeyValuePair<string, string>>(currLevel, item));
                }
                if (item.Value.EndsWith(end))
                {
                    grps.Add(new KeyValuePair<int, KeyValuePair<string, string>>(currLevel, item));
                    currLevel--;
                }
                else
                {
                    grps.Add(new KeyValuePair<int, KeyValuePair<string, string>>(currLevel, item));
                }
            }
            return grps;
        }
        public static void ReadAndProcessLargeFile(string theFilename, long whereToStartReading = 0)
        {
            FileStream fileStram = new FileStream(theFilename, FileMode.Open, FileAccess.Read);
            using (fileStram)
            {
                byte[] buffer = new byte[megabyte];
                fileStram.Seek(whereToStartReading, SeekOrigin.Begin);
                int bytesRead = fileStram.Read(buffer, 0, megabyte);
                while (bytesRead > 0)
                {
                    ProcessChunk(buffer, bytesRead);
                    bytesRead = fileStram.Read(buffer, 0, megabyte);
                }

            }
        }

        private static void ProcessChunk(byte[] buffer, int bytesRead)
        {
            // Do the processing here
        }
        static bool TestAndSet(string RegexPattern, ref string locTxt)
        {
            if (Regex.IsMatch(locTxt, RegexPattern))
            {
                locTxt = locTxt.Substring(1, locTxt.Length - 2);
                return true;
            }
            else
            {
                return false;
            }
        }
        //static void TestCol(string colTxt)
        //{
        //    var qry = new SelectHQLStatement();
        //    //TableRefIDs tableRefs = new TableRefIDs();            
        //    qry.tableRefIDs.AddTableRef("TABLE", -1010);
        //    qry.tableRefIDs.AddTableRef("COLID", -1010);
        //    qry.tableRefIDs.AddTableRef("IORECORDS", -1010);
        //    qry.tableRefIDs.AddTableRef("COLUMN", -1011);
        //    var tst = BaseLocation.LocationType(colTxt, qry);
        //    Dictionary<string, IRogueRow> rows = new Dictionary<string, IRogueRow>();
        //    var row = new ManualRogueRow(4);
        //    row.Add(row.NewPair(-1015, "OwnerIOItem"));

        //    var rowCol = new ManualRogueRow(6);
        //    rowCol.Add(rowCol.NewPair(-1024, "YEEP"));

        //    var rowColID = new ManualRogueRow(8);
        //    rowColID.Add(rowColID.NewPair(-1024, "COLIDVAL"));

        //    rows.Add("COLUMN", rowCol);
        //    rows.Add("IORECORDS", row);
        //    rows.Add("COLID", rowColID);
        //    string testis = tst.RetrieveStringValue(rows);
        //}
        //void RunLocationTestScenerios()
        //{
        //    var frm = new From("COLUMN AS TST", new SelectHQLStatement());
        //    TestCol("\"YOYO\"");
        //    TestCol("IORECORDS.[-1015]");
        //    TestCol("METAROW_NAME");
        //    TestCol("IORECORDS.METAROW_NAME AS TABLE");
        //    TestCol("COLUMN.{IORECORDS.METAROW_NAME} AS encodeName");
        //    TestCol("{IORECORDS.METAROW_NAME} AS TABLE");
        //}
        public static string InvokeStringMethod(string typeName, string methodName)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            // Invoke the method itself. The string returned by the method winds up in s
            String s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public |
                                BindingFlags.Static,
                            null,
                            null,
                            null);

            // Return the string that was returned by the called method.
            return s;
        }
        public static string InvokeStringMethod(Type calledType, string methodName, string[] parameters)
        {
            // Get the Type for the class
            //Type calledType = Type.GetType(typeName);
            MethodInfo methodInfo = calledType.GetMethod(methodName);
            return (string)methodInfo.Invoke(null, parameters);
            // Invoke the method itself. The string returned by the method winds up in s
            String s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public |
                                BindingFlags.Static, null, null, null);

            // Return the string that was returned by the called method.
            return s;
        }
    }
}
