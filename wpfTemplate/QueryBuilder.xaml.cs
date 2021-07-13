using rogue_core.RogueCode.hql.hqlSegments.table;
using rogue_core.RogueCode.hql.hqlSegments.where;
using rogue_core.rogueCore.database;
using rogue_core.rogueCore.hql;
using rogue_core.rogueCore.hql.hqlSegments.join;
using rogue_core.rogueCore.hql.hqlSegments.where;
using rogue_core.rogueCore.hql.segments.selects;
using rogue_core.rogueCore.hqlFilter;
using rogue_core.rogueCore.id;
using rogue_core.rogueCore.id.rogueID;
using rogue_core.rogueCore.queryResults;
using rogue_core.rogueCore.row;
using rogue_core.rogueCore.row.encoded.column;
using rogue_core.rogueCore.StoredProcedures.StoredQuery;
using rogue_core.rogueCore.table;
using rogue_core.rogueCore.table.encoded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpfTemplate;
using static rogue_core.rogueCore.codeGenerator.GeneratedRows.UI;
using static rogueCore.UI.UISegmentBuilder;

namespace wpfTemplate
{
    /// <summary>
    /// Interaction logic for QueryBuilder.xaml
    /// </summary>
    public partial class QueryBuilder : Window
    {
        int currRowNum = 0;
        public QueryBuilder()
        {
            InitializeComponent();
            //HQLQuery test = new HQLQuery("^<-1010>(0.-1015.METAROW_NAME)[]{*=-1.-1012}^<-1026>(1.-2006.CONTROLNAME)[]{*$0.-1012}^<-1026>(2.-2006.CONTROLNAME)[]{*=0.-1012}");
            HQLQuery tblQry = new HQLQuery(HQLEncoder.AllTablesQuery);
            foreach (var rowPair in tblQry.hierarchyGrid)
            {
                var row = rowPair.Value;
                lstAllTables.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName("METAROW_NAME"), Tag = row.GetValueByColName("ROGUECOLUMNID") });
            }
            mainTree.Tag = new AllTableInfos();
            HQLQuery qryNamesQry = new HQLQuery("<1211.QueryName>(*)[]{*=Root.-1012}");
            foreach (var rowPair in qryNamesQry.hierarchyGrid)
            {
                String qryName = rowPair.Value.GetValueByColName("QueryName");
                lstSavedQueries.Items.Add(new ComboBoxItem() { Content = qryName, Tag = rowPair.Value.rowID });
            }

