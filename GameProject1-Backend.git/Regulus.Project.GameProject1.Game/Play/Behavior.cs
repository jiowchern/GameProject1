using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Regulus.Framework;
using Regulus.Project.GameProject1.Game.Play;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game
{
    public abstract class Behavior : IUpdatable
    {
        

        private readonly GpiTransponder _Transponder;

        public GpiTransponder Transponder {  get { return _Transponder; } }

        Regulus.BehaviourTree.ITicker _Tree;

        private readonly Regulus.Utility.TimeCounter _DeltaTimeCounter;

        private float _LastDelta;

        public float LastDelta { get { return _LastDelta;} }

        protected Behavior()
        {
            
           
            _Transponder = new GpiTransponder();
            _DeltaTimeCounter = new TimeCounter();        
        }

        public IBinder GetSoulBinder()
        {
            return _Transponder;
        }

        protected abstract Regulus.BehaviourTree.ITicker _Launch();
        protected abstract void _Update(float delta);
        protected abstract void _Shutdown();
        void IBootable.Launch()
        {
            _Tree = _Launch();
        }

        

        void IBootable.Shutdown()
        {
            _Shutdown();
        }

        bool IUpdatable.Update()
        {
            var second = _DeltaTimeCounter.Second;
            _DeltaTimeCounter.Reset();
            _LastDelta = second;
            

            _Tree.Tick(second);
            _Update(second);

            return true;
        }
    }
}
