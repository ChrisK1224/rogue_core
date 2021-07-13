namespace rogue_core.rogueCore.id.rogueID.ioRecord
{
    public class DataTypeID : IORecordID
    {
        public DataTypeID(int id) : base(id){ }
        public static implicit operator int(DataTypeID id)
        {
            return id._intVal;
        }
        public static implicit operator DataTypeID(int id)
        {
            return new DataTypeID(id);
        }
    }
}