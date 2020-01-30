using System;

using Regulus.BehaviourTree;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class TraceStrategy
    {        

        private readonly StandardBehavior _Behavior;
        private Regulus.BehaviourTree.ITicker _Node;

        private Guid _Target;

        private float _Distance;

        public TraceStrategy(StandardBehavior behavior)
        {
            _Behavior = behavior;

            _BuildNode();
        }
        public TICKRESULT Reset(Guid target, float distance)
        {
            _Target = target;
            _Distance = distance;

            _Node.Reset();
            return TICKRESULT.SUCCESS;
        }

        private void _BuildNode()
        {
            var th = new TurnHandler(_Behavior);

            float angle = 0.0f;
            var builder = new Regulus.BehaviourTree.Builder();
            _Node = builder.Sequence()
                               .Action(
                                   (delta) =>
                                   {
                                       var result = _Behavior.GetTargetAngle(_Target, ref angle);
                                       th.Input(angle);
                                       return result;
                                   })
                               .Action((delta) => th.Run(delta))
                               .Not()
                                   .Sequence()
                                       .Action((delta) => _Not(_Behavior.CheckDistance(_Target, _Distance)))
                                       .Action(_Behavior.MoveForward)
                                   .End()
                               .End()
                               .Action((delta) => _Behavior.GetTargetAngle(_Target, ref angle))
                               .Action((delta) => _Behavior.CheckAngle(angle))
                               .Action(_Behavior.StopMove)
                           .End().Build();
        }

        public TICKRESULT Tick(float delta)
        {
            return _Node.Tick(delta);
        }


        private TICKRESULT _Not(TICKRESULT check)
        {
            if (check == TICKRESULT.FAILURE)
                return TICKRESULT.SUCCESS;
            if (check == TICKRESULT.SUCCESS)
                return TICKRESULT.FAILURE;
            return TICKRESULT.RUNNING;
        }
    }
}