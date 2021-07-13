using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.ioObject;
using rogue_core.rogueCore.syntaxCommand;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.row;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.segments.update;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static rogueCore.hqlSyntaxV3.IntellsenseDecor;
using System.Diagnostics;
using rogue_core.rogueCore.binary;
using GroupDocs.Classification;

namespace rogue_ui2
{
    /// <summary>
    /// Interaction logic for PageInsert.xaml
    /// </summary>
    public partial class PageInsert : Page
    {
        List<IValuedElement> insertControls;
        string insertID;
        string activeObjectID;
        string menuQry = @"FROM IORECORDS AS Bundles WHERE MetaRecordType = ""Bundle"" SELECT ROGUECOLUMNID, METAROW_NAME, ""BUNDLE"" AS TYP
                             FROM IORECORDS JOIN ON IORECORDS.OwnerIOItem = Bundles.ROGUECOLUMNID WHERE MetaRecordType = ""Database"" SELECT ROGUECOLUMNID, METAROW_NAME, ""DATABASE"" AS TYP
                             FROM IORECORDS AS TR JOIN ON TR.OwnerIOItem = IORECORDS.RogueColumnID SELECT ROGUECOLUMNID, METAROW_NAME, ""TABLE"" AS TYP
                             FROM COLUMN JOIN ON COLUMN.OWNERIOITEM  = TR.ROGUECOLUMNID SELECT COLUMNIDNAME AS METAROW_NAME, ""COLUMN"" AS TYP ";
        //public List<String> ContentAssistSource
        //{
        //    get { return (List<String>)GetValue(ContentAssistSourceProperty); }
        //    set { SetValue(ContentAssistSourceProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ContentAssisteSource.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ContentAssistSourceProperty =
        //    DependencyProperty.Register("ContentAssistSource", typeof(List<String>), typeof(MainWindow), new UIPropertyMetadata(new List<string>()));


