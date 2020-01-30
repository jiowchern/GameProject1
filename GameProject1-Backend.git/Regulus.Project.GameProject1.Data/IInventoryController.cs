using System;

namespace Regulus.Project.GameProject1.Data
{
    public interface IInventoryController
    {
        event Action<Item[]> BagItemsEvent;
        event Action<Item[]> EquipItemsEvent;
        void Refresh();

        void Discard(Guid id);
        void Equip(Guid id);

        void Use(Guid id);

        void Unequip(Guid id);
    }
}