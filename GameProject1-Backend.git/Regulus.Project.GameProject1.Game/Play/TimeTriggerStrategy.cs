using Regulus.BehaviourTree;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class TimeTriggerStrategy
    {
        private float _Second;

        public TimeTriggerStrategy(float second)
        {
            _Second = second;
        }

        public TICKRESULT Tick(float arg)
        {            
            if(_Second < 0)
                return TICKRESULT.SUCCESS;

            _Second -= arg;
            return TICKRESULT.FAILURE;
        }

        public TICKRESULT Reset(float second)
        {
            _Second = second;
            return TICKRESULT.SUCCESS;
        }
    }
}