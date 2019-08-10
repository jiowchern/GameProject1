using System.Collections.Generic;
using System;

using Regulus.BehaviourTree;
using Regulus.Project.GameProject1.Data;
using System.Linq;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class FieldBehavior : Behavior
    {
        private readonly ENTITY[] _Types;

        private readonly Entity _Owner;

        private readonly IMapGate _Gate;

        private readonly IMapFinder _Finder;

        private Queue<Guid> _Ids;

        public FieldBehavior(ENTITY[] types, Entity owner, IMapGate gate, IMapFinder finder)
        {
            _Ids = new Queue<Guid>();
            _Types = types;
            _Owner = owner;
            _Gate = gate;
            _Finder = finder;
        }

        protected override ITicker _Launch()
        {
            var builder = new Regulus.BehaviourTree.Builder();
            var ticker = builder
                .Sequence()
                    .Action(() => new WaitSecondStrategy(10f), t => t.Tick, t => t.Start, t => t.End)
                    .Selector()
                        .Sequence()
                            .Action(_Pass)
                        .End()
                        .Sequence()
                            .Action(_NeedSpawn)
                            .Action(_Spawn)
                        .End()
                    .End()

                .End().Build();

            return ticker;
        }

        private TICKRESULT _Pass(float arg)
        {
            bool success = false;
            success = _Ids.Count > 0;
            while (_Ids.Count > 0)
            {
                var id = _Ids.Dequeue();
                _Gate.Pass(_Owner.GetPosition(), id);                
            }
            if(success)
                return TICKRESULT.SUCCESS;
            return TICKRESULT.FAILURE;

        }

        protected override void _Update(float delta)
        {
            
        }

        protected override void _Shutdown()
        {
            
        }

        private TICKRESULT _NeedSpawn(float arg)
        {            
            var actors = _Finder.Find(_Owner.GetView());
            var anyActor = (from actor in actors where EntityData.IsActor(actor.EntityType) select actor).Any();
            if (anyActor)
                return TICKRESULT.FAILURE;
            return TICKRESULT.SUCCESS;
        }

        private TICKRESULT _Spawn(float arg)
        {

            var ids = _Gate.SpawnField(_Types);
            if (ids.Length > 0)
            {
                foreach (var id in ids)
                {
                    _Ids.Enqueue(id);
                }
                
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }
    }
}