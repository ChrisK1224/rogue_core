using FilesAndFolders;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.id;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.location
{
    public abstract class EncodedLocation<IDType> : BaseLocation, IOptionalDirect where IDType : RowID
    { 
        public string name { get; }
        protected IColumn encodedColumn { get;private set; }
        protected const string encodeStartKey = "{";
        const string endKey = "}";
        protected Func<string, IDType> EncodedIDPull;
        protected bool isDirect { get; }
        protected IDType ID { get; private set; }
        public EncodedLocation(string colTxt, QueryMetaData metaData) : base(colTxt, metaData)
        {
            name = (GetAliasName() == "") ? metaData.NextUnnamedColumn() : GetAliasName();
            encodedColumn = BaseColumn.ParseColumn(colTxt.get_string_between_2(encodeStartKey, endKey), metaData);
            //*Set column Table Ref Name if possible. Required if this is a join clause column
            //if (aliasName.isNameSet)
            //{
            //    columnName = aliasName.Name.ToUpper();
            //}
            //if (items.Length == 2)
            //{
            //    colTableRefName = items[0];
            //}
            //try
            //{
            //    this.metaData = metaData;
            //    //*Note encode column cannot rely on ID to Column Name for name since will be multiple columns so directID must have alias name set
            //    if (colTableRefName == "")
            //    {
            //        colTableRefName = assumedTblName;
            //    }
            //    colTableRefName = colTableRefName.ToUpper();
            //    int ownerTableID = metaData.GetTableIDByRefName(colTableRefName);
            //    encodedCol.PreFill(metaData, assumedTblName);
            //    metaData.AddUnsetParams(UnsetParams().ToList());
            //    LocalSyntaxParts = StandardSyntaxParts;
            //}
            //catch (Exception ex)
            //{
            //    LocalSyntaxParts = ErrorSyntaxParts;
            //}
        }
        protected abstract IDType NameToID(string ids);
        protected abstract IDType DirectToID(string directID);
        protected void ResetEncodedID(string ColOrID)
        {
            bool isDirect = (this.IsDirectID(ColOrID));
            EncodedIDPull = (isDirect) ? EncodedIDPull = DirectToID : NameToID;
            ID = EncodedIDPull(ColOrID);
        }       
    }
}
