
using System;
using System.Linq;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class InventoryProxy
    {
        private IInventoryNotifier _Notifier;
        private readonly List<Item> _Items;

        public InventoryProxy()
        {
            _Items = new List<Item>();
            
        }

        public void Clear()
        {
            _Items.Clear();
            _Unbind();
        }

        private void _Unbind()
        {            
            if (_Notifier != null)
            {
                _Notifier.AddEvent -= _Add;
                _Notifier.RemoveEvent -= _Remove;
                _Notifier = null;
            }
        }

        public void Set(IInventoryNotifier notifier)
        {
            _Unbind();
            _Notifier = notifier;
            _Notifier.AddEvent += _Add;
            _Notifier.RemoveEvent += _Remove;
        }
        private void _Remove(Guid obj)
        {
            _Items.RemoveAll(i => i.Id == obj);
        }

        private void _Add(Item obj)
        {
            _Items.Add(obj);
        }

        public IEnumerable<Item> FindByPart(EQUIP_PART part)
        {
            return from item in _Items where item.GetEquipPart() == part select item;
        }

        public int GetAmount(string item_name)
        {
            return (from item in _Items where item.Name == item_name select item.Count).Sum();
        }

        protected Guid _FindIdByName(string name)
        {
            var result = _Items.Find((item) => item.Name == name);
            if (result != null)
                return result.Id;
            return Guid.Empty;
        }

        public Guid FindIdByName(string name)
        {
            return _FindIdByName(name);
        }

        public void Refresh(Item[] notifier)
        {
            _Items.Clear();
            _Items.AddRange(notifier);
        }
    }
}