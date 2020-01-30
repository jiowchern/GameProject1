using System.Collections.Generic;

using Regulus.Project.GameProject1.Game.Data;

namespace Regulus.Project.GameProject1.Data
{
    public struct MazeUnitInfomation
    {
        public LEVEL_UNIT Type;

        public string Name;
    }
    public struct MazeInfomation
    {
        public int Dimension;
        public int Width;
        public int Height;

        public MazeUnitInfomation[] MazeUnits;
    }
    public class RealmInfomation
    {        
        public string Name;
        public MazeInfomation Maze;
        public TownInfomation Town;

        public Dictionary<ENTITY, int> EntityEnteranceResource { get; set; }

        public Dictionary<ENTITY, int> EntityFieldResource { get; set; }

        public bool HaveTown()
        {
            return string.IsNullOrEmpty(Town.Name) == false;
        }
    }

    public struct TownInfomation 
    {
        public string Name;
    }
}