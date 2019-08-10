using System;

using Regulus.BehaviourTree;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class ChestIdleAction 
    {
        private readonly Entity _Chest;

        private readonly IMapGate _Gate;


        private TICKRESULT _Status;

        public ChestIdleAction(Entity chest, IMapGate gate)
        {
            _Chest = chest;
            _Gate = gate;            
        }

        

        public TICKRESULT Tick(float delta)
        {
            return _Status;
        }

        public void Start()
        {
            _Chest.UnlockEvent += _OnUnlcokResult;
            _Gate.Join(_Chest);
            _Status = TICKRESULT.RUNNING;
        }

        

        public void End()
        {
            _Gate.Left(_Chest);
            _Chest.UnlockEvent -= _OnUnlcokResult;
        }

        private void _OnUnlcokResult(bool result)
        {
            if (result == false)
            {
                _Status = TICKRESULT.SUCCESS;
            }
                
        }
    }
}