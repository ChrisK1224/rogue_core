using System;
using System.Collections.Generic;

namespace rogue_core.rogueCore.api
{
    internal class APIRules
    {
        List<string> ignoreContainers;
        List<string> validContainers;
        Func<string, bool> CheckValids;
        Func<string, bool> CheckIgnores;
        string colNamesContainer;
        internal APIRules(string colNamesContainer, List<string> validContainers = null, List<string> ignoreContainers = null){
           this.colNamesContainer = colNamesContainer;
           this.validContainers = validContainers;
           this.ignoreContainers = ignoreContainers;
        }
        internal APIRules(List<string> validContainers = null, List<string> ignoreContainers = null){
           this.validContainers = validContainers;
           this.ignoreContainers = ignoreContainers;
        }
        public bool ValidContainer(string containerName){
            return CheckValids(containerName) && CheckIgnores(containerName) ? true : false;
        }
        void LoadMethods(){
            if(validContainers != null){
                CheckValids = CheckValidContainers;
            }else{
                CheckValids = NoFilter;
            }
            if(ignoreContainers != null){
                CheckIgnores = CheckIgnoreContainers;
            }else{
                CheckIgnores = NoFilter;
            }
        } 
        bool NoFilter(string name){
            return true;
        }
        bool CheckValidContainers(string name){
            return validContainers.Contains(name) ? true : false;
        }
        bool CheckIgnoreContainers(string name){
            return !ignoreContainers.Contains(name) ? true : false;
        }
    }
}