            HQLQuery qryDBsQry = new HQLQuery("<-1010>(*)[-1017=Database]{*=Root.-1012}");
            foreach (var rowPair in qryDBsQry.hierarchyGrid)
            {
                String dbID = rowPair.Value.GetValueByColName("ROGUECOLUMNID");
                String dbName = rowPair.Value.GetValueByColName("METAROW_NAME");
                lstDatabases.Items.Add(new ComboBoxItem() { Content = dbName, Tag = rowPair.Value.rowID });
            }
        }
        private void AddTableRow(TableInfo tblInfo, String ownerNodeRefName)
        {
            //TreeViewItem newNode = new TreeViewItem();
            //newNode.Name = "node" + refTableName;
            //StackPanel tableCellPanel = new StackPanel();
            //tableCellPanel.Orientation = Orientation.Horizontal;
            //tableCellPanel.Name = "tblCellPnl" + refTableName;
            //TableInfos tableInfos = new TableInfos(new TableInfo(tableID, tableName, refTableName), ((ComboBoxItem)lstParentTables.SelectedItem).Content.ToString(), currRowNum, newNode, mainTree);


            StackPanel lvlTblPanel = new StackPanel();
            lvlTblPanel.Orientation = Orientation.Vertical;

            //ListBox lstLevelTables = new ListBox();

            //lstLevelTables.Name = "lstLevelTables" + tblInfo.TableRefName;
            //lstLevelTables.Items.Add(new ListBoxItem() { Content = tblInfo.TableRefName });
            //lvlTblPanel.Children.Add(lstLevelTables);
            StackPanel levelGrid = new StackPanel();
            levelGrid.Orientation = Orientation.Horizontal;
            //levelGrid.ShowGridLines = true;
            //levelGrid.RowDefinitions.Add(new RowDefinition());
            //levelGrid.RowDefinitions.Add(new RowDefinition());
            //levelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //levelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //levelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //levelGrid.ColumnDefinitions.Add(new ColumnDefinition());
            StackPanel stackTblHeader = new StackPanel();
            stackTblHeader.Orientation = Orientation.Vertical;

            Label TableHeader = WPFFinder.HeaderLabel(tblInfo.TableRefName, 0, 0);
            TableHeader.FontWeight = FontWeights.Bold;
            Button btnDeleteTable = new Button();
            btnDeleteTable.Content = "Delete Table";
            btnDeleteTable.Tag = tblInfo.TableRefName;
            btnDeleteTable.Click += DeleteTable;
            stackTblHeader.Children.Add(TableHeader);
            stackTblHeader.Children.Add(btnDeleteTable);
            levelGrid.Children.Add(stackTblHeader);
            //levelGrid.Children.Add(btnDeleteTable);
            //*Join
            StackPanel stackAllJoins = new StackPanel();
            stackAllJoins.Orientation = Orientation.Vertical;
            Label JoinHeader = WPFFinder.HeaderLabel("Join Column", 0, 1);
            stackAllJoins.Children.Add(JoinHeader);
            //levelGrid.Children.Add(JoinHeader);

            Grid joinRow = tblInfo.GetJoinRow();
            Grid.SetRow(joinRow, 1);
            Grid.SetColumn(joinRow, 1);
            stackAllJoins.Children.Add(joinRow);
            levelGrid.Children.Add(stackAllJoins);

            //*Select
            StackPanel pnlCols = new StackPanel();
            pnlCols.Orientation = Orientation.Vertical;
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Vertical;
            pnlCols.Children.Add(stack);
            Grid.SetRow(stack, 0);
            Grid.SetColumn(stack, 2);
            Label SelectHeader = WPFFinder.HeaderLabel("Selects", 0, 1);
            stack.Children.Add(SelectHeader);

            StackPanel btnStack = new StackPanel();
            btnStack.Orientation = Orientation.Horizontal;
            stack.Children.Add(btnStack);

            Button btnAddColumnRow = new Button();
            btnAddColumnRow.Content = "Add Column";
            btnAddColumnRow.Tag = tblInfo;
            btnAddColumnRow.Click += AddColumnrow_Click;
            btnStack.Children.Add(btnAddColumnRow);
            //levelGrid.Children.Add(stack);

            Button btnDeleteColumnRow = new Button();
            btnDeleteColumnRow.Content = "Delete Column";
            btnDeleteColumnRow.Tag = tblInfo;
            btnDeleteColumnRow.Click += DeleteColumnrow_Click;
            //stack.Children.Add(btnDeleteColumnRow);
            btnStack.Children.Add(btnDeleteColumnRow);

            //levelGrid.Children.Add(stack);


            pnlCols.Orientation = Orientation.Vertical;
            //pnlCols.Name = "pnlColumns" + tblInfo.TableRefName;
            tblInfo.pnlColumns = pnlCols;
            //nodePnl.Children.Add(pnlCols);
            Grid.SetRow(pnlCols, 1);
            Grid.SetColumn(pnlCols, 2);
            levelGrid.Children.Add(pnlCols);

            StackPanel pnlWheres = new StackPanel();

            StackPanel stackWhere = new StackPanel();
            stackWhere.Orientation = Orientation.Vertical;
            Grid.SetRow(stackWhere, 0);
            Grid.SetColumn(stackWhere, 3);
            Label WhereHeader = WPFFinder.HeaderLabel("Where Clause", 0, 3);
            stackWhere.Children.Add(WhereHeader);
            StackPanel btnWherePanel = new StackPanel();
            btnWherePanel.Orientation = Orientation.Horizontal;

            Button btnAddWhereRow = new Button();
            btnAddWhereRow.Tag = tblInfo;
            btnAddWhereRow.Content = "Add Where Clause";
            btnAddWhereRow.Click += AddWhereClause_Click;
            btnWherePanel.Children.Add(btnAddWhereRow);

            Button btnDeleteWhereRow = new Button();
            btnDeleteWhereRow.Tag = tblInfo;
            btnDeleteWhereRow.Content = "Delete Where Clause";
            btnDeleteWhereRow.Click += DeleteWhereClause_Click;
            btnWherePanel.Children.Add(btnDeleteWhereRow);

            stackWhere.Children.Add(btnWherePanel);
            pnlWheres.Children.Add(stackWhere);

            pnlWheres.Orientation = Orientation.Vertical;
            pnlWheres.Name = "pnlWheres" + tblInfo.TableRefName;
            tblInfo.pnlWheres = pnlWheres;
            //nodePnl.Children.Add(pnlCols);
            Grid.SetRow(pnlWheres, 1);
            Grid.SetColumn(pnlWheres, 3);
            levelGrid.Children.Add(pnlWheres);


            Grid.SetRow(lvlTblPanel, 1);
            Grid.SetColumn(lvlTblPanel, 0);
            levelGrid.Children.Add(lvlTblPanel);

            currRowNum++;


            StackPanel nodePnl = new StackPanel();
            nodePnl.Orientation = Orientation.Horizontal;
            //nodePnl.Children.Add(lvlTblPanel);
            //TreeViewItem ownerItem = WPFFinder.FindChild<TreeViewItem>(mainTree, "node" + ownerNodeRefName);
            StackPanel tableCellPanel = (StackPanel)tblInfo.thsNode.Header;
            tableCellPanel.Children.Add(levelGrid);
        }
        private void AddLevel(TableInfo tblInfo, String parentTableRefName)
        {
            TreeViewItem newNode = new TreeViewItem();
            newNode.Name = "node" + tblInfo.TableRefName;
            tblInfo.thsNode = newNode;
            StackPanel tableCellPanel = new StackPanel();
            tableCellPanel.Orientation = Orientation.Horizontal;
            tableCellPanel.Name = "tblCellPnl" + tblInfo.TableRefName;
            TableInfos tableInfos = new TableInfos(tblInfo, parentTableRefName, currRowNum, newNode, mainTree);
            ((AllTableInfos)mainTree.Tag).Add(tableInfos);
            ((AllTableInfos)mainTree.Tag).nodeMatrix.Add(tblInfo.TableRefName, tblInfo.thsNode);
            newNode.Header = tableCellPanel;
            if (tableInfos.parentTableRefName != "Root")
            {
                TreeViewItem child = tableInfos.GetHeaderNode();
                child.Items.Add(newNode);
            }
            else
            {
                mainTree.Items.Add(newNode);
            }
            newNode.Tag = tableInfos;
        }
        private void AddLvl_Click(object sender, RoutedEventArgs e)
        {
            if (((AllTableInfos)mainTree.Tag).AllRefNames().Contains(txtSeudoName.Text))
            {
                return;
            }
            try
            {
                String baseTblName = ((ComboBoxItem)lstAllTables.SelectedItem).Content.ToString();
                int tableID = int.Parse(((ComboBoxItem)lstAllTables.SelectedItem).Tag.ToString());
                String tableRefName = txtSeudoName.Text;
                String parentTableRefName = ((ComboBoxItem)lstParentTables.SelectedItem).Content.ToString();
                int parentTableID = int.Parse(((ComboBoxItem)lstParentTables.SelectedItem).Tag.ToString());
                Char joinType = '=';
                String joinVal = ((ComboBoxItem)lstJoinOptions.SelectedItem).Tag.ToString();
                switch (joinVal)
                {
                    case "under":
                        joinType = '=';
                        break;
                    case "merge":
                        joinType = '$';
                        break;
                    case "rowToCol":
                        joinType = '?';
                        break;
                }
                TableInfo thsTblInfo = new TableInfo(tableID, tableRefName, mainTree, parentTableRefName, joinType, parentTableID);
                AddNewTableInfo(thsTblInfo);
            }
            catch (Exception ex)
            {
                String blah = ex.ToString();
            }
        }
        private void AddNewTableInfo(TableInfo thsTblInfo)
        {
            switch (thsTblInfo.joinType)
            {
                case '=':
                    AddLevel(thsTblInfo, thsTblInfo.parentTableRef);
                    AddTableRow(thsTblInfo, thsTblInfo.TableRefName);
                    lstParentTables.Items.Add(new ComboBoxItem() { Tag = thsTblInfo.TableID, Content = thsTblInfo.TableRefName });
                    lstParentTables.SelectedValue = thsTblInfo.TableRefName;
                    break;
                case '$':
                    thsTblInfo.thsNode = ((AllTableInfos)mainTree.Tag).nodeMatrix[thsTblInfo.parentTableRef];
                    AddTableRow(thsTblInfo, thsTblInfo.parentTableRef);
                    ((TableInfos)thsTblInfo.thsNode.Tag).Add(thsTblInfo);
                    //TreeViewItem foundTreeItem = FindChild<TreeViewItem>(mainTree, "node" + parentTableRefName);
                    //TableInfos tbls = (TableInfos)foundTreeItem.Tag;
                    //tbls.AddMergeTable(new TableInfo(tableID,baseTblName, txtSeudoName.Text, mainTree), parentTableRefName);
                    break;
                case '?':
                    AddLevel(thsTblInfo, thsTblInfo.parentTableRef);
                    AddTableRow(thsTblInfo, thsTblInfo.TableRefName);
                    lstParentTables.Items.Add(new ComboBoxItem() { Tag = thsTblInfo.TableID, Content = thsTblInfo.TableRefName });
                    lstParentTables.SelectedValue = thsTblInfo.TableRefName;
                    break;
            }
        }
        List<TableInfo> GetParentTableList(TreeViewItem parent, List<TableInfo> currLst)
        {
            foreach (TableInfo thsInfo in ((TableInfos)parent.Tag))
            {
                currLst.Add(thsInfo);
            }
            if (parent.Parent != null && parent.Parent is TreeViewItem)
            {
                GetParentTableList((TreeViewItem)parent.Parent, currLst);
            }
            return currLst;
        }
        private void LstAllTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String baseTblName = ((ComboBoxItem)lstAllTables.SelectedItem).Content.ToString();
            txtSeudoName.Text = baseTblName;
        }
        private void AddColumnrow_Click(object sender, RoutedEventArgs e)
        {
            // do something
            Button btn = (Button)sender;
            TableInfo info = (TableInfo)btn.Tag;
            info.AddColumnRow();
        }
        private void DeleteColumnrow_Click(object sender, RoutedEventArgs e)
        {
            // do something
            Button btn = (Button)sender;
            TableInfo info = (TableInfo)btn.Tag;
            info.DeleteColumnRow();
        }
        void AddWhereClause_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            TableInfo info = (TableInfo)btn.Tag;
            info.AddWhereClauseRow();
        }
        private void DeleteWhereClause_Click(object sender, RoutedEventArgs e)
        {
            // do something
            Button btn = (Button)sender;
            TableInfo info = (TableInfo)btn.Tag;
            info.DeleteWhereClause();
        }
        private void DeleteTable(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ((AllTableInfos)mainTree.Tag).DeleteTable(btn.Tag.ToString(), lstParentTables);

        }
        //private void GetColsPerTable(object sender, SelectionChangedEventArgs e)
        //{
        //    ComboBox thsBox = (ComboBox)sender;
        //    String tableRefName = thsBox.Tag.ToString();
        //    String baseTblName = ((ComboBoxItem)thsBox.SelectedItem).Content.ToString();
        //    ComboBox foundList = FindChild<ComboBox>(mainTree, "Columns" + tableRefName);
        //    //foreach (var rowPair in HQLEncoder.GetColumnsPerTable(tableID))
        //    //{
        //    //    var row = rowPair;
        //    //    foundList.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
        //    //}
        //    TreeViewItem foundTree  = FindChild<TreeViewItem>(mainTree, "node" + tableRefName);
        //    foreach (TableInfo thsTbl in (TableInfos)(foundTree.Tag))
        //    {
        //        foundList.Items.Clear();
        //        if(thsTbl.TableName == baseTblName)
        //        {
        //            foreach (var rowPair in HQLEncoder.GetColumnsPerTable(thsTbl.TableID))
        //            {
        //                var row = rowPair;
        //                foundList.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
        //            }
        //        }

        //    }

        //}
        public static IEnumerable<TreeViewItem> FindAllNodesByHeader(TreeView tree, string tblRefName)
        {
            return tree.Items.Cast<TreeViewItem>().SelectRecursive(node => node.Items.Cast<TreeViewItem>()).Where(node => ((TableInfo)node.Tag).TableRefName == tblRefName);
        }
        static TreeViewItem FindFirstNodeByHeader(TreeView tree, string header)
        {
            return tree.Items.Cast<TreeViewItem>().SelectRecursive(node => node.Items.Cast<TreeViewItem>()).FirstOrDefault(node => node.Header == header);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String qry = "";
                foreach (TableInfo thsTbl in ((AllTableInfos)mainTree.Tag).ParseAllTables())
                {
                    qry += thsTbl.GetSegment(((AllTableInfos)mainTree.Tag)).GetHQLText();
                }
                txtQuery.Text = qry;
                txtFullQry.Text = ConvertHumanHQL.DecodeHQLToHumanReadable(txtQuery.Text);
            }
            catch (Exception ex)
            {
                String blah = ex.ToString();
            }
        }
        private void RunDisplayResults(HierarchyGrid hierarchyGrid)
        {
            int currLvl = 0;
            TreeViewItem lastNode = null;
            treQryResults.Items.Clear();
            foreach (KeyValuePair<int, HierarchyRow> thsPair in hierarchyGrid)
            {
                int diff = thsPair.Key - currLvl;
                TreeViewItem newItem = new TreeViewItem();
                if (treQryResults.Items.Count == 0)
                {
                    treQryResults.Items.Add(newItem);
                }
                else
                {
                    if (diff > 0)
                    {
                        lastNode.Items.Add(newItem);
                    }
                    else
                    {
                        if (thsPair.Key == 0)
                        {
                            treQryResults.Items.Add(newItem);
                        }
                        else if (diff <= 0)
                        {
                            while (diff <= 0)
                            {
                                lastNode = (TreeViewItem)lastNode.Parent;
                                diff++;
                            }
                            lastNode.Items.Add(newItem);
                        }
                    }
                }
                lastNode = newItem;
                currLvl = thsPair.Key;
                HierarchyRow hiRow = thsPair.Value;
                StackPanel stackPairs = new StackPanel();
                stackPairs.Orientation = Orientation.Horizontal;
                foreach (SelectColumn thsCol in hiRow.allColumns)
                {
                    Label value = new Label();
                    value.Content = thsCol.columnAliasName + ":" + hiRow.Values[thsCol.columnName];
                    stackPairs.Children.Add(value);
                    //p++;
                }
                newItem.Header = stackPairs;
            }
        }
        private void RunQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HQLQuery qry = new HQLQuery(txtQuery.Text);
                RunDisplayResults(qry.hierarchyGrid);
            }
            catch (Exception ex)
            {
                String test = ex.ToString();
            }
        }
        private void CreateUI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window winUI = new Window();
                StackPanel allPnl = new StackPanel();
                winUI.Content = allPnl;
                WPFSegmentBuilder sectionBuilder = new WPFSegmentBuilder(new StoredHQLQuery(txtQuery.Text, 1), allPnl);
                //WindowsUIBuilderNew.BuildSection(new List<FrameworkElement>(), txtQuery.Text, allPnl);
                winUI.Show();
            }
            catch (Exception ex)
            {
                String blah = ex.ToString();
            }
        }
        private void BtnSaveQuery_Click(object sender, RoutedEventArgs e)
        {
            if (txtSaveQueryName.Text != "")
            {
                RogueDatabase<DataRowID> thsDB = new RogueDatabase<DataRowID>(new IORecordID((FillledIOIORecords.uiDatabase.rowID.ToString())));
                //*Query Name Table
                IRogueTable qryNameTable = thsDB.GetTable("QueryName", "This table is for storing the top level names for each query and its children");
                IRogueRow newQryNameRow = qryNameTable.NewIWriteRow();
                newQryNameRow.NewWritePair(qryNameTable.ioItemID, ColumnTypes.column, "QueryName", txtSaveQueryName.Text.ToDecodedRowID());
                qryNameTable.Write();
                foreach (TableInfo thsTbl in ((AllTableInfos)mainTree.Tag).ParseAllTables())
                {
                    thsTbl.GetSegment(((AllTableInfos)mainTree.Tag)).WriteData(thsTbl.TableRefName, newQryNameRow.rowID);
                }
            }
        }
        private void LstSavedQueries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //String qry = "^<1083>(*)[]{*=-1.-1012}^<1097>(*)[]{1.1102=0.-1012}^<1114>(*)[]{2.1119=0.-1012}^<1127>(*)[]{3.1132=0.-1012}";
            //String qry = "^<1083.table>(*)[]{*=-1.-1012}^<1097.join>(*)[]{1.1102=0.-1012}^<1114.where>(*)[]{2.1119=0.-1012}^<1127.select>(*)[]{3.1132=0.-1012}";
            String qryName = lstSavedQueries.SelectedValue.ToString();
            //String qry = "^<1211.QueryName>(*)[1217=" + qryName + "]{*=Root.-1012}^<1219.QueryTableSegment>(*)[]{1.1224=0.-1012}^<1240.QueryJoinInfo>(*)[]{2.1245=1.-1012}^<1255.QueryWriteInfo>(*)[]{3.1260=1.-1012}^<1268.QuerySelectInfo>(*)[]{4.1273=1.-1012}";
            String qry = "^<1211.QueryName>(*)[1217=" + qryName + "]{*=Root.-1012}^<1219.QueryTableSegment>(*)[]{QueryTableSegment.1224=QueryName.-1012}^<1240.QueryJoinInfo>(*)[]{QueryJoinInfo.1245=QueryTableSegment.-1012}^<1255.QueryWriteInfo>(*)[]{QueryWriteInfo.1260=QueryTableSegment.-1012}^<1268.QuerySelectInfo>(*)[]{QuerySelectInfo.1273=QueryTableSegment.-1012}";
            Dictionary<int, TableInfo> allTables = new Dictionary<int, TableInfo>();
            TableInfo currTblInfo = null;
            foreach (var thsRowPair in new HQLQuery(qry).hierarchyGrid)
            {
                HierarchyRow thsRow = thsRowPair.Value;
                int level = thsRowPair.Key;
                //int currWhereClauseNum = 0;
                if (thsRow.RowGroupName.ToUpper() != "QUERYNAME")
                {
                    switch (thsRow.RowGroupName)
                    {
                        case "QueryTableSegment":
                            int TableID = int.Parse(thsRow.GetValueByColName("TABLEROGUEID"));
                            String tableRefName = thsRow.GetValueByColName("TABLEREFERENCENAME");
                            Char joinType = thsRow.GetValueByColName("JOINTYPE")[0];
                            int parentMatrixNum = int.Parse(thsRow.GetValueByColName("ParentTableMatrixNum"));
                            int matrixNum = int.Parse(thsRow.GetValueByColName("TABLEMATRIXNUM"));
                            //currWhereClauseNum = 0;
                            String parentTableRefName;
                            int parentTableRogueID;
                            if (parentMatrixNum == -1)
                            {
                                parentTableRefName = "Root";
                                parentTableRogueID = -1;
                            }
                            else
                            {
                                TableInfo parentInfo = allTables[parentMatrixNum];
                                parentTableRefName = parentInfo.TableRefName;
                                parentTableRogueID = parentInfo.TableID;
                            }
                            currTblInfo = new TableInfo(TableID, tableRefName, mainTree, parentTableRefName, joinType, parentTableRogueID);
                            allTables.Add(matrixNum, currTblInfo);
                            AddNewTableInfo(currTblInfo);
                            break;
                        case "QueryJoinInfo":
                            //currTblInfo.GetJoinRow();
                            //currTblInfo.JoinParentColumns.SelectedValue =
                            String test = thsRow.GetValueByColName("ALLJOIN");
                            if (thsRow.GetValueByColName("ALLJOIN") != "" && thsRow.GetValueByColName("ALLJOIN") != "1")
                            {
                                String localTable = currTblInfo.TableRefName;
                                String localColumn = thsRow.GetValueByColName("LOCALJOINCOLUMNID");
                                currTblInfo.JoinLocalTables.SelectedValuePath = "Content";
                                currTblInfo.JoinLocalTables.SelectedValue = localTable;
                                currTblInfo.JoinLocalColumns.SelectedValuePath = "Tag";
                                currTblInfo.JoinLocalColumns.SelectedValue = localColumn;
                                int parentJoinMatrixNum = int.Parse(thsRow.GetValueByColName("PARENTJOINMATRIXNUM"));
                                if (parentJoinMatrixNum != -1)
                                {

                                    String parentTable = allTables[parentJoinMatrixNum].TableRefName;
                                    currTblInfo.JoinParentTables.SelectedValuePath = "Content";
                                    currTblInfo.JoinParentTables.SelectedValue = parentTable;
                                    String parentColumn = thsRow.GetValueByColName("PARENTJOINCOLUMNID");
                                    currTblInfo.JoinParentColumns.SelectedValuePath = "Tag";
                                    currTblInfo.JoinParentColumns.SelectedValue = parentColumn;
                                }
                            }
                            //String pallTables[parentMatrixNum];


                            break;
                        case "QueryWriteInfo":
                            //int i = 0;
                            //foreach(FrameworkElement child in currTblInfo.pnlWheres.Children)
                            //{
                            //String test = thsRow.GetValueByColName("LOCALCOLUMNID");
                            currTblInfo.AddWhereClauseRow(thsRow.GetValueByColName("LOCALCOLUMNID"), thsRow.GetValueByColName("WHEREVALUE"));

                            //ComboBox whereColumns = WPFFinder.FindChild<ComboBox>(currTblInfo.thsNode, "WhereColumns" + currTblInfo.TableRefName + i.ToString());
                            //whereColumns.SelectedValuePath = "Content";
                            //whereColumns.SelectedValue = thsRow.GetValueByColName("LocalColumnID");
                            //ColumnRowID whereColID = new ColumnRowID(int.Parse(((ComboBoxItem)whereColumns.SelectedItem).Tag.ToString()));
                            //ComboBox whereValueOptions = WPFFinder.FindChild<ComboBox>(mainTree, "WhereValueOptions" + currTblInfo.TableRefName + i.ToString());
                            //WhereClause whereClause = new WhereClause(new ColumnRowID(((ComboBoxItem)whereColumns.SelectedItem).Tag.ToString()), WhereClause.EvaluationTypes.equal, ((ComboBoxItem)whereValueOptions.SelectedItem).Content.ToString());
                            //whereClauses.Add(whereClause);
                            //currWhereClauseNum++;
                            //}
                            break;
                        case "QuerySelectInfo":
                            currTblInfo.AddColumnRow(allTables[int.Parse(thsRow.GetValueByColName("COLUMNMATRIXID"))].TableRefName, thsRow.GetValueByColName("COLUMNROWID"), thsRow.GetValueByColName("COLUMNALIASNAME"), thsRow.GetValueByColName("COLUMNCONSTVALUE"));
                            break;
                    }

                }
            }
        }
        private void BtnAddNewTable_Click(object sender, RoutedEventArgs e)
        {
            IORecordID dbID = new IORecordID(((ComboBoxItem)lstDatabases.SelectedItem).Tag.ToString());
            RogueDatabase<DataRowID> db = new RogueDatabase<DataRowID>(dbID);
            db.GetTable(txtTableName.Text, txtTableDesc.Text);
        }
        private void RunFromHumanHQL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HierarchyGrid grd = HQLQuery.RunFullHQLQuery(txtFullQry.Text);
                RunDisplayResults(grd);
            }
            catch (Exception ex)
            {
                String test = ex.ToString();
            }
        }

        private void RunHQLBreak_Click(object sender, RoutedEventArgs e)
        {
            HierarchyGrid grd = HQLQuery.RunFullHQLQuery(txtFullQry.Text);
            RunDisplayResults(grd);
        }

        private void CreateUIFromHumanHQL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window winUI = new Window();
                StackPanel allPnl = new StackPanel();
                winUI.Content = allPnl;
                String hql = ConvertHumanHQL.Convert(txtFullQry.Text);
                WPFSegmentBuilder sectionBuilder = new WPFSegmentBuilder(new StoredHQLQuery(txtFullQry.Text, 1), allPnl);
                //WindowsUIBuilderNew.BuildSection(new List<FrameworkElement>(), txtQuery.Text, allPnl);
                winUI.Show();
            }
            catch (Exception ex)
            {
                String blah = ex.ToString();
            }
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        //public static T FindChild<T>(DependencyObject parent, string childName)
        //   where T : DependencyObject
        //{
        //    // Confirm parent and childName are valid. 
        //    if (parent == null) return null;

        //    T foundChild = null;

        //    int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < childrenCount; i++)
        //    {
        //        var child = VisualTreeHelper.GetChild(parent, i);
        //        // If the child is not of the request child type child
        //        T childType = child as T;
        //        if (childType == null)
        //        {
        //            // recursively drill down the tree
        //            foundChild = FindChild<T>(child, childName);

        //            // If the child is found, break so we do not overwrite the found child. 
        //            if (foundChild != null)
        //            {
        //                break;
        //            }
        //        }
        //        else if (!string.IsNullOrEmpty(childName))
        //        {
        //            var frameworkElement = child as FrameworkElement;
        //            // If the child's name is set for search
        //            if (frameworkElement != null && frameworkElement.Name == childName)
        //            {
        //                // if the child's name is of the request name
        //                foundChild = (T)child;
        //                break;
        //            }
        //            else
        //            {
        //                foundChild = FindChild<T>(child, childName);
        //            }
        //        }
        //        else
        //        {
        //            // child element found.
        //            foundChild = (T)child;
        //            break;
        //        }
        //    }

        //    return foundChild;
        //}
    }
    public static class WPFFinder
    {
        public static T FindChild<T>(DependencyObject parent, string childName)
          where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                    else
                    {
                        foundChild = FindChild<T>(child, childName);
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        public static Label HeaderLabel(String text, int row = -1, int col = -1)
        {
            Label lbl = new Label();
            lbl.Content = text;
            lbl.FontSize = 15;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            if (row != -1)
            {
                Grid.SetRow(lbl, row);
            }
            if (col != -1)
            {
                Grid.SetColumn(lbl, col);
            }
            return lbl;
        }
        public static T FindChildOLD<T>(DependencyObject depObj, String Name) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)

                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T && child is FrameworkElement && ((FrameworkElement)child).Name == Name)
                    {

                        return (T)child;

                    }
                    return FindChild<T>(child, Name);
                }
            }
            return null;
        }
        //public static T HeaderItem<T>(T item, String text, int row = -1, int col = -1) where T : FrameWorkElement
        //{
        //    //lbl = new T();
        //    item.Content = text;
        //    item.FontSize = 15;
        //    lbl.HorizontalAlignment = HorizontalAlignment.Center;
        //    if (row != -1)
        //    {
        //        Grid.SetRow(lbl, row);
        //    }
        //    if (col != -1)
        //    {
        //        Grid.SetColumn(lbl, col);
        //    }
        //    return lbl;
        //}
        // Recursively find the named control.

    }

    class WPFSegmentBuilder
    {
        List<FrameworkElement> lstContainers = new List<FrameworkElement>();
        public Dictionary<String, FrameworkElement> namedControls = new Dictionary<String, FrameworkElement>();
        List<List<String>> postAttributes = new List<List<String>>();
        //Dictionary<string, FrameworkElement> results = new Dictionary<string, FrameworkElement>();
        public int tempTableID = 0;
        StoredHQLQuery thsQry;
        //FrameworkElement topStack;
        // Action<Object, RoutedEventArgs, ClickActions, UISegmentQuery, String> ClickEvent;
        public WPFSegmentBuilder(StoredHQLQuery qry, FrameworkElement pnl, Action<Object, RoutedEventArgs, ClickActions, StoredHQLQuery, String> selectEvent = null)
        {
            //this.ClickEvent = selectEvent;
            this.thsQry = qry;
            StackPanel single = new StackPanel();
            if (pnl is StackPanel)
            {
                ((StackPanel)pnl).Children.Add(single);
            }
            if (pnl is ItemsControl)
            {
                ((ItemsControl)pnl).Items.Add(single);
            }
            BuildSection(qry.FinalHQLStatement(), single);
        }
        //public WPFSegmentBuilder(StackPanel topPanel)
        //{
        //    topStack = topPanel;
        //    //**Bring BackBuildHQLDeveloper(-1010);

        //    //String FullMenuQry = "^<-1026.TopTable>(0.-2006.CONTROLNAME,0.-1012.PARENTRELATION.child)[-2006=table]{*=-1.-1012}^<-1026.TableRow>(1.-2006.CONTROLNAME,1.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.MenuTableCell>(2.-2006.CONTROLNAME,2.-1012.PARENTRELATION.child)[-2006=tablecell]{*=1.-1012}^<-1026.ContentTableCell>(3.-2006.CONTROLNAME,3.-1012.PARENTRELATION.child)[-2006=tablecell]{*=1.-1012}^<-2033.menuTableCellWidth>(4.-2037.ATTRIBUTETYPE,4.-1012.ATTRIBUTEVALUE.20,4.-1012.PARENTRELATION.attribute)[-2037=widthpercent]{*=2.-1012}^<-2033.contentTableCellWidth>(5.-2037.ATTRIBUTETYPE,5.-1012.PARENTRELATION.attribute,5.-2037.ATTRIBUTEVALUE.80)[-2037=widthpercent]{*=3.-1012}^<-1026.ContentPanel>(6.-2006.CONTROLNAME,6.-1012.PARENTRELATION.child)[-2006=groupbox]{*=3.-1012}^<-2033.menuRowSpanAtt>(7.-2037.ATTRIBUTETYPE,7.-1012.PARENTRELATION.attribute,7.-1012.ATTRIBUTEVALUE.3)[-2037=rowspan]{*=2.-1012}^<-1026.menuPanel>(8.-2006.CONTROLNAME,8.-1012.PARENTRELATION.child)[-2006=groupbox]{*=2.-1012}^<-1026.TableRowTwo>(9.-2006.CONTROLNAME,9.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.TableRowThree>(10.-2006.CONTROLNAME,10.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.TableCellOneRowTwo>(11.-2006.CONTROLNAME,11.-1012.PARENTRELATION.child)[-2006=tablecell]{*=9.-1012}^<-1026.TableCellOneRowThree>(12.-2006.CONTROLNAME,12.-1012.PARENTRELATION.child)[-2006=tablecell]{*=10.-1012}^<-1026.pnlRowThree>(13.-2006.CONTROLNAME,13.-1012.PARENTRELATION.child)[-2006=groupbox]{*=12.-1012}^<-1026.labelMenuPnl>(14.-2006.CONTROLNAME,14.-1012.PARENTRELATION.child)[-2006=label]{*=8.-1012}^<-2033.mnuPnlLblText>(15.-2037.ATTRIBUTETYPE,15.-1012.PARENTRELATION.attribute,15.-2037.ATTRIBUTEVALUE.MnuPnl)[-2037=text]{*=14.-1012}^<-1026.lblPnlRowThree>(16.-2006.CONTROLNAME,16.-1012.PARENTRELATION.child)[-2006=label]{*=13.-1012}^<-2033.pnlRowThreeText>(17.-2037.ATTRIBUTETYPE,17.-1012.PARENTRELATION.attribute,17.-1012.ATTRIUTEVALUE.RowThreeCellOne)[-2037=text]{*=16.-1012}^<-1026.RowOneCellTwoLbl>(18.-2006.CONTROLNAME,18.-1012.PARENTRELATION.child)[-2006=label]{*=6.-1012}^<-2033.RowOneCellTwoTexst>(19.-2037.ATTRIBUTETYPE,19.-1012.PARENTRELATION.attribute,19.-1012.ATTRIBUTEVALUE.rowOneCellTwo)[-2037=text]{*=18.-1012}^<-1026.TableCellTwoRowTwo>(20.-2006.CONTROLNAME,20.-1012.PARENTRELATION.child)[-2006=tablecell]{*=9.-1012}^<-1026.pnlCellTwoRowTwo>(21.-2006.CONTROLNAME,21.-1012.PARENTRELATION.child)[-2006=groupbox]{*=20.-1012}^<-1026.lblCelllToRowTwo>(22.-2006.CONTROLNAME,22.-1012.PARENTRELATION.child)[-2006=label]{*=21.-1012}^<-2033.txtCellTwoRowTwo>(23.-2037.ATTRIBUTETYPE,23.-1012.ATTRIBUTEVALUE.CellTwoRowTwo,23.-1012.PARENTRELATION.attribute)[-2037=text]{*=22.-1012}^<-1026.CellTwoRowThree>(24.-2006.CONTROLNAME,24.-1012.PARENTRELATION.child)[-2006=tablecell]{*=10.-1012}^<-1026.pnlCellTwoRowThree>(25.-2006.CONTROLNAME,25.-1012.PARENTRELATION.child)[-2006=groupbox]{*=24.-1012}^<-1026.lblCellTwoRowThree>(26.-2006.CONTROLNAME,26.-1012.PARENTRELATION.child)[-2006=label]{*=25.-1012}^<-2033.txtCellTwoRowThree>(27.-2037.ATTRIBUTETYPE,27.-1012.PARENTRELATION.attribute,27.-1012.ATTRIBUTEVALUE.CellTwo Row Three)[-2037=text]{*=26.-1012}^<-2033.mnuPanelName>(28.-2037.ATTRIBUTETYPE,28.-1012.ATTRIBUTEVALUE.MENUPANEL,28.-1012.PARENTRELATION.attribute)[-2037=idname]{*=8.-1012}^<-2033.contentPanelName>(29.-2037.ATTRIBUTETYPE,29.-1012.PARENTRELATION.attribute,29.-1012.ATTRIBUTEVALUE.SECTIONONE)[-2037=idname]{*=6.-1012}^<-2033.pnlCellTwoRowTwoName>(30.-2037.ATTRIBUTETYPE,30.-1012.ATTRIBUTEVALUE.SECTIONTWO,30.-1012.PARENTRELATION.attribute)[-2037=idname]{*=21.-1012}^<-2033.pnlCelltwoRowThreeName>(31.-2037.ATTRIBUTETYPE,31.-1012.ATTRIBUTEVALUE.SECTIONTHREE,31.-1012.PARENTRELATION.attribute)[-2037=idname]{*=25.-1012}";
        //    //// String FullMenuQry = "^<-1026.TopTable>(0.-2006.CONTROLNAME,0.-1012.PARENTRELATION.child)[-2006=table]{*=-1.-1012}^<-1026.TableRow>(1.-2006.CONTROLNAME,1.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.MenuTableCell>(2.-2006.CONTROLNAME,2.-1012.PARENTRELATION.child)[-2006=tablecell]{*=1.-1012}^<-1026.ContentTableCell>(3.-2006.CONTROLNAME,3.-1012.PARENTRELATION.child)[-2006=tablecell]{*=1.-1012}^<-2033.menuTableCellWidth>(4.-2037.ATTRIBUTETYPE,4.-1012.ATTRIBUTEVALUE.20,4.-1012.PARENTRELATION.attribute)[-2037=widthpercent]{*=2.-1012}^<-2033.contentTableCellWidth>(5.-2037.ATTRIBUTETYPE,5.-1012.PARENTRELATION.attribute,5.-2037.ATTRIBUTEVALUE.80)[-2037=widthpercent]{*=3.-1012}^<-1026.ContentPanel>(6.-2006.CONTROLNAME,6.-1012.PARENTRELATION.child)[-2006=groupbox]{*=3.-1012}^<-2033.menuRowSpanAtt>(7.-2037.ATTRIBUTETYPE,7.-1012.PARENTRELATION.attribute,7.-1012.ATTRIBUTEVALUE.3)[-2037=rowspan]{*=2.-1012}^<-1026.menuPanel>(8.-2006.CONTROLNAME,8.-1012.PARENTRELATION.child)[-2006=groupbox]{*=2.-1012}^<-1026.TableRowTwo>(9.-2006.CONTROLNAME,9.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.TableRowThree>(10.-2006.CONTROLNAME,10.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.TableCellOneRowTwo>(11.-2006.CONTROLNAME,11.-1012.PARENTRELATION.child)[-2006=tablecell]{*=9.-1012}^<-1026.TableCellOneRowThree>(12.-2006.CONTROLNAME,12.-1012.PARENTRELATION.child)[-2006=tablecell]{*=10.-1012}^<-1026.pnlRowThree>(13.-2006.CONTROLNAME,13.-1012.PARENTRELATION.child)[-2006=groupbox]{*=12.-1012}^<-1026.labelMenuPnl>(14.-2006.CONTROLNAME,14.-1012.PARENTRELATION.child)[-2006=label]{*=8.-1012}^<-2033.mnuPnlLblText>(15.-2037.ATTRIBUTETYPE,15.-1012.PARENTRELATION.attribute,15.-2037.ATTRIBUTEVALUE.MnuPnl)[-2037=text]{*=14.-1012}^<-1026.lblPnlRowThree>(16.-2006.CONTROLNAME,16.-1012.PARENTRELATION.child)[-2006=label]{*=13.-1012}^<-2033.pnlRowThreeText>(17.-2037.ATTRIBUTETYPE,17.-1012.PARENTRELATION.attribute,17.-1012.ATTRIUTEVALUE.RowThreeCellOne)[-2037=text]{*=16.-1012}^<-1026.RowOneCellTwoLbl>(18.-2006.CONTROLNAME,18.-1012.PARENTRELATION.child)[-2006=label]{*=6.-1012}^<-2033.RowOneCellTwoTexst>(19.-2037.ATTRIBUTETYPE,19.-1012.PARENTRELATION.attribute,19.-1012.ATTRIBUTEVALUE.rowOneCellTwo)[-2037=text]{*=18.-1012}^<-1026.TableCellTwoRowTwo>(20.-2006.CONTROLNAME,20.-1012.PARENTRELATION.child)[-2006=tablecell]{*=9.-1012}^<-1026.pnlCellTwoRowTwo>(21.-2006.CONTROLNAME,21.-1012.PARENTRELATION.child)[-2006=groupbox]{*=20.-1012}^<-1026.lblCelllToRowTwo>(22.-2006.CONTROLNAME,22.-1012.PARENTRELATION.child)[-2006=label]{*=21.-1012}^<-2033.txtCellTwoRowTwo>(23.-2037.ATTRIBUTETYPE,23.-1012.ATTRIBUTEVALUE.CellTwoRowTwo,23.-1012.PARENTRELATION.attribute)[-2037=text]{*=22.-1012}^<-1026.CellTwoRowThree>(24.-2006.CONTROLNAME,24.-1012.PARENTRELATION.child)[-2006=tablecell]{*=10.-1012}^<-1026.pnlCellTwoRowThree>(25.-2006.CONTROLNAME,25.-1012.PARENTRELATION.child)[-2006=groupbox]{*=24.-1012}^<-1026.lblCellTwoRowThree>(26.-2006.CONTROLNAME,26.-1012.PARENTRELATION.child)[-2006=label]{*=25.-1012}^<-2033.txtCellTwoRowThree>(27.-2037.ATTRIBUTETYPE,27.-1012.PARENTRELATION.attribute,27.-1012.ATTRIBUTEVALUE.CellTwo Row Three)[-2037=text]{*=26.-1012}^<-2033.mnuPanelName>(28.-2037.ATTRIBUTETYPE,28.-1012.ATTRIBUTEVALUE.MENUPANEL,28.-1012.PARENTRELATION.attribute)[-2037=idname]{*=8.-1012}^<-2033.contentPanelName>(29.-2037.ATTRIBUTETYPE,29.-1012.PARENTRELATION.attribute,29.-1012.ATTRIBUTEVALUE.SECTIONONE)[-2037=idname]{*=6.-1012}^<-2033.pnlCellTwoRowTwoName>(30.-2037.ATTRIBUTETYPE,30.-1012.ATTRIBUTEVALUE.SECTIONTWO,30.-1012.ROGUECOLUMNID)[-2037=idname]{*=21.-1012}^<-2033.pnlCelltwoRowThreeName>(31.-2037.ATTRIBUTETYPE,31.-1012.ATTRIBUTEVALUE.SECTIONTHREE,31.-1012.PARENTRELATION.attribute)[-2037=idname]{*=25.-1012}";
        //    ////String FullMenuQry = "^< -1026.TopTable > (0.- 2006.CONTROLNAME, 0.- 1012.PARENTRELATION.child)[-2006 = table]{*= -1.- 1012}^< -1026.TableRow > (1.- 2006.CONTROLNAME, 1.- 1012.PARENTRELATION.child)[-2006 = tablerow]{*= 0.- 1012}^< -1026.MenuTableCell > (2.- 2006.CONTROLNAME, 2.- 1012.PARENTRELATION.child)[-2006 = tablecell]{*= 1.- 1012} ^< -1026.ContentTableCell > (3.- 2006.CONTROLNAME, 3.- 1012.PARENTRELATION.child)[-2006 = tablecell]{*= 1.- 1012}^< -2033.menuTableCellWidth > (4.- 2037.ATTRIBUTETYPE, 4.- 1012.ATTRIBUTEVALUE.20,4.- 1012.PARENTRELATION.attribute)[-2037=widthpercent]{*=2.-1012}^<-2033.contentTableCellWidth>(5.-2037.ATTRIBUTETYPE,5.-1012.PARENTRELATION.attribute,5.-2037.ATTRIBUTEVALUE.80)[-2037=widthpercent]{*=3.-1012}^<-1026.ContentPanel>(6.-2006.CONTROLNAME,6.-1012.PARENTRELATION.child)[-2006=groupbox]{*=3.-1012}^<-2033.menuRowSpanAtt>(7.-2037.ATTRIBUTETYPE,7.-1012.PARENTRELATION.attribute,7.-1012.ATTRIBUTEVALUE.3)[-2037=rowspan]{*=2.-1012}^<-1026.menuPanel>(8.-2006.CONTROLNAME,8.-1012.PARENTRELATION.child)[-2006=groupbox]{*=2.-1012}^<-1026.TableRowTwo>(9.-2006.CONTROLNAME,9.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.TableRowThree>(10.-2006.CONTROLNAME,10.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.TableCellOneRowTwo>(11.-2006.CONTROLNAME,11.-1012.PARENTRELATION.child)[-2006=tablecell]{*=9.-1012}^<-1026.TableCellOneRowThree>(12.-2006.CONTROLNAME,12.-1012.PARENTRELATION.child)[-2006=tablecell]{*=10.-1012}^<-1026.pnlRowThree>(13.-2006.CONTROLNAME,13.-1012.PARENTRELATION.child)[-2006=groupbox]{*=12.-1012}^<-1026.labelMenuPnl>(14.-2006.CONTROLNAME,14.-1012.PARENTRELATION.child)[-2006=label]{*=8.-1012}^<-2033.mnuPnlLblText>(15.-2037.ATTRIBUTETYPE,15.-1012.PARENTRELATION.attribute,15.-2037.ATTRIBUTEVALUE.MnuPnl)[-2037=text]{*=14.-1012}^<-1026.lblPnlRowThree>(16.-2006.CONTROLNAME,16.-1012.PARENTRELATION.child)[-2006=label]{*=13.-1012}^<-2033.pnlRowThreeText>(17.-2037.ATTRIBUTETYPE,17.-1012.PARENTRELATION.attribute,17.-1012.ATTRIUTEVALUE.RowThreeCellOne)[-2037=text]{*=16.-1012}^<-1026.RowOneCellTwoLbl>(18.-2006.CONTROLNAME,18.-1012.PARENTRELATION.child)[-2006=label]{*=6.-1012}^<-2033.RowOneCellTwoTexst>(19.-2037.ATTRIBUTETYPE,19.-1012.PARENTRELATION.attribute,19.-1012.ATTRIBUTEVALUE.rowOneCellTwo)[-2037=text]{*=18.-1012}^<-1026.TableCellTwoRowTwo>(20.-2006.CONTROLNAME,20.-1012.PARENTRELATION.child)[-2006=tablecell]{*=9.-1012}^<-1026.pnlCellTwoRowTwo>(21.-2006.CONTROLNAME,21.-1012.PARENTRELATION.child)[-2006=groupbox]{*=20.-1012}^<-1026.lblCelllToRowTwo>(22.-2006.CONTROLNAME,22.-1012.PARENTRELATION.child)[-2006=label]{*=21.-1012}^<-2033.txtCellTwoRowTwo>(23.-2037.ATTRIBUTETYPE,23.-1012.ATTRIBUTEVALUE.CellTwoRowTwo,23.-1012.PARENTRELATION.attribute)[-2037=text]{*=22.-1012}^<-1026.CellTwoRowThree>(24.-2006.CONTROLNAME,24.-1012.PARENTRELATION.child)[-2006=tablecell]{*=10.-1012}^<-1026.pnlCellTwoRowThree>(25.-2006.CONTROLNAME,25.-1012.PARENTRELATION.child)[-2006=groupbox]{*=24.-1012}^<-1026.lblCellTwoRowThree>(26.-2006.CONTROLNAME,26.-1012.PARENTRELATION.child)[-2006=label]{*=25.-1012}^<-2033.txtCellTwoRowThree>(27.-2037.ATTRIBUTETYPE,27.-1012.PARENTRELATION.attribute,27.-1012.ATTRIBUTEVALUE.CellTwo Row Three)[-2037=text]{*=26.-1012}^<-2033.mnuPanelName>(28.-2037.ATTRIBUTETYPE,28.-1012.ATTRIBUTEVALUE.MENUPANEL,28.-1012.PARENTRELATION.attribute)[-2037=idname]{*=8.-1012}^<-2033.contentPanelName>(29.-2037.ATTRIBUTETYPE,29.-1012.PARENTRELATION.attribute,29.-1012.ATTRIBUTEVALUE.SECTIONONE)[-2037=idname]{*=6.-1012}^<-2033.pnlCellTwoRowTwoName>(30.-2037.ATTRIBUTETYPE,30.-1012.ATTRIBUTEVALUE.SECTIONTWO,30.-1012.PARENTRELATION.attribute)[-2037=idname]{*=21.-1012}^<-2033.pnlCelltwoRowThreeName>(31.-2037.ATTRIBUTETYPE,31.-1012.ATTRIBUTEVALUE.SECTIONTHREE,31.-1012.PARENTRELATION.attribute)[-2037=idname]{*=25.-1012}";
        //    //BuildSection(FullMenuQry, topPanel);
        //    //BuildSection("^<-1026.mainTable>(0.-2006.CONTROLNAME,0.-1012.PARENTRELATION.child)[-2006=table]{*=-1.-1012}^<-1026.HeaderTableRow>(1.-2006.CONTROLNAME,1.-1012.PARENTRELATION.child)[-2006=tablerow]{*=0.-1012}^<-1026.HeaderTableCellColName>(2.-2006.CONTROLNAME,2.-1012.PARENTRELATION.child)[-2006=tablecell]{*=1.-1012}^<-1026.headerColNameLabel>(3.-2006.CONTROLNAME,3.-1012.PARENTRELATION.child)[-2006=label]{*=2.-1012}^<-2033.headerColNameText>(4.-2037.ATTRIBUTETYPE,4.-1012.PARENTRELATION.attribute,4.-1012.ATTRIBUTEVALUE.Column Name)[-2037=text]{*=3.-1012}^<-1026.headerTableCellColTyp>(5.-2006.CONTROLNAME,5.-1012.PARENTRELATION.child)[-2006=tablecell]{*=1.-1012}^<-1026.headerColTypLabel>(6.-2006.CONTROLNAME,6.-1012.PARENTRELATION.child)[-2006=label]{*=5.-1012}^<-2033.headerColTypeType>(7.-2037.ATTRIBUTETYPE,7.-1012.PARENTRELATION.attribute,7.-1012.ATTRIBUTEVALUE.Column Type)[-2037=text]{*=6.-1012}^<-1026.CellOwnerIO>(8.-2006.CONTROLNAME,8.-1012.PARENTRELATION.child)[-2006=tablecell]{*=1.-1012}^<-1026.LabelOwnerIO>(9.-2006.CONTROLNAME,9.-1012.PARENTRELATION.child)[-2006=label]{*=8.-1012}^<-2033.colOwnerText>(10.-2037.ATTRIBUTETYPE,10.-1012.PARENTRELATION.attribute,10.-2037.ATTRIBUTEVALUE.Owner IO Item)[-2037=text]{*=9.-1012}^<-1011.Columns>()[-1024=-1010]{*=0.-1012}^<-1026.ColumnRow>(12.-2006.CONTROLNAME,12.-1012.PARENTRELATION.child)[-2006=tablerow]{*$11.-1012}^<-1026.contentCellColName>(13.-2006.CONTROLNAME,13.-1012.PARENTRELATION.child)[-2006=tablecell]{*=11.-1012}^<-1026.contentCellColType>(14.-2006.CONTROLNAME,14.-1012.PARENTRELATION.child)[-2006=tablecell]{*=11.-1012}^<-1026.contentCellColOwner>(15.-2006.CONTROLNAME,15.-1012.PARENTRELATION.child)[-2006=tablecell]{*=11.-1012}^<-1026.contentLabelColName>(16.-2006.CONTROLNAME,16.-1012.PARENTRELATION.child)[-2006=label]{*=13.-1012}^<-1026.contentLabelColType>(17.-2006.CONTROLNAME,17.-1012.PARENTRELATION.child)[-2006=label]{*=14.-1012}^<-1026.contentLabelColOwner>(18.-2006.CONTROLNAME,18.-1012.PARENTRELATION.child)[-2006=label]{*=15.-1012}^<-2033.contentTextColName>(19.-2037.ATTRIBUTETYPE,19.-1012.PARENTRELATION.attribute,11.-1023.ATTRIBUTEVALUE)[-2037=text]{*=16.-1012}^<-2033.contentTextColType>(20.-2037.ATTRIBUTETYPE,20.-1012.PARENTRELATION.attribute,11.-1020.ATTRIBUTEVALUE)[-2037=text]{*=17.-1012}^<-2033.contentTextColOwner>(21.-2037.ATTRIBUTETYPE,21.-1012.PARENTRELATION.attribute,11.-1024.ATTRIBUTEVALUE)[-2037=text]{*=18.-1012}^<-1026.InsertTable>(22.-2006.CONTROLNAME,22.-1012.PARENTRELATION.child)[-2006=table]{*=-1.-1012}^<-1026.InsertRow>(23.-2006.CONTROLNAME,23.-1012.PARENTRELATION.child)[-2006=tablerow]{*=22.-1012}^<-1011.ColumnCells>()[-1024=-1011]{*=23.-1012}^<-1026.InsertTableCell>(25.-2006.CONTROLNAME,25.-1012.PARENTRELATION.child)[-2006=tablecell]{*$24.-1012}^<-1026.AddCellGroupBox>(26.-2006.CONTROLNAME,26.-1012.PARENTRELATION.child)[-2006=groupbox]{*=24.-1012}^<-1026.LabelInsertColName>(27.-2006.CONTROLNAME,27.-1012.PARENTRELATION.child)[-2006=label]{*=26.-1012}^<-2033.lblInsertTextColName>(28.-2037.ATTRIBUTETYPE,28.-1012.PARENTRELATION.attribute,24.-1023.ATTRIBUTEVALUE)[-2037=text]{*=27.-1012}^<-1026.txtInsertColValue>(29.-2006.CONTROLNAME,29.-1012.PARENTRELATION.child)[-2006=textbox]{*=26.-1012}^<-2033.txtInsertNameAtt>(30.-2037.ATTRIBUTETYPE,30.-1012.PARENTRELATION.attribute,24.-1012.ATTRIBUTEVALUE)[-2037=idname]{*=29.-1012}^<-1026.InsertBtnCell>(31.-2006.CONTROLNAME,31.-1012.PARENTRELATION.child)[-2006=tablecell]{*=23.-1012}^<-1026.InsertBtnGroupBox>(32.-2006.CONTROLNAME,32.-1012.PARENTRELATION.child)[-2006=groupbox]{*=31.-1012}^<-1026.InsertBtnLabel>(33.-2006.CONTROLNAME,33.-1012.PARENTRELATION.child)[-2006=label]{*=32.-1012}^<-2033.insertLabelText>(34.-2037.ATTRIBUTETYPE,34.-1012.PARENTRELATION.attribute,34.-1012.ATTRIBUTEVALUE.Insert Column)[-2037=text]{*=33.-1012}^<-1026.insertButton>(35.-2006.CONTROLNAME,35.-1012.PARENTRELATION.child)[-2006=button]{*=32.-1012}^<-2033.insertButtonText>(36.-2037.ATTRIBUTETYPE,36.-1012.PARENTRELATION.attribute,36.-1012.ATTRIBUTEVALUE.Add Column)[-2037=text]{*=35.-1012}^<-2033.insertButtonName>(37.-2037.ATTRIBUTETYPE,37.-1012.PARENTRELATION.attribute,37.-1012.ATTRIBUTEVALUE.insertrow)[-2037=mouseclick]{*=35.-1012}", namedControls["SECTIONTWO"]);
        //    //BuildSection("^<-1026.HeaderLabel>(0.-2006.CONTROLNAME,0.-1012.PARENTRELATION.child)[-2006=label]{*=-1.-1012}^<-1026.UIControlTable>(1.-2006.CONTROLNAME,1.-1012.PARENTRELATION.child)[-2006=treeview]{*=-1.-1012}^<-1010.Databases>(2.-1015.METAROW_NAME,2.-1012.PARENTRELATION.child)[-1017=Database]{*=1.-1012}^<-1026.dbTreeViewItem>(3.-2006.CONTROLNAME)[-2006=treeviewnode]{*$2.-1012}^<-1026.dbTreeViewItemLabel>(4.-2006.CONTROLNAME,4.-1012.PARENTRELATION.header)[-2006=label]{*=2.-1012}^<-2033.dbTreeViewItemText>(5.-2037.ATTRIBUTETYPE,5.-1012.PARENTRELATION.attribute,2.-1015.ATTRIBUTEVALUE)[-2037=text]{*=4.-1012}^<-1010.tableRecords>(6.-1015.METAROW_NAME,6.-1012.PARENTRELATION.child)[]{6.-1016=2.-1012}^<-1026.tableTreeViewNode>(7.-2006.CONTROLNAME)[-2006=treeviewnode]{*$6.-1012}^<-1026.tableTreeViewNodeLabel>(8.-2006.CONTROLNAME,8.-1012.PARENTRELATION.header)[-2006=label]{*=6.-1012}^<-2033.attributesTableLabel>(9.-2037.ATTRIBUTETYPE,9.-1012.PARENTRELATION.attribute,6.-1015.ATTRIBUTEVALUE)[-2037=text]{*=8.-1012}^<-2033.headerText>(10.-2037.ATTRIBUTETYPE,10.-1012.PARENTRELATION.attribute,10.-1012.ATTRIBUTEVALUE.Choose Table)[-2037=text]{*=0.-1012}^<-2033.headerFontSize>(11.-2037.ATTRIBUTETYPE.fontsize,11.-2037.ATTRIBUTEVALUE.30,11.-1012.PARENTRELATION.attribute)[-2037=text]{*=0.-1012}", namedControls["MENUPANEL"]);
        //    //BuildSection("^<-1026.mainTable>(0.-2006.CONTROLNAME,0.-1012.PARENTRELATION.child)[-2006=table]{*=-1.-1012}^<-1010.IORecords>()[-1015=attributes]{*=0.-1012}^<-1026.TableRow>(2.-2006.CONTROLNAME,2.-1012.PARENTRELATION.child)[-2006=tablerow]{*$1.-1012}^<-1011.Column>()[]{3.-1024=1.-1012}^<-1026.ColumnTableCell>(4.-2006.CONTROLNAME,4.-1012.PARENTRELATION.child)[-2006=tablecell]{*$3.-1012}^<-1026.cellGroupBox>(5.-2006.CONTROLNAME,5.-1012.PARENTRELATION.child)[-2006=groupbox]{*=3.-1012}^<-1026.colNameLabel>(6.-2006.CONTROLNAME,6.-1012.PARENTRELATION.child)[-2006=label]{*=5.-1012}^<-2033.colNameAtt>(7.-2037.ATTRIBUTETYPE,7.-1012.PARENTRELATION.attribute,3.-1023.ATTRIBUTEVALUE)[-2037=text]{*=6.-1012}^<-1026.colNameTextbox>(8.-2006.CONTROLNAME,8.-1012.PARENTRELATION.child)[-2006=textbox]{*=5.-1012}^<-2033.textBoxAtt>(9.-2037.ATTRIBUTETYPE.idname,9.-1012.PARENTRELATION.attribute,3.-1012.ATTRIBUTEVALUE)[-2037=text]{*=8.-1012}^<-1026.InsertPanel>(10.-2006.CONTROLNAME,10.-1012.PARENTRELATION.child)[-2006=groupbox]{*=-1.-1012}^<-1026.InsertBtn>(11.-2006.CONTROLNAME,11.-1012.PARENTRELATION.child)[-2006=button]{*=10.-1012}^<-2033.insertBtnTxt>(12.-2037.ATTRIBUTETYPE,12.-1012.PARENTRELATION.attribute,12.-1012.ATTRIBUTEVALUE.Insert Row)[-2037=text]{*=11.-1012}^<-2033.insertBtnName>(13.-2037.ATTRIBUTETYPE,13.-1012.ATTRIBUTEVALUE,13.-1012.PARENTRELATION.attribute)[-2037=idname]{*=11.-1012}^<-2033.insertBtnEvent>(14.-2037.ATTRIBUTETYPE,14.-1012.ATTRIBUTEVALUE.insertrow,14.-1012.PARENTRELATION.attribute)[-2037=mouseclick]{*=11.-1012}", namedControls["SECTIONONE"]);


        //    //BuildSection("^<-1026.Label>(0.-2006,0.-1012.PARENTRELATION.child)[-2006=label]{*=-1.-1012}^<-2033.attributesLabelAttribute>(1.-2037.ATTRIBUTETYPE,1.-1012.PARENTRELATION.attribute,1.-1012.ATTRIBUTEVALUE.Table Viewer)[-2037=text]{*=0.-1012}^<-2033.attributesLabelFont>(2.-1012.ATTRIBUTEVALUE.35,2.-2037.ATTRIBUTETYPE.fontsize,2.-1012.PARENTRELATION.attribute)[-2037=text]{*=0.-1012}^<-1026.viewTable>(3.-2006.CONTROLNAME,3.-1012.PARENTRELATION.child)[-2006=table]{*=-1.-1012}^<-1010.IORecords>(*,4.-1012.CONTROLNAME.tablerow<maintainonrow>,4.-1012.PARENTRELATION.child<maintainonrow>)[]{*?3.-1012}^<-1026.valueTableCells>(5.-2006.CONTROLNAME,5.-1012.PARENTRELATION.child)[-2006=tablecell]{*=4.-1012}^<-1026.panelValues>(6.-2006.CONTROLNAME,6.-1012.PARENTRELATION.child)[-2006=groupbox]{*=5.-1012}^<-1026.lblColName>(7.-2006.CONTROLNAME,7.-1012.PARENTRELATION.child)[-2006=label]{*=6.-1012}^<-1026.lblColValue>(8.-2006.CONTROLNAME,8.-1012.PARENTRELATION.child)[-2006=label]{*=6.-1012}^<-2033.lblColNametxt>(9.-2037.ATTRIBUTETYPE,9.-1012.PARENTRELATION.attribute,4.0.ROGUEVALUE)[-2037=text]{*=8.-1012}^<-2033.lblColNametxt2>(10.-2037.ATTRIBUTETYPE,10.-1012.PARENTRELATION.attribute,4.0.ROGUEKEYNAME)[-2037=text]{*=7.-1012}", namedControls["SECTIONTHREE"]);

        //    //mainGrid.RowDefinitions.Add(new RowDefinition());
        //    //mainGrid.RowDefinitions.Add(new RowDefinition());
        //    //mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
        //    //mainGrid.ColumnDefinitions[0].Width = new GridLength(20, GridUnitType.Star);
        //    //mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
        //    //mainGrid.ColumnDefinitions[1].Width = new GridLength(80, GridUnitType.Star);
        //    //StackPanel menuPnl = new StackPanel();
        //    //Grid.SetRow(menuPnl, 0);
        //    //Grid.SetColumn(menuPnl, 0);
        //    //Grid.SetRowSpan(menuPnl, 2);
        //    //mainGrid.Children.Add(menuPnl);

        //    //StackPanel secondPnl = new StackPanel();
        //    //secondPnl.CanVerticallyScroll = true;
        //    //secondPnl.CanHorizontallyScroll = true;
        //    //ScrollViewer scroll = new ScrollViewer();
        //    //scroll.Content = secondPnl;
        //    //scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        //    //scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        //    //Grid.SetRow(scroll, 0);
        //    //Grid.SetColumn(scroll, 1);
        //    //mainGrid.Children.Add(scroll);
        //    ////**SPACE
        //    //StackPanel thirdPnl = new StackPanel();
        //    //Grid.SetRow(thirdPnl, 1);
        //    //Grid.SetColumn(thirdPnl, 1);
        //    //mainGrid.Children.Add(thirdPnl);

        //    //BuildSection(lstContainers, qrys[0], menuPnl);
        //    //lstContainers.Clear();
        //    //BuildSection(lstContainers, qrys[1], secondPnl);
        //    //if(qrys.Count > 2)
        //    //{
        //    //    BuildSection(lstContainers, qrys[2], thirdPnl);
        //    //}
        //}
        public void BuildSection(String qry, FrameworkElement top)
        {
            lstContainers.Clear();
            lstContainers.Add(top);
            HQLQuery qryResults = new HQLQuery(qry);
            int currLevel = 0;
            foreach (var thsRowPair in qryResults.hierarchyGrid)
            {
                int diff = currLevel - thsRowPair.Key;
                HierarchyRow thsRow = thsRowPair.Value;
                String strRelation = thsRow.GetValueByColName("PARENTRELATION");
                ParentRelationships parentRelation;
                Boolean foundRelation = System.Enum.TryParse<ParentRelationships>(strRelation, out parentRelation);
                if (!foundRelation)
                {
                    parentRelation = ParentRelationships.child;
                }
                if (diff > 0)
                {
                    for (int i = 0; i != diff; i++)
                    {
                        lstContainers.RemoveAt(lstContainers.Count - 1);
                    }
                }
                switch (parentRelation)
                {
                    case ParentRelationships.attribute:
                        String attributeType = thsRow.GetValueByColName("attributetype");
                        String attributeValue = thsRow.GetValueByColName("attributevalue");
                        SetAttribute(attributeType, attributeValue, lstContainers[lstContainers.Count - 1]);
                        break;
                    case ParentRelationships.child:
                    case ParentRelationships.header:
                        ParseControlItem(diff, lstContainers, thsRow);
                        break;
                }
                currLevel = thsRowPair.Key;
            }
        }
        private void ParseControlItem(int diff, List<FrameworkElement> lstContainers, HierarchyRow thsRow)
        {
            //int diff = currLevel - thsRowPair.Key;
            //if (diff > 0)
            //{
            //    for (int i = 0; i != diff; i++)
            //    {
            //        lstContainers.RemoveAt(lstContainers.Count - 1);
            //    }
            //}
            FrameworkElement thsControl = TranslateUIControlRow(thsRow, lstContainers);
            if (thsControl != null)
            {
                SetParentContent(lstContainers[lstContainers.Count - 1], thsControl, thsRow);
                if (thsRow.hasChildren)
                {
                    lstContainers.Add(thsControl);
                }
            }
            else
            {
                lstContainers.Add(lstContainers[lstContainers.Count - 1]);
            }
        }
        private void SetParentContent(FrameworkElement thsWinContainer, FrameworkElement newControl, HierarchyRow newControlRow)
        {
            if (newControlRow.GetValueByColName("PARENTRELATION").ToUpper() == "HEADER")
            {
                ((HeaderedItemsControl)thsWinContainer).Header = newControl;
            }
            else if (thsWinContainer is MyGrid && !(newControl is MyGrid))
            {
                MyGrid grid = ((MyGrid)thsWinContainer);
                grid.ShowGridLines = true;
                Grid.SetRow(newControl, grid.currRowNum);
                Grid.SetColumn(newControl, grid.currColNum);
                grid.Children.Add(newControl);
            }
            else if (thsWinContainer is Panel && !(thsWinContainer is MyGrid))
            {
                ((Panel)thsWinContainer).Children.Add(newControl);
            }
            else if (thsWinContainer is ItemsControl)
            {
                ((ItemsControl)thsWinContainer).Items.Add(newControl);
            }
        }
        private void SetAttribute(String attributeType, String attributeValue, FrameworkElement control, Boolean isPostAttribute = false)
        {
            switch (attributeType)
            {
                case "fontsize":
                    ((Control)control).FontSize = int.Parse(attributeValue);
                    break;
                case "mouseclick":
                    Button btn = (Button)control;
                    switch (attributeValue)
                    {
                        case "insertrow":
                            //control.Tag = namedControls;
                            //btn.Click += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.insert, (StoredHQLQuery)thsQry, btn.Tag.ToString()); };
                            break;
                        case "select":
                            //btn.Click += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.select, (UISegmentQuery)thsQry, btn.Tag.ToString()); };
                            break;
                    }
                    break;
                case "mousedoubleclick":
                    Control btnDoubleClick = (Control)control;
                    switch (attributeValue)
                    {
                        case "insertrow":
                            //control.Tag = namedControls;
                            //btnDoubleClick.MouseDoubleClick += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.insert, (UISegmentQuery)thsQry, btnDoubleClick.Tag.ToString()); };
                            break;
                        case "select":
                            //btnDoubleClick.MouseDoubleClick += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.select, (UISegmentQuery)thsQry, btnDoubleClick.Tag.ToString()); };
                            break;
                    }
                    break;
                case "text":
                    if (control is TextBox)
                    {
                        ((TextBox)control).Text = attributeValue;
                    }
                    else if (control is ContentControl)
                    {
                        ((ContentControl)control).Content = attributeValue;
                    }
                    else if (control is TreeViewItem)
                    {
                        ((TreeViewItem)control).Header = attributeValue;
                    }
                    break;
                case "orientation":
                    if (attributeValue == "horizontal")
                    {
                        ((StackPanel)control).Orientation = Orientation.Horizontal;
                    }
                    else
                    {
                        ((StackPanel)control).Orientation = Orientation.Vertical;
                    }
                    break;
                case "idname":
                    control.Tag = attributeValue;
                    namedControls.Add(attributeValue, control);
                    break;
                case "columnspan":
                case "rowspan":
                    if (isPostAttribute)
                    {
                        if (attributeType == "columnspan")
                        {
                            Grid.SetColumnSpan(control, int.Parse(attributeValue));
                        }
                        else
                        {
                            Grid.SetRowSpan(control, int.Parse(attributeValue));
                        }
                    }
                    else
                    {
                        postAttributes.Add(new List<String>() { attributeType, attributeValue });
                    }
                    break;
                case "widthpercent":
                    if (control is Grid)
                    {
                        Grid thsGrid = ((Grid)control);
                        int thsWidth = int.Parse(attributeValue);
                        thsGrid.ColumnDefinitions[thsGrid.ColumnDefinitions.Count - 1].Width = new GridLength(thsWidth, GridUnitType.Star);
                    }
                    else if (control is StackPanel)
                    {
                        StackPanel thsPnl = ((StackPanel)control);
                        int thsWidth = int.Parse(attributeValue);
                        thsPnl.Width = thsWidth;
                    }
                    break;
                case "heightpercent":
                    if (control is Grid)
                    {
                        Grid rowGrid = ((Grid)control);
                        int thsHeight = int.Parse(attributeValue);
                        rowGrid.RowDefinitions[rowGrid.RowDefinitions.Count - 1].Height = new GridLength(thsHeight, GridUnitType.Star);
                    }
                    break;
                case "fontweight":
                    ((ContentControl)control).FontWeight = FontWeights.Bold;
                    break;
            }
        }
        private FrameworkElement TranslateUIControlRow(HierarchyRow ths_control, List<FrameworkElement> lstContainers)
        {
            String controlName;
            if (ths_control.GetValueByColName("CONTROL_NM") != null)
            {
                controlName = ths_control.GetValueByColName("CONTROL_NM");
            }
            else
            {
                controlName = ths_control.GetValueByColName("CONTROLNAME");
            }
            FrameworkElement retElement = null;
            switch (controlName)
            {
                case "textbox":
                    TextBox txt = new TextBox();
                    retElement = txt;
                    break;
                case "label":
                    Label lbl = new Label();
                    retElement = lbl;
                    break;
                case "button":
                    Button btn = new Button();
                    retElement = btn;
                    break;
                case "imagebutton":
                    Image img = new Image();
                    //Uri imgPath = new Uri(ths_control.Value(), UriKind.Absolute);
                    //img.Source = new BitmapImage(imgPath);
                    img.Width = 30;
                    img.Height = 30;
                    retElement = img;
                    break;
                case "treeviewnode":
                    TreeViewItem new_tree = new TreeViewItem();
                    retElement = new_tree;
                    break;
                case "treeview":
                    retElement = new TreeView();
                    break;
                case "groupbox":
                    //ScrollViewer sw = new ScrollViewer();
                    retElement = new StackPanel();
                    //sw.Content = retElement;
                    break;
                case "tablerow<maintainonrow>":
                case "tablerow":
                    ((MyGrid)lstContainers[lstContainers.Count - 1]).AddRow();
                    retElement = lstContainers[lstContainers.Count - 1];
                    break;
                case "tablecell":
                    ((MyGrid)lstContainers[lstContainers.Count - 1]).AddColumn();
                    //*mod to aid fake rows. just use the same grid 
                    retElement = lstContainers[lstContainers.Count - 1];
                    break;
                case "table":
                    retElement = new MyGrid();
                    break;
                case "dropdownlist":
                    retElement = new ComboBox();
                    break;
                case "listitem":
                    retElement = new ComboBoxItem();
                    break;
                default:
                    return null;
            }
            //*This handles wpf out order things like row span. Assignes to next element since cant assign to actual columns or rows
            if (postAttributes.Count > 0)
            {
                foreach (var pair in postAttributes)
                {
                    SetAttribute(pair[0], pair[1], retElement, true);
                }
            }
            postAttributes.Clear();
            return retElement;
        }
        //public void RogueInsertEvent(Object sender, RoutedEventArgs e)
        //{
        //    var element = (FrameworkElement)sender;
        //    String tblID = element.Tag.ToString();
        //    IRogueTable thsTable = new IORecordID(tblID).ToTable();
        //    IRogueRow newRow = thsTable.NewIWriteRow();
        //    foreach (var thsControlPair in namedControls)
        //    {
        //        newRow.NewWritePair(new ColumnRowID(thsControlPair.Key.ToString()), thsControlPair.Value.GetControlValue().ToDecodedRowID().ToString());
        //    }
        //    thsTable.Write();
        //}
        //public void RogueSelectEvent(Object sender, RoutedEventArgs e)
        //{
        //    tempTableID = int.Parse(((FrameworkElement)sender).Tag.ToString());
        //    namedControls.Clear();
        //    selectControls.Clear();
        //    BuildHQLDeveloper(tempTableID);
        //}
    }
    public static class extensions
    {
        public static String GetControlValue(this FrameworkElement thsControl)
        {
            if (thsControl is TextBox)
            {
                return ((TextBox)thsControl).Text;
            }
            else
            {
                return ((ContentControl)thsControl).Content.ToString();
            }
        }
    }
    public class MyGrid : Grid
    {
        int maxRows = -1;
        int maxCols = -1;
        public int currRowNum { get; private set; } = -1;
        public int currColNum { get; private set; } = -1;
        public void AddRow()
        {
            currRowNum++;
            if (currRowNum > maxRows)
            {
                this.RowDefinitions.Add(new RowDefinition());
                maxRows++;
            }
            currColNum = -1;
        }
        public void AddColumn()
        {
            currColNum++;
            if (currColNum > maxCols)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition());
                maxCols++;
            }
        }
    }
    //public enum ParentRelationships
    //{
    //    child, header, attribute
    //}
    public enum ClickActions
    {
        insert, select, none
    }
}
class AllTableInfos : List<TableInfos>
{
    public Dictionary<String, TreeViewItem> nodeMatrix = new Dictionary<string, TreeViewItem>();
    public Dictionary<String, int> matrixRefNames { get; } = new Dictionary<String, int>();
    public void DeleteTable(String tableRefName, ComboBox lstParents)
    {
        lstParents.Items.Clear();
        for (int t = this.Count - 1; t >= 0; t--)
        {
            for (int p = this[t].Count - 1; p >= 0; p--)
            {
                if (this[t][p].TableRefName == tableRefName)
                {
                    //*maybe...
                    this.nodeMatrix.Remove(tableRefName);
                    if (p == 0)
                    {
                        RemoveLevel(t);
                    }
                    else
                    {
                        this[t].RemoveMergedTable(p);

                    }
                }
            }
        }
        lstParents.Items.Add(new ComboBoxItem() { Content = "Root", Tag = -1 });
        foreach (TableInfos infos in this)
        {
            lstParents.Items.Add(new ComboBoxItem() { Content = infos[0].TableRefName, Tag = infos[0].TableID });

        }

    }
    public void RemoveLevel(int level)
    {
        TableInfos thsInfo = this[level];
        TreeViewItem thsNode = thsInfo.thsNode;
        ItemsControl parent = (ItemsControl)thsNode.Parent;
        parent.Items.Remove(thsNode);
        this.RemoveAt(level);
    }
    public List<TableInfo> ParseAllTables()
    {
        int currMatrixNum = 0;
        matrixRefNames.Clear();
        List<TableInfo> allTbls = new List<TableInfo>();

        foreach (TableInfos infos in this)
        {
            //int i = 0;
            foreach (TableInfo tblInfo in infos)
            {
                allTbls.Add(tblInfo);
                tblInfo.matrixNum = currMatrixNum;
                matrixRefNames.Add(tblInfo.TableRefName, currMatrixNum);
                currMatrixNum++;
                //i++
            }
        }
        return allTbls;
    }
    public List<String> AllRefNames()
    {
        List<String> allTbls = new List<String>();
        foreach (TableInfos infos in this)
        {
            allTbls.Add(infos.baseTable.TableRefName);
        }
        return allTbls;
    }
    //public void SaveQuery()
    //{
    //    foreach (TableInfos infos in this)
    //    {
    //        foreach (TableInfo tblInfo in infos)
    //        {
    //            tblInfo.GetSegment(this).WriteData();
    //        }
    //    }
    //}
    //public void ResetParentTableListAfterDelete(List<String>)
    //{
    //    foreach (TableInfos infos in this)
    //    {
    //        foreach (TableInfo tblInfo in infos)
    //        {

