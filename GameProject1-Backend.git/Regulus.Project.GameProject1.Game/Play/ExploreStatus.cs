using System;
using System.Linq;

using Regulus.CustomType;
using Regulus.Extension;
using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class ExploreStatus : ActorStatus 
    {
        private readonly Entity _Player;

        private readonly IMapFinder _Map;

        private readonly Guid _TargetId;

        private readonly IBinder _Binder;

        private readonly TimeCounter _CastTimer;

        public event Action DoneEvent;

        private bool _Done;
        public ExploreStatus(IBinder binder, Entity player , IMapFinder map , Guid target_id)
        {
            _Binder = binder;
            _Player = player;
            _Map = map;
            _TargetId = target_id;
            _CastTimer = new TimeCounter();
        }

        

        public override void Leave()
        {
            if (_Done == false)
                return;
            var explore = _Player.GetExploreBound();
            var results = _Map.Find(explore.Points.ToRect());
            var target = (from indivude in results where indivude.Id == _TargetId select indivude).SingleOrDefault();
            if (target != null)
            {
                var result = Polygon.Collision(target.Mesh, explore, new Vector2());
                if (result.Intersect)
                {
                    var items = target.Stolen(_Player.Id);
                    foreach (var item in items)
                    {
                        _Player.Bag.Add(item);
                    }

                }
            }
        }

        public override void Update()
        {
            if (_CastTimer.Second > 1.0f)
            {
                _Done = true;
                DoneEvent();
            }
        }

        public override void Enter()
        {
            _Player.Explore();
        }

        

        
    }
}