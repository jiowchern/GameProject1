using System;

namespace Regulus.Project.GameProject1.Data
{
    public interface IInventoryNotifier
    {        
        event Action<Item> AddEvent;
        event Action<Guid> RemoveEvent;
    }
}