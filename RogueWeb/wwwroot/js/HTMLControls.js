class BaseControl {
    constructor(uiText, endTag) {
        this.uiText = uiText;
        this.endTag = endTag;
        this.styleAtts = [];
        this.classAtts = [];
        this.standardAtts = [];
        this.childElements = [];
        this.headerElements = [];
        this.contentAtt = '';
    }
    AddAtt(att) {
        //alert('testing:' + att.aType);
        switch (att.aType) {
            case ATypes.CSSSTYLE:
                //alert('found att style:' + att.uiText);
                this.styleAtts.push(att);
                break;
            case ATypes.STANDALONE:
                //alert('found att alone:' + att.uiText);
                this.standardAtts.push(att);
                break;
            case ATypes.CSSCLASS:
                this.classAtts.push(att);
                break;
            case ATypes.CONTENT:
                this.contentAtt = att.value;
                break;
        }
    }
    BaseStartText() {
        let htm = '<' + this.uiText;        
        htm += this.CreateListText(this.styleAtts, ' style="', '"', ':', ';');      
        //htm += this.CreateListText(this.standardAtts, '', '', '', '');   
        for (let i = 0; i < this.standardAtts.length; i++) {
            htm += this.standardAtts[i].uiText;
        }
        htm += this.CreateListText(this.classAtts, ' class="', '"', ' ', ' ');     
        htm += '>';
        htm += this.contentAtt;
      
        //alert('start htm final: ' + htm);
        return htm;
        //if (styleAtts.length > 0) {
        //    htm += ' style="';
        //    for (i = 0; i < styleAtts.length; i++) {
        //        html += styleAtts[i].uiText + ':' + styleAtts[i].Value() + ';';
        //    } 
        //    htm += '"';
        //}
        //if (standardAtts.length > 0) {
        //    htm += ' style="';
        //    for (i = 0; i < styleAtts.length; i++) {
        //        html += styleAtts[i].uiText + ':' + styleAtts[i].Value() + ';';
        //    }
        //    htm += '"';
        //}
        //if (classAtts.length > 0) {
        //    htm += ' style="';
        //    for (i = 0; i < styleAtts.length; i++) {
        //        html += styleAtts[i].uiText + ':' + styleAtts[i].Value() + ';';
        //    }
        //    htm += '"';
        //}
        
    }
    CreateListText(arr, startTxt, endTxt, sep, close) {
        let htm = '';
        //alert('in creat list: ' + arr);
        if (arr.length > 0) {
            htm = startTxt;
            for (let i = 0; i < arr.length; i++) {
                //alert('uitext' + arr[i].uiText);
                //alert('sep' + sep);
                //alert('val' + arr[i].Value());
                //alert('close' + close);
                htm += arr[i].uiText + sep + arr[i].Value() + close;
                //alert(arr[i].uiText + sep + arr[i].Value() + close);
            }
            htm += endTxt;
        }
        return htm;
    }
}
class NullElement extends BaseControl {    
      
