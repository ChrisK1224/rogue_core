
namespace rogueCore.rogueUIV3.web.element
{
    public class OverflowScrollX : UIWebAttribute, IStyleAttribute
    {
        public OverflowScrollX(string overflowScrollX) { Value = overflowScrollX; }
        public override string uiText { get { return "overflow-x"; } }
        public override string elementNM { get { return Attributes.scrollx; } }
    }
}