    //        }
    //    }
    //}
}
public static class Extensions
{
    public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildren)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (getChildren == null) return source;
        return SelectRecursiveIterator(source, getChildren);
    }

    private static IEnumerable<T> SelectRecursiveIterator<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getChildren)
    {
        var stack = new Stack<IEnumerator<T>>();
        try
        {
            stack.Push(source.GetEnumerator());
            while (stack.Count != 0)
            {
                IEnumerator<T> iter = stack.Peek();
                if (iter.MoveNext())
                {
                    T current = iter.Current;
                    yield return current;

                    IEnumerable<T> children = getChildren(current);
                    if (children != null) stack.Push(children.GetEnumerator());
                }
                else
                {
                    iter.Dispose();
                    stack.Pop();
                }
            }
        }
        finally
        {
            while (stack.Count != 0)
            {
                stack.Pop().Dispose();
            }
        }
    }
}
class TableInfos : List<TableInfo>
{
    public int level;
    public int colRowNum = 0;
    public int whereRowNum = 0;
    public TableInfo baseTable;
    public TreeViewItem thsNode;
    TreeView mainTree;
    public String parentTableRefName;
    //List<TableInfo> allParents = new List<TableInfo>();
    public TableInfos(TableInfo baseTbl, String parentTableRefName, int level, TreeViewItem thsNode, TreeView mainTree)
    {
        this.Add(baseTbl);
        this.level = level;
        this.baseTable = baseTbl;
        this.thsNode = thsNode;
        this.mainTree = mainTree;
        this.parentTableRefName = parentTableRefName;
        //if (thsNode.Parent is TreeViewItem)
        //{
        //    GetParentTableList((TreeViewItem)thsNode.Parent, allParents);
        //}
        //allParents.Add(baseTable);
    }
    //public List<String> GetRealTableNames(String refTableName)
    //    {
    //        List<String> tables = new List<String>();
    //        foreach(TableInfo thsTbl in this)
    //        {
    //            if (!tables.Contains(thsTbl.TableName))
    //            {
    //                tables.Add(thsTbl.TableName);
    //            }
    //        }
    //        return tables;
    //    }
    //public void AddMergeTable(TableInfo mergeTblInfo, String parentTableRefName)
    //    {
    //    ListBox lstContentTables = WPFFinder.FindChild<ListBox>(mainTree, "lstLevelTables" + parentTableRefName);
    //    lstContentTables.Items.Add(new ListBoxItem() { Content = mergeTblInfo.TableRefName });
    //    this.Add(mergeTblInfo);
    //}
    //public void AddColumnRow()
    //    {
    //    StackPanel stackCols = WPFFinder.FindChild<StackPanel>(mainTree, "pnlColumns" + baseTable.TableRefName);
    //    stackCols.Children.Add(GetColumnRow());
    //        colRowNum++;
    //    }
    public void DeleteColumnRow()
    {
        if (colRowNum > 0)
        {
            colRowNum--;
        }
    }
    public void RemoveMergedTable(int index)
    {
        StackPanel pnlCell = (StackPanel)this.thsNode.Header;
        this.RemoveAt(index);
        pnlCell.Children.RemoveAt(index);
    }
    //List<TableInfo> GetParentTableList(TreeViewItem parent, List<TableInfo> currLst)
    //{
    //    foreach (TableInfo thsInfo in ((TableInfos)parent.Tag))
    //    {
    //        currLst.Add(thsInfo);
    //    }
    //    if (parent.Parent != null && parent.Parent is TreeViewItem)
    //    {
    //        GetParentTableList((TreeViewItem)parent.Parent, currLst);
    //    }
    //    return currLst;
    //}
    //public Grid GetColumnRow()
    //    {
    //        Grid newRow = new Grid();
    //        // newRow.Margin = new Thickness((currRowNum*25), 0, 0, 0);
    //        newRow.RowDefinitions.Add(new RowDefinition());
    //        newRow.RowDefinitions.Add(new RowDefinition());
    //        newRow.ColumnDefinitions.Add(new ColumnDefinition());
    //        newRow.ColumnDefinitions.Add(new ColumnDefinition());
    //        newRow.ColumnDefinitions.Add(new ColumnDefinition());
    //        newRow.ColumnDefinitions.Add(new ColumnDefinition());
    //        newRow.ShowGridLines = true;

