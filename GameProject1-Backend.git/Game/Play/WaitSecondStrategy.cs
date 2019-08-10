using Regulus.BehaviourTree;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class WaitSecondStrategy 
    {
        private readonly float _Second;

        private float _Count;
        public WaitSecondStrategy(float second)
        {
            _Second = second;            
        }

        

        public TICKRESULT Tick(float delta)
        {
            _Count += delta;
            if (_Count > _Second)
                return TICKRESULT.SUCCESS;

            return TICKRESULT.RUNNING;
        }

        public void Start()
        {
            _Count = 0.0f;
        }

        public void End()
        {            
        }
    }
}