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
class baseAttribute {
    
    constructor(value) {
        this.value = value;
    }
}
class NullAttribute    {
    
    constructor() {
        this.value = value;
    }
    uiText = '';
}
class BackgroundColor  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return TranslateColor(this.value); }
    uiText = 'background-color';
}
class Border  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'border'; 
}
class BorderBottom  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'border-bottom';
}
class BorderTop  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'border-top';
}
class ColumnSpan  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = this.value;
    }
    Value() { return this.value; }
    //uiText = ' colspan="' + Value + "'";
    get uiText() {
        return ' colspan="' + this.value + "'";
    }
}
class CssClass  
{
    aType = ATypes.CSSCLASS;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = this.value;
}
class CssDisplayType  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'display'; 
}
class DDLChangeEvent  
{
    aType = ATypes.STANDALONE;
    Value() { return this.value; }
    //uiText = 'onchange="GenericClick("' + Value + '")';
    get uiText() {
        return 'onchange="GenericClick(\'' + this.value + '\')';
    }
}
class DDLItemChange  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    //uiText = 'onchange="GenericClick(this.value)"';
    get uiText() {
        return 'onchange="GenericClick(this.value)"';
    }
}
class EditableContent  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    //suiText = ' contenteditable="' + Value + '"'; 
    get uiText() {
        return ' contenteditable="' + this.value + '"'; 
    }
}
class Float  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText ='float'; 
}
class FontColor  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'color';
}
class FontFamily  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'font-family';
}
class FontSize  
 {
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'font-size';
}
class FontStyle  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'font-style';
}
class FontWeight  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'font-weight'; 
}
class HeightPixels  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'height';
}
class HeightPercent  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + '%'; }
    uiText = 'height';
}
class ImagePath  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
    }
    Value() { return ''; }
    //uiText = ' src=/"images/"' + this.value + '.png';
    get uiText() {
        return ' src="/images/' + this.value + '.png"';
    }
}
class IsSelected  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = ' selected '; 
}
class ItemValue  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    //uiText = ' value="' + Value + '" ';     
    get uiText() {
        return ' value="' + this.value + '" ';    ;
    }
}
class Margin  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin'; 
}
class MarginBottom  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-bottom'; 
}
class MarginLeft  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-left'; 
}
class MarginRight  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-right';
}
class MarginTop  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'margin-top'; 
}
class MouseClick  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    //uiText = 'onclick="GenericClick("' + Value + '")"'; 
    get uiText() {
        return 'onclick="GenericClick(\'' + this.value.replaceAll('\\', '\\\\') + '\')"';
    }

}
class MouseClickCustom  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;       
    }
    Value() { return this.value; }
    //uiText = 'onclick="' + value + '"';
    get uiText() {
        return ' onclick="' + this.value.replaceAll('\\', '\\\\') + '" ';
    }
}
class MouseDoubleClick  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
        //alert(this.value);
    }
    Value() { return this.value; }
    //uiText = ' ondblclick="GenericClick("' + this.value + '")';
    get uiText() {
        return ' ondblclick="GenericClick(\'' + this.value.replaceAll('\\','\\\\') + '\')"';
    }
}
class NameID  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
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
class OnTextChange  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = 'onclick="GenericClick("' + value + '")';
    }
    Value() { return this.value; }
    get uiText() {
        return this.value;
    }
    //uiText = 'onclick="GenericClick("' + Value + '")'; 
}
class OverflowScrollX  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'overflow-x'; 
}
class OverflowScrollY  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'overflow-y'; 
}
class RNoWrap  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = 'wrap:off;white-space'; 

}
class RowSpan  
{
    aType = ATypes.STANDALONE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = ' rowspan="' + Value + '"'; 
}
class PaddingLeft  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'padding-left'; 
}
class Text  
{   
    aType = ATypes.CONTENT;    
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    //uiText = ' text="' + value.replace('"', '&quot;');
    get uiText() {
        return this.value;
    }
    //uiText = ' text="' + this.value;
}
class TextAlign  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value; }
    uiText = "text-align"; 
}

class Underline  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return 'underline'; }
    uiText = 'text-decoration';
}
class WidthPixels  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + 'px'; }
    uiText = 'width'; 
}
class WidthPercent  
{
    aType = ATypes.CSSSTYLE;
    constructor(value) {
        this.value = value;
    }
    Value() { return this.value + '%'; }
    uiText = 'width'; 
}