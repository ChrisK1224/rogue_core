namespace rogue_core.rogueCore.id.rogueID.decodedIDs
{
    public class RogueIDRowID : DecodedRowID
    {
        public RogueIDRowID(int id) : base(id){ }
        public static implicit operator int(RogueIDRowID id)
        {
            return id._intVal;
        }
        public static implicit operator RogueIDRowID(int id)
        {
            return new RogueIDRowID(id);
        }
    }
}