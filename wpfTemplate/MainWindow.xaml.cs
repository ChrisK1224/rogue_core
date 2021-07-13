using rogue_core.rogueCore.queryResults;
using rogueCore.UI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfTemplate.RogueUI.WPF;

namespace wpfTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WPFPage pageBuilder;
        public MainWindow()
        {
            CreateInsertHql.CreateHQLInsertList();
            InitializeComponent();
            QueryBuilder win = new QueryBuilder();
            win.Show();
            pageBuilder = new WPFPage(1);
            //*Nav Menu. , "SECTIONHEADER");
            pageBuilder.AddSegment(6);
            //Tree Menu. "SECTION1"
            pageBuilder.AddSegment(2);
            //Table header query. 5"SECTION2"
            pageBuilder.AddSegment(5);
            //*"Row Insert. SECTION3"
            pageBuilder.AddSegment(3);
            //Column INsert query  "SECTION4"
            pageBuilder.AddSegment(4);
            pageBuilder.BuildControls();
            // DependencyObject rootObject = baseSegment
            foreach (UISegmentBuilder thsSeg in pageBuilder.segmentQueries.Values)
            {
                StackPanel sectionStarter = pageBuilder.baseSegment.namedControls[thsSeg.sectionID] as StackPanel;
                sectionStarter.Children.Add((StackPanel)thsSeg.topControl);
            }
            mainStack.Children.Add((FrameworkElement)pageBuilder.baseSegment.topControl);
        }
    }
}
