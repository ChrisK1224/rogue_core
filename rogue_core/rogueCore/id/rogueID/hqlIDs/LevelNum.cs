using rogue_core.rogueCore.id.rogueID;

namespace rogue_core.rogueCore.queryResults
{
    public class LevelNum :JustAnInt
    {
        public LevelNum(int val) : base(val){}
        public static implicit operator LevelNum(int id)
        {
            return new LevelNum(id);
        }
        public static implicit operator int(LevelNum id)
        {
            return id._intVal;
        }
    }
}