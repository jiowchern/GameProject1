using System;
using System.Collections.Generic;
using System.Linq;

using Regulus.BehaviourTree;
using Regulus.Project.GameProject1.Data;



namespace Regulus.Project.GameProject1.Game.Play
{
    internal class ActorMind
    {
        private readonly ENTITY _EntityType;

        internal class Actor
        {
            private readonly Guid _Id;

            private float _Durability;

            private float _Imperil;

            private bool _Looted;

            public Actor(Guid id, float imperil)
            {
                _Id = id;
                _Imperil = imperil;
                _Durability = 60.0f;
            }

            public Guid Id { get { return _Id; } }

            public float Imperil { get { return _Imperil; } }

            

            public bool TimeUp(float delta)
            {                
                _Durability -= delta;
                return _Durability < 0;
            }

            public void Hate(float damage)
            {
                _Durability += 10.0f;
                _Imperil += damage;
            }

            public void Loot()
            {
                _Looted = true;
            }

            public bool IsLooted()
            {
                    return _Looted;
            }
        }

        private readonly Dictionary<Guid, Actor> _Actors;        

        public ActorMind(ENTITY entity_type)
        {
            _EntityType = entity_type;
            _Actors = new Dictionary<Guid, Actor>();
            

        }

        public void Add(Guid id, float imperil)
        {
            if(_Actors.ContainsKey(id) == false)
                _Actors.Add(id , new Actor(id , imperil));
        }

        public bool Have(Guid id)
        {
            return _Actors.ContainsKey(id);
        }

        public void Forget(float delta)
        {
            List<Guid> removes = new List<Guid>();

            foreach (var keyPair in _Actors)
            {
                var actor = keyPair.Value;
                if (actor.TimeUp(delta))
                {
                    removes.Add(actor.Id);
                }
            }
            foreach (var remove in removes)
            {
                _Actors.Remove(remove);
            }
            
        }
        public IVisible FindResourceTarget(List<IVisible> field_of_vision)
        {
            return (from visible in field_of_vision
                    let target = (from actor in _Actors.Values
                                   where EntityData.IsResource(visible.EntityType) && actor.Id == visible.Id && actor.IsLooted() == false
                                   select visible).FirstOrDefault()
                    where target != null                                  
                    select target).FirstOrDefault();
        }
        public IVisible FindLootTarget(IEnumerable<IVisible> vision)
        {
            return (from visible in vision
                    let notLoot = (from actor in _Actors.Values
                                   where actor.Id == visible.Id && actor.IsLooted() == false                                   
                                   select true).FirstOrDefault()
                    let imperil = (from actor in _Actors.Values
                                   where actor.Imperil > 0 && actor.Id == visible.Id
                                   select actor.Imperil).Sum()
                    where notLoot && imperil >0  && visible.Status == ACTOR_STATUS_TYPE.STUN                    
                    select visible).FirstOrDefault();
        }

        public IVisible FindWounded(ENTITY type, IEnumerable<IVisible> field_of_vision)
        {

            var companions = _FindCompanion(type , field_of_vision);
            return companions.FirstOrDefault((companion) => companion.Status == ACTOR_STATUS_TYPE.STUN);
        }

        private IEnumerable<IVisible> _FindCompanion(ENTITY type, IEnumerable<IVisible> field_of_vision)
        {
            var companions = from vis in field_of_vision where vis.EntityType == type  select vis;

            return (from companion in companions
                    from value in _Actors.Values
                    where companion.Id == value.Id select companion);
        }

        public IVisible FindEnemy(IEnumerable<IVisible> field_of_vision)
        {
            return (from visible in field_of_vision
                    let imperil = (   from actor in _Actors.Values
                                    where actor.Imperil > 0 && actor.Id == visible.Id 
                                      select actor.Imperil).Sum()
                    where imperil > 0 && visible.Status != ACTOR_STATUS_TYPE.STUN
                    orderby imperil descending 
                    select visible).FirstOrDefault();

        }
        

        public bool Hate(Guid target, float damage)
        {

            Actor actor;
            if (_Actors.TryGetValue(target , out actor))
            {
                actor.Hate(damage);
                return true;
            }
            
            return false;
        }

        public void Loot(Guid target)
        {
            Actor actor;
            if (_Actors.TryGetValue(target, out actor))
            {
                actor.Loot();
            }
        }

        public void Release()
        {
            _Actors.Clear();
        }

        public int GetActorCount()
        {
            return _Actors.Count;
        }

        public bool IsLooted(Guid id)
        {
            Actor actor;
            if (_Actors.TryGetValue(id, out actor))
            {
                return actor.IsLooted();
            }
            return false;
        }
    }

    
}