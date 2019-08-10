using Regulus.BehaviourTree;
using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class EnteranceBehavior : Behavior
    {
        private readonly ENTITY[] _Types;

        private readonly Entity _Owner;

        private readonly IMapGate _Gate;

        public EnteranceBehavior(ENTITY[] types, Entity owner, IMapGate gate)
        {
            _Types = types;
            _Owner = owner;
            _Gate = gate;        
            
        }

        protected override ITicker _Launch()
        {
            var builder = new Regulus.BehaviourTree.Builder();
            var ticker = builder
                .Sequence()
                    .Action(() => new WaitSecondStrategy(0.1f) , t=>t.Tick , t=>t.Start , t=>t.End)
                    .Action(_Pass)
                .End().Build();

            return ticker;
        }

        private TICKRESULT _Pass(float arg)
        {
            _Gate.Pass( _Owner.GetPosition() , _Types);

            return TICKRESULT.SUCCESS;
        }

        protected override void _Update(float delta)
        {
            
        }

        protected override void _Shutdown()
        {
            
        }
    }
}