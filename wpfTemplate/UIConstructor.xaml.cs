using rogue_core.rogueCore.StoredProcedures.StoredQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpfTemplate
{
    /// <summary>
    /// Interaction logic for UIConstructor.xaml
    /// </summary>
    public partial class UIConstructor : Window
    {
        Dictionary<String, TreeViewItem> nodeLst = new Dictionary<string, TreeViewItem>();
        Dictionary<String, Dictionary<String, FrameworkElement>> keyControls = new Dictionary<String, Dictionary<String, FrameworkElement>>();
        int lastQryNum = 0;
        int currNodeNum = 0;
        public UIConstructor()
        {
            InitializeComponent();
            //TreeView mainTree = new TreeView();
            //mainStack.Children.Add(mainTree);
            //AddAttributeNode(mainSta);
            //AddControlNode(mainTree, "Root");
            //WindowsUIBuilderNew win = new WindowsUIBuilderNew(HQLQuery.ConvertToHQL(UIQuerySnippets.ddlControlNames(1)), mainStack);
            //new WindowsUIBuilderNew(HQLQuery.ConvertToHQL(UIQuerySnippets.ddlControlAttributes(2)), mainStack);
            //FullQuery finalQry  = new FullQuery();
            //QueryItem node = UIQuerySnippets.NodeItem("Root", "nodeBox@ID");
            //finalQry.NewQuery(node);
            //String qry = finalQry.fullQry;
            //QueryItem grpBoxOne = UIQuerySnippets.groupBox("NodePanel@ID");
            //finalQry.NewQuery(grpBoxOne);
            //qry += UIQuerySnippets.attributeName(2, "NodePanel@ID", "nodeGrpBox");
            //qry += UIQuerySnippets.textLabel(3,"grpBox1", "Attributes");
            //qry += UIQuerySnippets.ddlControlAttributes(4, "grpBox1");
            //qry += UIQuerySnippets.groupBox("NodePanel@ID");
            ////qry += UIQuerySnippets.ddlControlNames(6, "grpBox5");
            //qry += UIQuerySnippets.textLabel(6, "grpBox5", "AttributeValue");
            //qry += UIQuerySnippets.NamedTextbox(7, "grpBox5", "txtAttValue");

            //qry += UIQuerySnippets.Node(4, "dbTreeViewItem0");
            //qry += UIQuerySnippets.ddlControlAttributes(5, "NodePanel3");
            //qry += UIQuerySnippets.ddlControlNames(6, "NodePanel3");
            //WindowsUIBuilderNew win = new WindowsUIBuilderNew(HQLQuery.ConvertToHQL(qry), mainStack);
            //((StackPanel)win.namedControls["nodeBox0"]).Children.Add(BtnAddChildAttribute());
            //((StackPanel)win.namedControls["nodeBox0"]).Children.Add(BtnAddChildControl());
            //((StackPanel)win.namedControls["nodeBox0"]).Children.Add(BtnDeleteItem());
            //return qry;
        }
        public void AddAttributeNode(FrameworkElement parentStack, String parentName)
        {

            FullQuery finalQry = new FullQuery(lastQryNum);
            String nodeNM = finalQry.NewQuery(UIQuerySnippets.NodeQry, "Root", parentName).parentNm;
            QueryItem node = finalQry.NewQuery(UIQuerySnippets.attributeName, nodeNM, "node_" + currNodeNum, parentName);
            String nodeID = node.guessIDName();
            //String nodeConstructorID = node.parentNm;
            String nodePanel = finalQry.NewQuery(UIQuerySnippets.groupBoxHorizontal, nodeNM, "").parentNm;
            String stack = finalQry.NewQuery(UIQuerySnippets.groupBoxQry, nodePanel, "").parentNm;
            finalQry.NewQuery(UIQuerySnippets.textLabel, stack, "Attributes");
            String ddlAtt = finalQry.NewQuery(UIQuerySnippets.ddlControlAttributes, stack, "").parentNm;
            String ddlAttID = finalQry.NewQuery(UIQuerySnippets.attributeName, ddlAtt, "AttributeType").guessIDName();
            String stack2 = finalQry.NewQuery(UIQuerySnippets.groupBoxQry, nodePanel, "").parentNm;
            finalQry.NewQuery(UIQuerySnippets.textLabel, stack2, "AttributeValue");
            String txtboxID = finalQry.NewQuery(UIQuerySnippets.NamedTextbox, stack2, "AttributeValue").guessIDName();
            String qry = finalQry.fullQry;
            WPFSegmentBuilder win = new WPFSegmentBuilder(new StoredHQLQuery(qry, 6), parentStack);

            keyControls.Add(nodeID, new Dictionary<string, FrameworkElement>());
            keyControls[nodeID].Add(txtboxID, win.namedControls[txtboxID]);
            keyControls[nodeID].Add(ddlAttID, win.namedControls[ddlAttID]);
            ((TreeViewItem)win.namedControls[nodeID]).Tag = node;
            nodeLst.Add(nodeID, ((TreeViewItem)win.namedControls[nodeID]));
            lastQryNum = finalQry.currQryNum;
            currNodeNum++;
        }
        public void AddControlNode(FrameworkElement parentStack, String parentName)
        {
            FullQuery finalQry = new FullQuery(lastQryNum);
            String node = finalQry.NewQuery(UIQuerySnippets.NodeQry, "Root", parentName).parentNm;
            QueryItem nodeItem = finalQry.NewQuery(UIQuerySnippets.attributeName, node, "node_" + currNodeNum, parentName);
            // QueryItem nodeItem = finalQry.NewQuery(UIQuerySnippets.attributeName, node, node);
            String nodeID = nodeItem.guessIDName();
            String nodeConstructorID = nodeItem.parentNm;
            String nodePanel = finalQry.NewQuery(UIQuerySnippets.groupBoxHorizontal, node, "").parentNm;
            String stackID = finalQry.NewQuery(UIQuerySnippets.attributeName, nodePanel, "stackButton_" + currNodeNum).guessIDName();
            String stack = finalQry.NewQuery(UIQuerySnippets.groupBoxQry, nodePanel, "").parentNm;
            finalQry.NewQuery(UIQuerySnippets.textLabel, stack, "Controls");
            String ddlControls = finalQry.NewQuery(UIQuerySnippets.ddlControlNames, stack, "").parentNm;
            String ddlControlID = finalQry.NewQuery(UIQuerySnippets.attributeName, ddlControls, "ControlName").guessIDName();

            finalQry.NewQuery(UIQuerySnippets.textLabel, nodePanel, "Full Query");
            String txtQuery = finalQry.NewQuery(UIQuerySnippets.NamedTextbox, nodePanel, "txtQuery").guessIDName();
            //String ddlQueryID = finalQry.NewQuery(UIQuerySnippets.attributeName, ddlTables, "METAROW_NAME").guessIDName();
            String qry = finalQry.fullQry;
            WPFSegmentBuilder win = new WPFSegmentBuilder(new StoredHQLQuery(qry, 7), parentStack);
            ((StackPanel)win.namedControls[stackID]).Children.Add(BtnAddChildAttribute(nodeID));
            ((StackPanel)win.namedControls[stackID]).Children.Add(BtnAddChildControl(nodeID));
            ((StackPanel)win.namedControls[stackID]).Children.Add(BtnDeleteItem(nodeID));
            ((TreeViewItem)win.namedControls[nodeID]).Tag = nodeItem;
            nodeLst.Add(nodeID, ((TreeViewItem)win.namedControls[nodeID]));

            keyControls.Add(nodeID, new Dictionary<string, FrameworkElement>());
            keyControls[nodeID].Add(stackID, win.namedControls[stackID]);
            keyControls[nodeID].Add(ddlControlID, win.namedControls[ddlControlID]);
            keyControls[nodeID].Add(txtQuery, win.namedControls[txtQuery]);
            //keyControls[nodeID].Add(ddlAttID, win.namedControls[ddlAttID]);

            lastQryNum = finalQry.currQryNum;
            currNodeNum++;
        }
        const String controlQryTemplate = "FROM UICONTROLTABLE AS @UNIQUETABLENM JOIN ON * = @OWNERTABLE.ROGUECOLUMNID SELECT CONTROLNAME, \"@RELATIONTYPE\" AS PARENTRELATION WHERE CONTROLNAME = \"@CONTROLNAME\"";
        const String attQryTemplate = "FROM ATTRIBUTES AS @UNIQUETABLENM JOIN ON * = @OWNERTABLE.ROGUECOLUMNID SELECT ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,\"@ATTRIBUTEVALUE\" AS ATTRIBUTEVALUE WHERE ATTRIBUTETYPE = \"@ATTRIBUTETYPE\"";
        String GetControlQueryLine(String ownertable, string relationType, string controlname)
        {
            return controlQryTemplate.Replace("@OWNERTABLE", ownertable).Replace("@RELATIONTYPE", relationType).Replace("@CONTROLNAME", controlname);
        }
        String GetAttributeQueryLine(string ownertable, string attributeValue, string attributeType)
        {
            return attQryTemplate.Replace("@OWNERTABLE", ownertable).Replace("@ATTRIBUTEVALUE", attributeValue).Replace("@ATTRIBUTETYPE", attributeType);
        }
        //public void TreeViewItem CreateAttributeNode()
        //{
        //    String qry = UIQuerySnippets.Node(0, "Root", "nodeBox0");
        //    qry += UIQuerySnippets.groupBox(1, "NodePanel0");
        //    qry += UIQuerySnippets.attributeName(2, "NodePanel0", "nodeGrpBox");
        //    qry += UIQuerySnippets.textLabel(3, "grpBox1", "Attributes");
        //    qry += UIQuerySnippets.ddlControlAttributes(4, "grpBox1");
        //    qry += UIQuerySnippets.groupBox(5, "NodePanel0");
        //    //qry += UIQuerySnippets.ddlControlNames(6, "grpBox5");
        //    qry += UIQuerySnippets.textLabel(6, "grpBox5", "AttributeValue");
        //    qry += UIQuerySnippets.NamedTextbox(7, "grpBox5", "txtAttValue");
        //}
        public Button BtnAddChildAttribute(String nodeID)
        {
            Button attBtn = new Button();
            attBtn.Content = "Add Attribute";
            attBtn.Click += InsertChildAtt;
            attBtn.Tag = nodeID;
            return attBtn;
        }
        public Button BtnAddChildControl(String nodeID)
        {
            Button controlBtn = new Button();
            controlBtn.Content = "Add Control";
            controlBtn.Click += InsertChildControl;
            controlBtn.Tag = nodeID;
            return controlBtn;
        }
        public Button BtnDeleteItem(String nodeID)
        {
            Button controlBtn = new Button();
            controlBtn.Content = "Delete";
            controlBtn.Click += DeleteItem;
            controlBtn.Tag = nodeID;
            return controlBtn;
        }
        public void InsertChildAtt(Object sender, RoutedEventArgs e)
        {
            String nodeID = ((Button)sender).Tag.ToString();
            TreeViewItem thsNode = nodeLst[nodeID];
            AddAttributeNode(thsNode, ((QueryItem)thsNode.Tag).extra);
        }
        public void InsertChildControl(Object sender, RoutedEventArgs e)
        {
            String nodeID = ((Button)sender).Tag.ToString();
            TreeViewItem thsNode = nodeLst[nodeID];
            AddControlNode(thsNode, ((QueryItem)thsNode.Tag).extra);
        }
        public void DeleteItem(Object sender, RoutedEventArgs e)
        {
            String nodeID = ((Button)sender).Tag.ToString();
            TreeViewItem thsNode = nodeLst[nodeID];
            for (int i = thsNode.Items.Count - 1; i >= 0; i--)
            {
                StackPanel panelChild = (StackPanel)thsNode.Items[i];
                TreeViewItem treeItem = (TreeViewItem)panelChild.Children[0];
                keyControls.Remove(((QueryItem)treeItem.Tag).extra);
                nodeLst.Remove(((QueryItem)treeItem.Tag).extra);
            }
            thsNode.Items.Clear();
            //StackPanel panelParent = (StackPanel)thsNode.Parent;
            //StackPanel panelParentParnet = (StackPanel)thsNode.Parent;
            //TreeViewItem parentNode = (TreeViewItem)panelParentParnet.Children[0];
            //nodeLst.Remove(((QueryItem)thsNode.Tag).extra);
            ////TreeViewItem parentNode = (TreeViewItem)thsNode.Parent;
            //keyControls.Remove(nodeID);
            //panelParent.Children.Remove(thsNode);
            //panelParentParnet.Children.Remove(panelParent);
        }
        static class UIQuerySnippets
        {
            //public static String ddlControlNames(int id, String OwnerTable)
            //{
            //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = Root.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes  JOIN ON   * = topPanel.RogueColumnID SELECT listboxAttributes.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues  JOIN ON   * = listboxAttributes.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems  JOIN MERGE   * = attValues.RogueColumnID SELECT lstBoxItems.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText  JOIN ON   * = attValues.RogueColumnID SELECT attItemText.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = topPanel@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"".Replace("@ID", id.ToString()).Replace("@OWNERTABLE", OwnerTable);
            //public const String ddlControlNames = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = top@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            public const String ddlControlNames = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * =  @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues@ID  JOIN ON   * = top@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //}
            //public const String ddlControlAttributes = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = top@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.attributes AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.ATTRIBUTETYPE AS ATTRIBUTEVALUE WHERE attributetype = \"text\""; public const String ddlControlAttributes = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = top@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.attributes AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.ATTRIBUTETYPE AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            public const String ddlControlAttributes = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.attributes AS attValues@ID  JOIN ON  * = top@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.ATTRIBUTETYPE AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            public const String ddlAllTables = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM IORECORDS AS attValues@ID  JOIN ON  * = top@ID.RogueColumnID FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.METAROW_NAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //public static String ddlControlAttributes(int id, String OwnerTable)
            //{
            //    //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = topPanel@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.attributes AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.ATTRIBUTETYPE AS ATTRIBUTEVALUE WHERE attributetype = \"text\"".Replace("@ID",id.ToString()).Replace("@OWNERTABLE", OwnerTable);
            //    //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = Root.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes  JOIN ON   * = topPanel.RogueColumnID SELECT listboxAttributes.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues  JOIN ON   * = listboxAttributes.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems  JOIN MERGE   * = attValues.RogueColumnID SELECT lstBoxItems.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText  JOIN ON   * = attValues.RogueColumnID SELECT attItemText.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //    //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = Root.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = topPanel@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"".Replace("@ID", id.ToString());
            //}
            //public static String NodeQry()
            //{
            public const string NodeQry = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME, \"child\" AS PARENTRELATION WHERE ControlName = \"treeviewnode\"";
            //public const String groupBoxHorizontal = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = topNode@ID.RogueColumnID SELECT top@ID.CONTROLNAME,\"header\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.attributes AS colNameAtt@ID JOIN ON * = top@ID.RogueColumnID SELECT colNameAtt@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"horizontal\" AS ATTRIBUTEVALUE WHERE attributetype = \"orientation\" FROM Root.Stock.UIDatabase.attributes AS colIDName@ID JOIN ON * = top@ID.RogueColumnID SELECT colIDName@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"@NAME\" AS ATTRIBUTEVALUE WHERE attributetype = \"idname\"";
            public const String groupBoxHorizontal = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"header\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.attributes AS colNameAtt@ID JOIN ON * = top@ID.RogueColumnID SELECT colNameAtt@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"horizontal\" AS ATTRIBUTEVALUE WHERE attributetype = \"orientation\"";
            //.Replace("@ID", id.ToString()).Replace("@OWNERTABLE", OwnerTable).Replace("@NAME", idname);
            //return new QueryItem(baseQry, "NodePanel", ownerTable, idname);
            //}
            //public static String Node(int id, String OwnerTable, String idname)
            //{

            //    return "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT dbTreeViewItem@ID.CONTROLNAME, \"child\" AS PARENTRELATION WHERE ControlName = \"treeviewnode\" FROM Root.Stock.UIDatabase.UIControlTable AS NodePanel@ID  JOIN ON   * = dbTreeViewItem@ID.RogueColumnID SELECT NodePanel@ID.CONTROLNAME,\"header\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.attributes AS colNameAtt@ID JOIN ON * = NodePanel@ID.RogueColumnID SELECT colNameAtt@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"horizontal\" AS ATTRIBUTEVALUE WHERE attributetype = \"orientation\" FROM Root.Stock.UIDatabase.attributes AS colIDName@ID JOIN ON * = NodePanel@ID.RogueColumnID SELECT colIDName@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"@NAME\" AS ATTRIBUTEVALUE WHERE attributetype = \"idname\"".Replace("@ID", id.ToString()).Replace("@OWNERTABLE", OwnerTable).Replace("@NAME",idname);
            //    //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = Root.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = topPanel@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.attributes AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.ATTRIBUTETYPE AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //    //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = Root.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes  JOIN ON   * = topPanel.RogueColumnID SELECT listboxAttributes.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues  JOIN ON   * = listboxAttributes.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems  JOIN MERGE   * = attValues.RogueColumnID SELECT lstBoxItems.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText  JOIN ON   * = attValues.RogueColumnID SELECT attItemText.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //    //return "FROM Root.Stock.UIDatabase.UIControlTable AS topPanel@ID  JOIN ON   * = Root.RogueColumnID SELECT topPanel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\" FROM Root.Stock.UIDatabase.UIControlTable AS listboxAttributes@ID  JOIN ON   * = topPanel@ID.RogueColumnID SELECT listboxAttributes@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"dropdownlist\" FROM Root.Stock.UIDatabase.UIControlTable AS attValues@ID  JOIN ON   * = listboxAttributes@ID.RogueColumnID SELECT  WHERE  FROM Root.Stock.UIDatabase.UIControlTable AS lstBoxItems@ID  JOIN MERGE   * = attValues@ID.RogueColumnID SELECT lstBoxItems@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"listitem\" FROM Root.Stock.UIDatabase.attributes AS attItemText@ID  JOIN ON   * = attValues@ID.RogueColumnID SELECT attItemText@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,attValues@ID.CONTROLNAME AS ATTRIBUTEVALUE WHERE attributetype = \"text\"".Replace("@ID", id.ToString());
            //}
            public static String attributeText(String text, String OwnerTable)
            {
                return "FROM Root.Stock.UIDatabase.attributes AS colNameAtt JOIN ON * = @OWNERTABLE.RogueColumnID SELECT colNameAtt.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, @TEXT AS ATTRIBUTEVALUE WHERE attributetype = \"text\"".Replace("@TEXT", text).Replace("@OWNERTABLE", OwnerTable);
            }
            public static String attributeBold(String OwnerTable)
            {
                return "FROM Root.Stock.UIDatabase.attributes AS colNameAtt JOIN ON * = @OWNERTABLE.RogueColumnID SELECT colNameAtt.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"bold\" AS ATTRIBUTEVALUE WHERE attributetype = \"fontweight\"".Replace("@OWNERTABLE", OwnerTable);
            }
            public static String attributeHorizontalOrientation(String OwnerTable)
            {
                return "FROM Root.Stock.UIDatabase.attributes AS colNameAtt JOIN ON * = @OWNERTABLE.RogueColumnID SELECT colNameAtt.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"horizontal\" AS ATTRIBUTEVALUE WHERE attributetype = \"orientation\"".Replace("@OWNERTABLE", OwnerTable);
            }
            //public static QueryItem groupBox(String OwnerTable)
            //{
            public const String groupBoxQry = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID JOIN ON  * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\"";
            //return new QueryItem(baseQry, "grpBox@ID", OwnerTable, "");
            //"FROM Root.Stock.UIDatabase.UIControlTable AS grpBox@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT grpBox@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"groupbox\"".Replace("@OWNERTABLE", OwnerTable).Replace("@ID",id.ToString());
            //}
            public const String textLabel = "FROM Root.Stock.UIDatabase.UIControlTable AS top@ID JOIN ON  * = @OWNERTABLE.RogueColumnID SELECT top@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"label\" FROM Root.Stock.UIDatabase.attributes AS colNameAtt@ID JOIN ON * = top@ID.RogueColumnID SELECT colNameAtt@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"@TEXT\" AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //public static String textLabel(int id, String OwnerTable, String text)
            //{
            //    return "FROM Root.Stock.UIDatabase.UIControlTable AS colNameLabel@ID JOIN ON  * = @OWNERTABLE.RogueColumnID SELECT colNameLabel@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"label\" FROM Root.Stock.UIDatabase.attributes AS colNameAtt@ID JOIN ON * = colNameLabel@ID.RogueColumnID SELECT colNameAtt@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"@TEXT\" AS ATTRIBUTEVALUE WHERE attributetype = \"text\"".Replace("@TEXT",text).Replace("@OWNERTABLE",OwnerTable).Replace("@ID",id.ToString());
            //}
            //public static String attributeName(int id, String OwnerTable, String idname)
            //{
            public const String attributeName = "FROM Root.Stock.UIDatabase.attributes AS colNameAtt@ID JOIN ON * = @OWNERTABLE.RogueColumnID SELECT colNameAtt@ID.ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION, \"@NAME\" AS ATTRIBUTEVALUE WHERE attributetype = \"idname\"";
            //}
            //public static String NamedTextbox(int id, String OwnerTable, String idname)
            //{
            public const String NamedTextbox = "FROM Root.Stock.UIDatabase.UIControlTable AS colNameTextbox@ID  JOIN ON   * = @OWNERTABLE.RogueColumnID SELECT colNameTextbox@ID.CONTROLNAME,\"child\" AS PARENTRELATION WHERE ControlName = \"textbox\" FROM Root.Stock.UIDatabase.attributes AS textBoxAtt@ID  JOIN ON   * = colNameTextbox@ID.RogueColumnID SELECT \"idname\" AS ATTRIBUTETYPE,\"attribute\" AS PARENTRELATION,\"@NAME\" AS ATTRIBUTEVALUE WHERE attributetype = \"text\"";
            //}
            public static String button(int id, String OwnerTable, String text)
            {
                return "";
            }
        }
        private void BtnCreateQry_Click(object sender, RoutedEventArgs e)
        {
            String qry = "";
            foreach (TreeViewItem thsItem in nodeLst.Values)
            {
                QueryItem qryItem = (QueryItem)(thsItem.Tag);
                String thsTableName = qryItem.extra;
                //if(((TextBox)keyControls[thsTableName]["AttributeValue"]).Text != "")
                //{
                //    String segment = ((TextBox)keyControls[thsTableName]["AttributeValue"]).Text;

                //}
                if (keyControls[thsTableName].ContainsKey("AttributeValue"))
                {
                    String attValue = ((TextBox)keyControls[thsTableName]["AttributeValue"]).Text;
                    String attType = ((ComboBox)keyControls[thsTableName]["AttributeType"]).Text;
                    qry += Environment.NewLine + attQryTemplate.Replace("@ATTRIBUTEVALUE", attValue).Replace("@ATTRIBUTETYPE", attType);
                }
                else
                {
                    String controlName = ((ComboBox)keyControls[thsTableName]["ControlName"]).Text;
                    qry += Environment.NewLine + controlQryTemplate.Replace("@CONTROLNAME", controlName).Replace("@RELATIONTYPE", "child");
                }
                qry = qry.Replace("@OWNERTABLE", qryItem.parentJoinName);
                qry = qry.Replace("@UNIQUETABLENM", thsTableName);
            }
            txtQuery.Text = qry;
            //String encodedQry = HQLQuery.ConvertToHQL(qry);
            //WindowsUIBuilderNew win = new WindowsUIBuilderNew(encodedQry, uiTestPanel);
        }
        private void BtnAddToRoot_Click(object sender, RoutedEventArgs e)
        {
            AddControlNode(mainTree, "Root");
        }
        private void BtnRunUIQry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uiTestPanel.Children.Clear();
                //String encodedQry = HQLQuery.ConvertToHQL(txtQuery.Text.Replace(Environment.NewLine, " "));
                WPFSegmentBuilder win = new WPFSegmentBuilder(new StoredHQLQuery(txtQuery.Text, 5), uiTestPanel);
            }
            catch (Exception ex)
            {
                String blah = ex.ToString();
            }
        }
    }
    public class QueryItem
    {
        public String parentJoinName { get; set; }
        //int id { get; set; }
        public String finalQry;
        public int id;
        public string extra;
        public String parentNm { get; set; }
        public String ownerTable;
        public String guessIDName()
        {
            return extra;
        }
        public QueryItem(String qry, String OwnerTable, String extra, String parnetJoinName)
        {
            this.extra = extra;
            this.ownerTable = OwnerTable;
            this.parentJoinName = parnetJoinName;
            finalQry = qry.Replace("@OWNERTABLE", OwnerTable).Replace("@NAME", extra).Replace("@TEXT", extra);
        }
    }
    public class FullQuery : List<QueryItem>
    {
        public int currQryNum { get; private set; }
        public String fullQry { get { return CreateQuery(); } }
        public FullQuery(int startInt)
        {
            currQryNum = startInt + 1;
        }
        public QueryItem NewQuery(String baseQry, String ownerTable, String extra, String joinParentName = "")
        {
            this.Add(new QueryItem(baseQry, ownerTable, extra, joinParentName));
            this[this.Count - 1].finalQry = baseQry.Replace("@OWNERTABLE", ownerTable).Replace("@NAME", extra).Replace("@TEXT", extra).Replace("@ID", currQryNum.ToString());
            this[this.Count - 1].parentNm = "top" + currQryNum.ToString();
            this[this.Count - 1].id = currQryNum;
            //this[this.Count - 1].guessIDName = extra + "_" + currQryNum.ToString();
            currQryNum++;
            return this[this.Count - 1];
        }
        String CreateQuery()
        {
            String fullQry = "";
            foreach (QueryItem thsQry in this)
            {
                fullQry += thsQry.finalQry.Trim() + " ";
            }
            return fullQry;
        }
    }
}
