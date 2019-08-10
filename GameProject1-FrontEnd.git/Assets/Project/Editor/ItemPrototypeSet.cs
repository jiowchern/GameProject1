using System.Collections;
using System.Collections.Generic;

using Regulus.Project.GameProject1.Data;

namespace Assets.Project.Editor
{
    public class ItemPrototypeSet
    {
        private readonly Dictionary<string, ItemPrototype> _Items;

        public ItemPrototypeSet(ItemPrototype[] items) : this()
        {
            foreach (var item in items)
            {
                _Items.Add(item.Id , item);
            }
        }
        public ItemPrototypeSet()
        {
            _Items = new Dictionary<string, ItemPrototype>();
        }

        public System.Collections.Generic.IEnumerable<ItemPrototype> GetItems()
        {
            return _Items.Values;
        }

        public void Add(ItemPrototype item)
        {
            _Items[item.Id] = item;
        }

        public ItemPrototype Find(string item_name)
        {
            if (_Items.ContainsKey(item_name))
            {
                return _Items[item_name];
            }
            return default(ItemPrototype);
        }

        public void Remove(string item_name)
        {
            _Items.Remove(item_name);
        }
    }
}