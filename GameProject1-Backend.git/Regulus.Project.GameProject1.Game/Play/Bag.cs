using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Text;


using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    public class Bag : IEnumerable<Item> , IBagNotifier
    {
        private readonly List<Item> _Items;

        private int _Weight;


        public Bag()
        {
            this._Items = new List<Item>();
        }
        public void Add(Item item)
        {
            if (item.Effects.Length == 0)
            {
                var inBagItem = (from i in _Items where i.Name == item.Name && i.Effects.Length == 0 select i).FirstOrDefault();
                if(Item.IsValid(inBagItem))
                {
                    item.Count += inBagItem.Count;
                    Remove(inBagItem.Id);
                }                
            }
            item.RefCount ++;
            this._Items.Add(item);
            this._Weight += item.Weight;

            if (AddEvent != null)
                AddEvent(item);
        }

        public int GetWeight()
        {
            return this._Weight;
        }

        public void Remove(Guid id)
        {
            int weight = 0;
            var removeCount = _Items.RemoveAll(
                (item) =>
                {
                    if (item.Id == id)
                    {
                        item.RefCount --;
                        weight+=item.Weight;                        
                        return true;        
                    }
                    return false;
                });

            this._Weight -= weight;
            if (RemoveEvent != null)
            {
                RemoveEvent(id);
            }

            
            if (removeCount != 1)
                throw new Exception("錯誤的道具刪除");
            
        }


        public event Action<Item> AddEvent;

        event Action<Item> IInventoryNotifier.AddEvent
        {
            add { this.AddEvent += value; }
            remove { this.AddEvent -= value; }
        }

        public event Action<Guid> RemoveEvent;

        event Action<Guid> IInventoryNotifier.RemoveEvent
        {
            add { this.RemoveEvent += value; }
            remove { this.RemoveEvent -= value; }
        }

        public int GetItemAmount(string item)
        {

            return (from i in _Items
                    where i.Name == item
                    select i.Count).Sum();
        }

        public int Remove(string name, int amount)
        {
            List<Guid> removeItems = new List<Guid>();
            Item newItem = new Item();
            foreach(var item in _Items)
            {
                if(item.Name != name)
                    continue;

                amount -= item.Count;
                removeItems.Add(item.Id);

                if (amount == 0)
                    break;
                
                if (amount < 0)
                {
                    newItem = item.Clone();
                    newItem.Count =- amount;
                    break;
                }                
            }
            

            foreach(var item in removeItems)
            {
                Remove(item);
            }

            if (Item.IsValid(newItem))
            {
                Add(newItem);
            }
            return amount ;
        }

        public Item Find(Guid id)
        {
            return _Items.FirstOrDefault(i => i.Id == id);
        }

        public void Add(Item[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        IEnumerator<Item> IEnumerable<Item>.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        public void Remove(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                Remove(item.Id);
            }
        }
    }
}
