using FilesAndFolders;
using rogueCore.hqlSyntaxV3.filledSegments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueCore.rogueUIV3.web
{
    public class UIWebPage : UIPage<UIWebSection>
    {
        StringBuilder uiText = new StringBuilder();
        public UIWebPage(int pageOID) : base(pageOID) { }
        public UIWebPage() : base() { }
        protected override UIWebSection NewSection(IMultiRogueRow sectionID, Dictionary<int, string> pageContent)
        {
            return new UIWebSection(sectionID, pageContent);
        }
        protected override void BuildPage()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            baseSegment.BuildSection();
            //* TODO bring back parrallel need to get rid of static variables in query
            //Parallel.ForEach(sections, command => command.BuildSection());
            foreach (var thsSegment in sections)
            {
                thsSegment.BuildSection();
            }
            watch.Stop();
            long blah = watch.ElapsedMilliseconds;            
        }
        public String BuildHTML()
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();
                BuildPage();
                uiText = new StringBuilder();
                uiText.Append(baseSegment.finalHTML);
                foreach (var thsSegment in sections)
                {
                    int insertNum = uiText.ToString().FindCharIndexAfterString("=\"" + thsSegment.SectionIDText() + "\"", '>');
                    uiText.Insert(insertNum, thsSegment.finalHTML);
                }
                String blah = uiText.ToString();
                tmr.Stop();
                string fse = tmr.ElapsedMilliseconds.ToString();
                return uiText.ToString();
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
        public static UIWebPage InstancePage(string clickEvent, Dictionary<string,string> pageContent)
        {
            var newPage = new UIWebPage();
            newPage.RogueClickEvent(clickEvent, pageContent);
            return newPage;
        }
        public string JsonSectionCollection()
        {
            StringBuilder retJson = new StringBuilder();
            retJson.Append("{ ");
            //List<string> sects = new List<string>();
            //*THIS TAKES WAY TOO LONG
            //Parallel.ForEach(sections, sect => sects.Add(("\"" + sect.SectionIDText().Replace("@", "") + "\" : " + sect.AsJson)));
            //retJson += string.Join(",", sects);   
            //Stopwatch spo = new Stopwatch();
            //spo.Start(); 
            foreach (var sect in sections)
            {
                retJson.Append("\"" + sect.SectionIDText().Replace("@", "") + "\" : " + sect.WebJson() + ",");
            }
            string metaDataJ = "[{\"1\":\"TEXTBOX\",\"rogueContent\" : [{\"1\":\"TEXT\", \"2\": \"" + pageID.ToString() + "\"}, {\"1\":\"IDNAME\", \"2\": \"ROGUEPAGEID\"}]}]";
            retJson.Append("\"RogueMetaData\" : " + metaDataJ + "");
            //retJson = retJson.Substring(0, retJson.Length - 1);
            retJson.Append("}");
            //spo.Stop();
            string bll = retJson.ToString();
            //string gll = spo.ElapsedMilliseconds.ToString();
            return retJson.ToString();
        }
    }
}
