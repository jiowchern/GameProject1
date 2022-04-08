using System.Linq;

using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{

    internal struct MazeCell
    {
        public int Row;
        public int Column;

        
        public Utility.Flag<MAZEWALL> Walls;

        public bool IsRoom()
        {
            return Walls.Count() == 3;
        }

        public Vector2 GetPosition(float width , float height)
        {
            var x = Row * width;
            var y = Column * height;
            return new Vector2(x, y);
        }

        public bool HaveWall()
        {
            return Walls.Any();
        }
        
    }
}