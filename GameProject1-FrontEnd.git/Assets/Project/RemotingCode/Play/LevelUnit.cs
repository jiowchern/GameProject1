using Regulus.CustomType;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class LevelUnit
    {
        public readonly Data.LEVEL_UNIT Type;

        public readonly Vector2 Center;

        public readonly float Direction;

        public LevelUnit(Data.LEVEL_UNIT type, Vector2 center, float direction)
        {
            Type = type;
            Center = center;
            Direction = direction;
        }
    }
}