    //        Label lblFrom = new Label();
    //        lblFrom.Content = "Choose Column Table";
    //        Grid.SetRow(lblFrom, 0);
    //        Grid.SetColumn(lblFrom, 0);
    //        newRow.Children.Add(lblFrom);

    //        Label lblColumn = new Label();
    //        lblColumn.Content = "Column Name";
    //        Grid.SetRow(lblColumn, 0);
    //        Grid.SetColumn(lblColumn, 1);
    //        newRow.Children.Add(lblColumn);

    //        Label lblColumnAlias = new Label();
    //        lblColumnAlias.Content = "Column Alias Name";
    //        Grid.SetRow(lblColumnAlias, 0);
    //        Grid.SetColumn(lblColumnAlias, 2);
    //        newRow.Children.Add(lblColumnAlias);

    //        Label lblStaticValue = new Label();
    //        lblStaticValue.Content = "Column Static Value";
    //        Grid.SetRow(lblStaticValue, 0);
    //        Grid.SetColumn(lblStaticValue, 3);
    //        newRow.Children.Add(lblStaticValue);

    //        ComboBox lstTables = new ComboBox();
    //        lstTables.Name = "ColumnTables" + baseTable.TableRefName;
    //        lstTables.Tag = baseTable.TableRefName;
    //        Grid.SetRow(lstTables, 1);
    //        Grid.SetColumn(lstTables, 0);
    //        newRow.Children.Add(lstTables);

