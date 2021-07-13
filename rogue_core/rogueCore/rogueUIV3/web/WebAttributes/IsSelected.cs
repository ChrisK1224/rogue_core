namespace rogueCore.rogueUIV3.web.element
{
    public class IsSelected : UIWebAttribute
    {
        public IsSelected(string isSelected) { Value = isSelected; }
        public override string uiText { get{ return " selected "; } }
        public override string elementNM { get { return Attributes.isselected; } }
    }
}