using System.Linq;

using Regulus.BehaviourTree;

using Regulus.Project.GameProject1.Data;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class PortalBehavior : Behavior 
    {
        private readonly Entity _Entity;

        private readonly string _TargetRealm;

        private readonly ENTITY[] _PassEntity;

        private readonly IMapGate _Gate;

        private readonly IMapFinder _Finder;

        public PortalBehavior(Entity entity, string target_realm, ENTITY[] pass_entity, IMapGate gate, IMapFinder finder)
        {
            _Entity = entity;
            _TargetRealm = target_realm;
            _PassEntity = pass_entity;
            _Gate = gate;
            _Finder = finder;
        }

        protected override ITicker _Launch()
        {
            _Gate.Join(_Entity);


            var builder = new Regulus.BehaviourTree.Builder();
            var ticker = builder
                    .Sequence()
                        .Action(()=>new WaitSecondStrategy(0.1f), t => t.Tick, t => t.Start, t => t.End)
                        .Action(_HandlePassing)
                    .End()
                .Build();
            return ticker;
        }

        protected override void _Update(float delta)
        {
            
        }

        private TICKRESULT _HandlePassing(float delta)
        {
            var targets = _Finder.Find(_Entity.GetBound());
            foreach (var individual in targets)
            {
                
                if (_PassEntity.Any( e => e == individual.EntityType))
                    individual.Transmit(_TargetRealm);
            }
            return TICKRESULT.SUCCESS;
        }

        protected override void _Shutdown()
        {
            _Gate.Left(_Entity);
        }

       
    }
}