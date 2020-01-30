using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Regulus.BehaviourTree;
using Regulus.CustomType;
using Regulus.Extension;
using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class StrongholdBehavior : Behavior
    {
        private readonly ENTITY[] _Entitys;

        private readonly IMapFinder _Finder;
        private readonly IMapGate _Gate;

        private readonly IIndividual _Player;

        private Queue<Guid> _Ids;
        public StrongholdBehavior(ENTITY[] entitys, IIndividual player, IMapGate gate, IMapFinder finder)
        {
            _Ids = new Queue<Guid>();
            _Entitys = entitys;
            _Player = player;
            _Gate = gate;
            _Finder = finder;
        }

        protected override ITicker _Launch()
        {

            var builder = new Regulus.BehaviourTree.Builder();
            var ticker = builder
                .Sequence()
                    .Action(()=> new WaitSecondStrategy(1f), t => t.Tick, t => t.Start, t => t.End)                    
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
            if (_Ids.Count > 0)
            {
                var id = _Ids.Dequeue();
                _Gate.Pass(_Player.Position , id);
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;            

        }

        private TICKRESULT _NeedSpawn(float arg)
        {
            IIndividual individual = _Player;
            var actors = _Finder.Find(individual.Bounds);
            var count = (from actor in actors where _Entitys.Any(t => t == actor.EntityType) select actor).Count();
            if(count > 3)
                return TICKRESULT.FAILURE;
            return TICKRESULT.SUCCESS;
        }

        private TICKRESULT _Spawn(float arg)
        {

            var id = _Gate.SpawnEnterance(_Entitys.Shuffle().First() ) ;
            if (id != Guid.Empty)
            {
                _Ids.Enqueue(id);
                return TICKRESULT.SUCCESS;
            }
            return TICKRESULT.FAILURE;
        }
        

        protected override void _Update(float delta)
        {
            
        }

        protected override void _Shutdown()
        {
            
        }
    }
}