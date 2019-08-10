using System;
using System.Linq;

using Regulus.Project.GameProject1.Data;

namespace Regulus.Project.GameProject1.Game.Play
{
    internal class ResourceBag : Bag
    {
        private readonly ResourceItem[] _Items;

        public ResourceBag(ResourceItem[] items)
        {
            _Items = items;
            _Supplement();
            RemoveEvent += _Supplement;
        }

        private void _Supplement(Guid id)
        {
            _Supplement();
        }

        private void _Supplement()
        {
            if ( this.Count() <= 1  )
            {
                var idx = Regulus.Utility.Random.Instance.NextInt(0, _Items.Length);
                var itemProvider = new ItemProvider();
                Add(itemProvider.CreateItem(_Items[idx].Name));
            }
        }

        
    }
}