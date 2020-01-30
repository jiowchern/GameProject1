using System.Collections.Generic;
using System.Linq;

using Regulus.Framework;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class InorganicsWisdom  : IUpdatable
    {
        private readonly IMapGate _Gate;

        private readonly Entity[] _Entities;
        public InorganicsWisdom(IEnumerable<Entity> inorganics, IMapGate gate)
        {
            _Entities = inorganics.ToArray();
            _Gate = gate;
        }

        void IBootable.Launch()
        {
            foreach (var entity in _Entities)
            {
                _Gate.Join(entity);                
            }
        }

        void IBootable.Shutdown()
        {
            foreach (var entity in _Entities)
            {
                _Gate.Left(entity);
            }
        }

        bool IUpdatable.Update()
        {
            return true;
        }
    }
}