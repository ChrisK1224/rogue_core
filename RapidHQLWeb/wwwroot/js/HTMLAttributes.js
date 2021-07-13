const ATypes = {
    CSSSTYLE: 'style',
    STANDALONE: 'standard',
    CSSCLASS: 'class',
    CONTENT: 'content'
}
const ATypes2 = {
    CSSSTYLE: 'style',
    STANDALONE: 'standard'
}

function TranslateColor(colorName) {
        switch (colorName) {
            case 'BACKGROUNDMAIN':
                return '#97CAEF';
            case 'SECONDBACKGROUND':
                return '#CAFAFE';
        }
        return colorName;
}
class BaseAttribute {
    constructor() { }
    isControl = false;
}
class NullAttribute extends BaseAttribute  {
    
    constructor() {
        super();
        this.value = '';
    }
    uiText = '';
}
class BackgroundColor extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return TranslateColor(this.value); }
    uiText = 'background-color';
}
class Border extends BaseAttribute  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'border'; 
}
class BorderBottom extends BaseAttribute  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'border-bottom';
}
class BorderTop extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'border-top';
}
class ColumnSpan extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = this.value;
    }
    Value() { return this.value; }
    //uiText = ' colspan="' + Value + "'";
    get uiText() {
        return ' colspan="' + this.value + "'";
    }
}
class CssClass extends BaseAttribute    
{
    aType = ATypes.CSSCLASS;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = this.value;
}
class CssDisplayType extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'display'; 
}
class DDLChangeEvent extends BaseAttribute    
{
    constructor(value) {
        super();
        this.value = value;
    }
    aType = ATypes.STANDALONE;
    Value() { return this.value; }
    //uiText = 'onchange="GenericClick("' + Value + '")';
    get uiText() {
        return 'onchange="GenericClick(\'' + this.value + '\')';
    }
}
class DDLItemChange extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    //uiText = 'onchange="GenericClick(this.value)"';
    get uiText() {
        return 'onchange="GenericClick(this.value)"';
    }
}
class EditableContent extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    //suiText = ' contenteditable="' + Value + '"'; 
    get uiText() {
        return ' contenteditable="' + this.value + '"'; 
    }
}
class Float extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText ='float'; 
}
class FontColor extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'color';
}
class FontFamily extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'font-family';
}
class FontSize extends BaseAttribute    
 {
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'font-size';
}
class FontStyle extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'font-style';
}
class FontWeight extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'font-weight'; 
}
class HeightPixels extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'height';
}
class HeightPercent extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + '%'; }
    uiText = 'height';
}
class ImagePath extends BaseAttribute   
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return ''; }
    //uiText = ' src=/"images/"' + this.value + '.png';
    get uiText() {
        return ' src="/images/' + this.value + '.png"';
    }
}
class IsSelected extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = ' selected '; 
}
class ItemValue extends BaseAttribute   
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    //uiText = ' value="' + Value + '" ';     
    get uiText() {
        return ' value="' + this.value + '" ';    ;
    }
}
class Margin extends BaseAttribute   
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin'; 
}
class MarginBottom extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-bottom'; 
}
class MarginLeft extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-left'; 
}
class MarginRight extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-right';
}
class MarginTop extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-top'; 
}
class MouseClick extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    //uiText = 'onclick="GenericClick("' + Value + '")"'; 
    get uiText() {
        return 'onclick="GenericClick(\'' + this.value.replaceAll('\\', '\\\\') + '\')"';
    }

}
class MouseClickCustom extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;       
    }
    Value() { return this.value; } 
    //uiText = 'onclick="' + value + '"';
    get uiText() {
        return ' onclick="' + this.value.replaceAll('\\', '\\\\') + '" ';
    }
}
class MouseDoubleClick extends BaseAttribute   
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
        //alert(this.value);
        //alert(this.value);
    }
    Value() { return this.value; }
    //uiText = ' ondblclick="GenericClick("' + this.value + '")';
    get uiText() {
        //alert(' ondblclick="GenericClick(\'' + this.value.replaceAll('\\', '\\\\') + '\')"');
        return ' ondblclick="GenericClick(\'' + this.value.replaceAll('\\','\\\\') + '\')"';
    }
}
class NameID extends BaseAttribute   
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    get uiText()
    {
       //alert(' id="' + this.value + '" Name="' + this.value + '" ');
        return ' id="' + this.value + '" Name="' + this.value + '" ';
    }
   //uiText = ' id="' + this.value + '" Name="' + this.value + '"'; 
}
class OnTextChange extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = 'onclick="GenericClick("' + value + '")';
    }
    Value() { return this.value; }
    get uiText() {
        return this.value;
    }
    //uiText = 'onclick="GenericClick("' + Value + '")'; 
}
class OverflowScrollX extends BaseAttribute   
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'overflow-x'; 
}
class OverflowScrollY extends BaseAttribute   
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'overflow-y'; 
}
class RNoWrap extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'wrap:off;white-space'; 

}
class RowSpan extends BaseAttribute    
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = ' rowspan="' + Value + '"'; 
}
class PaddingLeft extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'padding-left'; 
}
class Text extends BaseAttribute    
{   
    aType = ATypes.CONTENT;    
    constructor(value) {
        super();
        this.value = value.replaceAll('"', '&quot;');
    }
    Value() { return this.value; }
    //uiText = ' text="' + value.replace('"', '&quot;');
    get uiText() {
        return this.value;
    }
    //uiText = ' text="' + this.value;
}
class TextAlign extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value; }
    uiText = "text-align"; 
}

class Underline extends BaseAttribute   
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return 'underline'; }
    uiText = 'text-decoration';
}
class WidthPixels extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'width'; 
}
class WidthPercent extends BaseAttribute    
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        super();
        this.value = value;
    }
    Value() { return this.value + '%'; }
    uiText = 'width'; 
}