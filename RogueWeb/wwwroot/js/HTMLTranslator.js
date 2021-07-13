function TranslateToControl(elementNameTxt) {
    let elementName = elementNameTxt.toLocaleLowerCase();
    switch (elementName) {
        case Elements.fullHTMLOutline2Side:
            return new FullHTMLOutline2Sides();
        case Elements.sideNavOne:
            return new SideNavOne();
        case "textbox":
            return new RTextBox();
        case "textarea":
            return new RTextArea();
        case "label":
            return new RLabel();
        case "button":
            return new RButton();
        case "image":
            return new RImage();
        case Elements.fileupload:
            return new FileUpload();
        case "treeviewnode":
            return new RTreeViewNode();
        case "emptytreeviewnode":
            return new REmptyTreeViewNode();
        case "treeview":
            return new RTreeView();
        case "groupbox":
            return new RGroupBox();
        case "tablerow<maintainonrow>":
        case "tablerow":
            return new RTableRow();
        case "tablecell":
            return new RTableCell();
        case "headertablegroup":
            return new RHeaderGroup();
        case "datarowgroup":
            return new RDataRowGroup();
        case "headertablecell":
            return new RHeaderCell();
        case "table":
            return new RTable();
        case "dropdownlist":
            return new RDropDownList();
        case "listitem":
            return new RListItem();
        case "displaycell":
            return new RDisplayCell();
        case "displayrow":
            return new RDisplayRow();
        case "displaytable":
            return new RDisplayTable();
        case "navbar":
            return new RNavBar();
        case "navitem":
            return new RNavItem();
        case "orientation":
            return null;
        case "breakline":
            return new RBreakline();
        case "listview":
            return new RListView();
        case "listviewitem":
            return new RListViewItem();
        case "tabspace":
            return new TabSpace();
        default:
            return new NullElement();
    }
}
function TranslateToAttribute(elementNameTxt, elementValue)
{
   let elementName = elementNameTxt.toLocaleLowerCase();
        switch (elementName) {
            case "textalign":
                return new TextAlign(elementValue);
            case "bordertop":
                return new BorderTop(elementValue);
            case "border":
                return new Border(elementValue);
            case "borderbottom":
                return new BorderBottom(elementValue);
            case Attributes.margin:
                return new Margin(elementValue);
            case "margintop":
                return new MarginTop(elementValue);
            case "marginbottom":
                return new MarginBottom(elementValue);
            case "float":
                return new Float(elementValue);
            case "marginright":
                return new MarginRight(elementValue);
            case "marginleft":
                return new MarginLeft(elementValue);
            case "fontweight":
                return new FontWeight(elementValue);
            case "fontcolor":
                return new FontColor(elementValue);
            case "fontstyle":
                return new FontStyle(elementValue);
            case "onchange":
                return new DDLItemChange(elementValue);
            case Attributes.fontFamily:
                return new FontFamily(elementValue);
            case "contenteditable":
                return new EditableContent(elementValue);
            case "mouseclickcustom":
                return new MouseClickCustom(elementValue);
            case Attributes.underline:
                return new Underline(elementValue);
            case Attributes.displayType:
                return new CssDisplayType(elementValue);
            case "nowrap":
                return new RNoWrap(elementValue);
            case "idname":
                return new NameID(elementValue);
            case "scrollx":
                return new OverflowScrollX(elementValue);
            case "scrolly":
                return new OverflowScrollY(elementValue);
            case "columnspan":
                return new ColumnSpan(elementValue);
            case "rowspan":
                return new RowSpan(elementValue);
            case "widthpercent":
                return new WidthPercent(elementValue);
            case "heightpercent":
                return new HeightPercent(elementValue);
            case "backgroundcolor":
                return new BackgroundColor(elementValue);
            case "cssclass":
                return new CssClass(elementValue);
            case "imagepath":
                return new ImagePath(elementValue);
            case "paddingleft":
                return new PaddingLeft(elementValue);
            case "widthpixels":
                return new WidthPixels(elementValue);
            case "heightpixels":
                return new HeightPixels(elementValue);
            case "fontsize":
                return new FontSize(elementValue);
            case "mouseclick":
                return new MouseClick(elementValue);
            case "mousedoubleclick":
                return new MouseDoubleClick(elementValue);
            case "text":
                return new Text(elementValue);
            case "itemvalue":
                return new ItemValue(elementValue);
            case "isselected":
                return new IsSelected("");
            default:
                return new NullAttribute();
        }
    }

const Attributes = {

    isselected: "isselected",
    widthpixels: "widthpixels",
    heightpixels: "heightpixels",
    fontsize: "fontsize",
    mouseclick: "mouseclick",
    mousedoubleclick: "mousedoubleclick",
    text: "text",
    orientation: "orientation",
    idname: "idname",
    scrollx: "scrollx",
    scrolly: "scrolly",
    columnspan: "columnspan",
    rowspan: "rowspan",
    widthpercent: "widthpercent",
    heightpercent: "heightpercent",
    backgroundcolor: "backgroundcolor",
    cssclass: "cssclass",
    imagepath: "imagepath",
    paddingleft: "paddingleft",
    itemvalue: "itemvalue",
    nowrap: "nowrap",
    listview: "listview",
    listviewitem: "listviewitem",
    onchange: "onchange",
    tabspace: "tabspace",
    textalign: "textalign",
    bordertop: "bordertop",
    border: "border",
    borderbottom: "borderbottom",
    margintop: "margintop",
    marginbottom: "marginbottom",
    orientationFloat: "float",
    marginright: "marginright",
    marginleft: "marginleft",
    margin: "margin",
    fontweight: "fontweight",
    fontcolor: "fontcolor",
    fontstyle: "fontstyle",
    contenteditable: "contenteditable",
    mouseclickCustom: "mouseclickcustom",
    fontFamily: "fontfamily",
    underline: "underline",
    displayType: "cssdisplay"

}
const Elements = {
    textbox: "textbox",
    textarea: "textarea",
    label: "label",
    button: "button",
    image: "image",
    fileupload: "fileupload",
    treeviewnode: "treeviewnode",
    emptytreeviewnode: "emptytreeviewnode",
    treeview: "treeview",
    groupbox: "groupbox",
    tablerow: "tablerow",
    tablecell: "tablecell",
    headertablegroup: "headertablegroup",
    datarowgroup: "datarowgroup",
    headertablecell: "headertablecell",
    table: "table",
    dropdownlist: "dropdownlist",
    listitem: "listitem",
    displaycell: "displaycell",
    displayrow: "displayrow",
    displaytable: "displaytable",
    navbar: "navbar",
    navitem: "navitem",
    breakline: "breakline",
    sideNavOne: "sidenavone",
    fullHTMLOutline2Side: "fulllhtmloutline2side"
}