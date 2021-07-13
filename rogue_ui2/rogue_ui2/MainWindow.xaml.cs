using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rogue_ui2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string qry = @"FROM ""MENU"" SELECT ""TABLE"" 
FROM ""MENUROW"" JOIN TO MENU SELECT ""TABLEROW"" 
FROM ""MENUCELL"" JOIN TO MENUROW SELECT ""TABLECELL""
FROM ""COLUMNWIDTH"" JOIN TO MENUCELL SELECT ""WIDTHPERCENT"", ""10""
FROM ""MENUTABLE"" JOIN TO MENUCELL SELECT ""TABLE""
FROM ""MENUTABLEROW"" JOIN TO MENUTABLE SELECT ""TABLEROW""
FROM ""MENUTABLECELL"" JOIN TO MENUTABLEROW SELECT ""TABLECELL""
FROM ""BTN"" JOIN TO MENUTABLECELL SELECT ""button"" AS CONTROLNAME
FROM ""BTNOPENMENU"" JOIN TO BTN SELECT ""image"" AS CONTROLNAME, ""database"" AS CONTROLVALUE
FROM ""MENUTABLEROW2"" JOIN TO MENUTABLE SELECT ""TABLEROW""
FROM ""BTN2"" JOIN TO MENUTABLEROW2 SELECT ""button"" AS CONTROLNAME
FROM ""BTNOPENMENU2"" JOIN TO BTN2 SELECT ""image"" AS CONTROLNAME, ""database"" AS CONTROLVALUE
FROM ""CONTENTCELL"" JOIN TO MENUROW SELECT ""TABLECELL""
FROM ""COLUMNWIDTH"" JOIN TO CONTENTCELL SELECT ""WIDTHPERCENT"", ""90""
FROM ""TESTLBL"" JOIN TO CONTENTCELL SELECT ""label""
FROM ""LBLTXT"" JOIN TO TESTLBL SELECT ""TEXT"",""HEYYOO""";
            WPFSegmentBuilder sectionBuil = new WPFSegmentBuilder(qry,stackMain);//"FROM \"test\" SELECT \"button\" AS CONTROLNAME FROM \"BTNOPENMENU\" JOIN TO test SELECT \"image\" AS CONTROLNAME, \"database\" AS CONTROLVALUE  ", stackMain);//FROM \"TXT\" JOIN TO test SELECT \"text\" AS CONTROLNAME, \"HEYOOOO\" AS CONTROLVALUE                                                                                                                                                                                                                     // WPFSegmentBuilder.BuildSection("FROM \"test\" SELECT \"button\" AS CONTROLNAME", stackMain);//FROM \"TXT\" JOIN TO test SELECT \"text\" AS CONTROLNAME, \"HEYOOOO\" AS CONTROLVALUE
        }
    }
}
