using rogue_core.rogueCore.UI.UIControls;
using rogueCore.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using wpfTemplate.RogueUI.WPF.Attributes;
using wpfTemplate.RogueUI.WPF.Controls;

namespace wpfTemplate.RogueUI.WPF
{
    public class WPFPage : UIPageBuilder
    {
        ////static Dictionary<String, IUIElement> namedControls = new Dictionary<string, IUIElement>();
        public WPFPage(int queryID) : base(queryID, UIFrameworks.wpf, WPFTranslater){ }
        public static IUIElement WPFTranslater(String controlName, String attributeValue = "")
        {
            switch (controlName)
            {
                case "textbox":
                    return new WPFTextBox();
                case "label":
                    return new WPFLabel();
                case "button":
                    return new WPFButton();
                case "image":
                    return new WPFImage();
                case "treeviewnode":
                case "emptytreeviewnode":
                    return new WPFTreeViewNode();
                case "treeview":
                    return new WPFTreeView();
                case "groupbox":
                    return new WPFGroupBox();
                //case "tablerow<maintainonrow>":
                //case "tablerow":
                //case "headertablerow":
                //    return new WPFTableRow();
                //case "tablecell":
                //case "headertablecell":
                //    return new WPFTableCell();
               // case "table":
                    //return new WPFTable();
                case "dropdownlist":
                    return new WPFDropDownList();
                case "listitem":
                    return new WPFListItem();
                case "displaycell":
                case "tablecell":
                case "headertablecell":
                    return new WPFDisplayCell();
                case "displayrow":
                case "tablerow<maintainonrow>":
                case "tablerow":
                case "headertablerow":
                    return new WPFDisplayRow();
                case "displaytable":
                case "table":
                    return new WPFDisplayTable();
                case "navbar":
                    return new WPFNavBar();
                case "navitem":
                    return new WPFNavItem();
                case "widthpixels":
                    return new WPFWidthPixels(attributeValue);
                case "heightpixels":
                    return new WPFHeightPixels(attributeValue);
                case "fontsize":
                    return new WPFFontSize(attributeValue);
                case "mouseclick":
                    return new WPFMouseClick(attributeValue);
                case "mousedoubleclick":
                    return new WPFMouseDoubleClick(attributeValue);
                case "text":
                    return new WPFText(attributeValue);
                case "orientation":
                    return null;
                case "idname":
                    //namedControls.Add();
                    return new WPFNameID(attributeValue);
                case "widthpercent":
                    return new WPFWidthPercent(attributeValue);
                case "heightpercent":
                    return new WPFHeightPercent(attributeValue);
                case "fontweight":
                    return null;
                case "backgroundcolor":
                    return new WPFBackgroundColor(attributeValue);
                //case "cssclass":
                //    return new CssClass(attributeValue);
                case "imagepath":
                    return new WPFImagePath(attributeValue);
                case "paddingleft":
                    return new WPFPaddingLeft(attributeValue);
                default:
                    return null;
            }
        }
        private static void SetAttribute(String attributeType, String attributeValue, FrameworkElement control, Boolean isPostAttribute = false)
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
                            //btn.Click += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.insert, (UISegmentQuery)thsQry, btn.Tag.ToString()); };
                            break;
                        case "select":
                           // btn.Click += (sender, EventArgs) => { ClickEvent(sender, EventArgs, ClickActions.select, (UISegmentQuery)thsQry, btn.Tag.ToString()); };
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
                    //namedControls.Add(attributeValue, control);
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
        private static FrameworkElement TranslateUIControlRow(String controlName, List<FrameworkElement> lstContainers)
        {
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
                    ((WPFDisplayTable)lstContainers[lstContainers.Count - 1]).AddRow();
                    retElement = lstContainers[lstContainers.Count - 1];
                    break;
                case "tablecell":
                    ((WPFDisplayTable)lstContainers[lstContainers.Count - 1]).AddColumn();
                    //*mod to aid fake rows. just use the same grid 
                    retElement = lstContainers[lstContainers.Count - 1];
                    break;
                case "table":
                    retElement = new WPFDisplayTable();
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
            return retElement;
        }
    }
}
