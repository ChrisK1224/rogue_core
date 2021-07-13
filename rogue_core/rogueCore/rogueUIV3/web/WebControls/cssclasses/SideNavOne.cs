using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.rogueUIV3.web.element
{
    class SideNavOne : WebBaseControl
    {
        public override string elementNM { get { return Elements.sideNavOne; } }
        protected override string uiText { get { return @"<style>
.SideNavOne {
  position: fixed;
  width: 220px;
  height: 100%;
  left: 0;
  overflow-x: auto;
  overflow-y: auto;
  background: #1a1a1a;
  color: #fff;
  
}

.content-container {
  padding-top: 20px;
}

.SideNavOne-logo {
  padding: 10px 15px 10px 30px;
  font-size: 20px;
  background-color: #2574A9;
}

.SideNavOne-navigation {
  padding: 0;
  margin: 0;
  list-style-type: none;
  position: relative;
}

.SideNavOne-navigation li {
  background-color: transparent;
  position: relative;
  display: inline-block;
  width: 100%;
  line-height: 20px;
}

.SideNavOne-navigation li a {
  padding: 10px 15px 10px 30px;
  display: block;
  color: #fff;
}

.SideNavOne-navigation li .fa {
  margin-right: 10px;
}

.SideNavOne-navigation li a:active,
.SideNavOne-navigation li a:hover,
.SideNavOne-navigation li a:focus {
  text-decoration: none;
  outline: none;
}

.SideNavOne-navigation li::before {
  background-color: #2574A9;
  position: absolute;
  content: '';
  height: 100%;
  left: 0;
  top: 0;
  -webkit-transition: width 0.2s ease-in;
  transition: width 0.2s ease-in;
  width: 3px;
  z-index: -1;
}

.SideNavOne-navigation li:hover::before {
  width: 100%;
}

.SideNavOne-navigation .header {
  font-size: 12px;
  text-transform: uppercase;
  background-color: #151515;
  padding: 10px 15px 10px 30px;
}

.SideNavOne-navigation .header::before {
  background-color: transparent;
}

.content-container {
  padding-left: 220px;
}
</style>"; } }
        public override string endTag { get { return ""; } }
    }
}
