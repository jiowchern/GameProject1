using System;
using System.Linq;
using System.Collections.Generic;

using Regulus.Utility;

using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class Realm : IUpdatable
    {
        private readonly RealmInfomation _RealmInfomation;

        public class Map
        {
            public IMapFinder Finder;
            public IMapGate Gate;
        }

        


        
        private readonly TimesharingUpdater _Updater;
        private readonly List<Dungeon> _Dungeons;

        private readonly Queue<Value<Map>> _QueryRequesters;

        

        

        
        public Realm(RealmInfomation realm_infomation)
        {
            _RealmInfomation = realm_infomation;

            _Updater = new TimesharingUpdater(1f/10f);            
            _QueryRequesters = new Queue<Value<Map>>();
            _Dungeons = new List<Dungeon>();
        }
      
                

        public Regulus.Remote.Value<Map> QueryMap()
        {
            var val = new Regulus.Remote.Value<Map>();
            _PushQueryRequest(val);
            return val;
        }

        private void _PushQueryRequest(Value<Map> val)
        {
            _QueryRequesters.Enqueue(val);
        }

        void IBootable.Launch()
        {
            
        }

        void IBootable.Shutdown()
        {
            _Updater.Shutdown();
        }

        bool IUpdatable.Update()
        {
            _Updater.Working();

            if (_QueryRequesters.Count > 0)
            {
                var map = _FindMap();
                if (map != null)
                {
                    var val = _QueryRequesters.Dequeue();
                    val.SetValue(map);
                }
            }

            _Clean();
            return true;
        }

        private void _Clean()
        {
            var dungeons = from dungeon in _Dungeons where dungeon.IsValid() == false select dungeon;
            foreach (var source in dungeons.ToArray())
            {
                _Dungeons.Remove(source);
                _Updater.Remove(source);
            }
        }

        private Map _FindMap()
        {
            var dungeon = (from d in _Dungeons where d.IsReady() select d).FirstOrDefault();
            if (dungeon != null)
            {
                return new Map
                {
                    Finder = dungeon.Map,
                    Gate = dungeon
                };
            }


            _Spawn();
            return null;
        }

        private void _Spawn()
        {
            var dungeon = new Dungeon(_RealmInfomation);
            _Dungeons.Add(dungeon);
            _Updater.Add(dungeon);
        }
    }
}
