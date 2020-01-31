using System;

using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class StunStatus : IStatus
    {
        private readonly IBinder _Binder;

        private readonly Entity _Player;

        private readonly Regulus.Utility.TimeCounter _Counter;

        public Action ExitEvent;
        public Action WakeEvent;

        private float _Stun;

        public StunStatus(IBinder binder, Entity player)
        {
            _Binder = binder;
            _Player = player;

            _Counter = new TimeCounter();
            _Stun = 10f;
        }

        void IStatus.Enter()
        {
            _Player.Stun();
            _Counter.Reset();


        }

        void IStatus.Leave()
        {            
        }

        void IStatus.Update()
        {
            if (_Counter.Second > 60f)
            {
                ExitEvent();
            }
            else
            {
                var aid = _Player.HaveAid();
                _Stun -= aid;
                if (_Stun <= 0f)
                {
                    WakeEvent();
                }
            }
            
            
        }
    }
}