    //        ComboBox lstAvailableColumns = new ComboBox();
    //        Grid.SetRow(lstAvailableColumns, 1);
    //        Grid.SetColumn(lstAvailableColumns, 1);
    //        newRow.Children.Add(lstAvailableColumns);
    //        lstAvailableColumns.Name = "Columns" + baseTable.TableRefName;

    //    TextBox txtColumnAlias = new TextBox();
    //    Grid.SetRow(txtColumnAlias, 1);
    //    Grid.SetColumn(txtColumnAlias, 2);
    //    newRow.Children.Add(txtColumnAlias);

    //    TextBox txtColumnStaticValue = new TextBox();
    //    Grid.SetRow(txtColumnStaticValue, 1);
    //    Grid.SetColumn(txtColumnStaticValue, 3);
    //    newRow.Children.Add(txtColumnStaticValue);
    //    //String parentRefName = ((ComboBoxItem)lstParentTables.SelectedItem).Content.ToString();
    //    //TreeViewItem node = WPFFinder.FindChild<TreeViewItem>(mainTree, "node" + parentTableInfo.TableRefName);
    //    foreach (TableInfo thsTbl in allParents)
    //    {
    //        lstTables.Items.Add(new ComboBoxItem() { Content = thsTbl.TableRefName, Tag = thsTbl.TableID });
    //    }
    //    lstTables.SelectionChanged += new SelectionChangedEventHandler(GetColsPerTable);
    //    foreach (var rowPair in HQLEncoder.GetColumnsPerTable(baseTable.TableID))
    //    {
    //        var row = rowPair;
    //        lstAvailableColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
    //    }
    //    return newRow;
    //    }
    //public Grid GetWhereClauseRow()
    //{
    //    Grid whereRow = GetColumnRow();
    //    whereRow.ColumnDefinitions.Add(new ColumnDefinition());
    //    whereRow.ColumnDefinitions.Add(new ColumnDefinition());
    //    whereRow.ColumnDefinitions.Add(new ColumnDefinition());
    //    Label lblCompareTypes = new Label();
    //    lblCompareTypes.Content = "Compare Types";
    //    Grid.SetRow(lblCompareTypes, 0);
    //    Grid.SetColumn(lblCompareTypes, 4);
    //    whereRow.Children.Add(lblCompareTypes);

