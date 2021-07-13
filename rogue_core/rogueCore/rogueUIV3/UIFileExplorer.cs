using rogueCore.hqlSyntaxV3;
using rogueCore.hqlSyntaxV3.filledSegments;
using rogueCore.rogueUIV3;
using rogueCore.rogueUIV3.web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rogueCore.rogueUIV3
{
    class UIFileExplorer
    {
        public IMultiRogueRow topDiv { get; private set; }
        string clickEventNm { get; set; }
        static int id = 1;
        public UIFileExplorer(string path, string clickEvent, IMultiRogueRow divRow)
        {
            topDiv = divRow;
            this.clickEventNm = clickEvent;
            var topTree = IntellsenseDecor.ManualUIElementRow(divRow, Elements.treeview, UISection.ParentRelationships.child);
            var rootNode = GetNode(topTree, new DirectoryInfo(path).Name, path, "bundle", FolderNode);
            foreach (var dirInfo in new DirectoryInfo(path.Trim()).GetDirectories())
            {
                DirectoryLoop(dirInfo, rootNode);
            }
        }
        void DirectoryLoop(DirectoryInfo thsDir, IMultiRogueRow topNode)
        {
            var treNode = GetNode(topNode,thsDir.Name, thsDir.FullName,"bundle", FolderNode);
            foreach (var thsFile in thsDir.GetFiles())
            {
                GetNode(treNode, thsFile.Name, thsFile.FullName, "data_table", FileNode);
            }
            foreach (var dirInfo in thsDir.GetDirectories())
            {
                DirectoryLoop(dirInfo, treNode);
            }
        }
        IMultiRogueRow GetNode(IMultiRogueRow parent, string txt,string path,  string imgName, Func<IMultiRogueRow,string, IMultiRogueRow> StartNode)
        {
            var treNode = StartNode(parent, path);
            var img = IntellsenseDecor.ManualUIElementRow(treNode, Elements.image, UISection.ParentRelationships.header);
            IntellsenseDecor.ManualUIAttributeRow(img, Attributes.heightpixels, "25");
            IntellsenseDecor.ManualUIAttributeRow(img, Attributes.widthpixels, "25");
            IntellsenseDecor.ManualUIAttributeRow(img, Attributes.imagepath, imgName);
            var lbl = IntellsenseDecor.ManualUIElementRow(treNode, Elements.label, UISection.ParentRelationships.header);
            IntellsenseDecor.ManualUIAttributeRow(lbl, Attributes.text, txt);
            return treNode;
        }
        IMultiRogueRow FileNode(IMultiRogueRow parent, string path)
        {
            var treNode = IntellsenseDecor.ManualUIElementRow(parent, Elements.emptytreeviewnode, UISection.ParentRelationships.child);
            string finalClick = clickEventNm + ";FILE;" + path;
            IntellsenseDecor.ManualUIAttributeRow(treNode, Attributes.mousedoubleclick, finalClick);
            return treNode;
        }
        IMultiRogueRow FolderNode(IMultiRogueRow parent, string path)
        {
            var treNode = IntellsenseDecor.ManualUIElementRow(parent, Elements.treeviewnode, UISection.ParentRelationships.child);
            string finalClick = clickEventNm + ";FOLDER;" + path;
            IntellsenseDecor.ManualUIAttributeRow(treNode, Attributes.mousedoubleclick, finalClick);
            var addFolderDiv = IntellsenseDecor.ManualUIElementRow(treNode, Elements.groupbox, UISection.ParentRelationships.child);
            var addFolderBox = IntellsenseDecor.ManualUIElementRow(addFolderDiv, Elements.textbox);
            string txtID = "FOLDERNAME_TXT_" + id.ToString();
            IntellsenseDecor.ManualUIAttributeRow(addFolderBox, Attributes.idname, txtID);           
            var btnAddFolder = IntellsenseDecor.ManualUIElementRow(addFolderDiv, Elements.button);
            IntellsenseDecor.ManualUIAttributeRow(btnAddFolder, Attributes.text, "+");
            IntellsenseDecor.ManualUIAttributeRow(btnAddFolder, Attributes.mouseclick, "AddDirectoryClick;FOLDER;" + path + ";' + document.getElementById('" + txtID + "').value + '");
            id++;
            return treNode;
        }
    }
}
