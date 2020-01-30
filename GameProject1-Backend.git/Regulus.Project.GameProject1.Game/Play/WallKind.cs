using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class WallKind
    {
        private readonly ENTITY _Room;

        private readonly ENTITY _Aisle;

        public WallKind(ENTITY room, ENTITY aisle)
        {
            this._Room = room;
            this._Aisle = aisle;            
        }

        public ENTITY Get(bool room)
        {
            if(room)
                return _Room;
            return _Aisle;
        }
    }
}