    //    ComboBox lstCompareTypes = new ComboBox();
    //    lstCompareTypes.Items.Add(new ComboBoxItem() { Content = "=", Tag = "=" });
    //    lstCompareTypes.Items.Add(new ComboBoxItem() { Content = "!=", Tag = "!=" });
    //    lstCompareTypes.Items.Add(new ComboBoxItem() { Content = ">", Tag = ">" });
    //    lstCompareTypes.Items.Add(new ComboBoxItem() { Content = "<", Tag = "<" });
    //    Grid.SetRow(lstCompareTypes, 1);
    //    Grid.SetColumn(lstCompareTypes, 4);
    //    whereRow.Children.Add(lstCompareTypes);

    //    Label lblValue = new Label();
    //    lblValue.Content = "Value";
    //    Grid.SetRow(lblValue, 0);
    //    Grid.SetColumn(lblValue, 5);
    //    whereRow.Children.Add(lblValue);


    //    TextBox whereValue = new TextBox();
    //    Grid.SetRow(whereValue, 1);
    //    Grid.SetColumn(whereValue, 5);
    //    whereRow.Children.Add(whereValue);


    //    Label lblAndOr = new Label();
    //    lblAndOr.Content = "And/Or";
    //    Grid.SetRow(lblAndOr, 0);
    //    Grid.SetColumn(lblAndOr, 6);
    //    whereRow.Children.Add(lblAndOr);

    //    ComboBox lstAndOr = new ComboBox();
    //    lstAndOr.Items.Add(new ComboBoxItem() { Content = "And", Tag = "And" });
    //    lstAndOr.Items.Add(new ComboBoxItem() { Content = "Or", Tag = "Or" });
    //    Grid.SetRow(lstAndOr, 1);
    //    Grid.SetColumn(lstAndOr, 6);
    //    whereRow.Children.Add(lstAndOr);

