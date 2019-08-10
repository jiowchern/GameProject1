using System.Collections;
using System.Collections.Generic;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class Level : IEnumerable<LevelUnit>
    {
        private readonly List<LevelUnit> _Units;

        public Level()
        {
            _Units = new List<LevelUnit>();
        }

        public void Add(LevelUnit level_unit)
        {
            _Units.Add(level_unit);
        }

        IEnumerator<LevelUnit> IEnumerable<LevelUnit>.GetEnumerator()
        {
            return _Units.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Units.GetEnumerator();
        }
    }
}