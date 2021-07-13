using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rogue_core.rogueCore.binary.rogueTypes.simple
{
    public class SimpleRefInstance : ValueRefInstance
    {
        public SimpleRefInstance() : base(install.RootVariables.sharedDataPath + Path.DirectorySeparatorChar + "SimpleString_Instance.rogueInstance")
        {

        }
    }
}
