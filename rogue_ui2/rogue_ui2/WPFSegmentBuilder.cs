using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using rogue_core.rogueCore.StoredProcedures.StoredQuery;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.hqlSyntaxV3.filledSegments;
using static rogueCore.rogueUIV3.UISection;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Input;
using System.Windows.Documents;

namespace rogue_ui2
{
    class WPFSegmentBuilder
    {
        public Dictionary<String, IMyControl> namedControls = new Dictionary<String, IMyControl>();
        List<List<String>> postAttributes = new List<List<String>>();
        public int tempTableID = 0;
        IMyControl topControl;
        public WPFSegmentBuilder(string qry, FrameworkElement pnl, Action<Object, RoutedEventArgs, ClickActions, StoredHQLQuery, String> selectEvent = null)
        {
            topControl = MyGroupBox.HeaderGroupBox();
            if (pnl is StackPanel)
            {
                ((StackPanel)pnl).Children.Add((UIElement)topControl);
            }
            if (pnl is ItemsControl)
            {
                ((ItemsControl)pnl).Items.Add(topControl);
            }
            BuildSection(qry, topControl);
        }
        public void BuildSection(String qry, IMyControl top)
        {
            //MyGroupBox myStackPanel = MyGroupBox.HeaderGroupBox();
            //top.Children.Add(myStackPanel);
            List<IMyElement> lstContainers = new List<IMyElement>();
            lstContainers.Add(top);
            SelectHQLStatement buildQry = new SelectHQLStatement(qry);
            buildQry.Fill();
            Action<IMultiRogueRow> OpenWPFControl = (row) =>
            {
                List<KeyValuePair<string, string>> rowValues = row.GetValueList().ToList();
                string controlNM = row.GetValueAt(0);
                string controlVal = "";
                if (rowValues.Count > 1)
                {
                    controlVal = row.GetValueAt(1);
                }
                IMyControl parentControl = lstContainers[lstContainers.Count - 1] as IMyControl;
                var element = TranslateUIControlRow(controlNM, lstContainers, controlVal);
                ////if (element.IsElement)
                ////{
                //SetParentChild(parentControl, controlVal, element as UIElement);
                lstContainers.Add(element);
                //}
            };
            Action<IMultiRogueRow> CloseWPFControl = (row) =>
            {
                //if (element.IsElement)
                //{
                lstContainers.RemoveAt(lstContainers.Count - 1);
                //}
            };
            buildQry.IterateRows(OpenWPFControl, CloseWPFControl);
            //int currLevel = 0;
            //foreach (var thsRowPair in qryResults.hierarchyGrid)
            //{
            //    int diff = currLevel - thsRowPair.Key;
            //    HierarchyRow thsRow = thsRowPair.Value;
            //    String strRelation = thsRow.GetValueByColName("PARENTRELATION");
            //    ParentRelationships parentRelation;
            //    Boolean foundRelation = System.Enum.TryParse<ParentRelationships>(strRelation, out parentRelation);
            //    if (!foundRelation)
            //    {
            //        parentRelation = ParentRelationships.child;
            //    }
            //    if (diff > 0)
            //    {
            //        for (int i = 0; i != diff; i++)
            //        {
            //            lstContainers.RemoveAt(lstContainers.Count - 1);
            //        }
            //    }
            //    switch (parentRelation)
            //    {
            //        case ParentRelationships.attribute:
            //            String attributeType = thsRow.GetValueByColName("attributetype");
            //            String attributeValue = thsRow.GetValueByColName("attributevalue");
            //            SetAttribute(attributeType, attributeValue, lstContainers[lstContainers.Count - 1]);
            //            break;
            //        case ParentRelationships.child:
            //        case ParentRelationships.header:
            //            ParseControlItem(diff, lstContainers, thsRow);
            //            break;
            //    }
            //    currLevel = thsRowPair.Key;
            //}
        }
        //private void ParseControlItem(int diff, List<FrameworkElement> lstContainers, IMultiRogueRow thsRow)
        //{
        //    FrameworkElement thsControl = TranslateUIControlRow(lstContainers);
        //    if (thsControl != null)
        //    {
        //        SetParentContent(lstContainers[lstContainers.Count - 1], thsControl, thsRow);
        //        if (thsRow.hasChildren)
        //        {
        //            lstContainers.Add(thsControl);
        //        }
        //    }
        //    else
        //    {
        //        lstContainers.Add(lstContainers[lstContainers.Count - 1]);
        //    }
        //}
        //private void SetParentContent(FrameworkElement thsWinContainer, FrameworkElement newControl, HierarchyRow newControlRow)
        //{
        //    if (newControlRow.GetValueByColName("PARENTRELATION").ToUpper() == "HEADER")
        //    {
        //        ((HeaderedItemsControl)thsWinContainer).Header = newControl;
        //    }
        //    else if (thsWinContainer is MyTable && !(newControl is MyTable))
        //    {
        //        MyTable grid = ((MyTable)thsWinContainer);
        //        grid.ShowGridLines = true;
        //        Grid.SetRow(newControl, grid.currRowNum);
        //        Grid.SetColumn(newControl, grid.currColNum);
        //        grid.Children.Add(newControl);
        //    }
        //    else if (thsWinContainer is Panel && !(thsWinContainer is MyTable))
        //    {
        //        ((Panel)thsWinContainer).Children.Add(newControl);
        //    }
        //    else if (thsWinContainer is ItemsControl)
        //    {
        //        ((ItemsControl)thsWinContainer).Items.Add(newControl);
        //    }
        //}
        //private void SetAttribute(String attributeType, String attributeValue, FrameworkElement control, Boolean isPostAttribute = false)
        //{
        //    switch (attributeType)
        //    {
        //        case "fontsize":
        //            ((Control)control).FontSize = int.Parse(attributeValue);
        //            break;
        //        case "mouseclick":
        //            Control btnClick = (Control)control;
        //            switch (attributeValue)
        //            {
        //                case "insertrow":
        //                    break;
        //                case "select":
        //                    break;
        //            }
        //            break;
        //        case "mousedoubleclick":
        //            Control btnDoubleClick = (Control)control;
        //            switch (attributeValue)
        //            {
        //                //btnDoubleClick.MouseDown += 
        //                case "insertrow":
        //                    break;
        //                case "select":
        //                    break;
        //            }
        //            break;
        //        case "text":
        //            if (control is TextBox)
        //            {
        //                ((TextBox)control).Text = attributeValue;
        //            }
        //            else if (control is ContentControl)
        //            {
        //                ((ContentControl)control).Content = attributeValue;
        //            }
        //            else if (control is TreeViewItem)
        //            {
        //                ((TreeViewItem)control).Header = attributeValue;
        //            }
        //            break;
        //        case "orientation":
        //            if (attributeValue == "horizontal")
        //            {
        //                ((StackPanel)control).Orientation = Orientation.Horizontal;
        //            }
        //            else
        //            {
        //                ((StackPanel)control).Orientation = Orientation.Vertical;
        //            }
        //            break;
        //        case "idname":
        //            control.Tag = attributeValue;
        //            namedControls.Add(attributeValue, control);
        //            break;
        //        case "columnspan":
        //        case "rowspan":
        //            if (isPostAttribute)
        //            {
        //                if (attributeType == "columnspan")
        //                {
        //                    Grid.SetColumnSpan(control, int.Parse(attributeValue));
        //                }
        //                else
        //                {
        //                    Grid.SetRowSpan(control, int.Parse(attributeValue));
        //                }
        //            }
        //            else
        //            {
        //                postAttributes.Add(new List<String>() { attributeType, attributeValue });
        //            }
        //            break;
        //        case "widthpercent":
        //            if (control is Grid)
        //            {
        //                Grid thsGrid = ((Grid)control);
        //                int thsWidth = int.Parse(attributeValue);
        //                thsGrid.ColumnDefinitions[thsGrid.ColumnDefinitions.Count - 1].Width = new GridLength(thsWidth, GridUnitType.Star);
        //            }
        //            else if (control is StackPanel)
        //            {
        //                StackPanel thsPnl = ((StackPanel)control);
        //                int thsWidth = int.Parse(attributeValue);
        //                thsPnl.Width = thsWidth;
        //            }
        //            break;
        //        case "heightpercent":
        //            if (control is Grid)
        //            {
        //                Grid rowGrid = ((Grid)control);
        //                int thsHeight = int.Parse(attributeValue);
        //                rowGrid.RowDefinitions[rowGrid.RowDefinitions.Count - 1].Height = new GridLength(thsHeight, GridUnitType.Star);
        //            }
        //            break;
        //        case "fontweight":
        //            ((ContentControl)control).FontWeight = FontWeights.Bold;
        //            break;
        //    }
        //}
        private IMyElement TranslateUIControlRow(string controlName, List<IMyElement> lstControls, string controlValue)
        {
            var parentControl = (lstControls[lstControls.Count - 1] as IMyControl).MyControl;
            switch (controlName.ToLower())
            {
                case UIElements.textbox:
                    return new MyTextBox(parentControl);
                case UIElements.label:
                    return new MyLabel(parentControl);
                case UIElements.button:
                    return new MyButton(parentControl);
                case UIElements.image:
                    return new MyImage(parentControl, controlValue);
                case UIElements.treeviewnode:
                    return new MyTreeViewNode(parentControl, controlValue);
                case UIElements.treeview:
                    return new MyTreeView(parentControl);
                case UIElements.groupbox:
                    return new MyGroupBox(parentControl);
                case UIElements.tablerow:
                    return new MyTableRow(parentControl);
                case UIElements.tablecell:
                    return new MyTableCell((lstControls[lstControls.Count - 2] as IMyControl).MyControl);
                case UIElements.table:
                    return new MyTable(parentControl);
                case UIElements.dropdownlist:
                    return new MyDropDownList(parentControl);
                case UIElements.listitem:
                    return new MyDropDownListItem(parentControl);
                case UIElements.fontsize:
                    return new MyFontSize(parentControl, controlValue);
                case UIElements.idname:
                    var retElement = new MyIDName(parentControl, controlValue);
                    namedControls.Add(controlValue, lstControls[lstControls.Count-1] as IMyControl);
                    return retElement;
                case UIElements.orientation:
                    return new MyOrientation(parentControl, controlValue);
                case UIElements.text:
                    return new MyText(parentControl, controlValue);
                case UIElements.fontweight:
                    return new MyFontWeight(parentControl, controlValue);
                case UIElements.widthpercent:
                    return new MyWidthPercent(parentControl, controlValue);
                case UIElements.heightpercent:
                    return new MyHeightPercent(parentControl, controlValue);
                case UIElements.mouseclick:
                    return new MyMouseClick(parentControl, controlValue);
                case UIElements.mousedoubleclick:
                    return new MyMouseDoubleClick(parentControl, controlValue);
                //case UIElements.rowspan:
                //    return new MyRowSpan(parentControl, controlValue);
                default:
                    return new MyNullControl();
            }
            ////*This handles wpf out order things like row span. Assignes to next element since cant assign to actual columns or rows
            ////if (postAttributes.Count > 0)
            ////{
            ////    foreach (var pair in postAttributes)
            ////    {
            ////        SetAttribute(pair[0], pair[1], retElement, true);
            ////    }
            ////}
            ////postAttributes.Clear();
            //return retElement;
        }
    }
    /// <summary>
    /// ** These are all Controls
    /// </summary>
    public class MyNullControl : Label, IMyElement {
        public bool IsElement { get { return false; } }
        
    }
    public class MyText : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }

        public MyText(IMyControl control, string attributeValue)
        {
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
        }
    }
    public class MyOrientation : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyOrientation(IMyControl control, string attributeValue)
        {
            if (attributeValue.ToUpper() == "horizontal")
            {
                ((StackPanel)control).Orientation = Orientation.Horizontal;
            }
            else
            {
                ((StackPanel)control).Orientation = Orientation.Vertical;
            }
        }
    }
    public class MyIDName : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyIDName(IMyControl control, string attributeValue)
        {
            ((Control)control).Tag = attributeValue;            
        }
    }
    public class MyRowSpan : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyRowSpan(IMyControl parentControl)
        {

        }
    }
    public class MyColumnSpan : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyColumnSpan(IMyControl parentControl)
        {

        }
    }
    public class MyWidthPercent : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyWidthPercent(IMyControl control, string attributeValue)
        {
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
        }
    }
    public class MyHeightPercent : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyHeightPercent(IMyControl control, string attributeValue)
        {
            if (control is Grid)
            {
                Grid rowGrid = ((Grid)control);
                int thsHeight = int.Parse(attributeValue);
                rowGrid.RowDefinitions[rowGrid.RowDefinitions.Count - 1].Height = new GridLength(thsHeight, GridUnitType.Star);
            }
        }
    }
    public class MyFontWeight : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyFontWeight(IMyControl control, string attributeValue)
        {
            ((ContentControl)control).FontWeight = FontWeights.Bold;
        }
    }
    public class MyFontSize : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyFontSize(IMyElement control, string attributeValue)
        {
            ((Control)control).FontSize = int.Parse(attributeValue);
        }
    }
    public class MyMouseClick : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyMouseClick(IMyElement control, string attributeValue)
        {
            Control btnClick = (Control)control;
            switch (attributeValue)
            {
                //btnDoubleClick.MouseDown += 
                case "insertrow":
                    break;
                case "select":
                    break;
            }
        }
    }
    public class MyMouseDoubleClick : IMyElement, IMyAttribute
    {
        public bool IsElement { get { return false; } }
        public MyMouseDoubleClick(IMyElement control, string attributeValue)
        {
            Control btnDoubleClick = (Control)control;
            switch (attributeValue)
            {
                //btnDoubleClick.MouseDown += 
                case "insertrow":
                    break;
                case "select":
                    break;
            }
        }
    }
    /// <summary>
    /// ** These are all Controls
    /// </summary>
    /// 
    public class MyTable : Grid, IMyControl
    {
        int maxRows = -1;
        int maxCols = -1;
        public int currRowNum { get; private set; } = -1;
        public int currColNum { get; private set; } = -1;
        public bool IsElement { get { return true; } }
        public IMyControl MyControl { get { return this; } }
        public MyTable(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
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
    public class MyTableRow : IMyControl
    {
        public bool IsElement { get { return true; } }
        public IMyControl MyControl { get; set; }
        public MyTableRow(IMyControl parentControl)
        {
            ((MyTable)parentControl).AddRow();
            MyControl = parentControl;
        }
    }
    public class MyTableCell : IMyControl
    {
        public IMyControl MyControl { get; set; }
        public bool IsElement { get { return true; } }
        public MyTableCell(IMyControl parentControl)
        {
            ((MyTable)parentControl).AddColumn();
            MyControl = parentControl;
        }
    }
    public class MyTextBox : TextBox, IMyControl, IValuedElement 
    {
        public bool IsElement { get { return true; } }
        public IMyControl MyControl { get { return this; } }
        public new string Tag { get; set; }
        public string Content { get { return this.Text; } }

        public MyTextBox(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
        public MyTextBox(){ }
    }
    public class MyLabel : Label, IMyControl
    {
        public bool IsElement { get { return true; } }
        public IMyControl MyControl { get { return this; } }
        public MyLabel(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
    }
    public class MyButton : Button, IMyControl
    {
        public bool IsElement { get { return true; } }
        public IMyControl MyControl { get { return this; } }
        public MyButton(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
    }
    public class MyImage : Image, IMyControl
    {
        public IMyControl MyControl { get { return this; } }
        public bool IsElement { get { return true; } }
        public MyImage(IMyControl parentControl,string controlValue)
        {
            //this.Source = "/pics/" + controlValue + ".png";
            this.Width = 80;
            this.Source = new BitmapImage(new Uri("pack://application:,,,/pics/" + controlValue + ".png"));
            parentControl.SetParentChild(controlValue, this);
        }
    }
    public class MyTreeViewNode : TreeViewItem, IMyControl
    {
        public IMyControl MyControl { get { return this; } }
        public bool IsElement { get { return true; } }
        public MyTreeViewNode(IMyControl parentControl, string controlValue)
        {
            parentControl.SetParentChild(controlValue, this);
        }
    }
    public class MyTreeView : TreeView, IMyControl
    {
        public IMyControl MyControl { get { return this; } }
        public bool IsElement { get { return true; } }
        public MyTreeView(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
    }
    public class MyGroupBox : StackPanel, IMyControl
    {
        public IMyControl MyControl { get { return this; } }
        public bool IsElement { get { return true; } }
        public MyGroupBox(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
        public MyGroupBox()
        {

        }
        public static MyGroupBox HeaderGroupBox()
        {
            return new MyGroupBox();
        }
    }
    public class MyDropDownList : ComboBox, IMyControl, IValuedElement
    {
        public new string Tag { get; set; }
        public IMyControl MyControl { get { return this; } }
        public bool IsElement { get { return true; } }
        public string Content { get { 
                if((Control)this.SelectedValue != null){
                    return ((Control)this.SelectedValue).Tag.ToString();
                }
                else
                {
                    return "";
                }
            }                        
        }
        public MyDropDownList(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
        public MyDropDownList() { }
    }
    public class MyDropDownListItem : ComboBoxItem, IMyControl
    {
        public IMyControl MyControl { get { return this; } }
        public bool IsElement { get { return true; } }
        public MyDropDownListItem(IMyControl parentControl)
        {
            parentControl.SetParentChild("", this);
        }
    }
    public class RichTextBoxEx : RichTextBox, IValuedElement
    {
        public new string Tag { get; set; }
        public bool AutoAddWhiteSpaceAfterTriggered
        {
            get { return (bool)GetValue(AutoAddWhiteSpaceAfterTriggeredProperty); }
            set { SetValue(AutoAddWhiteSpaceAfterTriggeredProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AutoAddWhiteSpaceAfterTriggered.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoAddWhiteSpaceAfterTriggeredProperty =
            DependencyProperty.Register("AutoAddWhiteSpaceAfterTriggered", typeof(bool), typeof(RichTextBoxEx), new UIPropertyMetadata(true));

        public IList<String> ContentAssistSource
        {
            get { return (IList<String>)GetValue(ContentAssistSourceProperty); }
            set { SetValue(ContentAssistSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentAssistSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistSourceProperty =
            DependencyProperty.Register("ContentAssistSource", typeof(IList<String>), typeof(RichTextBoxEx), new UIPropertyMetadata(new List<string>()));

        public IList<char> ContentAssistTriggers
        {
            get { return (IList<char>)GetValue(ContentAssistTriggersProperty); }
            set { SetValue(ContentAssistTriggersProperty, value); }
        }

        public string Content { get { return this.StringFromRichetTextbox(); } }

        // Using a DependencyProperty as the backing store for ContentAssistSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistTriggersProperty =
            DependencyProperty.Register("ContentAssistTriggers", typeof(IList<char>), typeof(RichTextBoxEx), new UIPropertyMetadata(new List<char>()));

        #region constructure
        public RichTextBoxEx()
        {
            this.Loaded += new RoutedEventHandler(RichTextBoxEx_Loaded);
        }
        void RichTextBoxEx_Loaded(object sender, RoutedEventArgs e)
        {
            //init the assist list box
            if (this.Parent.GetType() != typeof(Grid))
            {
                throw new Exception("this control must be put in Grid control");
            }
            if (ContentAssistTriggers.Count == 0)
            {
                ContentAssistTriggers.Add('@');
            }
            if(AssistListBox.Parent != null)
            {
                (this.Parent as Grid).Children.Add(AssistListBox);
            }            
            AssistListBox.MaxHeight = 100;
            AssistListBox.MinWidth = 100;
            AssistListBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            AssistListBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            AssistListBox.Visibility = System.Windows.Visibility.Collapsed;
            AssistListBox.MouseDoubleClick += new MouseButtonEventHandler(AssistListBox_MouseDoubleClick);
            AssistListBox.PreviewKeyDown += new KeyEventHandler(AssistListBox_PreviewKeyDown);
        }

        void AssistListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if Enter\Tab\Space key is pressed, insert current selected item to richtextbox
            if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Space)
            {
                InsertAssistWord();
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                //Baskspace key is pressed, set focus to richtext box
                if (sbLastWords.Length >= 1)
                {
                    sbLastWords.Remove(sbLastWords.Length - 1, 1);
                }
                this.Focus();
            }
        }

        void AssistListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            InsertAssistWord();
        }

        private bool InsertAssistWord()
        {
            bool isInserted = false;
            if (AssistListBox.SelectedIndex != -1)
            {
                string selectedString = AssistListBox.SelectedItem.ToString().Remove(0, sbLastWords.Length);
                if (AutoAddWhiteSpaceAfterTriggered)
                {
                    selectedString += " ";
                }
                this.InsertText(selectedString);
                isInserted = true;
            }
            AssistListBox.Visibility = System.Windows.Visibility.Collapsed;
            sbLastWords.Clear();
            IsAssistKeyPressed = false;
            return isInserted;
        }
        #endregion

        #region check richtextbox's document.blocks is available
        private void CheckMyDocumentAvailable()
        {
            if (this.Document == null)
            {
                this.Document = new System.Windows.Documents.FlowDocument();
            }
            if (Document.Blocks.Count == 0)
            {
                Paragraph para = new Paragraph();
                Document.Blocks.Add(para);
            }
        }
        #endregion

        #region Insert Text
        public void InsertText(string text)
        {
            Focus();
            CaretPosition.InsertTextInRun(text);
            TextPointer pointer = CaretPosition.GetPositionAtOffset(text.Length);
            if (pointer != null)
            {
                CaretPosition = pointer;
            }
        }
        #endregion

        #region Content Assist
        private bool IsAssistKeyPressed = false;
        private System.Text.StringBuilder sbLastWords = new System.Text.StringBuilder();
        private ListBox AssistListBox = new ListBox();

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (!IsAssistKeyPressed)
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            ResetAssistListBoxLocation();

            if (e.Key == System.Windows.Input.Key.Back)
            {
                if (sbLastWords.Length > 0)
                {
                    sbLastWords.Remove(sbLastWords.Length - 1, 1);
                    FilterAssistBoxItemsSource();
                }
                else
                {
                    IsAssistKeyPressed = false;
                    sbLastWords.Clear();
                    AssistListBox.Visibility = System.Windows.Visibility.Collapsed;
                }
            }

            //enter key pressed, insert the first item to richtextbox
            if ((e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.Tab))
            {
                AssistListBox.SelectedIndex = 0;
                if (InsertAssistWord())
                {
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Down)
            {
                AssistListBox.Focus();
            }

            base.OnPreviewKeyDown(e);
        }

        private void FilterAssistBoxItemsSource()
        {
            IEnumerable<string> temp = ContentAssistSource.Where(s => s.ToUpper().StartsWith(sbLastWords.ToString().ToUpper()));
            AssistListBox.ItemsSource = temp;
            AssistListBox.SelectedIndex = 0;
            if (temp.Count() == 0)
            {
                AssistListBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                AssistListBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        protected override void OnTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
            if (IsAssistKeyPressed == false && e.Text.Length == 1)
            {
                if (ContentAssistTriggers.Contains(char.Parse(e.Text)) || e.Text.EndsWith(" "))
                {
                    ResetAssistListBoxLocation();
                    IsAssistKeyPressed = true;
                    FilterAssistBoxItemsSource();
                    return;
                }
            }

            if (IsAssistKeyPressed)
            {
                sbLastWords.Append(e.Text);
                FilterAssistBoxItemsSource();
            }
        }

        private void ResetAssistListBoxLocation()
        {
            Rect rect = this.CaretPosition.GetCharacterRect(LogicalDirection.Forward);
            double left = rect.X >= 20 ? rect.X : 20;
            double top = rect.Y >= 20 ? rect.Y + 20 : 20;
            left += this.Padding.Left;
            top += this.Padding.Top;
            AssistListBox.SetCurrentValue(ListBox.MarginProperty, new Thickness(left, top, 0, 0));
        }
        string StringFromRichetTextbox()
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                this.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                this.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }
        #endregion
    }
    public static class ControlExtensinos
    {
        public static void SetParentChild(this IMyControl parentControl, string controlValue, UIElement childControl)
        {
            if (controlValue.ToUpper() == "HEADER")
            {
                ((HeaderedItemsControl)parentControl).Header = childControl;
            }
            else if (parentControl is MyTable && !(childControl is MyNullControl))
            {
                MyTable grid = ((MyTable)parentControl);
                //grid.ShowGridLines = false;
                Grid.SetRow(childControl, grid.RowDefinitions.Count - 1);
                Grid.SetColumn(childControl, grid.ColumnDefinitions.Count - 1);
                grid.Children.Add(childControl);
            }
            else if (childControl is MyImage)
            {
                ((Button)parentControl).Content = childControl;
            }
            else if (parentControl is Panel && !(parentControl is MyTable))
            {
                ((Panel)parentControl).Children.Add(childControl);
            }
            else if (parentControl is ItemsControl)
            {
                ((ItemsControl)parentControl).Items.Add(childControl);
            }
        }
    }
    //public class MyTableRow : IMyElement, IMyControl
    //{
    //    public bool IsElement { get { return true; } }
    //    public MyTableRow(IMyControl parentControl)
    //    {

    //    }
    //}
    //public class MyTableCell : IMyElement, IMyControl
    //{
    //    public bool IsElement { get{return true;} }
    //    public MyTableCell(IMyControl parentControl)
    //    {

    //    }
    //}
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
    public enum ClickActions
    {
        insert, select, none
    }
    public interface IMyElement
    {
        bool IsElement { get; }
    }
    public interface IMyControl : IMyElement
    {
        IMyControl MyControl { get; }
    }
    public interface IMyAttribute : IMyElement
    {
        
    }
    public enum MyElementTypes
    {
        attribute, 
    }
    interface IValuedElement
    {
        string Content { get; }
        string Tag { get; set; } 
    }
}

