using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.hqlSyntaxV3.row;
using rogueCore.hqlSyntaxV3.segments;
using rogueCore.rogueUIV3.web;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static rogueCore.rogueUIV3.UISection;

namespace rogueCore.hqlSyntaxV3
{
    public static class IntellsenseDecor
    {
        public static IMultiRogueRow ManualUIAttributeRow(IMultiRogueRow parentRow, string attributeType, string attributeValue)
        {
            ManualMultiRogueRow row = new ManualMultiRogueRow(parentRow);
            row.Add("PARENTRELATION", "attribute");
            row.Add("ATTRIBUTEVALUE", attributeValue);
            row.Add("ATTRIBUTETYPE", attributeType);
            return row;
        }
        public static IMultiRogueRow ManualUIElementRow(IMultiRogueRow parentRow, string controlName, ParentRelationships relType = ParentRelationships.child)
        {
            ManualMultiRogueRow row = new ManualMultiRogueRow(parentRow);
            row.Add("PARENTRELATION", relType.ToString());
            row.Add("CONTROLNAME", controlName);
            return row;
        }
        public static IMultiRogueRow MyTextbox(IMultiRogueRow parentRow, string txt, string id)
        {
            IMultiRogueRow lblRow = ManualUIElementRow(parentRow, Elements.textbox);
            ManualUIAttributeRow(lblRow, Attributes.text, txt);
            ManualUIAttributeRow(lblRow, Attributes.idname, id);
            ManualUIAttributeRow(lblRow, Attributes.cssclass,"form-control");
            return lblRow;
        }
        public static IMultiRogueRow MyTable(IMultiRogueRow parentRow)
        {
            IMultiRogueRow lblRow = ManualUIElementRow(parentRow, Elements.table);
            ManualUIAttributeRow(lblRow, Attributes.cssclass, "tbl tbl-striped");
            return lblRow;
        }
        public static IMultiRogueRow MyTableRow(IMultiRogueRow parentRow)
        {
            IMultiRogueRow lblRow = ManualUIElementRow(parentRow, Elements.tablerow);
            return lblRow;
        }
        public static IMultiRogueRow MyTableCell(IMultiRogueRow parentRow)
        {
            IMultiRogueRow lblRow = ManualUIElementRow(parentRow, Elements.tablecell);
            return lblRow;
        }
        public static IMultiRogueRow MyLabel(IMultiRogueRow parentRow, string txt, MyColors myColor = MyColors.black, Boldness boldness = Boldness.none, FontSize fontSize = FontSize.regular, Underline isUnderlined = Underline.none)
        {
            IMultiRogueRow lblRow = ManualUIElementRow(parentRow, Elements.label);
            ManualUIAttributeRow(lblRow, Attributes.text, txt);
            if(myColor != MyColors.black) { ManualUIAttributeRow(lblRow, Attributes.fontcolor, myColor.GetStringValue()); }
            if (fontSize != FontSize.regular) { ManualUIAttributeRow(lblRow, Attributes.fontsize, fontSize.GetStringValue()); }
            if(boldness != Boldness.none) { ManualUIAttributeRow(lblRow, Attributes.fontweight, boldness.GetStringValue()); }
            if (isUnderlined != Underline.none) { ManualUIAttributeRow(lblRow, Attributes.underline, ""); }
            return lblRow;
        }
        public static IMultiRogueRow IndentedGroupbox(IMultiRogueRow parentRow, int levelNum)
        {
            MyLabel(parentRow, "&nbsp;");
            var divRow = ManualUIElementRow(parentRow, Elements.groupbox);
            ManualUIAttributeRow(divRow, Attributes.marginleft, "25");
            ManualUIAttributeRow(divRow, Attributes.margintop, "15");            
            return divRow;
        }
        public static IMultiRogueRow Groupbox(IMultiRogueRow parentRow)
        {
            var divRow = ManualUIElementRow(parentRow, Elements.groupbox);
            return divRow;
        }
        public static void BreakLine(IMultiRogueRow parentRow)
        {
            MyLabel(parentRow, "&nbsp;");
            ManualUIElementRow(parentRow, Elements.breakline, ParentRelationships.child);
        }
        public enum Boldness : int
        {
            [StringValue("bold")] bold = 1,
            [StringValue("bolder")] bolder = 2,
            [StringValue("none")] none = 3
        }
        public enum MyColors : int
        {
            [StringValue("red")] red = 1,
            [StringValue("blue")] blue = 2,
            [StringValue("black")] black  = 3,
            [StringValue("green")] green = 4,
            [StringValue("yellow")] yellow = 5,
            [StringValue("orange")] orange = 6
        }
        public enum Underline
        {
            none, underline
        }
        public enum FontSize : int
        {
            [StringValue("medium")] regular = 1,
            [StringValue("small")] small = 2,
            [StringValue("large")] large = 3,
            [StringValue("x-large")] xLarge = 4,
            [StringValue("xx-large")] xxLlarge = 5
        }
    }
    /// <summary>
    /// Will get the string value for a given enums value, this will
    /// only work if you assign the StringValue attribute to
    /// the items in your enum.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>

    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion
       
    }
    public static class extensions
    {
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
    public interface InCodeLabel : IMultiRogueRow
    {

    }
}