    //    return whereRow;
    //}
    //public void AddWhereClauseRow()
    //{
    //    StackPanel stackCols = WPFFinder.FindChild<StackPanel>(mainTree, "pnlWheres" + baseTable.TableRefName);
    //    stackCols.Children.Add(GetWhereClauseRow());
    //    whereRowNum++;
    //}
    //private void GetColsPerTable(object sender, SelectionChangedEventArgs e)
    //{
    //    ComboBox thsBox = (ComboBox)sender;
    //    //int tableID = int.Parse(thsBox.Tag.ToString());
    //    String tableRefName = ((ComboBoxItem)thsBox.SelectedItem).Content.ToString();
    //    int tableID = int.Parse(((ComboBoxItem)thsBox.SelectedItem).Tag.ToString());
    //    ComboBox foundList = WPFFinder.FindChild<ComboBox>(mainTree, "Columns" + baseTable.TableRefName);
    //    //foreach (var rowPair in HQLEncoder.GetColumnsPerTable(tableID))
    //    //{
    //    //    var row = rowPair;
    //    //    foundList.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
    //    //}
    //    //TreeViewItem foundTree = WPFFinder.FindChild<TreeViewItem>(mainTree, "node" + tableRefName);
    //    //foreach (TableInfo thsTbl in (TableInfos)(foundTree.Tag))
    //    //{
    //    //    foundList.Items.Clear();
    //    //    if (thsTbl.TableName == baseTblName)
    //    //    {
    //    foundList.Items.Clear();
    //            foreach (var rowPair in HQLEncoder.GetColumnsPerTable(tableID))
    //            {
    //                var row = rowPair;
    //                foundList.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
    //            }
    //        //}
    //    //}
    //}
    public TreeViewItem GetHeaderNode()
    {
        return ((AllTableInfos)mainTree.Tag).nodeMatrix[parentTableRefName];
        //WPFFinder.FindChild<TreeViewItem>(mainTree, "node" + parentTableRefName);
    }
}
class TableInfo
{
    public int TableID;
    //public String TableName;
    public String TableRefName;
    public int colRowNum = 0;
    public int whereRowNum = 0;
    TreeViewItem parentNode;
    WhereClauses whereClauses;
    JoinClauses joinClauses;
    FullColumnLocations allCols;
    public StackPanel pnlColumns;
    public StackPanel pnlWheres;
    public String parentTableRef;
    public int parentTableID;
    public Char joinType;
    public TreeViewItem thsNode;
    public ComboBox JoinParentColumns;
    public ComboBox JoinParentTables;
    public ComboBox JoinLocalTables;
    public ComboBox JoinLocalColumns;
    //public List<ComboBox> selectColumnsList = new List<ComboBox>();
    List<ComboBox> whereColumnValueOptions = new List<ComboBox>();
    List<ComboBox> whereColumnsList = new List<ComboBox>();
    List<ComboBox> selectColumnTables = new List<ComboBox>();
    List<ComboBox> selectColumns = new List<ComboBox>();
    List<TextBox> selectColumnAliases = new List<TextBox>();
    List<TextBox> selectConstValues = new List<TextBox>();
    // List<ComboBox> whereValueOptions;
    // List<ComboBox> whereColumns;
    TreeView mainTree;
    List<TableInfo> allParents = new List<TableInfo>();
    public int matrixNum;
    public TableInfo(int TableID, String TableRefName, TreeView mainTree, String parentTableRefName, Char joinType, int parentTableID)
    {
        this.TableID = TableID;
        //this.TableName = TableName;
        this.TableRefName = TableRefName;
        this.mainTree = mainTree;
        this.joinType = joinType;
        this.parentTableRef = parentTableRefName;
        this.parentTableID = parentTableID;
        if (parentTableRefName != "Root")
        {
            parentNode = ((AllTableInfos)mainTree.Tag).nodeMatrix[parentTableRefName];
            //WPFFinder.FindChild<TreeViewItem>(mainTree, "node" + parentTableRefName);
            GetParentTableList((TreeViewItem)parentNode, allParents);
        }
        allParents.Add(this);
    }
    List<TableInfo> GetParentTableList(TreeViewItem parent, List<TableInfo> currLst)
    {
        foreach (TableInfo thsInfo in ((TableInfos)parent.Tag))
        {
            currLst.Add(thsInfo);
        }
        if (parent.Parent != null && parent.Parent is TreeViewItem)
        {
            GetParentTableList((TreeViewItem)parent.Parent, currLst);
        }
        return currLst;
    }
    public Grid GetJoinRow()
    {
        Grid newRow = new Grid();
        newRow.RowDefinitions.Add(new RowDefinition());
        newRow.RowDefinitions.Add(new RowDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ShowGridLines = true;

        Label lblFrom = new Label();
        lblFrom.Content = "Choose Column Table";
        Grid.SetRow(lblFrom, 0);
        Grid.SetColumn(lblFrom, 0);
        newRow.Children.Add(lblFrom);

        Label lblColumn = new Label();
        lblColumn.Content = "Column Name";
        Grid.SetRow(lblColumn, 0);
        Grid.SetColumn(lblColumn, 1);
        newRow.Children.Add(lblColumn);
        ComboBox lstTables = new ComboBox();
        lstTables.Name = "JoinColumnTables" + TableRefName;
        JoinLocalTables = lstTables;
        lstTables.SelectedValuePath = "Content";
        lstTables.Tag = TableRefName;
        Grid.SetRow(lstTables, 1);
        Grid.SetColumn(lstTables, 0);
        newRow.Children.Add(lstTables);

        ComboBox lstAvailableColumns = new ComboBox();
        Grid.SetRow(lstAvailableColumns, 1);
        Grid.SetColumn(lstAvailableColumns, 1);
        newRow.Children.Add(lstAvailableColumns);
        lstAvailableColumns.Name = "JoinColumns" + TableRefName;
        JoinLocalColumns = lstAvailableColumns;

        Label lblCompare = new Label();
        lblCompare.Content = joinType;
        Grid.SetRow(lblCompare, 1);
        Grid.SetColumn(lblCompare, 2);
        newRow.Children.Add(lblCompare);

        Label lblParentTable = new Label();
        lblParentTable.Content = "Choose Parent Table";
        Grid.SetRow(lblParentTable, 1);
        Grid.SetColumn(lblParentTable, 3);
        newRow.Children.Add(lblParentTable);

        ComboBox lstParentTables = new ComboBox();
        lstParentTables.Name = "ParentColumnTables" + TableRefName;
        JoinParentTables = lstParentTables;
        lstParentTables.Tag = TableRefName;
        lstParentTables.SelectedValuePath = "Content";

        Grid.SetRow(lstParentTables, 1);
        Grid.SetColumn(lstParentTables, 3);
        newRow.Children.Add(lstParentTables);

        Label lblparents = new Label();
        lblparents.Content = "Parent Columns";
        Grid.SetRow(lblparents, 0);
        Grid.SetColumn(lblparents, 4);
        newRow.Children.Add(lblparents);

        ComboBox lstParentAvailableColumns = new ComboBox();
        Grid.SetRow(lstParentAvailableColumns, 1);
        Grid.SetColumn(lstParentAvailableColumns, 4);
        newRow.Children.Add(lstParentAvailableColumns);
        lstParentAvailableColumns.Name = "JoinParentColumns" + TableRefName;
        JoinParentColumns = lstParentAvailableColumns;
        //JoinParentColumns = lstParentAvailableColumns;
        // JoinParentColumns.SelectedValuePath = "Content";
        lstTables.Items.Add(new ComboBoxItem() { Content = "All", Tag = -1 });
        lstParentTables.Items.Add(new ComboBoxItem() { Content = "Root", Tag = -1 });
        foreach (TableInfo thsTbl in allParents)
        {
            lstTables.Items.Add(new ComboBoxItem() { Content = thsTbl.TableRefName, Tag = thsTbl.TableID });
            lstParentTables.Items.Add(new ComboBoxItem() { Content = thsTbl.TableRefName, Tag = thsTbl.TableID });
        }
        lstTables.SelectedValue = "All";
        lstTables.SelectionChanged += new SelectionChangedEventHandler(GetJoinColsPerTable);
        lstParentTables.SelectedValue = parentTableRef;
        lstParentTables.SelectionChanged += new SelectionChangedEventHandler(GetJoinParentColsPerTable);

        foreach (var rowPair in HQLEncoder.GetColumnsPerTable(parentTableID))
        {
            var row = rowPair;
            String test = row.GetValueByColName("ROGUECOLUMNID");
            if (rowPair.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()) == "RogueColumnID")
            {
                lstParentAvailableColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID"), IsSelected = true });
            }
            else
            {
                lstParentAvailableColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
            }
        }
        return newRow;
    }
    public Grid GetColumnRow(String selectedTblRefName = "", String selectedColName = "", String selectedColAlias = "", String selectedStaticValue = "")
    {
        Grid newRow = new Grid();
        // newRow.Margin = new Thickness((currRowNum*25), 0, 0, 0);
        newRow.RowDefinitions.Add(new RowDefinition());
        newRow.RowDefinitions.Add(new RowDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ColumnDefinitions.Add(new ColumnDefinition());
        newRow.ShowGridLines = true;

        Label lblFrom = new Label();
        lblFrom.Content = "Choose Column Table";
        Grid.SetRow(lblFrom, 0);
        Grid.SetColumn(lblFrom, 0);
        newRow.Children.Add(lblFrom);

        Label lblColumn = new Label();
        lblColumn.Content = "Column Name";
        Grid.SetRow(lblColumn, 0);
        Grid.SetColumn(lblColumn, 1);
        newRow.Children.Add(lblColumn);

        Label lblColumnAlias = new Label();
        lblColumnAlias.Content = "Column Alias Name";
        Grid.SetRow(lblColumnAlias, 0);
        Grid.SetColumn(lblColumnAlias, 2);
        newRow.Children.Add(lblColumnAlias);

        Label lblStaticValue = new Label();
        lblStaticValue.Content = "Column Static Value";
        Grid.SetRow(lblStaticValue, 0);
        Grid.SetColumn(lblStaticValue, 3);
        newRow.Children.Add(lblStaticValue);

        ComboBox lstTables = new ComboBox();
        lstTables.Name = "ColumnTables" + TableRefName + colRowNum; ;
        selectColumnTables.Add(lstTables);
        lstTables.Tag = TableRefName;
        Grid.SetRow(lstTables, 1);
        Grid.SetColumn(lstTables, 0);
        newRow.Children.Add(lstTables);

        ComboBox lstAvailableColumns = new ComboBox();
        Grid.SetRow(lstAvailableColumns, 1);
        Grid.SetColumn(lstAvailableColumns, 1);
        newRow.Children.Add(lstAvailableColumns);
        lstAvailableColumns.Name = "Columns" + TableRefName + colRowNum;
        selectColumns.Add(lstAvailableColumns);
        TextBox txtColumnAlias = new TextBox();
        txtColumnAlias.Name = "TxtColumnAlias" + TableRefName + colRowNum;
        selectColumnAliases.Add(txtColumnAlias);
        Grid.SetRow(txtColumnAlias, 1);
        Grid.SetColumn(txtColumnAlias, 2);
        newRow.Children.Add(txtColumnAlias);

        TextBox txtColumnStaticValue = new TextBox();
        txtColumnStaticValue.Name = "TxtColumnStaticValue" + TableRefName + colRowNum;
        selectConstValues.Add(txtColumnStaticValue);
        Grid.SetRow(txtColumnStaticValue, 1);
        Grid.SetColumn(txtColumnStaticValue, 3);
        newRow.Children.Add(txtColumnStaticValue);
        foreach (TableInfo thsTbl in allParents)
        {
            lstTables.Items.Add(new ComboBoxItem() { Content = thsTbl.TableRefName, Tag = thsTbl.TableID });
        }
        lstTables.SelectedValuePath = "Content";
        lstTables.SelectedValue = TableRefName;
        lstTables.SelectionChanged += new SelectionChangedEventHandler(GetColsPerTable);

        foreach (var rowPair in HQLEncoder.GetColumnsPerTable(TableID))
        {
            var row = rowPair;
            lstAvailableColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
        }
        if (selectedTblRefName != "")
        {
            lstTables.SelectedValue = selectedTblRefName;
        }
        if (selectedColName != "")
        {
            SetColsByTable(lstTables, lstAvailableColumns);
            lstAvailableColumns.SelectedValuePath = "Tag";
            lstAvailableColumns.SelectedValue = selectedColName;
        }
        if (selectedColAlias != "")
        {
            txtColumnAlias.Text = selectedColAlias;
        }
        if (selectedStaticValue != "")
        {
            txtColumnStaticValue.Text = selectedStaticValue;
        }
        return newRow;
    }
    public void DeleteColumnRow()
    {
        pnlColumns.Children.RemoveAt(pnlColumns.Children.Count - 1);
        colRowNum--;
    }
    public void DeleteWhereClause()
    {
        pnlWheres.Children.RemoveAt(pnlWheres.Children.Count - 1);
        whereRowNum--;
    }
    private void GetWhereClauseColValues(object sender, SelectionChangedEventArgs e)
    {
        ComboBox whereColumns = (ComboBox)(sender);
        ComboBox whereValueOptions = WPFFinder.FindChild<ComboBox>(mainTree, "WhereValueOptions" + TableRefName + (whereRowNum - 1));
        SetWhereClauseValues(whereColumns, whereValueOptions);
    }
    private void SetWhereClauseValues(ComboBox whereColumns, ComboBox whereValueOptions)
    {
        whereValueOptions.Items.Clear();
        ColumnRowID colID = new ColumnRowID(((ComboBoxItem)whereColumns.SelectedItem).Tag.ToString());
        foreach (var rowPair in HQLEncoder.GetDisctintValues(TableID, colID))
        {
            whereValueOptions.Items.Add(new ComboBoxItem() { Content = rowPair.GetValueByColID(colID) });
        }
    }
    private void GetColsPerTable(object sender, SelectionChangedEventArgs e)
    {
        ComboBox thsBox = (ComboBox)sender;
        //String tableRefName = ((ComboBoxItem)thsBox.SelectedItem).Content.ToString();

        ComboBox foundList = WPFFinder.FindChild<ComboBox>(mainTree, "Columns" + TableRefName + (colRowNum - 1));
        if (foundList != null)
        {
            SetColsByTable(thsBox, foundList);
        }

    }
    private void SetColsByTable(ComboBox lstTables, ComboBox lstColumns)
    {
        int tableID = int.Parse(((ComboBoxItem)lstTables.SelectedItem).Tag.ToString());
        lstColumns.Items.Clear();
        foreach (var rowPair in HQLEncoder.GetColumnsPerTable(tableID))
        {
            var row = rowPair;
            lstColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
        }
    }
    private void GetJoinColsPerTable(object sender, SelectionChangedEventArgs e)
    {
        ComboBox thsBox = (ComboBox)sender;
        String tableRefName = ((ComboBoxItem)thsBox.SelectedItem).Content.ToString();
        int tableID = int.Parse(((ComboBoxItem)thsBox.SelectedItem).Tag.ToString());
        //ComboBox foundList = WPFFinder.FindChild<ComboBox>(mainTree, "JoinColumns" + TableRefName);
        JoinLocalColumns.Items.Clear();
        foreach (var rowPair in HQLEncoder.GetColumnsPerTable(tableID))
        {
            var row = rowPair;
            JoinLocalColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
        }
    }
    private void GetJoinParentColsPerTable(object sender, SelectionChangedEventArgs e)
    {
        ComboBox thsBox = (ComboBox)sender;
        String tableRefName = ((ComboBoxItem)thsBox.SelectedItem).Content.ToString();
        int tableID = int.Parse(((ComboBoxItem)thsBox.SelectedItem).Tag.ToString());
        //ComboBox foundList = WPFFinder.FindChild<ComboBox>(mainTree, "JoinParentColumns" + TableRefName);
        JoinParentColumns.Items.Clear();
        foreach (var rowPair in HQLEncoder.GetColumnsPerTable(tableID))
        {
            var row = rowPair;
            JoinParentColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
        }
    }
    public void AddColumnRow(String selectedTblRefName = "", String colName = "", String colAlias = "", String staticValue = "")
    {
        //StackPanel stackCols = WPFFinder.FindChild<StackPanel>(mainTree, "pnlColumns" + TableRefName);
        pnlColumns.Children.Add(GetColumnRow(selectedTblRefName, colName, colAlias, staticValue));
        colRowNum++;
    }
    public Grid GetWhereClauseRow(String localColID = "", String whereVal = "")
    {
        Grid whereRow = new Grid();
        whereRow.RowDefinitions.Add(new RowDefinition());
        whereRow.RowDefinitions.Add(new RowDefinition());
        whereRow.ColumnDefinitions.Add(new ColumnDefinition());
        whereRow.ColumnDefinitions.Add(new ColumnDefinition());
        whereRow.ColumnDefinitions.Add(new ColumnDefinition());
        whereRow.ColumnDefinitions.Add(new ColumnDefinition());

        Label lblColumn = new Label();
        lblColumn.Content = "Column Name";
        Grid.SetRow(lblColumn, 0);
        Grid.SetColumn(lblColumn, 0);
        whereRow.Children.Add(lblColumn);

        ComboBox lstAvailableColumns = new ComboBox();
        whereColumnsList.Add(lstAvailableColumns);
        Grid.SetRow(lstAvailableColumns, 1);
        Grid.SetColumn(lstAvailableColumns, 0);
        whereRow.Children.Add(lstAvailableColumns);
        lstAvailableColumns.Name = "WhereColumns" + TableRefName + whereRowNum;
        lstAvailableColumns.SelectedValuePath = "Tag";
        foreach (var rowPair in HQLEncoder.GetColumnsPerTable(TableID))
        {
            var row = rowPair;
            lstAvailableColumns.Items.Add(new ComboBoxItem() { Content = row.GetValueByColName(ColumnCols.ColumnNameID.ColumnNameID().DisplayValue()), Tag = row.GetValueByColName("ROGUECOLUMNID") });
        }
        //*Set Values if present
        if (localColID != "")
        {
            lstAvailableColumns.SelectedValue = localColID;
        }
        lstAvailableColumns.SelectionChanged += new SelectionChangedEventHandler(GetWhereClauseColValues);

        Label lblCompareTypes = new Label();
        lblCompareTypes.Content = "Compare Types";
        Grid.SetRow(lblCompareTypes, 0);
        Grid.SetColumn(lblCompareTypes, 1);
        whereRow.Children.Add(lblCompareTypes);

        ComboBox lstCompareTypes = new ComboBox();
        lstCompareTypes.Items.Add(new ComboBoxItem() { Content = "=", Tag = "=", IsSelected = true });
        lstCompareTypes.Items.Add(new ComboBoxItem() { Content = "!=", Tag = "!=" });
        lstCompareTypes.Items.Add(new ComboBoxItem() { Content = ">", Tag = ">" });
        lstCompareTypes.Items.Add(new ComboBoxItem() { Content = "<", Tag = "<" });
        Grid.SetRow(lstCompareTypes, 1);
        Grid.SetColumn(lstCompareTypes, 1);
        whereRow.Children.Add(lstCompareTypes);

        Label lblValue = new Label();
        lblValue.Content = "Value";
        Grid.SetRow(lblValue, 0);
        Grid.SetColumn(lblValue, 2);
        whereRow.Children.Add(lblValue);

        ComboBox whereValue = new ComboBox();
        whereValue.Name = "WhereValueOptions" + TableRefName + whereRowNum;
        whereColumnValueOptions.Add(whereValue);
        Grid.SetRow(whereValue, 1);
        Grid.SetColumn(whereValue, 2);
        whereRow.Children.Add(whereValue);


        Label lblAndOr = new Label();
        lblAndOr.Content = "And/Or";
        Grid.SetRow(lblAndOr, 0);
        Grid.SetColumn(lblAndOr, 3);
        whereRow.Children.Add(lblAndOr);

        ComboBox lstAndOr = new ComboBox();
        lstAndOr.Items.Add(new ComboBoxItem() { Content = "And", Tag = "And", IsSelected = true });
        lstAndOr.Items.Add(new ComboBoxItem() { Content = "Or", Tag = "Or" });
        Grid.SetRow(lstAndOr, 1);
        Grid.SetColumn(lstAndOr, 3);
        whereRow.Children.Add(lstAndOr);


        if (whereVal != "")
        {
            SetWhereClauseValues(lstAvailableColumns, whereValue);
            whereValue.SelectedValuePath = "Content";
            whereValue.SelectedValue = whereVal;
        }
        return whereRow;
    }
    public void AddWhereClauseRow(String localColID = "", String whereValue = "")
    {
        //StackPanel stackCols = WPFFinder.FindChild<StackPanel>(mainTree, "pnlWheres" + TableRefName);
        pnlWheres.Children.Add(GetWhereClauseRow(localColID, whereValue));
        whereRowNum++;
    }
    //public String CalcSegment(AllTableInfos allInfo)
    //{
    //    //*Join Clause
    //    //JoinClauses joinClauses = new JoinClauses();
    //    //Boolean allCols = false;
    //    //ComboBox joinColTables = WPFFinder.FindChild<ComboBox>(thsNode, "JoinColumnTables" + TableRefName);
    //    //ComboBox joinCols = WPFFinder.FindChild<ComboBox>(thsNode, "JoinColumns" + TableRefName);
    //    //FullColumnLocation thsCol;
    //    //if (((ComboBoxItem)joinColTables.SelectedItem).Content.ToString() == "All"){
    //    //    allCols = true;
    //    //    thsCol = new FullColumnLocation(allInfo.matrixRefNames[TableRefName], SystemIDs.Columns.rogueColumnID);
    //    //}
    //    //else
    //    //{
    //    //    String columnTableRefName = ((ComboBoxItem)joinColTables.SelectedItem).Content.ToString();
    //    //    ColumnRowID columnID = new ColumnRowID(int.Parse(((ComboBoxItem)joinCols.SelectedItem).Tag.ToString()));
    //    //    thsCol = new FullColumnLocation(allInfo.matrixRefNames[columnTableRefName], columnID);
    //    //}
    //    //String tableRefName = ((ComboBoxItem)joinColTables.SelectedItem).Tag.ToString();
    //    //ColumnRowID parentColID;
    //    //ComboBox parentTables = WPFFinder.FindChild<ComboBox>(mainTree, "ParentColumnTables" + TableRefName);
    //    //String parentTblName = ((ComboBoxItem)parentTables.SelectedItem).Content.ToString();
    //    //FullColumnLocation parentCol;
    //    //if (parentTblName == "Root") {
    //    //    parentColID = SystemIDs.Columns.rogueColumnID;
    //    //    parentCol = new FullColumnLocation(-1, parentColID);
    //    //}
    //    //else
    //    //{
    //    //    ComboBox parentCols = WPFFinder.FindChild<ComboBox>(mainTree, "JoinParentColumns" + TableRefName);
    //    //    parentColID = int.Parse(((ComboBoxItem)parentCols.SelectedItem).Tag.ToString());
    //    //    parentCol = new FullColumnLocation(allInfo.matrixRefNames[parentTblName], parentColID);
    //    //}
    //    //WhereClause.EvaluationTypes evaluationType = (WhereClause.EvaluationTypes)joinType;
    //    //JoinClause joinClause = new JoinClause(allCols,thsCol, parentCol , evaluationType);
    //    //joinClauses.Add(joinClause);
    //    ////*Where Clause
    //    //whereClauses = new WhereClauses();
    //    //for(int i = 0; i < whereRowNum; i++)
    //    //{
    //    //    ComboBox whereColumns =  WPFFinder.FindChild<ComboBox>(mainTree, "WhereColumns" + TableRefName + i.ToString());
    //    //    ColumnRowID whereColID = new ColumnRowID(int.Parse(((ComboBoxItem)whereColumns.SelectedItem).Tag.ToString()));
    //    //    ComboBox whereValueOptions = WPFFinder.FindChild<ComboBox>(mainTree, "WhereValueOptions" + TableRefName + i.ToString());
    //    //    WhereClause whereClause = new WhereClause(new ColumnRowID(((ComboBoxItem)whereColumns.SelectedItem).Tag.ToString()), WhereClause.EvaluationTypes.equal, ((ComboBoxItem)whereValueOptions.SelectedItem).Content.ToString());
    //    //    whereClauses.Add(whereClause);
    //    //}
    //    ////*ComboBox columns 
    //    //FullColumnLocations columns = new FullColumnLocations();
    //    //for (int i = 0; i < colRowNum; i++)
    //    //{
    //    //    ComboBox selectColTables = WPFFinder.FindChild<ComboBox>(mainTree, "ColumnTables" + TableRefName + i.ToString());
    //    //    String ColumnOwnerTable = ((ComboBoxItem)selectColTables.SelectedItem).Content.ToString();
    //    //    ComboBox selectCols = WPFFinder.FindChild<ComboBox>(mainTree, "Columns" + TableRefName + i.ToString());
    //    //    ColumnRowID colID = new ColumnRowID(int.Parse(((ComboBoxItem)selectCols.SelectedItem).Tag.ToString()));
    //    //    TextBox colAlias = WPFFinder.FindChild<TextBox>(mainTree, "TxtColumnAlias" + TableRefName + i.ToString());
    //    //    TextBox colStaticValue = WPFFinder.FindChild<TextBox>(mainTree, "TxtColumnStaticValue" + TableRefName  + i.ToString());
    //    //    columns.AddColumn(new FullColumnLocation(allInfo.matrixRefNames[ColumnOwnerTable], colID, colAlias.Text, colStaticValue.Text));
    //    //}
    //    //TableSegment tbl = new TableSegment(matrixNum, new IORecordID(TableID), whereClauses, columns, joinClauses);
    //    return tbl.GetHQLText();
    //}
    public TableSegment GetSegment(AllTableInfos allInfo)
    {
        //*Table Info 
        // rogue_core.rogueCore.hql.segments.tableInfo.TableInfo tblinfo = new rogue_core.rogueCore.hql.segments.tableInfo.TableInfo(allinfo., new IORecordID(TableID));
        //*Join Clause
        JoinClauses joinClauses = new JoinClauses();
        //Boolean allCols = false;
        //ComboBox joinColTables = WPFFinder.FindChild<ComboBox>(thsNode, "JoinColumnTables" + TableRefName);
        //ComboBox joinCols = WPFFinder.FindChild<ComboBox>(thsNode, "JoinColumns" + TableRefName);
        LocationColumn thsCol;
        if (((ComboBoxItem)JoinLocalTables.SelectedItem).Content.ToString() == "All")
        {
            //allCols = true;
            thsCol = new LocationColumn("*");
        }
        else
        {
            String columnTableRefName = ((ComboBoxItem)JoinLocalTables.SelectedItem).Content.ToString();
            ColumnRowID columnID = new ColumnRowID(int.Parse(((ComboBoxItem)JoinLocalColumns.SelectedItem).Tag.ToString()));
            thsCol = new SelectColumn(columnTableRefName, columnID);
        }
        ColumnRowID parentColID;
        //ComboBox parentTables = WPFFinder.FindChild<ComboBox>(mainTree, "ParentColumnTables" + TableRefName);
        String parentTblName = ((ComboBoxItem)JoinParentTables.SelectedItem).Content.ToString();
        SelectColumn parentCol;
        if (parentTblName == "Root")
        {
            parentColID = SystemIDs.Columns.rogueColumnID;
            parentCol = new SelectColumn("Root", parentColID);
        }
        else
        {
            //ComboBox parentCols = WPFFinder.FindChild<ComboBox>(mainTree, "JoinParentColumns" + TableRefName);
            parentColID = int.Parse(((ComboBoxItem)JoinParentColumns.SelectedItem).Tag.ToString());
            parentCol = new SelectColumn(parentTblName, parentColID);
        }
        WhereClause.EvaluationTypes evaluationType = (WhereClause.EvaluationTypes)joinType;
        JoinClause joinClause = new JoinClause(thsCol, parentCol, evaluationType);
        joinClauses.Add(joinClause);
        //*Where Clause
        whereClauses = new WhereClauses();
        for (int i = 0; i < whereRowNum; i++)
        {
            //ComboBox whereColumns = WPFFinder.FindChild<ComboBox>(mainTree, "WhereColumns" + TableRefName + i.ToString());
            ColumnRowID whereColID = new ColumnRowID(int.Parse(((ComboBoxItem)whereColumnsList[i].SelectedItem).Tag.ToString()));
            // ComboBox whereValueOptions = WPFFinder.FindChild<Com>
            // ComboBox whereValueOptions = WPFFinder.FindChild<ComboBox>(mainTree, "WhereValueOptions" + TableRefName + i.ToString());
            WhereClause whereClause = new WhereClause(new ColumnRowID(((ComboBoxItem)whereColumnsList[i].SelectedItem).Tag.ToString()), WhereClause.EvaluationTypes.equal, ((ComboBoxItem)whereColumnValueOptions[i].SelectedItem).Content.ToString());
            whereClauses.Add(whereClause);
        }
        //*ComboBox columns 
        FullColumnLocations columns = new FullColumnLocations();
        for (int i = 0; i < colRowNum; i++)
        {
            //ComboBox selectColTables = WPFFinder.FindChild<ComboBox>(mainTree, "ColumnTables" + TableRefName + i.ToString());
            String ColumnOwnerTable = ((ComboBoxItem)selectColumnTables[i].SelectedItem).Content.ToString();
            //ComboBox selectCols = WPFFinder.FindChild<ComboBox>(mainTree, "Columns" + TableRefName + i.ToString());
            ColumnRowID colID = new ColumnRowID(int.Parse(((ComboBoxItem)selectColumns[i].SelectedItem).Tag.ToString()));
            //TextBox colAlias = WPFFinder.FindChild<TextBox>(mainTree, "TxtColumnAlias" + TableRefName + i.ToString());
            //TextBox colStaticValue = WPFFinder.FindChild<TextBox>(mainTree, "TxtColumnStaticValue" + TableRefName + i.ToString());
            columns.AddColumn(new SelectColumn(ColumnOwnerTable, colID, selectColumnAliases[i].Text, selectConstValues[i].Text));
        }
        rogue_core.rogueCore.hql.segments.tableInfo.TableInfo tblInfo = new rogue_core.rogueCore.hql.segments.tableInfo.TableInfo(new IORecordID(TableID), TableRefName);
        TableSegment tbl = new TableSegment(tblInfo, whereClauses, columns, joinClauses);
        return tbl;
    }
}
class ColumnTableBoxItem
{
    ComboBox cols;
    int tableID;
    public ColumnTableBoxItem(ComboBox cols, int tableID)
    {
        this.cols = cols;
        this.tableID = tableID;
    }
}

