//using FilesAndFolders;
//using rogueCore.hqlSyntax;
//using rogueCore.rogueUI.web.element;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace rogueCore.rogueUI.web
//{
//    public class UIWebPage : UIPage<UIWebSection>
//    {
//        StringBuilder uiText = new StringBuilder();
//        public UIWebPage(int pageOID) : base(pageOID) { }
//        protected override UIWebSection NewSection(FilledSelectRow sectionID, Dictionary<int, string> pageContent)
//        {
//            return new UIWebSection(sectionID, pageContent);
//        }
//        protected override void BuildPage()
//        {
//            baseSegment.BuildSection();
//            //* TODO bring back parrallel need to get rid of static variables in query
//            Parallel.ForEach(sections, command => command.BuildSection());
//            //foreach (var thsSegment in sections)
//            //{
//            //    thsSegment.BuildSection();
//            //}
//        }
//        public String BuildHTML()
//        {
//            BuildPage();
//            uiText = new StringBuilder();
//            uiText.Append(baseSegment.finalHTML);
//            //uiText.Append((UIWebSection.GenerateSegmentHTML((WebBaseControl)baseSegment.topControl, new StringBuilder())));
//            //Parallel.ForEach(segmentQueries, command => command.Value.BuildSection());
//            foreach (var thsSegment in sections)
//            {
//                int insertNum = uiText.ToString().FindCharIndexAfterString("=\"" + thsSegment.SectionIDText() +"\"", '>');
//                uiText.Insert(insertNum, thsSegment.finalHTML);
//                //uiText.Insert(insertNum, UIWebSection.GenerateSegmentHTML((WebBaseControl)thsSegment.topControl, new StringBuilder()));
//            }
//            String blah = uiText.ToString();
//            return uiText.ToString();
//        }
//    }
//}
