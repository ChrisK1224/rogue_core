using rogue_core.rogueCore.hqlSyntaxV3.segments;
using rogue_core.rogueCore.id;
using FilesAndFolders;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV3.segments.select;
using System;
using System.Collections.Generic;
using System.Text;

namespace rogueCore.hqlSyntaxV3.segments.locationColumn.columnTypes.idableLocation.encoded.table
{
    abstract class EncodedBase<IDType> : IDableLocation<IDType> where IDType : RowID
    {
        protected ILocationColumn encodedCol { get; private set; }
        protected Func<string, IDType> EncodedIDPull;
        public EncodedBase(string colTxt) : base(RemoveEncoderSymbol(colTxt))
        {
            encodedCol = BaseLocation.LocationType(colTxt.SplitBetween("{","}"));
            EncodedIDPull = (isDirectID) ? EncodedIDPull = EncodedDirectPull : EncodedNonDirectPull;                      
        }
        protected abstract IDType NameToID(string[] ids);
        protected abstract IDType DirectToID(string directID);
        IDType EncodedDirectPull(string decodedVal)
        {
            return DirectToID(decodedVal);
        }
        IDType EncodedNonDirectPull(string decodedVal)
        {
            return NameToID(decodedVal.Split('.'));
            ////int parentID = 0;
            //string[] tableAndCol = decodedVal.Split(".");
            //int parentID = refTbls.TryGetID(upperName);
            //return new ColumnRowID(HQLEncoder.GetColumnIDByNameAndOwnerID(decodedVal, parentID));
        }
        protected void ResetEncodedID(IDType id)
        {
            ID = id;
        }
        static string RemoveEncoderSymbol(string colTxt)
        {
            return colTxt.Replace("{", "").Replace("}", "");
        }
    }
}