    constructor() {
        super('null', '</null>');
    }
    StartText() {
       return super.BaseStartText();
    }
}
class Anchor extends BaseControl
{
    constructor() {
        super('a ', '</a>');
    } 
    StartText() {
        return super.BaseStartText();
    }
}
class CSSClass extends BaseControl
{
    constructor() {
        super('style ', '</style>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RBreakline extends BaseControl
{
    constructor() {
        super('br / ', '');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RButton extends BaseControl
{
    constructor() {
        super('button ', '</button>');
    }
    StartText() {
       //alert('btn:  ' + super.BaseStartText());
        return super.BaseStartText();
    }
}
class RDataRowGroup extends BaseControl
{
    constructor() {
        super('tbody ', '</tbody>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RDisplayCell extends BaseControl
{
    constructor() {
        super('div ', '</div>');
        this.styleAttributes.Add(new CssDisplayType("table-cell"));
    }
}
class RDisplayRow extends BaseControl
{
    constructor() {
        super('div ', '</div>');
        styleAttributes.Add(new CssDisplayType("table-row"));
    }
}
class RDisplayTable extends BaseControl
{
    constructor()
    {
        super('div ', '</div>');
        styleAttributes.Add(new HeightPercent("100"));
        styleAttributes.Add(new WidthPercent("100"));
        styleAttributes.Add(new CssDisplayType("table"));
    }
}
class RDropDownList extends BaseControl
{
    constructor() {
        super('select ', '</select>');
    }
}
class REmptyTreeViewNode extends BaseControl
{
        //public List<IUIControl> Header { get; set; }
        //public void Text(String text)
        //{
        //    this.attributes.Add(new Text(text));
        //}
        //public void IDName(String idName) { this.attributes.Add(new NameID(idName)); }
        //public void MouseClick(String mouseClickEvent) { this.attributes.Add(new MouseClick(mouseClickEvent)); }
        //public void MouseDoubleClick(String mouseDoubleEvent) { this.attributes.Add(new MouseDoubleClick(mouseDoubleEvent)); }
        //public void SetHeader(IUIControl thsHeader) { Header.Add(thsHeader); }
    constructor() {
        super('li ', '</li>');
    }
    StartText()
    {
        let buildText = "<" + this.uiText;
        for (let i = 0; i < this.standardAtts.length; i++) {
            buildText += this.standardAtts[i].uiText;
        }
        buildText += " >";
        for (let i = 0; i < this.headerElements.length; i++) {
            buildText += this.headerElements[i].StartText() + this.headerElements[i].endTag;
        }
       // alert(buildText);
        return buildText;
    }
}
class RGroupBox extends BaseControl
{
    constructor() {
        super('div ', '</div>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RHeaderCell extends BaseControl
{
    constructor() {
        super('th ', '</th>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class FileUpload extends BaseControl {
    constructor() {
        //alert('infileupload');
        super('input id="files" name="files" type="file" size="1" multiple onchange="uploadFiles(\'files\') ', '');
    }
    StartText() {
        let html = '<input id="files" name="files" type="file" size="1" multiple onchange="uploadFiles(\'files\')" />';
        return html;
    }
}
class RHeaderGroup extends BaseControl
{
    constructor() {
        super('thead ', '</thead>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RImage extends BaseControl
{
    constructor() {
        super('img ', '</img>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RLabel extends BaseControl
{
    constructor() {
        super('label ', '</label>');
    }
    StartText() {
        
        return super.BaseStartText();
    }
}
class RListItem extends BaseControl
{
    constructor() {
        super('option ', '</option>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RListView extends BaseControl
{
    constructor() {
        super('ul ', '</ul>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RListViewItem extends BaseControl
{
    constructor() {
        super('li ', '</li>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RNavBar extends BaseControl
{
    constructor() {
        super('nav class="navbar navbar-expand-sm bg-dark navbar-dark"> <a class="navbar-brand" href="#">HQL</a>  <ul class="navbar-nav"', '</ul></nav>' );
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RNavItem extends BaseControl
{
    constructor() {
        super('li class="nav-item"><a class="nav-link" href="#"', '</a></li>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RRadioList extends BaseControl
{
    constructor() {
        super('input type="radio" ', '');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RTable extends BaseControl
{
    constructor() {
        super('table ', '</table>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RTableCell extends BaseControl
{
    constructor() {
        super('td ', '</td>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RTableRow extends BaseControl
{
    constructor() {
        super('tr ', '</tr>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RTextArea extends BaseControl
{
    constructor() {
        super('textarea ', '</textarea>' );
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RTextBox extends BaseControl
{
    constructor() {
        super('input type="text" ', '');
    }
    StartText() {
        let htm = '<' + this.uiText;
        if (this.styleAtts.length > 0) {
            htm += ' style="';
            for (let i = 0; i < this.styleAtts.length; i++) {
                html += this.styleAtts[i].uiText + ':' + this.styleAtts[i].Value() + ';';
            } 
            htm += '"';
        }
        for (let i = 0; i < this.standardAtts.length; i++) {
            htm += this.standardAtts[i].uiText;
        }
        //htm += '"';
        if (this.classAtts.length > 0) {
            htm += ' class="';
            for (let i = 0; i < this.classAtts.length; i++) {
                htm += this.classAtts[i].Value();
            }
            htm += '"';
        }
        if (typeof this.ContentAttribute !== "undefined") {
            htm += " value=\"" + this.ContentAttribute.Value() + "\"";
        }
        htm += " >";
        //alert(htm);
        return htm;
    }
}
class RTreeView extends BaseControl
{
    constructor() {
        super('ul id="myUL" ', '</ul>');
    }
    StartText() {
        return super.BaseStartText();
    }
}
class RTreeViewNode extends BaseControl
{
    constructor() {
        super('li ', '</ul></li>');
    }
    StartText()
    {
        let buildText = '<' + this.uiText;
        for (let i = 0; i < this.standardAtts.length; i++) {
            buildText += this.standardAtts[i].uiText;
        }
        buildText += '>';
        buildText += '<span class="caret">';
        for (let i = 0; i < this.headerElements.length; i++) {
            buildText += this.headerElements[i].StartText() + this.headerElements[i].endTag;
        }
        buildText += '</span>';
        buildText += '<ul class="nested" >';
        return buildText;
    }
}
class TabSpace extends BaseControl
{
    constructor() {
        super(' &nbsp; ', '');
    }
    StartText() {
        return super.BaseStartText();
    }
}
//public abstract class WebBaseControl : IUIControl
//{
//        public List < UIWebAttribute > attributes { get; set; } = new List<UIWebAttribute>();
//        protected abstract String uiText { get; }
//    internal List < IUIControl > childControls { get; set; } = new List<IUIControl>();
//        public abstract String endTag { get; }
//        public ParentRelationships parentRelation { get; set; }
//        protected UIWebAttribute ContentAttribute = null;
//        protected List < UIWebAttribute > styleAttributes = new List<UIWebAttribute>();
//        protected List < UIWebAttribute > classAttributes = new List<UIWebAttribute>();
//    internal List < IUIControl > Header { get; set; } = new List<IUIControl>();
//        public void SetHeader(IUIElement thsHeader) { Header.Add((IUIControl)thsHeader); }
//    internal void SetAttribute(UIWebAttribute thsAtt)
//    {
//        if (thsAtt is IStyleAttribute)
//        {
//            styleAttributes.Add(thsAtt);
//        } 
//            else if (thsAtt is CssClass)
//        {
//            classAttributes.Add(thsAtt);
//        } 
//            else if (thsAtt is Text)
//        {
//            ContentAttribute = thsAtt;
//        } 
//            else
//        {
//            attributes.Add(thsAtt);
//        }
//    }
//    internal virtual String StartText()
//    {
//        String buildText = "<" + uiText;
//        foreach(UIWebAttribute thsAtt in attributes)
//        {
//            buildText += thsAtt.uiText;
//        }
//        if (styleAttributes.Count > 0) {
//            buildText += " style=\"";
//            foreach(UIWebAttribute thsAtt in styleAttributes)
//            {
//                buildText += thsAtt.uiText + ":" + thsAtt.Value + ";";
//            }
//            buildText += "\"";
//        }
//        if (classAttributes.Count > 0) {
//            buildText += " class=\"";
//            foreach(UIWebAttribute thsAtt in classAttributes)
//            {
//                buildText += thsAtt.Value + " ";
//            }
//            buildText += "\"";
//        }
//        if (ContentAttribute != null) {
//            buildText += ">" + ContentAttribute.Value;
//        }
//        else {
//            buildText += ">";
//        }
//        return buildText;
//    }
    //    public IUIElement SetChildContent(IUIElement thsElement)
    //{
    //    if (thsElement is UIWebAttribute)
    //    {
    //        Stopwatch stopwatch = new Stopwatch();
    //        stopwatch.Start();
    //        SetAttribute((UIWebAttribute)thsElement);
    //        stopwatch.Stop();
    //        var tim = stopwatch.ElapsedMilliseconds;
    //    }
    //        else
    //    {
    //        if (thsElement.parentRelation == UISection.ParentRelationships.child) {
    //            childControls.Add((IUIControl)thsElement);
    //        }
    //        else {
    //            SetHeader((IUIControl)thsElement);
    //        }
    //    }
    //    return thsElement;
    //}
//}