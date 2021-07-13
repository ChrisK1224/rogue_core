using rogue_core.rogueCore.id;
using System;

namespace rogue_core.rogueCore.bundle.presetBundle
{
    public class RootBundle : RogueBundle
    {
        public RootBundle() : base(SystemIDs.IOTableRecords.Bundles.rootBundleID)
        {
            
        }
        //public override String GetFolderPath(){ return install.RootVariables.rootPath;}
    }
}