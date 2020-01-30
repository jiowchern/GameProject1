using System;
using System.Linq;

using Regulus.Project.GameProject1.Data;
using Regulus.Remote;
using Regulus.Utility;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class NormalStatus : ActorStatus , INormalSkill , IInventoryController
    {
        private readonly ISoulBinder _Binder;

        private readonly Entity _Player;

        public event Action<Guid> ExploreEvent;

        public event Action<Guid> AidEvent;

        public event Action<SkillCaster> BattleEvent;
        public event Action MakeEvent;

        private readonly MoveController _MoveController;

        private bool _RequestAllItems;

        private readonly Regulus.Utility.TimeCounter _TimeCounter;

        private float _UpdateAllItemTime;

        public NormalStatus(ISoulBinder binder, Entity player) 
        {
            _TimeCounter = new TimeCounter();
            _Binder = binder;
            _Player = player;

            _MoveController = new MoveController(_Player);
        }
        

        public override void Leave()
        {
            _Binder.Unbind<INormalSkill>(this);
            _Binder.Unbind<IMoveController>(_MoveController);
            
            _Binder.Unbind<IInventoryController>(this);
        }

        public override void Update()
        {
            _ResponseItems(_TimeCounter.Second);
            _TimeCounter.Reset();
        }

        public override void Enter()
        {
            _Binder.Bind<IInventoryController>(this);            
            _Binder.Bind<IMoveController>(_MoveController);
            _Binder.Bind<INormalSkill>(this);

            _Player.Normal();
        }

        private void _ResponseItems(float deltaTime)
        {
            if (_UpdateAllItemTime - deltaTime <= 0)
            {
                if (_RequestAllItems)
                {
                    if (_EquipItemsEvent != null)
                    {
                        _EquipItemsEvent(_Player.Equipment.GetItems());
                    }
                    if (_BagItemsEvent != null)
                    {
                        _BagItemsEvent.Invoke(_Player.Bag.ToArray());
                    }
                    _UpdateAllItemTime = 10f;
                    _RequestAllItems = false;
                }
            }
            else
            {
                _UpdateAllItemTime -= deltaTime;
            }
        }

        
        

        

        void INormalSkill.Explore(Guid target)
        {
            ExploreEvent(target);
        }

        public void Battle()
        {
            BattleEvent(_Player.GetBattleCaster());
        }

        void INormalSkill.Make()
        {
            MakeEvent();
        }


        private event Action<Item[]> _BagItemsEvent;
        event Action<Item[]> IInventoryController.BagItemsEvent
        {
            add { _BagItemsEvent += value; }
            remove { _BagItemsEvent -= value; }
        }

        private event Action<Item[]> _EquipItemsEvent;
        event Action<Item[]> IInventoryController.EquipItemsEvent
        {
            add { _EquipItemsEvent += value; }
            remove { _EquipItemsEvent -= value; }
        }

        void IInventoryController.Refresh()
        {
            _RequestAllItems = true;            
        }

        

        void IInventoryController.Unequip(Guid id)
        {
            var items = _Player.Equipment.Unequip(id);
            foreach(var item in items)
            {
                _Player.Bag.Add(item);
            }
        }

       

        void IInventoryController.Discard(Guid id)
        {
            _Player.Bag.Remove(id);
        }

        void IInventoryController.Equip(Guid id)
        {
            var item = _Player.Bag.Find(id);

            if (Item.IsValid(item) && item.IsEquipable())
            {
                if (_Player.Equipment.Equip(item))
                {
                    _Player.Bag.Remove(item.Id);
                }
                
            }
        }

        void IInventoryController.Use(Guid id)
        {
            var item = _Player.Bag.Find(id);
            if (item != null)
            {
                if (item.GetAid() > 0)
                    AidEvent(id);
            }
        }
        

        

        
    }
}