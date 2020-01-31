using System;

using Regulus.Framework;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class ControlStatus : Regulus.Utility.IStatus
    {
        private readonly IBinder _Binder;

        private readonly Entity _Player;        

        private readonly IMapFinder _Map;

        private readonly StatusMachine _Status;

        public event Action StunEvent;

        private Regulus.Utility.TimeCounter _TimeCounter;


        public ControlStatus(IBinder binder, Entity player, IMapFinder map)
        {
            _Binder = binder;
            _Player = player;            
            _Map = map;
            _Status = new StatusMachine();
            _TimeCounter = new TimeCounter();
        }
        

        private void _ToDone()
        {            
            var status = new NormalStatus(_Binder, _Player);
            status.ExploreEvent += _ToExplore;
            status.BattleEvent += _ToCast;
            status.MakeEvent += _ToMake;
            status.AidEvent += _ToAid;
            _SetStatus(status);
        }

        private void _ToAid(Guid item_id)
        {
            var status = new AidStatus(_Binder, _Player,_Map , item_id);
            status.DoneEvent += _ToDone;
            _SetStatus(status);
        }

        private void _ToMake()
        {
            var status = new MakeStatus(_Binder, _Player);
            status.DoneEvent += _ToDone;
            _SetStatus(status);
        }

       

        private void _ToCast(SkillCaster caster)
        {
            var status = new BattleCasterStatus(_Binder, _Player, _Map , caster);
            status.NextEvent += _ToCast;
            status.BattleIdleEvent += _ToBattle;
            status.DisarmEvent += _ToDone;
            _SetStatus(status);
        }

        private void _SetStatus(IStatus status)
        {            
            _Status.Push(status);
        }

        private void _ToExplore(Guid obj)
        {
            var status = new ExploreStatus(_Binder, _Player , _Map , obj);
            status.DoneEvent += _ToDone;
            _SetStatus(status);
        }

        
        void IStatus.Enter()
        {
            _Binder.Bind<IEquipmentNotifier>(_Player.Equipment);
            _Binder.Bind<IBagNotifier>(_Player.Bag);
            _Player.ResetProperty();
            _ToDone();
        }

        void IStatus.Leave()
        {
            _Status.Termination();
            _Binder.Unbind<IBagNotifier>(_Player.Bag);
            _Binder.Unbind<IEquipmentNotifier>(_Player.Equipment);
        }

        void IStatus.Update()
        {
            _Status.Update();

            var damage = _Player.HaveDamage();
            _ProcessDamage(damage);

            var deltaTime = _TimeCounter.Second;
            _TimeCounter.Reset();
            _Player.RecoveryStrength(deltaTime);
            _Player.RecoveryHealth(deltaTime);

        }

        

        private void _ProcessDamage(float damage)
        {
            var hp = _Player.Health(-damage);
            if (hp < 0)
            {
                StunEvent();
            }
            else if (damage > 2)
                _ToKnockout();
            else if (damage > 0)
                _ToDamage();
        }
        

        private void _ToKnockout()
        {

            var skill = _Player.Equipment.GetSkill();
            var skillData = Resource.Instance.FindSkill(skill.Knockout);
            var caster = new SkillCaster(skillData, new Determination(skillData));
            var stage = new BattleCasterStatus(_Binder, _Player, _Map, caster);
            stage.BattleIdleEvent += _ToBattle;
            stage.NextEvent += _ToCast;
            
            _SetStatus(stage);
        }

        private void _ToBattle()
        {
            var status = new BattleCasterStatus(_Binder, _Player, _Map, _Player.GetBattleCaster());
            status.NextEvent += _ToCast;
            status.DisarmEvent += _ToDone;
            _SetStatus(status);
        }

        private void _ToDamage()
        {

            var skill = _Player.Equipment.GetSkill();
            var skillData = Resource.Instance.FindSkill(skill.Injury);
            
            var caster = new SkillCaster(skillData, new Determination(skillData));
            var stage = new BattleCasterStatus(_Binder , _Player , _Map , caster);
            stage.BattleIdleEvent += _ToBattle;
            stage.NextEvent += _ToCast;            
            _SetStatus(stage);
        }
        
    }
}