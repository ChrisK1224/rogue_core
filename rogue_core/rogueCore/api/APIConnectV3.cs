using rogue_core.rogueCore.bundle;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.apiV3.formats.json;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.bundle.presetBundle;

namespace rogueCore.apiV3
{
public class APIConnect
    {
        const string apiBridgePath = @"C:\Users\chris\Documents\Development\RedditAPI\ApiBridge.py";
        public Stream stream;
        public StreamReader stream_read;
        public String con_str;
        int storedQueryOID = 4;
        RogueBundle apiBundle;
        internal RogueDatabase<DataRowID> apiDB;
        internal string segment_NM;
        internal string source_NM;
        public IORecordID ghostTableID { get; }
        public RunStatus runStatus { get; private set; } = RunStatus.fail;
        public string resultPath { get; } = "";
        public string database_ID { get { return apiDB.ioItemID.ToString(); } }
        //*SQL CONNECTION
        public APIConnect(IMultiRogueRow parentRow)
        {
            var apiTmr = new Stopwatch();
            apiTmr.Start();
            string type = parentRow.GetValue(ConnectInfoCols.API_SOURCE_NM.ToString());
            string segmentNM = parentRow.GetValue(ConnectInfoCols.API_SEGMENT_NM.ToString());
            var lst = parentRow.GetValueList().ToList();
            var apiMetaDataTbl = new BinaryDataTable(39889);
            IRogueRow runInstanceRow = apiMetaDataTbl.NewWriteRow();
            this.source_NM = segmentNM;
            //lst.Add(new KeyValuePair<string, string>("TOP_TABLE_NM", segmentNM));
            lst.Add(new KeyValuePair<string, string>("API_RUN_INSTANCE_OID", runInstanceRow.rowID.ToString()));
            var stringList = DictionaryToArray(lst);
            ghostTableID = 2076411;
            //List<KeyValuePair<string, string>> runParams = new List<KeyValuePair<string, string>>();
            switch (type.ToLower())
            {
                case "robinhood":
                    apiDB = new RogueDatabase<DataRowID>(2069081);
                    apiBundle = new RogueBundle(2069078);
                    this.segment_NM = segmentNM;
                    this.source_NM = segmentNM;
                    resultPath = run_cmd(apiBridgePath, new string[] { "robinhood", "crypto_history", "ETH", "week", "5minute" });
                    break;
                case "reddit":
                    apiDB = new RogueDatabase<DataRowID>(39853);
                    apiBundle = new RogueBundle(39846);
                    string redditType = "submission";
                    var beforeDate = DateTime.Parse(parentRow.GetValue("BEFOREDATE"));
                    this.source_NM = segmentNM;
                    string duration = parentRow.GetValue("DURATION");
                    string epochTim = new DateTimeOffset(beforeDate).ToUnixTimeSeconds().ToString();
                    resultPath = run_cmd(apiBridgePath, new string[] { "reddit", "https://api.pushshift.io/reddit/search/submission?subreddit=wallstreetbets", epochTim, duration, redditType });
                    break;
                case "newsapi":
                    apiBundle = new RootBundle().GetBundle("NewsConnect", "This contains all databases relating to News Api");
                    apiDB = apiBundle.GetDatabase<DataRowID>("NewsApi", "The Database for the News API contents. Currently 100 calls per day brings in articles from multiple sources");
                    //this.source_NM = "Article";
                    //segmentNM = "Article";
                    //resultPath = run_cmd(@"H:\Development\Visaul Studio Code\RedditAPI\ApiBridge.py", new string[] { type, "bitcoin", DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd") });
                    resultPath = run_cmd(apiBridgePath, stringList);
                    break;
                case "alphavantage":
                    apiBundle = new RootBundle().GetBundle("StockHistory", "Holds different sources for history of crypto and stock values");
                    apiDB = apiBundle.GetDatabase<DataRowID>("AlphaVantage", "Api to get historical crypto values");
                    this.source_NM = "PricePointDate";
                    segmentNM = "PricePointDate";
                    resultPath = run_cmd(apiBridgePath, new string[] { type, "bitcoin", DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd") });
                    break;
                case "cryptocompare":
                    apiBundle = new RootBundle().GetBundle("StockHistory", "Holds different sources for history of crypto and stock values");
                    apiDB = apiBundle.GetDatabase<DataRowID>("CryptoCompare", "Detailed api for getting crypto details and apparantly can do sockets for current prices.");
                    this.source_NM = "ApiRuns";
                    segmentNM = "ApiRuns";
                    //runParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("BaseTableName", type), new KeyValuePair<string, string>("Coin", "ETH"), new KeyValuePair<string, string>("ToDate", DateTime.Today.ToString("yyyy-MM-dd")), new KeyValuePair<string, string>("RunTimeFrame", "day") };
                    //resultPath = run_cmd(@"H:\Development\Visaul Studio Code\RedditAPI\ApiBridge.py", new string[] { type, "ETH", DateTime.Today.ToString("yyyy-MM-dd"), "day" });
                    resultPath = run_cmd(apiBridgePath, stringList);
                    break;
            }
            LogApiMetaRow(resultPath, apiTmr.ElapsedMilliseconds.ToString(), runInstanceRow);
            apiMetaDataTbl.Write();
            this.stream_read = new StreamReader(DataFilePath(resultPath));
            this.segment_NM = segmentNM;
            var finalParams = new Dictionary<string, string>();
            parentRow.GetValueList().ToList().ForEach(x => finalParams.Add(x.Key, x.Value));
            //jsonTester jsonRunner = new jsonTester(this, finalParams);
        }
        internal APIConnect(rogue_core.rogueCore.hqlSyntaxV4.IMultiRogueRow parentRow)
        {
            var apiTmr = new Stopwatch();
            apiTmr.Start();
            string type = parentRow.GetValue(ConnectInfoCols.API_SOURCE_NM.ToString());
            string segmentNM = parentRow.GetValue(ConnectInfoCols.API_SEGMENT_NM.ToString());
            var lst = parentRow.GetValueList().ToList();
            var apiMetaDataTbl = new BinaryDataTable(39889);
            IRogueRow runInstanceRow = apiMetaDataTbl.NewWriteRow();
            this.source_NM = segmentNM;
            //lst.Add(new KeyValuePair<string, string>("TOP_TABLE_NM", segmentNM));
            lst.Add(new KeyValuePair<string, string>("API_RUN_INSTANCE_OID", runInstanceRow.rowID.ToString()));
            var stringList = DictionaryToArray(lst);
            ghostTableID = 2076411;
            //List<KeyValuePair<string, string>> runParams = new List<KeyValuePair<string, string>>();
            switch (type.ToLower())
            {
                case "robinhood":
                    apiDB = new RogueDatabase<DataRowID>(2069081);
                    apiBundle = new RogueBundle(2069078);
                    this.segment_NM = segmentNM;
                    this.source_NM = segmentNM;
                    resultPath = run_cmd(apiBridgePath, new string[] { "robinhood", "crypto_history", "ETH", "week", "5minute" });
                    break;
                case "reddit":
                    apiDB = new RogueDatabase<DataRowID>(39853);
                    apiBundle = new RogueBundle(39846);
                    string redditType = "submission";
                    var beforeDate = DateTime.Parse(parentRow.GetValue("BEFOREDATE"));
                    this.source_NM = segmentNM;
                    string duration = parentRow.GetValue("DURATION");
                    string epochTim = new DateTimeOffset(beforeDate).ToUnixTimeSeconds().ToString();
                    resultPath = run_cmd(apiBridgePath, new string[] { "reddit", "https://api.pushshift.io/reddit/search/submission?subreddit=wallstreetbets", epochTim, duration, redditType });
                    break;
                case "newsapi":
                    apiBundle = new RootBundle().GetBundle("NewsConnect", "This contains all databases relating to News Api");
                    apiDB = apiBundle.GetDatabase<DataRowID>("NewsApi", "The Database for the News API contents. Currently 100 calls per day brings in articles from multiple sources");
                    //this.source_NM = "Article";
                    //segmentNM = "Article";
                    //resultPath = run_cmd(@"H:\Development\Visaul Studio Code\RedditAPI\ApiBridge.py", new string[] { type, "bitcoin", DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd") });
                    resultPath = run_cmd(apiBridgePath, stringList);
                    break;
                case "alphavantage":
                    apiBundle = new RootBundle().GetBundle("StockHistory", "Holds different sources for history of crypto and stock values");
                    apiDB = apiBundle.GetDatabase<DataRowID>("AlphaVantage", "Api to get historical crypto values");
                    this.source_NM = "PricePointDate";
                    segmentNM = "PricePointDate";
                    resultPath = run_cmd(apiBridgePath, new string[] { type, "bitcoin", DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd") });
                    break;
                case "cryptocompare":
                    apiBundle = new RootBundle().GetBundle("StockHistory", "Holds different sources for history of crypto and stock values");
                    apiDB = apiBundle.GetDatabase<DataRowID>("CryptoCompare", "Detailed api for getting crypto details and apparantly can do sockets for current prices.");
                    this.source_NM = "ApiRuns";
                    segmentNM = "ApiRuns";
                    //runParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("BaseTableName", type), new KeyValuePair<string, string>("Coin", "ETH"), new KeyValuePair<string, string>("ToDate", DateTime.Today.ToString("yyyy-MM-dd")), new KeyValuePair<string, string>("RunTimeFrame", "day") };
                    //resultPath = run_cmd(@"H:\Development\Visaul Studio Code\RedditAPI\ApiBridge.py", new string[] { type, "ETH", DateTime.Today.ToString("yyyy-MM-dd"), "day" });
                    resultPath = run_cmd(apiBridgePath, stringList);
                    break;
            }
            LogApiMetaRow(resultPath, apiTmr.ElapsedMilliseconds.ToString(), runInstanceRow);
            apiMetaDataTbl.Write();
            this.stream_read = new StreamReader(DataFilePath(resultPath));
            this.segment_NM = segmentNM;
            var finalParams = new Dictionary<string, string>();
            parentRow.GetValueList().ToList().ForEach(x => finalParams.Add(x.Key, x.Value));
            //jsonTester jsonRunner = new jsonTester(this, finalParams);
        }
        public APIConnect(IMultiRogueRow qryConnectInfo, string old)
        {
            //string apiMetaDataQry = FilledHQLQuery.GetQueryByID(storedQueryOID);
            //apiMetaDataQry = apiMetaDataQry.Replace("@API_SEGMENT_OID", segment_oid.ToString());
            //MultiRogueRow qryConnectInfo = new FilledHQLQuery(apiMetaDataQry).Fill().TopRows().First();
            string apiURL = qryConnectInfo.GetValue(ConnectInfoCols.BASE_URL.ToString());
            apiURL += qryConnectInfo.GetValue(ConnectInfoCols.URL_EXTENSION.ToString());
            apiBundle = new RogueBundle(new IORecordID(qryConnectInfo.GetValue(ConnectInfoCols.BUNDLE_OID.ToString())));
            segment_NM = qryConnectInfo.GetValue(ConnectInfoCols.API_SEGMENT_NM.ToString());
            source_NM = qryConnectInfo.GetValue(ConnectInfoCols.API_SOURCE_NM.ToString());
            apiURL += "?";
            foreach(MultiRogueRow paramRow in qryConnectInfo.childRows)
            {
                apiURL += paramRow.GetValue(paramInfoCols.param_nm.ToString()) + "=";
                apiURL += paramRow.GetValue(paramInfoCols.param_val.ToString()) + "&";
            }
            apiURL = apiURL.Substring(0, apiURL.Length - 1);
            apiDB = apiBundle.GetDatabase<DataRowID>(source_NM);
            WebRequest request = WebRequest.Create(apiURL);
            WebResponse response = request.GetResponse();
            stream = response.GetResponseStream();
            stream_read = new StreamReader(stream);
            //jsonTester test = new jsonTester(this, new KeyValuePair<string, string>(), new Dictionary<string, string>());
        }
        //public APIConnect(int segment_oid)
        //{
        //    string apiMetaDataQry = FilledHQLQuery.GetQueryByID(storedQueryOID);
        //    apiMetaDataQry = apiMetaDataQry.Replace("@API_SEGMENT_OID", segment_oid.ToString());
        //    MultiRogueRow qryConnectInfo = new FilledHQLQuery(apiMetaDataQry).Fill().TopRows().First();
        //    string apiURL = qryConnectInfo.GetValue(ConnectInfoCols.BASE_URL.ToString());
        //    apiURL += qryConnectInfo.GetValue(ConnectInfoCols.URL_EXTENSION.ToString());
        //    WebRequest request = WebRequest.Create(apiURL);
        //    WebResponse response = request.GetResponse();
        //    stream = response.GetResponseStream();
        //    stream_read = new StreamReader(stream);
        //    jsonTester test = new jsonTester(this);
        //}
        enum ConnectInfoCols
        {
            API_SOURCE_NM, BASE_URL, URL_EXTENSION, BUNDLE_OID, API_SEGMENT_NM
        }
        enum paramInfoCols
        {
            param_nm, param_val, is_optional
        }
        public enum RunStatus
        {
            success,
            fail
        }
        public APIConnect(String server_address, String user, string password, string database)
        {
            con_str = "Server = @address; Database = @database; User Id = @user; Password = @pass;";
            con_str = con_str.Replace("@address", server_address).Replace("@database", database).Replace("@user", user).Replace("@pass", password);
           
        }
        public APIConnect(String url, Dictionary<string,string> urlParams, String accessToken = "",String accessSecret = "",String consumerKey = "",String consumerSecret = "")
        {
            //string consumerKey = "yiUAbDbDWhzD0u67ARqlHt2wb";
            //string consumerSecret = "SPGtW2rz7dNP8Qj0B4Rzt60yLFRHX2CuIDhFChCxS7EFnInNwF";
            //string accessToken = "732600599200555008-l4eMlTCxF7y7YNbZCKIgc9kj2OXv5r6";
            //string accessTokenSecret = "kthDsFRVaOAIx4eApKuiFJrwANf2TGNNOiikQbyiH01Za";


            //string URL = "https://api.twitter.com/1.1/search/tweets.json";
            //Dictionary<string, string> parameters = new Dictionary<string, string>() { { "q", "lang:en" }, { "tweet_mode", "extended" } };
            WebRequest request = CreateRequest(consumerKey, consumerSecret, accessToken, accessSecret, "GET", url, urlParams);
            WebResponse response = request.GetResponse();
            stream = response.GetResponseStream();
            stream_read = new StreamReader(stream);
            //String full_text = stream_read.ReadToEnd();
        }
        public APIConnect(String url, String api_key)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12;
            WebClient web = new WebClient();
            stream = web.OpenRead("https://www.quandl.com/api/v3/datasets/FRED/NROUST.json?api_key=x8LRTeBsGSxjdXjzcfJM");
            stream_read = new StreamReader(stream);
            //String full_url = url + "?api_key=" + api_key;
            //stream = web.OpenRead(full_url);
        }
        public APIConnect(string reddit, DateTime beforeDate, string duration)
        {
            apiDB = new RogueDatabase<DataRowID>(39853);
            apiBundle = new RogueBundle(39846);
            source_NM = "Reddit";
            //int x = 1;
            //int y = 2;
            //string progToRun = "C:\\Python27\\hello.py";
            //char[] splitter = { '\r' };
            var apiTmr = new Stopwatch();
            apiTmr.Start();
            string redditType = "submission";
            //86400
            //string blah = 
            //DateTimeOffset dto = new DateTimeOffset(1970, 1, 1, 0, 0, 0,, TimeSpan.Zero);
            string epochTim = new DateTimeOffset(beforeDate).ToUnixTimeSeconds().ToString();
            string resultPath = run_cmd(@"H:\Development\Visaul Studio Code\RedditAPI\ApiBridge.py", new string[] { "reddit", "https://api.pushshift.io/reddit/search/submission?subreddit=wallstreetbets", epochTim, duration, redditType });
            apiTmr.Stop();           
            //RowID apiMetaRowID = apiDB.new LogApiMetaRow(resultPath, apiTmr.ElapsedMilliseconds.ToString());
            //jsonTester jsonRunner = new jsonTester(this);
            //foreach(string path in Directory.EnumerateFiles(resultPath))
            //{
            //this.stream_read = new StreamReader(DataFilePath(resultPath));
            //this.segment_NM = redditType;
            //jsonTester jsonRunner = new jsonTester(this, new KeyValuePair<string, string>("api_run_instance_oid", apiMetaRowID.ToString()), new Dictionary<string, string>());
            //}
            //*DateTimeOffset.Now.ToUnixTimeSeconds();
            //string ans = RunPythonProc(@"H:\Development\Visaul Studio Code\RedditAPI\RedditApiRunner.py", "");
            //Process proc = new Process();
            //proc.StartInfo.FileName = "python.exe";
            //proc.StartInfo.RedirectStandardOutput = true;
            //proc.StartInfo.UseShellExecute = false;

            //// call hello.py to concatenate passed parameters
            //proc.StartInfo.Arguments = string.Concat(progToRun, " ", x.ToString(), " ", y.ToString());
            //proc.Start();

            //StreamReader sReader = proc.StandardOutput;
            //string[] output = sReader.ReadToEnd().Split(splitter);

            //foreach (string s in output)
            //    Console.WriteLine(s);

            //proc.WaitForExit();
            //Console.ReadLine();
            //WebRequest request;
            //if (method == "GET")
            //    request = WebRequest.Create(string.Format("{0}?{1}", url, encodedParams));
            //else
            //WebRequest request = WebRequest.Create("https://www.reddit.com/api/v1/access_token");
            //request.Method = "POST";?
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.Headers.Add("Authorization", "Basic aVpld0ZscXYxLWRFZmc6S3JhZzdYTzRaWjJockpKVjR0QWtuNmxvY2pCRE1n");
            //request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //request.Headers.Add("Cookie", "edgebucket=0EQAaIGgyGtxtvPenu; token_v2=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MTIzMzA4MDAsInN1YiI6Ii1td2Q2djJyQVhOMjFtSVI3Mms1enoySnM1MXMiLCJsb2dnZWRJbiI6ZmFsc2UsInNjb3BlcyI6WyIqIiwiZW1haWwiXX0.6RjkoiQzDW3YwVUkDrREvWtEzZtmdJkz6XH3AVa7DdM");
            //request.Headers.Add("grant_type", "password");
            //request.Headers.Add("username", "theshoveler12");
            //request.Headers.Add("password", "shallNotpass12");
            //var handler = new HttpClientHandler();
            //handler.UseCookies = false;
            //string tokenTxt;
            //using (var httpClient = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://www.reddit.com/api/v1/access_token"))
            //    {
            //        request.Headers.TryAddWithoutValidation("Authorization", "Basic aVpld0ZscXYxLWRFZmc6S3JhZzdYTzRaWjJockpKVjR0QWtuNmxvY2pCRE1n");
            //        request.Headers.TryAddWithoutValidation("Cookie", "edgebucket=0EQAaIGgyGtxtvPenu; token_v2=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MTIzMzA4MDAsInN1YiI6Ii1td2Q2djJyQVhOMjFtSVI3Mms1enoySnM1MXMiLCJsb2dnZWRJbiI6ZmFsc2UsInNjb3BlcyI6WyIqIiwiZW1haWwiXX0.6RjkoiQzDW3YwVUkDrREvWtEzZtmdJkz6XH3AVa7DdM");

            //        var contentList = new List<string>();
            //        contentList.Add($"grant_type={Uri.EscapeDataString("password")}");
            //        contentList.Add($"username={Uri.EscapeDataString("theshoveler12")}");
            //        contentList.Add($"password={Uri.EscapeDataString("shallNotpass12")}");
            //        request.Content = new StringContent(string.Join("&", contentList));
            //        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            //        Task<HttpResponseMessage> t = httpClient.SendAsync(request);
            //        t.Wait();
            //        var finalResponse = t.Result;
            //        var jsonToken = finalResponse.Content.ReadAsStringAsync();
            //        var options = new JsonDocumentOptions
            //        {
            //            AllowTrailingCommas = true
            //        };
            //        JsonDocument document = JsonDocument.Parse(jsonToken.Result, options);
            //        tokenTxt = document.RootElement.GetProperty("access_token").GetString();

            //        //URL https://oauth.reddit.com

            //        //{
            //        //    JsonProperty element in document.RootElement.GetString("")
            //        //    //{
            //        //        var result = element.
            //        //if (date.DayOfWeek == DayOfWeek.Monday)
            //        //{
            //        //    int temp = element.GetProperty("temp").GetInt32();
            //        //    sumOfAllTemperatures += temp;
            //        //    count++;
            //        //}
            //        //}

            //        //var averageTemp = (double)sumOfAllTemperatures / count;
            //        //return averageTemp;
            //        //string token;
            //        //}
            //    }

            //}
            //***BEGIN WORKING EX OF REDDIT C#
            //string hotTakeUrl = "https://oauth.reddit.com/r/wallstreetbets/hot/.json?limit=50";
            //string token = GetRedditToken();
            //string currCount = "2";
            //string afterToken = "";
            //List<string> titles = new List<string>();
            //int fullCount = 0;
            //while(currCount != "0")
            //{
            //    string hotTakes = RunRedditGet(hotTakeUrl, token, afterToken);
            //    var obj = (JObject.Parse(hotTakes))["data"];
            //    afterToken = obj["after"].ToString();
            //    currCount = obj["dist"].ToString();
            //    fullCount += int.Parse(currCount);                

            //    foreach (var ren in obj["children"])
            //    {
            //        var post = ren["data"];
            //        string created_utc = post["created_utc"].ToString();
            //        string id = post["id"].ToString();
            //        titles.Add(post["title"].ToString());
            //        Console.WriteLine("TITLE:" + post["title"].ToString());
            //        string commentJson = RunRedditGet("https://oauth.reddit.com/r/wallstreetbets/comments/" + id +  ".json", token);//v
            //        string commenJson = RunRedditGet("https://oauth.reddit.com/r/wallstreetbets/comments/gmrihna.json", token);//v
            //        //var firstData = JObject.Parse(hotTakes).Values(1);
            //        //foreach (var comObj in JArray.Parse(hotTakes))
            //        //{
            //        //    string commentTxt = comObj["data"]["body"].ToString();
            //        //    Console.WriteLine("---Comment: " + commentTxt);
            //        //}
            //        //var bll = firstData["data"];
            //        //foreach (var comObj in bll["chldren"])
            //        //{
            //        //    string commentTxt = comObj["data"]["body"].ToString();
            //        //    Console.WriteLine("---Comment: " + commentTxt);
            //        //}
            //        foreach (var comObj in JArray.Parse(commentJson)[1]["data"]["children"])
            //        {
            //            var dataComObj = comObj["data"];
            //            if (dataComObj["body"] != null)
            //            {
            //                string commentTxt = dataComObj["body"].ToString();
            //                Console.WriteLine("---Comment: " + commentTxt);
            //            }
            //            //if["body"].ToString();                        
            //        }
            //    }
            //}
            //***END WORKING EX OF REDDIT C#
            //string inii = "SF";
            //var afterToken = ((JObject.Parse(hotTakes))["data"]["after"]).ToString();
            //JToken afterTken = obj["data"]["after"];
            //string afterID = token.ToString();
            // string hotTakes2 = RunRedditGet(hotTakeUrl, token, afterToken);
            //string ll = hotTakes2;
            //HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("https://oauth.reddit.com/r/wallstreetbets/hot/.json?limit=5");
            //request2.Method = "GET";
            //request2.Accept = "application/json";
            //request2.UserAgent = "api/v1/me";
            //request2.Headers.Add("Authorization", "bearer " + tokenTxt);
            //request2.ContentType = "application/x-www-form-urlencoded";
            //var response = request2.GetResponse();
            //string text;
            //string after;
            //using (var sr = new StreamReader(response.GetResponseStream()))
            //{
            //    text = sr.ReadToEnd();
            //    JObject obj = JObject.Parse(text);
            //    JToken token = obj["data"]["after"];
            //    after= token.ToString();
            //    Console.WriteLine(token.ToString());
            //}
            //string ll = "SDF";
            //HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create("https://oauth.reddit.com/r/wallstreetbets/hot/.json?limit=2&after=" + after);
            //request3.Method = "GET";
            //request3.Accept = "application/json";
            //request3.UserAgent = "r/wallstreetbets/hot/.json?limit=4";
            //request3.Headers.Add("Authorization", "bearer " + tokenTxt);
            //request3.ContentType = "application/x-www-form-urlencoded";
            //var response2 = request3.GetResponse();
            //string text2;
            //using (var sr = new StreamReader(response2.GetResponseStream()))
            //{
            //    text2 = sr.ReadToEnd();
            //    string bllll = text2;
            //    Console.WriteLine(text2);
            //}
            //using (var httpClient = new HttpClient())
            //{
            //    //using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://oauth.reddit.com/api/v1/me"))
            //    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://oauth.reddit.com/"))
            //    {
            //        request.Headers.TryAddWithoutValidation("Authorization", "bearer " + tokenTxt);
            //        request.Headers.TryAddWithoutValidation("User-Agent", "search.json?q=cats");
            //        //request.Headers.TryAddWithoutValidation("User-Agent", "r/wallstreetbets/hot/.json\\?limit=50 by theshoveler12");
            //        Task<HttpResponseMessage> t = httpClient.SendAsync(request);
            //        //Task<HttpRe> t = httpClient.SendAsync(request);
            //        t.Wait();
            //        var finalResponse = t.Result;


            //    }
            //}

            //Dictionary<string, string> parameters = new Dictionary<string, string>() { { "grant_type", "password" }, { "username", "theshoveler12" }, { "password", "shallNotpass12" } };
            //string encodedParams = EncodeParameters(parameters);
            //byte[] postBody = new ASCIIEncoding().GetBytes(encodedParams);
            //using (Stream stream2 = request.GetRequestStream())
            //{
            //    stream2.Write(postBody, 0, postBody.Length);
            //}
            //WebResponse response = request.GetResponse();
            //stream = response.GetResponseStream();
            //stream_read = new StreamReader(stream);
            //string txt = stream_read.ReadToEnd();
            //JsonTextReader reader = new JsonTextReader(thsAPIConnect.stream_read);
            //jsonTester json = new jsonTester(this);
            //var client = new RestClient("https://www.reddit.com/api/v1/access_token");
            //client.Timeout = -1;
            //var request = new WebResponse(Method.POST);
            //request.AddHeader("Authorization", "Basic aVpld0ZscXYxLWRFZmc6S3JhZzdYTzRaWjJockpKVjR0QWtuNmxvY2pCRE1n");
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddHeader("Cookie", "edgebucket=0EQAaIGgyGtxtvPenu; token_v2=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MTIzMzA4MDAsInN1YiI6Ii1td2Q2djJyQVhOMjFtSVI3Mms1enoySnM1MXMiLCJsb2dnZWRJbiI6ZmFsc2UsInNjb3BlcyI6WyIqIiwiZW1haWwiXX0.6RjkoiQzDW3YwVUkDrREvWtEzZtmdJkz6XH3AVa7DdM");
            //request.AddParameter("grant_type", "password");
            //request.AddParameter("username", "theshoveler12");
            //request.AddParameter("password", "shallNotpass12");
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //var url = "https://www.reddit.com/api/v1/access_token";
            //var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpRequest.Method = "POST";
            //httpRequest.ContentType = "application/x-www-form-urlencoded";
            //httpRequest.Headers["Authorization"] = "Basic cC1qY29MS0J5blRMZXc6Z2tvX0xYRUxvVjA3WkJOVVhydldaZnpFM2FJ";
            //var data = "grant_type=password&username=reddit_bot&password=snoo";
            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //}
            //Console.WriteLine(httpResponse.StatusCode);
            //using (var httpClient = new HttpClient())
            //{
            //    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://www.reddit.com/api/v1/access_token"))
            //    {
            //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("Nykv3LpDwTi0Zg:exyC1fvXALcTSONoWVK0eZwdoFKEbg"));
            //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

            //        request.Content = new StringContent("grant_type=password&username=theshoveler12&password=shallNotpass12");
            //        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            //        var response =  httpClient.SendAsync(request);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            Console.WriteLine("Request Message Information:- \n\n" + response.RequestMessage + "\n");
            //            Console.WriteLine("Response Message Header \n\n" + response.Content.Headers + "\n");
            //            // Get the response
            //            var customerJsonString = await response.Content.ReadAsStringAsync();
            //            Console.WriteLine("Your response data is: " + customerJsonString);

            //            // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
            //            var deserialized = JsonConvert.DeserializeObject<IEnumerable<Customer>>(custome‌​rJsonString);
            //            // Do something with it
            //        }
            //        //               HttpResponseMessage response = await client.PostAsync(
            //        //"http://api.repustate.com/v2/demokey/score.json",
            //        //requestContent);
            //        // Get the response content.
            //        //HttpContent responseContent = request.Content;

            //        // Get the stream of the content.
            //        //using (var reader = new StreamReader(response.))
            //        //{
            //        //    // Write the output.
            //        //    Console.WriteLine(reader.ReadToEndAsync());
            //        //}
            //    }


        }
        public APIConnect(string type, string segmentNM, DateTime beforeDate, string duration)
        {
            string resultPath = "";
            var apiTmr = new Stopwatch();
            apiTmr.Start();
            List<KeyValuePair<string, string>> runParams = new List<KeyValuePair<string, string>>();
            
            switch (type)
            {
                case "robinhood":
                    apiDB = new RogueDatabase<DataRowID>(2069081);
                    apiBundle = new RogueBundle(2069078);
                    this.segment_NM = segmentNM;
                    this.source_NM = segmentNM;
                    resultPath = run_cmd(apiBridgePath, new string[] { "robinhood","crypto_history", "ETH", "week", "5minute" });
                    break;
                case "reddit":
                    apiDB = new RogueDatabase<DataRowID>(39853);
                    apiBundle = new RogueBundle(39846);
                    string redditType = "submission";
                    this.source_NM = segmentNM;
                    string epochTim = new DateTimeOffset(beforeDate).ToUnixTimeSeconds().ToString();
                    resultPath = run_cmd(apiBridgePath, new string[] { "reddit", "https://api.pushshift.io/reddit/search/submission?subreddit=wallstreetbets", epochTim, duration, redditType });
                    break;
                case "news":                    
                    apiBundle = new RootBundle().GetBundle("NewsConnect", "This contains all databases relating to News Api");
                    apiDB = apiBundle.GetDatabase<DataRowID>("NewsApi", "The Database for the News API contents. Currently 100 calls per day brings in articles from multiple sources");
                    this.source_NM = "Article";
                    segmentNM = "Article";
                    resultPath = run_cmd(apiBridgePath, new string[] { type, "bitcoin", DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd")});
                    break;
                case "alphavantage":
                    apiBundle = new RootBundle().GetBundle("StockHistory", "Holds different sources for history of crypto and stock values");
                    apiDB = apiBundle.GetDatabase<DataRowID>("AlphaVantage", "Api to get historical crypto values");
                    this.source_NM = "PricePointDate";
                    segmentNM = "PricePointDate";
                    resultPath = run_cmd(apiBridgePath, new string[] { type, "bitcoin", DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd") });
                    break;
                case "cryptocompare":
                    apiBundle = new RootBundle().GetBundle("StockHistory", "Holds different sources for history of crypto and stock values");
                    apiDB = apiBundle.GetDatabase<DataRowID>("CryptoCompare", "Detailed api for getting crypto details and apparantly can do sockets for current prices.");
                    this.source_NM = "CryptoApiRuns";
                    segmentNM = "CryptoApiRuns";
                    runParams = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("BaseTableName", type), new KeyValuePair<string, string>("Coin", "ETH"), new KeyValuePair<string, string>("ToDate", DateTime.Today.ToString("yyyy-MM-dd")), new KeyValuePair<string, string>("RunTimeFrame", "day") };
                    resultPath = run_cmd(apiBridgePath, new string[] { type, "ETH", DateTime.Today.ToString("yyyy-MM-dd"), "day" });
                    break;
            }
            //RowID apiMetaRowID = LogApiMetaData(resultPath, apiTmr.ElapsedMilliseconds.ToString());
            //jsonTester jsonRunner = new jsonTester(this);
            //foreach(string path in Directory.EnumerateFiles(resultPath))
            //{
            this.stream_read = new StreamReader(DataFilePath(resultPath));
            this.segment_NM = segmentNM;
            var finalParams = new Dictionary<string,string>();
            runParams.ForEach(x => finalParams.Add(x.Key, x.Value));
            //jsonTester jsonRunner = new jsonTester(this, new KeyValuePair<string, string>("api_run_instance_oid", apiMetaRowID.ToString()), finalParams);
        }
        public string RunPythonProc(string filename, string arguments)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = @"C:\Users\Chris\AppData\Local\Programs\Python\Python35-32\python.exe";
                process.StartInfo.Arguments = filename;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) => {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            error.AppendLine(e.Data);
                        }
                    };

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    int timeout = 100000;
                    if (process.WaitForExit(timeout) &&
                        outputWaitHandle.WaitOne(timeout) &&
                        errorWaitHandle.WaitOne(timeout))
                    {
                        // Process completed. Check process.ExitCode here.
                        return output.ToString();
                    }
                    else
                    {
                        return error.ToString();
                        // Timed out.
                    }
                }
            }
        }
        public string run_cmd(string cmd, string[] args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            //start.FileName = @"H:\Development\Visaul Studio Code\RedditAPI\RedditApiRunner.py";
            start.FileName = @"C:\Users\Chris\AppData\Local\Programs\Python\Python39\python.exe";
            //start.FileName = @"C:\Users\chris\AppData\Local\Programs\Python\Python39\python.exe";
            //start.FileName = @"C:\Program Files\WindowsApps\PythonSoftwareFoundation.Python.3.9_3.9.752.0_x64__qbz5n2kfra8p0\python3.9.exe";
            //start.FileName = @"C:\Program Files\Python39\python.exe";
            switch (args.Length)
            {
                case 1:
                    start.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, args[0]);
                    break;
                case 2:
                    start.Arguments = string.Format("{0} {1} {2}", cmd, args[0], args[1]);
                    break;
                case 3:
                    start.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", cmd, args[0], args[1], args[2]);
                    break;
                case 4:
                    start.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", cmd, args[0], args[1], args[2], args[3]);
                    break;
                case 5:
                    start.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\"", cmd, args[0], args[1], args[2], args[3], args[4]);
                    break;
                case 6:
                    start.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\"", cmd, args[0], args[1], args[2], args[3], args[4], args[5]);
                    break;
                case 7:
                    start.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\"", cmd, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    break;
            }            
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                    string resultPath = reader.ReadToEnd();// Here is the result of StdOut(for example: print "test")
                    process.WaitForExit();
                    //"H:\Development\Visaul Studio Code\RedditAPI\ApiRuns\13.json"
                    //"H:\Development\Visaul Studio Code\RedditAPI\ApiRuns\13.json"

