using FilesAndFolders;
using rogue_core.rogueCore.binary;
using rogue_core.rogueCore.hqlSyntaxV4.level;
using rogue_core.rogueCore.hqlSyntaxV4.limit;
using rogue_core.rogueCore.hqlSyntaxV4.location;
using rogue_core.rogueCore.hqlSyntaxV4.location.column;
using rogue_core.rogueCore.hqlSyntaxV4.location.from;
using rogue_core.rogueCore.hqlSyntaxV4.table;
using rogue_core.rogueCore.hqlSyntaxV4.where;
using rogue_core.rogueCore.id.rogueID;
using rogueCore.hqlSyntaxV4.join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace rogue_core.rogueCore.hqlSyntaxV4.insert
{
    abstract class HQLInsert : BaseLocation, IFrom, IIdableFrom
    {
        public override List<SplitKey> splitKeys { get{ return new List<SplitKey>() { LocationSplitters.AsKey, InsertSplitters.intoKey,InsertSplitters.byKey, CommandSplitters.colSeparator, CommandSplitters.openCommand, CommandSplitters.closeCommand }; } }
        protected List<string> insertStrParams { get; private set; } = new List<string>();
        protected ICalcableFromId tableFrom { get; }
        public string idName { get; }
        //* TODO HQLINsert shouldn't have tableID avaialbe or cant be encoded***
        public IORecordID tableId { get { return ((IIdableFrom)tableFrom).tableId; } }
        public HQLInsert(string qry, QueryMetaData metaData) : base(qry, metaData)
        {
            tableFrom = ((ICalcableFromId)HQLTable.ParseFromClause(splitList.Where(x => x.Key == KeyNames.into).FirstOrDefault(), metaData));            
            if(tableFrom is StandardFrom)
            {
                idName = (GetAliasName() == "") ? ((StandardFrom)tableFrom).idName : GetAliasName();
            }
            else
            {
                idName = (GetAliasName() == "") ? metaData.NextUnnamedColumn() : GetAliasName();
            }
            //hqlInsertType = GetInsertType(splitList.Where(x => x.Key == KeyNames.byKey).FirstOrDefault().Value, metaData);
        }
        public static HQLInsert GetInsertType(string colTxt, QueryMetaData metaData)
        {
            Type parentType = typeof(HQLInsert);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            List<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType)).ToList();
            Dictionary<string, Type> classKeys = new Dictionary<string, Type>();
            subclasses.ForEach(x => classKeys.Add(x.GetProperty("insertType").GetValue(x, null).ToString(), x));
            string parseTxt = colTxt.ToUpper();
            string idName = colTxt.BeforeFirstKey(KeyNames.openParenthensis).AfterLastSpace().ToUpper();
            switch (idName)
            {
                case JsonInsert.codeMatchName:
                    return new JsonInsert(colTxt, metaData);
                default:
                    throw new Exception("Unknown command insert type");
            }
        }
        protected abstract IEnumerable<IReadOnlyRogueRow> Execute(IMultiRogueRow parentRow, ICalcableFromId tableFrom);
        //{
        //    foreach(var row in hqlInsertType.Execute(parentRow, tableFrom))
        //    {
        //        yield return row;
        //    }
        //}    
        public List<string> UnsetParams()
        {
            throw new NotImplementedException();
        }
        public override string PrintDetails()
        {
            return "IdName:" + idName;
        }
        public IEnumerable<IMultiRogueRow> FilterAndStreamRows(ILimit limit, IJoinClause joinClause, IWhereClause whereClause, IHQLLevel parentLvl, Func<string, IReadOnlyRogueRow, IMultiRogueRow, IMultiRogueRow> NewRow)
        {
            int rowCount = 0;
            int snapshotRowAmount = parentLvl.rows.Count;
            var fakeRow = new ManualBinaryRow();
            foreach (IMultiRogueRow parentRow in joinClause.JoinRows(parentLvl, fakeRow, snapshotRowAmount))
            {
                if (whereClause.CheckWhereClause(idName, fakeRow, parentRow))
                {                                     
                    foreach(IReadOnlyRogueRow recordRow in Execute(parentRow, tableFrom))
                    {
                        yield return NewRow(idName, recordRow, parentRow);
                    }
                }
                rowCount++;
            }
        }
    }
}
