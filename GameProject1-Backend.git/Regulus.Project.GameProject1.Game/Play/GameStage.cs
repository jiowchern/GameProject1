using System;
using System.Collections.Generic;


using System.Linq;


using Regulus.Collection;
using Regulus.Utility;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;


namespace Regulus.Project.GameProject1.Game.Play
{
    internal class GameStage : IStatus, IPlayerProperys,IEmotion        
    {
        private readonly IBinder _Binder;

        private readonly IMapFinder _Map;


        private readonly TimeCounter _DeltaTimeCounter;
        private readonly TimeCounter _UpdateTimeCounter;
        private const float _UpdateTime = 1.0f / 30.0f;

        

        private readonly Entity _Player;

        private readonly Mover _Mover;

        private readonly DifferenceNoticer<IIndividual> _DifferenceNoticer;
        

        private readonly Regulus.Utility.Updater _Updater;

        private readonly Regulus.Utility.StatusMachine _Machine;

        private readonly Behavior _Behavior;

        public event Action ExitEvent;
        public event Action<string> TransmitEvent;

        private readonly IMapGate _Gate;
        private readonly System.Collections.Generic.List<ISoul> _BroadcastSouls;
        UnbindHelper _UnbindHelper;
        Regulus.Remote.Property<float> _PlayerHealth;//_Player.Health(0)
        Regulus.Remote.Property<float> _PlayerStrength;//_Player.Strength(0)

        public GameStage(IBinder binder, IMapFinder map , IMapGate gate, Entity entity)
        {
            _UnbindHelper = new UnbindHelper(binder);
            _PlayerHealth = new Property<float>();
            _PlayerStrength = new Property<float>();
            _Gate = gate;
            _Map = map;
            _Binder = binder;
            _DeltaTimeCounter = new TimeCounter();
            _UpdateTimeCounter = new TimeCounter();
            _Updater = new Updater();
            _Machine = new StatusMachine();
            _DifferenceNoticer = new DifferenceNoticer<IIndividual>();

            _Player = entity;
            _Mover = new Mover(this._Player);            
        }
        public GameStage(IBinder binder, IMapFinder map, IMapGate gate, Entity entity, Behavior behavior) : this(binder, map, gate, entity)
        {
            _Behavior = behavior;
        }
        void IStatus.Leave()
        {
            _Machine.Termination();
            _Updater.Shutdown();
            
            _DifferenceNoticer.Set(new IIndividual[0]) ;
            _DifferenceNoticer.JoinEvent -= this._BroadcastJoin;
            _DifferenceNoticer.LeaveEvent -= this._BroadcastLeft;

            _UnbindHelper.Release();
            _Gate.Left(_Player);
        }

        void IStatus.Enter()
        {
            this._DifferenceNoticer.JoinEvent += this._BroadcastJoin;
            this._DifferenceNoticer.LeaveEvent += this._BroadcastLeft;

            this._Gate.Join(this._Player);
            _UnbindHelper += this._Binder.Bind<IPlayerProperys>(this);
            _UnbindHelper += _Binder.Bind<IDevelopActor>(_Player);
            _UnbindHelper += _Binder.Bind<IEmotion>(this);
            _ToSurvival();

            if (_Behavior != null)
                _Updater.Add(_Behavior);
        }

        

        void IStatus.Update()
        {
            if (_UpdateTimeCounter.Second < _UpdateTime)
                return;

            _PlayerHealth.Value = _Player.Health(0);
            _PlayerStrength.Value = _Player.Strength(0);    

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
                var soul = _BroadcastSouls.Find(s => s.Instance == controller);
                if (soul == null)
                    continue;
                _BroadcastSouls.Remove(soul);
                this._Binder.Unbind(soul);
            }
        }

        private void _BroadcastJoin(IEnumerable<IIndividual> controllers)
        {
            foreach (var controller in controllers)
            {
                var soul = this._Binder.Bind<IVisible>(controller);
                _BroadcastSouls.Add(soul);
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

        Regulus.Remote.Property<string> IPlayerProperys.Realm { get { return new Regulus.Remote.Property<string>(_Gate.Name); } }

        Regulus.Remote.Property<Guid> IPlayerProperys.Id
        {
            get { return new Regulus.Remote.Property<Guid> (_Player.Id); }
        }

        
        Regulus.Remote.Property<float> IPlayerProperys.Strength
        {
            get { return _PlayerStrength; }
        }

        
        Regulus.Remote.Property<float> IPlayerProperys.Health
        {
            get { return _PlayerHealth; }
        }
    }
}