using System;
using System.Collections.Generic;
using System.Text;
using static rogueCore.rogueUIV3.UISection;

namespace rogueCore.rogueUIV3
{
    public interface IUIElement
    {
        ParentRelationships parentRelation { get; set; }
        string elementNM { get; }
    }
    public abstract class IUIAttribute : IUIElement
    {
        public String Value { get; protected set; }
        public UISection.ParentRelationships parentRelation { get; set; }
        public abstract string elementNM { get; }
        protected String ColorTranslation(String colorName)
        {
            switch (colorName)
            {
                case "BACKGROUNDMAIN":
                    return "#97CAEF";
                case "SECONDBACKGROUND":
                    return "#CAFAFE";
            }
            return colorName;
        }
    }
    public interface IUIControl : IUIElement
    {
        IUIElement SetChildContent(IUIElement thsElement);
        void SetHeader(IUIElement thsHeader);        
    }
}
