﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using static rogueCore.rogueUI.UISection;

//namespace rogueCore.rogueUI
//{
//    public interface IUIElement
//    {
//        ParentRelationships parentRelation { get; set; }
//    }
//    public abstract class IUIAttribute : IUIElement
//    {
//        public String Value { get; protected set; }
//        public UISection.ParentRelationships parentRelation { get; set; }

//        protected String ColorTranslation(String colorName)
//        {
//            switch (colorName)
//            {
//                case "BACKGROUNDMAIN":
//                    return "#97CAEF";
//                case "SECONDBACKGROUND":
//                    return "#CAFAFE";
//            }
//            return "";
//        }
//    }
//    public interface IUIControl : IUIElement
//    {
//        IUIElement SetChildContent(IUIElement thsElement);
//        void SetHeader(IUIElement thsHeader);
//    }
//}
