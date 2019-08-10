using System;

using Regulus.CustomType;

namespace Regulus.Project.GameProject1.Data
{
    public interface IVisible
    {
        ENTITY EntityType { get; }
        Guid Id { get; }

        string Name { get; }

        float View { get; }

        float Direction { get; }

        ACTOR_STATUS_TYPE Status { get; }

        event Action<EquipStatus[]> EquipEvent;

        
        event Action<VisibleStatus> StatusEvent;

        event Action<Energy> EnergyEvent;

        Vector2 Position { get; }

        void QueryStatus();

        event Action<string> TalkMessageEvent;
    }
}