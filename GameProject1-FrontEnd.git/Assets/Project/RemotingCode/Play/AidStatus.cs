using System;
using System.Collections.Generic;
using System.Linq;

using Regulus.CustomType;
using Regulus.Extension;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class AidStatus : IStage
    {
        private readonly Guid _ItemId;

        private readonly Entity _Player;

        private readonly IMapFinder _Map;

        private readonly ISoulBinder _Binder;

        private SkillCaster _Caster;

        private readonly Regulus.Utility.TimeCounter _TimeCounter;

        private bool _Complete  ;

        private float _CurrentCastTime;
        private Vector2 _DatumPosition;

        private readonly Dictionary<Guid, IIndividual> _Targets;

        public Action DoneEvent;

        public AidStatus(ISoulBinder binder, Entity player,IMapFinder map, Guid item_id)
        {
            _Targets = new Dictionary<Guid, IIndividual>();
            _TimeCounter = new TimeCounter();
            _Binder = binder;
            _Player = player;
            _Map = map;
            _ItemId = item_id;
        }

        void IStage.Enter()
        {
            _DatumPosition = _Player.GetPosition();
            _Player.Aid();
            _Caster = SkillCaster.Build(ACTOR_STATUS_TYPE.AID);
            _TimeCounter.Reset();
        }

        void IStage.Leave()
        {
            if (_Complete)
            {
                var item = _Player.Bag.Find(_ItemId);
                if (item != null)
                {
                    
                    var target = _Targets.Values.FirstOrDefault( t => EntityData.IsActor(t.EntityType) && t.Status == ACTOR_STATUS_TYPE.STUN );
                    if (target != null)
                    {
                        HitForce hit = new HitForce();
                        var aid = item.GetAid() + _Caster.GetAid();
                        hit.Aid = aid;
                        target.AttachHit(_Player.Id , hit);
                        _Player.Bag.Remove(_ItemId);
                    }

                }
            }
        }

        void IStage.Update()
        {
            var second = _TimeCounter.Second;
            var poly = _Caster.FindDetermination(_CurrentCastTime, second);
            _CurrentCastTime = second;

            if (poly != null)
            {
                var dir = -_Player.Direction * ((float)Math.PI / 180f);
                var center = _DatumPosition;
                if (_Caster.IsControll())
                {
                    center = _Player.GetPosition();
                }

                poly.Rotation(dir, new Vector2());
                poly.Offset(center);

                var results = _Map.Find(poly.Points.ToRect());
                _Attach(results);
            }

            if (_Caster.IsDone(second))
            {
                _Complete = true;
                DoneEvent();
            }
        }

        private void _Attach(IIndividual[] actors)
        {
            foreach (var actor in actors)
            {
                _Attach(actor);
            }
        }

        private void _Attach(IIndividual actor)
        {            
            if (_Targets.ContainsKey(actor.Id) == false)
            {
                _Targets.Add(actor.Id , actor);
            }
        }
    }
}