                    return resultPath;
                }
               
            }

            //*Proc might read line by line so might be better
            //Process proc = new Process();
            //proc.StartInfo.FileName = "python.exe";
            //proc.StartInfo.RedirectStandardOutput = true;
            //proc.StartInfo.UseShellExecute = false;

            //// call hello.py to concatenate passed parameters
            //proc.StartInfo.Arguments = string.Concat(progToRun, " ", x.ToString(), " ", y.ToString());
            //proc.Start();

            //StreamReader sReader = proc.StandardOutput;
            //string[] output = sReader.ReadToEnd().Split(splitter);

            //foreach (string s in output)
            //    Console.WriteLine(s);

            //proc.WaitForExit();
            //Console.ReadLine();
        }
        string[] DictionaryToArray(List<KeyValuePair<string,string>> vals)
        {
            string[] retVals = new string[vals.Count];
            for(int i =0; i < vals.Count; i++)
            {
                retVals[i] = vals[i].Key + "="+ vals[i].Value;
            }
            return retVals;
            //vals.Add("", "");
        }
        void LogApiMetaRow(string basePath, string runTime, IRogueRow newRow)
        {
            string metaPath = MetaFilePath(basePath);
            var metaData = JObject.Parse(File.ReadAllText(metaPath)).ToObject<Dictionary<string, string>>();
            foreach (var pair in metaData)
            {
                newRow.NewWritePair(39889, pair.Key, metaData[pair.Key]);
            }
            runStatus = (metaData["status"].ToString().ToLower() == "success") ? RunStatus.success : RunStatus.fail;
            newRow.NewWritePair(39889, "run_time", runTime);
            newRow.NewWritePair(39889, "file_path", new FileInfo(metaPath).FullName);
            newRow.NewWritePair(39889, "file_size", new FileInfo(metaPath).Length.ToString());
            newRow.NewWritePair(39889, "database_oid", apiDB.ioItemID.ToString());
            newRow.NewWritePair(39889, "api_source_nm", source_NM);
        }
        string RunRedditGet(string url, string tokenTxt, string afterToken = "")
        {
            if(afterToken != "")
            {
                url += "&after=" + afterToken;
            }
            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url);
            request2.Method = "GET";
            request2.Accept = "application/json";
            request2.UserAgent = "api/v1/me";
            request2.Headers.Add("Authorization", "bearer " + tokenTxt);
            request2.ContentType = "application/x-www-form-urlencoded";
            var response = request2.GetResponse();
            string text;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            return text;
        }
        string GetRedditToken()
        {
            string tokenTxt;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://www.reddit.com/api/v1/access_token"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Basic aVpld0ZscXYxLWRFZmc6S3JhZzdYTzRaWjJockpKVjR0QWtuNmxvY2pCRE1n");
                    request.Headers.TryAddWithoutValidation("Cookie", "edgebucket=0EQAaIGgyGtxtvPenu; token_v2=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MTIzMzA4MDAsInN1YiI6Ii1td2Q2djJyQVhOMjFtSVI3Mms1enoySnM1MXMiLCJsb2dnZWRJbiI6ZmFsc2UsInNjb3BlcyI6WyIqIiwiZW1haWwiXX0.6RjkoiQzDW3YwVUkDrREvWtEzZtmdJkz6XH3AVa7DdM");

                    var contentList = new List<string>();
                    contentList.Add($"grant_type={Uri.EscapeDataString("password")}");
                    contentList.Add($"username={Uri.EscapeDataString("theshoveler12")}");
                    contentList.Add($"password={Uri.EscapeDataString("shallNotpass12")}");
                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    Task<HttpResponseMessage> t = httpClient.SendAsync(request);
                    t.Wait();
                    var finalResponse = t.Result;
                    var jsonToken = finalResponse.Content.ReadAsStringAsync();
                    var options = new JsonDocumentOptions
                    {
                        AllowTrailingCommas = true
                    };
                    JsonDocument document = JsonDocument.Parse(jsonToken.Result, options);
                    
                    tokenTxt = document.RootElement.GetProperty("access_token").GetString();
                }
            }
            return tokenTxt;
        }
        string DataFilePath (string basePath) { return basePath + "data.json"; }
        string MetaFilePath(string basePath) { return basePath + "metadata.json"; }
        private WebRequest CreateRequest(string consumerKey, string consumerSecret, string accessToken, string accessKey,string method, string url, Dictionary<string, string> parameters)
        {
            string encodedParams = EncodeParameters(parameters);
            WebRequest request;
            if (method == "GET")
                request = WebRequest.Create(string.Format("{0}?{1}", url, encodedParams));
            else
                request = WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Authorization", MakeOAuthHeader(consumerKey, consumerSecret, accessToken, accessKey, method, url, parameters));
            if (method == "POST")
            {
                byte[] postBody = new ASCIIEncoding().GetBytes(encodedParams);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postBody, 0, postBody.Length);
                }
            }
            return request;
        }
        private static string EncodeParameters(Dictionary<string, string> parameters)
        {
            if (parameters.Count == 0)
                return string.Empty;
            Dictionary<string, string>.KeyCollection.Enumerator keys = parameters.Keys.GetEnumerator();
            keys.MoveNext();
            StringBuilder sb = new StringBuilder(
                string.Format("{0}={1}", keys.Current, Uri.EscapeDataString(parameters[keys.Current])));
            while (keys.MoveNext())
                sb.AppendFormat("&{0}={1}", keys.Current, Uri.EscapeDataString(parameters[keys.Current]));
            return sb.ToString();
        }
        public enum rowstatus
        {
            open, close
        }
        private string MakeOAuthHeader(string consumerKey, string consumerSecret, string accessToken, string accessKey, string method, string url, Dictionary<string, string> parameters)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            string oauth_consumer_key = consumerKey;
            string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            string oauth_signature_method = "HMAC-SHA1";
            string oauth_token = accessToken;
            string oauth_timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            string oauth_version = "1.0";

            SortedDictionary<string, string> sd = new SortedDictionary<string, string>();
            if (parameters != null)
                foreach (string key in parameters.Keys)
                    sd.Add(key, Uri.EscapeDataString(parameters[key]));
            sd.Add("oauth_version", oauth_version);
            sd.Add("oauth_consumer_key", oauth_consumer_key);
            sd.Add("oauth_nonce", oauth_nonce);
            sd.Add("oauth_signature_method", oauth_signature_method);
            sd.Add("oauth_timestamp", oauth_timestamp);
            sd.Add("oauth_token", oauth_token);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}&{1}&", method, Uri.EscapeDataString(url));
            foreach (KeyValuePair<string, string> entry in sd)
                sb.Append(Uri.EscapeDataString(string.Format("{0}={1}&", entry.Key, entry.Value)));
            string baseString = sb.ToString().Substring(0, sb.Length - 3);

            string oauth_token_secret = accessKey;
            string signingKey = string.Format(
                "{0}&{1}", Uri.EscapeDataString(consumerSecret), Uri.EscapeDataString(oauth_token_secret));
            HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));
            string oauth_signature = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString)));

            sb = new StringBuilder("OAuth ");
            sb.AppendFormat("oauth_consumer_key=\"{0}\",", Uri.EscapeDataString(oauth_consumer_key));
            sb.AppendFormat("oauth_nonce=\"{0}\",", Uri.EscapeDataString(oauth_nonce));
            sb.AppendFormat("oauth_signature=\"{0}\",", Uri.EscapeDataString(oauth_signature));
            sb.AppendFormat("oauth_signature_method=\"{0}\",", Uri.EscapeDataString(oauth_signature_method));
            sb.AppendFormat("oauth_timestamp=\"{0}\",", Uri.EscapeDataString(oauth_timestamp));
            sb.AppendFormat("oauth_token=\"{0}\",", Uri.EscapeDataString(oauth_token));
            sb.AppendFormat("oauth_version=\"{0}\"", Uri.EscapeDataString(oauth_version));

            return sb.ToString();
        }
    }
}
