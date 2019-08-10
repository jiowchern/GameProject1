using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

using Regulus.CustomType;
using Regulus.Framework;
using Regulus.Project.GameProject1.Data;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class Dungeon :Regulus.Utility.IUpdatable , IMapGate
    {
        private readonly RealmInfomation _RealmInfomation;

        private readonly Dictionary<ENTITY, int> _EntityEnteranceResource;
        private readonly Dictionary<ENTITY, int> _EntityFieldResource;
        private readonly Dictionary<Data.LEVEL_UNIT, EntityGroupBuilder> _LevelUnitToGroupBuilder;
        private readonly TimesharingUpdater _Updater;

        private readonly List<Aboriginal> _Aboriginals;
        

        private readonly List<Entity> _WaitingRoom;        

        private readonly Map _Map;

        private bool _Ready;

        private readonly bool _Valid;

        public Dungeon(RealmInfomation realm_infomation)
        {
            _RealmInfomation = realm_infomation;
            _Valid = true;
            _Map = new Map();
            _WaitingRoom = new List<Entity>();            
            _Updater = new TimesharingUpdater(1.0f / 30.0f);
            _Aboriginals = new List<Aboriginal>();

            _EntityEnteranceResource = realm_infomation.EntityEnteranceResource;
            /*_EntityEnteranceResource = new Dictionary<ENTITY, int>
            {
                { ENTITY.ACTOR1, 10},
                { ENTITY.ACTOR2, 20},
                { ENTITY.ACTOR3, 20},
                { ENTITY.ACTOR4, 20},
                { ENTITY.ACTOR5, 20},
            };*/
            _EntityFieldResource = realm_infomation.EntityFieldResource;
            /*_EntityFieldResource = new Dictionary<ENTITY, int>
            {
                { ENTITY.ACTOR1, 10},
                { ENTITY.ACTOR2, 10},
                { ENTITY.ACTOR3, 10},
                { ENTITY.ACTOR4, 10},
                { ENTITY.ACTOR5, 10},
            };*/

            _LevelUnitToGroupBuilder = new Dictionary<Data.LEVEL_UNIT, EntityGroupBuilder>();
            foreach (var mazeUnitInfomation in _RealmInfomation.Maze.MazeUnits)
            {
                _LevelUnitToGroupBuilder.Add(mazeUnitInfomation.Type , new EntityGroupBuilder(mazeUnitInfomation.Name, _Map, this));
            }
        }

        public IMapFinder Map { get { return _Map;} }

        private Aboriginal _Create(Map map,ENTITY type, ItemProvider item_provider)
        {
            
            var entiry = EntityProvider.Create(type);
            var items = item_provider.FromStolen();
            foreach (var item in items)
            {
                entiry.Bag.Add(item);
            }            
            var wisdom = new StandardBehavior(entiry);
            var aboriginal = new Aboriginal(map, this, entiry, wisdom);            
            return aboriginal;
        }

        void IBootable.Launch()
        {
            
            var map = _Map;

            _BuildMaze(map);
            _BuildTown(map);

            _Ready = true;
        }

        private void _BuildTown(Map map)
        {
            if (_RealmInfomation.HaveTown() == false)
                return;


            var layout = new EntityGroupBuilder(_RealmInfomation.Town.Name, map, this);
            foreach (var updatable in layout.Create(0, new Vector2()))
            {
                _Updater.Add(updatable);
            }
        }

        private void _BuildMaze(Map map)
        {
            var level = _CreateLevel();

            foreach (var unit in level)
            {
                var builder = _LevelUnitToGroupBuilder[unit.Type];
                var updaters = builder.Create(unit.Direction, unit.Center);

                foreach (var updatable in updaters)
                {
                    _Updater.Add(updatable);
                }                   
            }
        }

        private Level _CreateLevel()
        {            
            var builder = new LevelGenerator(_RealmInfomation.Maze.Width, _RealmInfomation.Maze.Height);
            var level = builder.Build(_RealmInfomation.Maze.Dimension);
            return level;
        }

        private void _Join(Aboriginal aboriginal)
        {
            _Updater.Add(aboriginal);
            _Aboriginals.Add(aboriginal);
        }

        void IBootable.Shutdown()
        {

            _Updater.Shutdown();
        }

        bool IUpdatable.Update()
        {
            _Updater.Working();
            return true;
        }

        string IMapGate.Name { get { return _RealmInfomation.Name; } }
        

        void IMapGate.Left(Entity player)
        {
            _Map.Left(player);
            _WaitingRoom.RemoveAll((e) => e.Id == player.Id);
        }

        void IMapGate.Join(Entity player)
        {
            if (_InWaitingRoom(player))
            {
                _WaitingRoom.Add(player);
                if(_WaitEvent != null)
                    _WaitEvent(player.Id);
            }
                
            else
            {
                _Map.JoinStaff(player);
            }
        }

        private bool _InWaitingRoom(Entity player)
        {
            if (EntityData.IsActor(player.Type))
            {                
                return true;
            }
            return false;
        }

        void IMapGate.Pass(Vector2 position, ENTITY[] types)
        {
            var entity = (from e in _WaitingRoom where types.Any( t => t == e.Type) select e).FirstOrDefault();
            if (entity != null)
            {
                _Join(position, entity);
            }                
        }

        
        void IMapGate.Pass(Vector2 position, Guid id)
        {
            var entity = (from e in _WaitingRoom where e.Id == id select e).FirstOrDefault();
            if (entity != null)
            {
                _Join(position, entity);
            }
        }

        private void _Join(Vector2 position, Entity entity)
        {
            IIndividual individual = entity;
            _WaitingRoom.RemoveAll((e) => e.Id == entity.Id);
            individual.SetPosition(position);
            _Map.JoinStaff(entity);
        }

        Guid IMapGate.SpawnEnterance(ENTITY type)
        {
            if (_EntityEnteranceResource[type] > 0)
            {
                var itemProvider = new ItemProvider();
                var aboriginal = _Create(_Map, type, itemProvider);
                _EntityEnteranceResource[type]--;
                _Join(aboriginal);                
                aboriginal.DoneEvent += () =>
                {
                    _EntityEnteranceResource[type]++;
                    _Left(aboriginal.Entity.Id);
                };
                return aboriginal.Entity.Id;
            }
            return Guid.Empty;
        }

        Guid[] IMapGate.SpawnField(ENTITY[] types)
        {
            foreach (var type in types)
            {
                if(_EntityFieldResource[type] <= 0)
                    return new Guid[0];
            }
            List<Guid> ids = new List<Guid>();
            foreach (var type in types)
            {
                if (_EntityFieldResource[type] > 0)
                {
                    var itemProvider = new ItemProvider();
                    var aboriginal = _Create(_Map, type, itemProvider);
                    _EntityFieldResource[type]--;
                    _Join(aboriginal);
                    aboriginal.DoneEvent += () =>
                    {
                        _EntityFieldResource[type]++;

                        _Left(aboriginal.Entity.Id);
                        
                    };
                    ids.Add(aboriginal.Entity.Id);
                }
            }
            return ids.ToArray();
        }

        Guid IMapGate.Spawn(ENTITY type)
        {
            var itemProvider = new ItemProvider();
            var aboriginal = _Create(_Map, type, itemProvider);
            aboriginal.DoneEvent += () =>
            {                
                _Left(aboriginal.Entity.Id);
            };
            _Join(aboriginal);
            return aboriginal.Entity.Id;
        }

        void IMapGate.Exit(Guid contestant)
        {
            _Left(contestant);
        }

        private void _Left(Guid contestant)
        {
            var aboriginals = (from a in _Aboriginals where a.Entity.Id == contestant select a);

            foreach (var aboriginal in aboriginals)
            {
                _Updater.Remove(aboriginal);
            }
            _Aboriginals.RemoveAll((a1) => aboriginals.Any((a2) => a2.Entity.Id == a1.Entity.Id));
        }

        private event Action<Guid> _WaitEvent;

        event Action<Guid> IMapGate.WaitEvent
        {
            add { this._WaitEvent += value; }
            remove { this._WaitEvent -= value; }
        }

        public bool IsReady()
        {

            return _Ready;
        }

        public bool IsValid()
        {
            return _Valid;
        }
    }
}