        //public List<char> ContentAssistTriggers
        //{
        //    get { return (List<char>)GetValue(ContentAssistTriggersProperty); }
        //    set { SetValue(ContentAssistTriggersProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ContentAssistTriggers.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ContentAssistTriggersProperty =
        //    DependencyProperty.Register("ContentAssistTriggers", typeof(List<char>), typeof(MainWindow), new UIPropertyMetadata(new List<char>()));
        public PageInsert()
        {
            InitializeComponent();
            
            InitRichTextBoxSource();
            InitRichTextBoxIntellisenseTrigger();
            txtSyntax.Focus();
            DataContext = this;
            LoadMenu();
            LoadQueryEdit(menuQry);
            LoadInsertContent("COLUMN", "TABLE", "-1011");

            ////WPFSegmentBuilder sectionBuil = new WPFSegmentBuilder("7443", pnlMenu);
        }
        private void InitRichTextBoxIntellisenseTrigger()
        {
            txtSyntax.ContentAssistTriggers.Add('@');
            txtSyntax.ContentAssistTriggers.Add('.');
            txtSyntax.ContentAssistTriggers.Add((char)32);
        }
        private void InitRichTextBoxSource()
        {
            txtSyntax.ContentAssistSource.Add("aaal");
            txtSyntax.ContentAssistSource.Add("as");
            txtSyntax.ContentAssistSource.Add("aacp");
            txtSyntax.ContentAssistSource.Add("aid");
            txtSyntax.ContentAssistSource.Add("asap");
            txtSyntax.ContentAssistSource.Add("boy");
            txtSyntax.ContentAssistSource.Add("big");
            txtSyntax.ContentAssistSource.Add("before");
            txtSyntax.ContentAssistSource.Add("belong");
            txtSyntax.ContentAssistSource.Add("can");
            txtSyntax.ContentAssistSource.Add("clever");
            txtSyntax.ContentAssistSource.Add("cool");
            txtSyntax.ContentAssistSource.Add("data");
            txtSyntax.ContentAssistSource.Add("delete");
        }
        void LoadMenu()
        {            
            SelectHQLStatement dbQry = new SelectHQLStatement(menuQry);
            dbQry.Fill();
            List<ItemsControl> lstItems = new List<ItemsControl>();
            treMenu.Items.Clear();
            lstItems.Add(treMenu);
            Action<IMultiRogueRow> OpenWPFControl = (row) =>
            {
                TreeViewItem newItem = new TreeViewItem();
                newItem.Tag = row.GetValue("TYP") + "*" + row.GetValue("ROGUECOLUMNID");
                var pnlHeader = new StackPanel();
                pnlHeader.Orientation = Orientation.Horizontal;
                newItem.Header = pnlHeader;
                TextBlock itemNM = new TextBlock();
                Image pic = new Image();
                pic.Height = 20;
                pic.Width = 20;
                string picBasePath = @"C:\Users\chris\Documents\Development\rogue_core\rogue_ui2\rogue_ui2\pics\";
                //string picBasePath = @"H:\Development\Visual Studio\Rogue\WPF_Interface\rogue_ui2\rogue_ui2\pics\";
                switch (row.GetValue("TYP"))
                {
                    case "BUNDLE":
                        //pic.Source = new BitmapImage(new Uri("pack://application:,,,/pics/bundle.png")); 
                        pic.Source = new BitmapImage(new Uri(picBasePath + "bundle.png"));
                        break;
                    case "DATABASE":
                        pic.Source = new BitmapImage(new Uri((picBasePath + "database.png")));
                        break;
                    case "TABLE":
                        pic.Source = new BitmapImage(new Uri((picBasePath +  "table.png")));
                        break;
                    case "COLUMN":
                        pic.Source = new BitmapImage(new Uri((picBasePath + "meta_table.png")));
                        break;
                }
                pnlHeader.Children.Add(pic);
                itemNM.Text = row.GetValue("METAROW_NAME");
                pnlHeader.Children.Add(itemNM);
                lstItems[lstItems.Count - 1].Items.Add(newItem);
                lstItems.Add(newItem);
            };
            Action<IMultiRogueRow> CloseWPFControl = (row) =>
            {
                lstItems.RemoveAt(lstItems.Count - 1);
            };
            dbQry.IterateRows(OpenWPFControl, CloseWPFControl);
        }
        void LoadInsertContent(string name, string dbTyp, string dbItemID)
        {
            insertControls = new List<IValuedElement>();
            grdInsertContent.Children.Clear();
            grdInsertContent.RowDefinitions.Clear();
            lblTableNM.Text = name;
            lblObjID.Text = dbItemID;
            activeObjectID = dbItemID;
            string objWhere = " WHERE OWNERIOITEM = \"" + activeObjectID + "\" ";
            //Button btn = new Button();
            //btn.Content = "INSERT";
            //btn.Click += btnInsert_Click;
            switch (dbTyp)
            {
                case "BUNDLE":
                    if(name.ToUpper() == "ROOT")
                    {
                        lblInsertObjectOwner.Content = name;
                        lblInsertObjectTyp.Content = "Bundle";
                        LoadTableData("-1010", objWhere);
                    }
                    else
                    {
                        lblInsertObjectOwner.Content = name;
                        lblInsertObjectTyp.Content = "Database";
                        LoadTableData("-1010",objWhere);
                    }
                    grdInsertContent.Visibility = Visibility.Hidden;
                    grdInsertObject.Visibility = Visibility.Visible;
                    //((Grid)btnInsert.Parent).Children.Remove(btnInsert);                    
                    //Grid.SetRow(btn, grdInsertObject.RowDefinitions.Count-1);
                    break;
                case "DATABASE":
                    lblInsertObjectOwner.Content = name;
                    lblInsertObjectTyp.Content = "Table";
                    grdInsertContent.Visibility = Visibility.Hidden;
                    grdInsertObject.Visibility = Visibility.Visible;                    
                    LoadTableData("-1010", objWhere);
                    //((Grid)btnInsert.Parent).Children.Remove(btnInsert);
                    //Grid.SetRow(btn, grdInsertObject.RowDefinitions.Count - 1);
                    //foreach(IMultiRogueRow row in new SelectHQLStatement("FROM IORECORDS WHERE METARECORDTYPE = \"Table\" SELECT *").Fill().TopRows())
                    //{
                    //    ddlInsertObjectOwner.Items.Add(new ComboBoxItem() { Content = row.GetValue("METAROW_NAME").ToString(), Tag = row.GetValue("ROGUECOLUMNID").ToString() });
                    //}
                    break;
                case "TABLE":
                    int currRow = 0;
                    insertID = dbItemID;
                    foreach (var row in new SelectHQLStatement("FROM COLUMN WHERE OWNERIOITEM  = \"" + dbItemID.ToString() + "\" SELECT * FROM COLUMNENUMERATIONS AS CE JOIN ON CE.COLUMN_OID = COLUMN.ROGUECOLUMNID SELECT *").Fill().TopRows())
                    {
                        string colNM = row.GetValue("COLUMNIDNAME");
                        string colID = row.GetValue("ROGUECOLUMNID");
                        grdInsertContent.RowDefinitions.Add(new RowDefinition());
                        var lbl = new TextBlock();
                        lbl.Text = colNM;
                        Grid.SetRow(lbl, currRow);
                        Grid.SetColumn(lbl, 0);
                        grdInsertContent.Children.Add(lbl);
                        var valItem = ToValueElement(row,currRow);
                        //var txtBox = new MyTextBox();
                        //txtBox.Width = 200;
                        //txtBox.Tag = colID;
                        //Grid.SetRow(txtBox, currRow);
                        //Grid.SetColumn(txtBox, 1);
                        //grdInsertContent.Children.Add(valItem);
                        insertControls.Add(valItem);
                        currRow++;                       
                    }
                    grdInsertContent.RowDefinitions.Add(new RowDefinition());                                        
                    if(btnInsert.Parent != null)
                    {
                        ((Grid)btnInsert.Parent).Children.Remove(btnInsert);
                    }                    
                    Grid.SetRow(btnInsert, currRow);
                    Grid.SetColumn(btnInsert, 1);
                    grdInsertContent.Children.Add(btnInsert);
                    grdInsertContent.Visibility = Visibility.Visible;
                    grdInsertObject.Visibility = Visibility.Hidden;
                    LoadTableData(dbItemID);
                    break;
                case "COLUMN":

                    break;
            }
        }
        //void LoadOwnerIOItemView(string ownerRogueID, string name, string desc)
        //{
        //    //string objectRogueID = parentRogueID;
        //    Container.CreateObject(activeObjectID, txtInsertObjectName.Text, txtInsertObjectDesc.Text);
        //}
        IValuedElement ToValueElement(IMultiRogueRow colRow, int currRow)
        {
            switch (colRow.GetValue("COLUMNTYPE").ToString().ToUpper())
            {
                case "COLUMN":
                    if(colRow.childRows.Count == 0)
                    {
                        var txtBox = new MyTextBox();
                        txtBox.Width = 200;
                        txtBox.Tag = colRow.GetValue("ROGUECOLUMNID");
                        Grid.SetRow(txtBox, currRow);
                        Grid.SetColumn(txtBox, 1);
                        grdInsertContent.Children.Add(txtBox);
                        return txtBox;
                    }
                    else
                    {
                        var ddl = new MyDropDownList();
                        foreach(var enumRow in colRow.childRows)
                        {
                            string balh = enumRow.GetValue("ROGUECOLUMNID");
                            ddl.Items.Add(new ComboBoxItem() { Content = enumRow.GetValue("ENUMERATION_VALUE"), Tag = enumRow.GetValue("ENUMERATION_VALUE") });                            
                        }
                        ddl.Width = 200;
                        ddl.Tag = colRow.GetValue("ROGUECOLUMNID");
                        Grid.SetRow(ddl, currRow);
                        Grid.SetColumn(ddl, 1);
                        grdInsertContent.Children.Add(ddl);
                        return ddl;
                    }
                case "PARENTTABLEREF":

                    //string Qry = "FROM IORECORDS AS COLID WHERE NAME_COLUMN_OID != \"\" AND COLID.ROGUECOLUMNID = \"" + colRow.GetValue("ParentTableID") + "\" AND COLUMN.IS_EDITABLE != \"false\" SELECT \"dropdownlist\" AS CONTROLNAME  FROM [" +colRow.GetValue("ParentTableID") +  "] AS PARENTCOLENUM JOIN ON * = COLID.ROGUECOLUMNID SELECT PARENTCOLENUM.[{COLID.NAME_COLUMN_OID}] as COL_TXT, PARENTCOLENUM.ROGUECOLUMNID AS VALUEID";
                    //**RECENT RISKY CHANGE got rid of AND COLUMN.IS_EDITABLE != \"false\" not sure how this is working since not linked to COLUMN table.
                    string bll = colRow.GetValue("ParentTableID");
                    string Qry = "FROM IORECORDS AS COLID WHERE NAME_COLUMN_OID != \"\" AND COLID.ROGUECOLUMNID = \"" + colRow.GetValue("ParentTableID") + "\"  SELECT \"dropdownlist\" AS CONTROLNAME  FROM [" + colRow.GetValue("ParentTableID") + "] AS PARENTCOLENUM JOIN ON * = COLID.ROGUECOLUMNID SELECT PARENTCOLENUM.[{COLID.NAME_COLUMN_OID}] as COL_TXT, PARENTCOLENUM.ROGUECOLUMNID AS VALUEID";
                    var ddlRef = new MyDropDownList();
                    var runQry = new SelectHQLStatement(Qry).Fill();
                    if(runQry.TopRows().ToList().Count > 0)
                    {
                        foreach (var enumRow in runQry.TopRows().First().childRows)
                        {
                            string test = enumRow.GetValue("COL_TXT");
                            string balh = enumRow.GetValue("VALUEID");
                            ddlRef.Items.Add(new ComboBoxItem() { Content = enumRow.GetValue("COL_TXT"), Tag = enumRow.GetValue("VALUEID") });
                        }
                    }
                    ddlRef.Width = 200;
                    ddlRef.Tag = colRow.GetValue("ROGUECOLUMNID");
                    Grid.SetRow(ddlRef, currRow);
                    Grid.SetColumn(ddlRef, 1);
                    grdInsertContent.Children.Add(ddlRef);
                    return ddlRef;
            }
            return null;
        }
        void LoadQueryResults()
        {
            List<ItemsControl> lstItems = new List<ItemsControl>();
            lstItems.Add(treResults);
            Action<IMultiRogueRow> openResult = (row) =>
            {
                TreeViewItem itm = new TreeViewItem();
                StackPanel headerPnl = new StackPanel();
                itm.Header = headerPnl;
                headerPnl.Orientation = Orientation.Horizontal;
                foreach (var pair in row.GetValueList())
                {
                    headerPnl.Children.Add(new TextBlock() { Text = pair.Key + ":" , Foreground = new SolidColorBrush(Colors.Blue)});
                    headerPnl.Children.Add(new TextBlock() { Text = pair.Value +", " });
                }
                lstItems[lstItems.Count - 1].Items.Add(itm);
                lstItems.Add(itm);
            };
            Action<IMultiRogueRow> closeResult = (row) =>
            {
                lstItems.RemoveAt(lstItems.Count - 1);   
            };
            new SelectHQLStatement(txtSyntax.Content).Fill().IterateRows(openResult, closeResult);
        }
        void LoadTableData(string rogueID, string txtWhere = "")
        {
            string whreTxt = (txtWhere != "") ? txtWhere : txtWhereClause.Text;
            Stopwatch tmr = new Stopwatch();
            tmr.Start();
            var qry = new SelectHQLStatement("FROM [" + rogueID + "] AS TBL @WHERE SELECT * ".Replace("@WHERE", whreTxt)).Fill();
            tmr.Stop();
            string initFill = (tmr.ElapsedMilliseconds).ToString();
            string fillTicks = (tmr.ElapsedTicks).ToString();
            tmr.Restart();
            DataTable data = qry.AsDataTable();
            tmr.Stop();
            lblTableMetaData.Content = "Ticks: " + fillTicks + " Fill Time: " + initFill + ",DataTable Time: " + tmr.ElapsedMilliseconds + ", Rows:" + data.Rows.Count + ", Columns: " +data.Columns.Count;

            grdInsertData.ItemsSource = data.DefaultView;
            grdInsertData.AutoGenerateColumns = true;
            grdInsertData.CanUserAddRows = false;
            //grdInsertData.RowEditEnding += DriversDataGrid_RowEditEnding;
            grdInsertData.CellEditEnding += myDG_CellEditEnding;
            var rowIndex = grdInsertData.Columns.IndexOf(grdInsertData.Columns.FirstOrDefault(c => c.Header.ToString() == "ROGUECOLUMNID"));
            if (rowIndex > -1)
            {
                grdInsertData.Columns[rowIndex].Visibility = Visibility.Collapsed;
            }
        }
        //private void DriversDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        //{
        //    // Only act on Commit
        //    //if (e.EditAction == DataGridEditAction.Commit)
        //    //{
        //    //    DataRowView rowView = e.Row.Item as DataRowView;
        //    //    //rowBeingEdited = rowView;
        //    //    DataRow updateRow  = ((DataRowView)e.Row.DataContext).Row;
        //    //    string ss = "SDF";
        //    //    //DataRow driver = e.Row.DataContext as DataRow;
        //    //    //driver.Save();
        //    //}
        //}
        void myDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //**TEMP
            //return;
            //**Ready to get update workng with below info
            if (e.EditAction == DataGridEditAction.Commit)
            {
                int rowIndex = e.Row.GetIndex();
                int colID = e.Column.DisplayIndex;
                string rowID = ((DataView)grdInsertData.ItemsSource).Table.Rows[rowIndex]["ROGUECOLUMNID"].ToString();
                string colnm = ((DataView)grdInsertData.ItemsSource).Table.Columns[colID].ColumnName;
                ColumnRowID colRowID = BinaryDataTable.columnTable.GuessColumnIDByName(colnm, activeObjectID); //HQLEncoder.GuessColumnIDByName(colnm, int.Parse(activeObjectID));
                if(colRowID == -1012)
                {
                    return;
                }
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    //int rowIndex = e.Row.GetIndex();
                    var el = e.EditingElement as TextBox;
                    string ll = el.Text;
                    UpdateRow(colRowID, rowID, el.Text);
                    //if (bindingPath == "Col2")
                    //{
                    //    int rowIndex = e.Row.GetIndex();
                    //    var el = e.EditingElement as TextBox;
                    //    // rowIndex has the row index
                    //    // bindingPath has the column's binding
                    //    // el.Text has the new, user-entered value
                    //}
                }
            }
        }
        void LoadQueryEdit(string qry)
        {
            //string template = @"FROM ""PARAM_BOX"" SELECT ""groupbox"" AS CONTROLNAME, ""child"" AS PARENTRELATION 
            //       FROM ""PARAM_BOX_DIVPADDING"" JOIN TO PARAM_BOX SELECT ""margin"" as ATTRIBUTETYPE,""attribute"" AS PARENTRELATION,""25"" AS ATTRIBUTEVALUE
            //       FROM EXECUTE(UI_PARAMETER_FIELDS, ""{0}"") AS QRYUNSETPARAMS JOIN TO PARAM_BOX";
            //string finalQry = string.Format(template, qry);
            SelectHQLStatement statement = new SelectHQLStatement(qry);
            var syntaxCMD = new WPFSyntaxCommands(txtSyntax);
            statement.GenerateIntellisenseParts(new ManualMultiRogueRow(), syntaxCMD);
            //statement.Fill();            
            //Action<IMultiRogueRow> NewQueryTextControl = (row) =>
            //{

            //    //TreeViewItem newItem = new TreeViewItem();
            //    //newItem.Tag = row.GetValue("ROGUECOLUMNID");
            //    //newItem.MouseDoubleClick += DBItemClick;
            //    //var pnlHeader = new StackPanel();
            //    //pnlHeader.Orientation = Orientation.Horizontal;
            //    //newItem.Header = pnlHeader;
            //    //TextBlock itemNM = new TextBlock();
            //    //Image pic = new Image();
            //    //pic.Height = 20;
            //    //pic.Width = 20;
              
            //};
            //Action<IMultiRogueRow> EndQueryTextControl = (row) =>
            //{
            //    //lstItems.RemoveAt(lstItems.Count - 1);
            //};
            //statement.IterateRows(NewQueryTextControl, EndQueryTextControl);
           
        }
        private void txtSyntax_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = txtSyntax.Content;
        }
        void IntellTest()
        {

        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            String tableID = insertID;
            IRogueTable tbl = new IORecordID(tableID).ToTable();
            IRogueRow newRow = tbl.NewWriteRow();
            foreach (var selectElement in insertControls)
            {
                newRow.NewWritePair(new ColumnRowID(selectElement.Tag), selectElement.Content);
            }
            tbl.Write();
            LoadTableData(tableID);
        }
        private void treMenu_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string item = ((TreeViewItem)((TreeView)sender).SelectedItem).Tag.ToString();
            var nm = ((TreeViewItem)((TreeView)sender).SelectedItem).Header;
            string[] ids = item.Split('*');
            string dbTyp = ids[0];
            string dbItemID = ids[1];
            LoadInsertContent(((TextBlock)(((StackPanel)nm).Children[1])).Text, dbTyp, dbItemID);
        }
        private void btnRunQuery_Click(object sender, RoutedEventArgs e)
        {
            LoadQueryResults();
        }
        private void btnInsertObj_Click(object sender, RoutedEventArgs e)
        {
            Container.CreateObject(activeObjectID, txtInsertObjectName.Text, txtInsertObjectDesc.Text);
            //();
        }
        void UpdateRow(ColumnRowID colRowID, string rowID, string newValue)
        {
            IORecordID updateTableID = new IORecordID(colRowID.ToOwnerIORecord());
            UpdateHQLStatement updateStatement = new UpdateHQLStatement(updateTableID);            
            updateStatement.AddUpdateField(colRowID, newValue);
            updateStatement.AddWhereClause(-1012, rowID);
            //updateStatement.Fill();
            updateStatement.Execute();
            //String tableID = insertID;
            //foreach (var selectValue in insertControls)
            //{
            //    ColumnRowID updateColumnID = new ColumnRowID(selectValue.Tag);
            //    IORecordID updateTableID = new IORecordID(updateColumnID.ToOwnerIORecord());
            //    UpdateHQLStatement updateStatement = new UpdateHQLStatement(updateTableID);
            //    String value = selectValue.Content;
            //    updateStatement.AddUpdateField(updateColumnID, value);
            //    updateStatement.AddWhereClause(-1012, rowID);
            //    updateStatement.Fill();
            //}
        }
    }
    class WPFSyntaxCommands : ISyntaxPartCommands
    {
        RichTextBox txtBox;
        bool isFirst = true;
        int currLevel = 0;
        List<IMultiRogueRow> currRows = new List<IMultiRogueRow>();
        public WPFSyntaxCommands(RichTextBox txtBox)
        {
            this.txtBox = txtBox;
            txtBox.AcceptsReturn = true;
            txtBox.AcceptsTab = true;
            
        }
        public IMultiRogueRow GetLabel(IMultiRogueRow parentRow, string txt, MyColors myColor = MyColors.black, Boldness boldness = Boldness.none, FontSize fontSize = FontSize.regular, rogueCore.hqlSyntaxV3.IntellsenseDecor.Underline isUnderlined = rogueCore.hqlSyntaxV3.IntellsenseDecor.Underline.none)
        {
            currRows.Add(parentRow);
            TextRange tr = new TextRange(txtBox.Document.ContentEnd, txtBox.Document.ContentEnd);
            //tr.ApplyPropertyValue(TextElement.FontSizeProperty, 10);
            tr.Text = txt.Replace("&nbsp;", " ");
            SolidColorBrush setColor;
            switch (myColor)
            {
                case MyColors.blue:
                    setColor = Brushes.Blue;
                    break;
                case MyColors.green:
                    setColor = Brushes.Green;
                    break;
                case MyColors.orange:
                    setColor = Brushes.Orange;
                    break;
                case MyColors.red:
                    setColor = Brushes.Red;
                    break;
                case MyColors.yellow:
                    setColor = Brushes.Yellow;
                    break;
                default:
                    setColor = Brushes.Black;
                    break;
            }
            double setFontSize = 12;
            switch (fontSize)
            {
                case FontSize.large:
                    setFontSize = 16;
                    break;
                case FontSize.regular:
                    setFontSize = 12;
                    break;
                case FontSize.small:
                    setFontSize = 10;
                    break;
                case FontSize.xLarge:
                    setFontSize = 20;
                    break;
                case FontSize.xxLlarge:
                    setFontSize = 25;
                    break;
            }
            switch (boldness)
            {
                case Boldness.bolder:
                    tr.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.SemiBold);
                    break;
                case Boldness.bold:
                    tr.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    break;
            }
            if (isUnderlined == rogueCore.hqlSyntaxV3.IntellsenseDecor.Underline.underline)
            {
                tr.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, setColor);
            tr.ApplyPropertyValue(TextElement.FontSizeProperty, setFontSize);
            //int i = parentRow.levelNum;
            return new ManualMultiRogueRow();
        }
        public IMultiRogueRow IndentedGroupBox(IMultiRogueRow parentRow, int levelNum)
        {
            currLevel = levelNum;
            if (!isFirst)
            {
                txtBox.AppendText("\r");
                txtBox.AppendText("\r");                
            }            
            for (int i = 1; i < currLevel; i++)
            {
                txtBox.AppendText("\t");
            }
            isFirst = false;
            return new ManualMultiRogueRow();
        }
        public void BreakLine(IMultiRogueRow parentRow)
        {
            txtBox.AppendText("\r");
            for (int i = 1; i < currLevel; i++)
            {
                txtBox.AppendText("\t");
            }
        }
    }
}
