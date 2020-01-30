using System;

using Regulus.Framework;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class Aboriginal : Regulus.Utility.IUpdatable
    {
        private readonly IMapFinder _Map;

        private readonly Entity _Actor;

        private readonly Regulus.Utility.StageMachine _Machine;

        private readonly Regulus.Utility.Updater _Updater;

        private readonly Behavior _Behavior;

        private readonly IMapGate _Gate;

        public event Action DoneEvent;
        public Aboriginal(IMapFinder map,IMapGate gate, Entity actor ,  Behavior behavior)
        {
            _Gate = gate;
            _Behavior = behavior;
            _Updater = new Updater();
            _Map = map;
            _Actor = actor;
            _Machine = new StageMachine();
        }

        public IIndividual Entity { get { return _Actor; } }

        void IBootable.Launch()
        {
            var provider = new ItemProvider();            
            _Actor.Bag.Add(provider.BuildItem(1 , "AidKit1" , new [] {new ItemEffect()
            {
                Effects = new []{ new Effect() { Type =  EFFECT_TYPE.AID , Value = 10 } },
                Quality = 0.5f                                 
            } }));

            _Actor.Bag.Add(provider.BuildItem(1, "Axe1", new[] {new ItemEffect()
            {
                Effects = new []{ new Effect() { Type =  EFFECT_TYPE.ATTACK_ADD , Value = 1 } },
                Quality = 0.5f
            } }));
            _ToGame(_Map);
        }

        private void _ToGame(IMapFinder map)
        {            
            var stage = new GameStage(_Behavior.GetSoulBinder() ,  map , _Gate, _Actor , _Behavior);
            stage.ExitEvent += _ToDone ;
            stage.TransmitEvent += (target) => { };
            _Machine.Push(stage);
        }

        private void _ToDone()
        {
            _Machine.Empty();
            DoneEvent();
        }

        void IBootable.Shutdown()
        {
            _Machine.Termination();
            _Updater.Shutdown();
        }

        bool IUpdatable.Update()
        {
            _Updater.Working();
            _Machine.Update();
            return true;

        }

        
    }
}