using System;
using System.Collections.Generic;


using System.Linq;


using Regulus.Collection;
using Regulus.CustomType;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;


namespace Regulus.Project.GameProject1.Game.Play
{
    internal class GameStage : IStage, IPlayerProperys,IEmotion        
    {
        private readonly ISoulBinder _Binder;

        private readonly IMapFinder _Map;


        private readonly TimeCounter _DeltaTimeCounter;
        private readonly TimeCounter _UpdateTimeCounter;
        private const float _UpdateTime = 1.0f / 30.0f;

        

        private readonly Entity _Player;

        private readonly Mover _Mover;

        private readonly DifferenceNoticer<IIndividual> _DifferenceNoticer;
        

        private readonly Regulus.Utility.Updater _Updater;

        private readonly Regulus.Utility.StageMachine _Machine;

        private readonly Behavior _Behavior;

        public event Action ExitEvent;
        public event Action<string> TransmitEvent;

        private readonly IMapGate _Gate;

        

        public GameStage(ISoulBinder binder, IMapFinder map , IMapGate gate, Entity entity)
        {
            _Gate = gate;
            _Map = map;
            _Binder = binder;
            _DeltaTimeCounter = new TimeCounter();
            _UpdateTimeCounter = new TimeCounter();
            _Updater = new Updater();
            _Machine = new StageMachine();
            _DifferenceNoticer = new DifferenceNoticer<IIndividual>();

            _Player = entity;
            _Mover = new Mover(this._Player);            
        }
        public GameStage(ISoulBinder binder, IMapFinder map, IMapGate gate, Entity entity, Behavior behavior) : this(binder, map, gate, entity)
        {
            _Behavior = behavior;
        }
        void IStage.Leave()
        {
            _Machine.Termination();
            _Updater.Shutdown();
            
            _DifferenceNoticer.Set(new IIndividual[0]) ;
            _DifferenceNoticer.JoinEvent -= this._BroadcastJoin;
            _DifferenceNoticer.LeftEvent -= this._BroadcastLeft;            

            _Binder.Unbind<IEmotion>(this);
            _Binder.Unbind<IDevelopActor>(_Player);            
            _Binder.Unbind<IPlayerProperys>(this);            
            _Gate.Left(_Player);
        }

        void IStage.Enter()
        {
            this._DifferenceNoticer.JoinEvent += this._BroadcastJoin;
            this._DifferenceNoticer.LeftEvent += this._BroadcastLeft;

            this._Gate.Join(this._Player);
            this._Binder.Bind<IPlayerProperys>(this);                        
            _Binder.Bind<IDevelopActor>(_Player);
            _Binder.Bind<IEmotion>(this);
            _ToSurvival();

            if (_Behavior != null)
                _Updater.Add(_Behavior);
        }

        

        void IStage.Update()
        {
            if (_UpdateTimeCounter.Second < _UpdateTime)
                return;
            var deltaTime = this._GetDeltaTime();
            _Machine.Update();
            _Updater.Working();

            var lastDeltaTime = deltaTime;
            _Move(lastDeltaTime);
            _Broadcast(_Map.Find(_Player.GetView()));
            _Player.Equipment.UpdateEffect(lastDeltaTime);

            var target = _Player.HaveTransmit();
            if (target != null)
                TransmitEvent(target);
        }

        private void _Move(float deltaTime)
        {
            
            _Player.TrunDirection(deltaTime);
            var velocity = this._Player.GetVelocity(deltaTime);
            var orbit = this._Mover.GetOrbit(velocity);
            var entitys = this._Map.Find(orbit);
            var hitthetargets = _Mover.Move(velocity, entitys.Where(x => x.Id != this._Player.Id));
            
            if (hitthetargets.Any())
            {
                _Player.SetCollisionTargets(hitthetargets);
                this._Player.ClearOffset();
            }
        }

        private void _Broadcast(IEnumerable<IIndividual> controllers)
        {
            
            this._DifferenceNoticer.Set(controllers);
        }
        

        private IIndividual _CheckHit(IEnumerable<IIndividual> controllers, Vector2 center,Guid self, Vector2 ray)
        {
            
            foreach (var individual in controllers)
            {                
                if(individual.Id == _Player.Id)
                    continue;
                if (individual.Id == self)
                    continue;

                float distance;
                Vector2 hitPoint;
                Vector2 normal;
                if (StandardBehavior.RayPolygonIntersect(
                    center,
                    ray,
                    individual.Mesh.Points,
                    out distance,
                    out hitPoint,
                    out normal))
                {
                    return individual;
                }
            }
            return null;
        }

        private void _BroadcastLeft(IEnumerable<IIndividual> controllers)
        {
            foreach (var controller in controllers)
            {
                this._Binder.Unbind<IVisible>(controller);
            }
        }

        private void _BroadcastJoin(IEnumerable<IIndividual> controllers)
        {
            foreach (var controller in controllers)
            {
                this._Binder.Bind<IVisible>(controller);
            }
        }

        private float _GetDeltaTime()
        {
            var second = this._DeltaTimeCounter.Second;
            this._DeltaTimeCounter.Reset();
            return second;
        }

        
        
        
        private void _ToSurvival()
        {
            var status = new ControlStatus(_Binder, _Player, _Map);
            status.StunEvent += _ToStun;
            _Machine.Push(status);
        }

        private void _ToStun()
        {
            var status = new StunStatus(_Binder, _Player);
            status.ExitEvent += ExitEvent;
            status.WakeEvent += _ToSurvival;

            _Machine.Push(status);
        }

        void IEmotion.Talk(string message)
        {
            _Player.Talk(message);
        }

        string IPlayerProperys.Realm { get { return _Gate.Name; } }

        Guid IPlayerProperys.Id
        {
            get { return _Player.Id; }
        }

        float IPlayerProperys.Strength
        {
            get { return _Player.Strength(0); }
        }

        float IPlayerProperys.Health
        {
            get { return _Player.Health(0); }
        }
    }
}