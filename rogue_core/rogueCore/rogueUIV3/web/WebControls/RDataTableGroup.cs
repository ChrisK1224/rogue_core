

namespace rogueCore.rogueUIV3.web.element
{
    class RDataRowGroup : WebBaseControl
    {
        public void HeightPercent(string HeightPercent) { this.attributes.Add(new HeightPercent(HeightPercent)); }
        public void RowSpan(string rowSpan) { this.attributes.Add(new RowSpan(rowSpan)); }
        protected override string uiText { get { return "tbody "; } }
        public override string endTag { get { return "</tbody>"; } }
        public override string elementNM { get { return Elements.datarowgroup; } }
    }
}