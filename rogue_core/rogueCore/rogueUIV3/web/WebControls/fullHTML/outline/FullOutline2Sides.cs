using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class FullHTMLOutline2Sides : WebBaseControl
    {
        public override string elementNM { get { return Elements.fullHTMLOutline2Side; } }
        protected override String uiText { get { return @"style>
.leftnav {
  
  z-index: 1;
  top: 150px;
  left: 10px;
  overflow-x: scroll;
  padding: 8px 0;
}
.rightnav {
  
  z-index: 1;
  top: 150px;
  right: 10px;
  overflow-x: scroll;
  padding: 8px 0;
}
</style>
<div style=""display:table;width:100%;"">
  <div style=""display:table-row;height:125px;"">
    <div style=""display:table-cell;"" id=""@PAGEHEADER"" Name=""@PAGEHEADER"">
      
    </div>
  </div>
  <div style=""display:table-row;"">
    <div style=""display:table-cell;"">
      <div style=""display:table;"">
        <div style=""display:table-row;"">
          <div class=""leftnav"" style=""display:table-cell;width:15%;"" id=""@SIDEMENU"" Name=""@SIDEMENU""></div>

          <div style=""display:table-cell;width:70%"">
            <div style=""height:100%;width:100%;display:table;"">
              <div style=""display:table-row;"">
                <div style=""display:table-cell;width:100%;"">
                  <div id=""@BODYHEADER"" Name=""@BODYHEADER""></div>
                </div>
              </div>
              <div style=""display:table-row;"">
                <div style=""display:table-cell;width:100%;"">
                  <div id=""@BODYSECTION1"" Name=""@BODYSECTION1""></div>
                </div>
              </div>
              <div style=""display:table-row;"">
                <div style=""display:table-cell;width:100%;"">
                  <div id=""@BODYSECTION2"" Name=""@BODYSECTION2""></div>
                </div>
              </div>
              <div style=""display:table-row;"">
                <div style=""display:table-cell;width:100%;"">
                  <div id=""@BODYSECTION3"" Name=""@BODYSECTION3""></div>
                </div>
              </div>
            </div>
          </div>
          <div id=""@RIGHTSIDEMENU"" NAME=""@RIGHTSIDEMENU"" class=""rightnav"" style=""display:table-cell;width:15%;"">

          </div>
        </div>
      </div>
    </div>
  </div>
</div"; } }
        public override string endTag { get { return ""; } }
    }
}
