
namespace rogueCore.rogueUIV3.web.element
{
    public class OverflowScrollY : UIWebAttribute, IStyleAttribute
    {
         public OverflowScrollY(string overflowScrollY) { Value = overflowScrollY; }
        public override string uiText { get { return "overflow-y"; } }
        public override string elementNM { get { return Attributes.scrolly; } }
    }
}