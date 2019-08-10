using System;
using System.Linq;
using System.Collections.Generic;

using Regulus.BehaviourTree;
using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class ChestExcludeAction 
    {
        private readonly Entity _Exit;

        private readonly Entity _Door;

        private readonly Entity _Owner;

        

        private readonly IMapGate _Gate;

        private readonly IMapFinder _Finder;

        private float _Interval;

        readonly List<Guid> _Contestants ;

        public ChestExcludeAction(IMapGate gate , IMapFinder finder, Entity owner, Entity door, Entity exit)
        {
            _Contestants = new List<Guid>();
            _Gate = gate;
            _Finder = finder;

            _Owner = owner;
            _Door = door;
            _Exit = exit;
        }

        

        public TICKRESULT Tick(float delta)
        {
            _Interval += delta;

            if (_Interval > 1)
            {
                _Interval = 0;
                
                var entitys = _Finder.Find(_Owner.GetView());                

                var races = from e in entitys
                            where EntityData.IsActor(e.EntityType) && e.EntityType != ENTITY.ACTOR2
                            group e by e.EntityType;

                var aliveCount = (from r in races where r.Any() select r).Count();

                if (aliveCount <= 2)
                {
                    
                    return TICKRESULT.SUCCESS;
                }
            }

            return TICKRESULT.RUNNING;
        }

        private void _Kick(IEnumerable<IIndividual> onlookers)
        {
            var position = _Exit.GetPosition();
            foreach (var  o in onlookers)
            {
                o.SetPosition(position.X , position.Y);
            }
        }

        public void Start()
        {
            _Gate.WaitEvent += _OnWaitEntity;
            var entitys = _Finder.Find(_Owner.GetView());
            _Kick((from e in entitys where e.EntityType == ENTITY.ACTOR1 select e).Skip(3).ToArray());

            _Gate.Join(_Door);
            
            _Contestants.Add(_Gate.Spawn(ENTITY.ACTOR3));
            _Contestants.Add(_Gate.Spawn(ENTITY.ACTOR4));
            _Contestants.Add(_Gate.Spawn(ENTITY.ACTOR5));
        }

        private void _OnWaitEntity(Guid id)
        {
            if (_Contestants.Any(contestant => contestant == id))
            {
                _Gate.Pass(_Owner.GetPosition() , id);
            }
        }

        public void End()
        {
            _Gate.Left(_Door);

            foreach (var contestant in _Contestants)
            {
                _Gate.Exit(contestant);
            }
            _Gate.WaitEvent -= _OnWaitEntity;
        }


    }
}
